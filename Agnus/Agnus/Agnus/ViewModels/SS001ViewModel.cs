using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models.DB;
using Agnus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Agnus.Commands;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Agnus.Views;
using Agnus.DTO.Users;

namespace Agnus.ViewModels
{
    class SS001ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SD001");
        }
        #endregion

        #region Property
        private readonly Page _page;
        public Credencial _credencial { get; set; }
        public IUsers _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        private ObservableCollection<CheckBoxGeneric<UserDTO>> _users { get; set; }
        public ObservableCollection<CheckBoxGeneric<UserDTO>> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        private ObservableCollection<CheckBoxGeneric<UserDTO>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<UserDTO>> Filter
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }


        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                OnPropertyChanged(nameof(Search));
                ApplyFilter();
            }
        }

        #endregion

        #region Commands SS159
        public ICommand _commandSS159 { get; set; }

        public bool CanExecuteSS159(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS159"))
                return true;
            else return false;
        }

        public async void ExecuteSS159(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SS159");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SS159());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SS815
        public ICommand _commandSS815 { get; set; }

        public bool CanExecuteSS815(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS815"))
                return true;
            else return false;
        }

        public async void ExecuteSS815(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SS815?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SS815(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SS603
        public ICommand _commandSS603 { get; set; }

        public bool CanExecuteSS603(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS603"))
                return true;
            else return false;
        }

        public async void ExecuteSS603(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SS603?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SS603(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SS023
        public ICommand _commandSS023 { get; set; }

        public bool CanExecuteSS023(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS023"))
                return true;
            else return false;
        }

        public async void ExecuteSS023(object? parameter)
        {
            var id = (int)parameter;

            var response = await _repository.ResetPassword(id);
            if (response.Result)
            {
                await _page.DisplayAlert("Usuarios", response.Message, "OK");
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        public SS001ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            UsersAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSS159 = new BaseCommand(ExecuteSS159, CanExecuteSS159);
            _commandSS815 = new BaseCommand(ExecuteSS815, CanExecuteSS815);
            _commandSS603 = new BaseCommand(ExecuteSS603, CanExecuteSS603);
            _commandSS023 = new BaseCommand(ExecuteSS023, CanExecuteSS023);

            ToggleExpandCommand = new Command<CheckBoxGeneric<Users>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SS001ViewModel(Page page, IUsers repository)
        {
            _page = page;
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            UsersAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSS159 = new BaseCommand(ExecuteSS159, CanExecuteSS159);
            _commandSS815 = new BaseCommand(ExecuteSS815, CanExecuteSS815);
            _commandSS603 = new BaseCommand(ExecuteSS603, CanExecuteSS603);
            _commandSS023 = new BaseCommand(ExecuteSS023, CanExecuteSS023);

            ToggleExpandCommand = new Command<CheckBoxGeneric<UserDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public async Task InitializeAsync()
        {
            string? userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
            }
            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }

        public async Task UsersAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<UserDTO>> api = new ObservableCollection<CheckBoxGeneric<UserDTO>>();

                var list = await _repository.ListUsers();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<UserDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                };
                Users = new ObservableCollection<CheckBoxGeneric<UserDTO>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<UserDTO>>(Users);
                OnPropertyChanged(nameof(Users));
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

            _hubConnection.On("UpdateUser", async () => await UsersAPI());

            await _hubConnection.StartAsync();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<UserDTO>>(Users);
            }
            else
            {
                var filtrados = Users
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<UserDTO>>(Users);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<UserDTO>>(filtrados);
            }
        }
    }
}
