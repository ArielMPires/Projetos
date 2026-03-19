using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Agnus.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    public class SD001ViewModel : ViewModelBase
    {
        #region Property
        private HubConnection _hubConnection;
        private ImageSource _Photo { get; set; }
        public ImageSource Photo
        {
            get { return _Photo; }
            set
            {
                _Photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }
        public Credencial _credencial { get; set; }
        private readonly Page _page;
        public IUsers _repository;
        private Tenant _selectedTenant { get; set; }
        public Tenant _SelectedTenant
        {
            get { return _selectedTenant; }
            set
            {
                _selectedTenant = value;
                OnPropertyChanged(nameof(_SelectedTenant));
                TenantChanged();
            }
        }
        private ObservableCollection<Tenant> _tenant { get; set; }
        public ObservableCollection<Tenant> Tenant
        {
            get { return _tenant; }
            set
            {
                _tenant = value;
                OnPropertyChanged(nameof(Tenant));
            }
        }
        public int? _selectedtenantIndex { get; set; }
        public int? _SelectedTenantIndex
        {
            get => _selectedtenantIndex;
            set
            {
                _selectedtenantIndex = value;
                OnPropertyChanged(nameof(_SelectedTenantIndex));
            }
        }
        #endregion

        #region Commands SD641
        public ICommand _commandSD641 { get; set; }

        public bool CanExecuteSD641(object? parameter)
        {
            return true;
        }

        public async void ExecuteSD641(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SD641");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SD641());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Switch Password
        public ICommand _SwitchCommand { get; }
        public bool CanExecuteSwitch(object? parameter)
        {
            return true;
        }

        public async void ExecuteSwitch(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Senha", "Digite Sua Senha Atual:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Senha", "Obrigatorio colocar a Senha!", "OK");
            }
            else
            {
                string result2 = await _page.DisplayPromptAsync("Senha", "Digite A Senha Nova:");
                if (String.IsNullOrEmpty(result))
                {
                    await _page.DisplayAlert("Chamados", "Obrigatorio colocar a Senha!", "OK");
                }
                else
                {
                    var pass = new SwitchPass()
                    {
                        Id = Convert.ToInt32(_credencial.ID),
                        CurrentPassword = result,
                        NewPassword = result2
                    };
                    var response = await _repository.SwitchPassword(pass);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Usuarios", response.Message, "OK");
                    }
                    else
                    {
                        await _page.DisplayAlert(response.Message, response.Error, "OK");
                    }
                }
            }
        }

        #endregion

        public SD001ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            PhotoAPI();
            ThemeAPI();
            StartSignalR();
            _SwitchCommand = new BaseCommand(ExecuteSwitch, CanExecuteSwitch);
            _commandSD641 = new BaseCommand(ExecuteSD641, CanExecuteSD641);
        }
        public SD001ViewModel(Page page, IUsers repository)
        {
            _page = page;
            _repository = repository;
            InitializeAsync();
            PhotoAPI();
            ThemeAPI();
            StartSignalR();
            _SwitchCommand = new BaseCommand(ExecuteSwitch, CanExecuteSwitch);
            _commandSD641 = new BaseCommand(ExecuteSD641, CanExecuteSD641);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                await TenantData();
            }

        }
        public async Task TenantData()
        {
            try
            {
                Tenant = new ObservableCollection<Tenant>(Agnus.Models.Tenant.Tenants);
                OnPropertyChanged(nameof(Tenant));

                if (Tenant != null && _credencial != null)
                {
                    _SelectedTenant = Tenant.FirstOrDefault(e => e.Id == Convert.ToInt32(_credencial.tenantId));
                    _SelectedTenantIndex = Tenant.ToList().FindIndex(e => e.Id == _SelectedTenant.Id);
                    OnPropertyChanged(nameof(_selectedtenantIndex));
                }
                else
                {
                    _selectedtenantIndex = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }

        public async Task TenantChanged()
        {

            _credencial.tenantId = Convert.ToString(value: _SelectedTenant.Id);
            var lista = await _repository.ListPermissionsByUser(Convert.ToInt32(_credencial.ID));
            List<Agnus.Models.DB.Permissions> permissions = new List<Agnus.Models.DB.Permissions>(lista);
            TokenProcessor.ProcessToken(_credencial.Token, _credencial.TokenExpired, _credencial.tenantId, permissions);
        }

        public async Task PhotoAPI()
        {
            try
            {
                var photo = await _repository.Photo(Convert.ToInt32(_credencial.ID));
                if (photo != null)
                    Photo = ImageSource.FromStream(() => new MemoryStream(photo));
                else
                    Photo = ImageSource.FromFile("logo.png");
                OnPropertyChanged(nameof(Photo));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        private async void StartSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(Setting.BaseUrl + "Notification")
            .Build();

            _hubConnection.On("UpdateTheme", async () => await ThemeAPI());
            _hubConnection.On("UpdatePhoto", async () => await PhotoAPI());

            await _hubConnection.StartAsync();
        }
        public async Task ThemeAPI()
        {
            try
            {
                var theme = await _repository.ThemeByUser(Convert.ToInt32(_credencial.ID));
                if (theme != null)
                    ThemeManager.ApplyTheme(theme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
