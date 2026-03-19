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
using System.Windows.Input;

namespace Agnus.ViewModels
{
    public class SA803ViewModel : ViewModelBase
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

        #region Command SA191
        public ICommand _SA191Command { get; }
        public bool CanExecuteSA191(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA191"))
                return true;
            else return false;
        }

        public async void ExecuteSA191(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA191());
                Application.Current?.OpenWindow(secondWindow);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SA191");
            }
        }
        #endregion

        #region Command SA227
        public ICommand _commandSA227 { get; set; }

        public bool CanExecuteSA227(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA227"))
                return true;
            else return false;
        }

        public async void ExecuteSA227(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SA227?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA227(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Property
        private ObservableCollection<CheckBoxGeneric<Computer>> _computers { get; set; }
        public ObservableCollection<CheckBoxGeneric<Computer>> Computers
        {
            get { return _computers; }
            set
            {
                _computers = value;
                OnPropertyChanged(nameof(Computers));
            }
        }

        public IPatrimony _repository;
        public Credencial _credencial { get; set; }

        private HubConnection _hubConnection;

        private readonly Page _page;
        public ICommand ToggleExpandCommand { get; }

        #endregion

        public SA803ViewModel(IPatrimony repository)
        {
            _repository = repository;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SA191Command = new BaseCommand(ExecuteSA191, CanExecuteSA191);
            _commandSA227 = new BaseCommand(ExecuteSA227, CanExecuteSA227);
            InitializeAsync();
            StartSignalR();
            ComputerAPI();
            ToggleExpandCommand = new Command<CheckBoxGeneric<Computer>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SA803ViewModel(Page page, IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SA191Command = new BaseCommand(ExecuteSA191, CanExecuteSA191);
            _commandSA227 = new BaseCommand(ExecuteSA227, CanExecuteSA227);
            InitializeAsync();
            StartSignalR();
            ComputerAPI();
            ToggleExpandCommand = new Command<CheckBoxGeneric<Computer>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }
        public async Task ComputerAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<Computer>> api = new ObservableCollection<CheckBoxGeneric<Computer>>();
                var list = await _repository.ComputerList();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<Computer>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                Computers = new ObservableCollection<CheckBoxGeneric<Computer>>(api.OrderBy(e => e.value.ID));
                OnPropertyChanged(nameof(Computers));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar: {ex.Message}");
            }
        }
        private async void StartSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(Setting.BaseUrl + "Notification")
            .Build();

            _hubConnection.On("UpdatePC", async () => await ComputerAPI());

            await _hubConnection.StartAsync();
        }
    }
}
