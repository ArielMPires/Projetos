using Agnus.Commands;
using Agnus.DTO.Products;
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
    public class SK538ViewModel : ViewModelBase
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
        public IProducts _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        private ObservableCollection<CheckBoxGeneric<ProductsDTO>> _products { get; set; }
        public ObservableCollection<CheckBoxGeneric<ProductsDTO>> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        private ObservableCollection<CheckBoxGeneric<ProductsDTO>> _Filter { get; set; }

        public ObservableCollection<CheckBoxGeneric<ProductsDTO>> Filter
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }




        #endregion

        #region Commands SK991
        public ICommand _commandSK991 { get; set; }

        public bool CanExecuteSK991(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK991"))
                return true;
            else return false;
        }

        public async void ExecuteSK991(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SK991");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SK991());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SK172
        public ICommand _commandSK172 { get; set; }

        public bool CanExecuteSK172(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK172"))
                return true;
            else return false;
        }

        public async void ExecuteSK172(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SK172?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SK172(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SK323
        public ICommand _commandSK323 { get; set; }

        public bool CanExecuteSK323(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK323"))
                return true;
            else return false;
        }

        public async void ExecuteSK323(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SK323?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SK323(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        public SK538ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            ProductsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK991 = new BaseCommand(ExecuteSK991, CanExecuteSK991);
            _commandSK172 = new BaseCommand(ExecuteSK172, CanExecuteSK172);
            _commandSK323 = new BaseCommand(ExecuteSK323, CanExecuteSK323);
            ToggleExpandCommand = new Command<CheckBoxGeneric<ProductsDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SK538ViewModel(Page page, IProducts repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            ProductsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK991 = new BaseCommand(ExecuteSK991, CanExecuteSK991);
            _commandSK172 = new BaseCommand(ExecuteSK172, CanExecuteSK172);
            _commandSK323 = new BaseCommand(ExecuteSK323, CanExecuteSK323);
            ToggleExpandCommand = new Command<CheckBoxGeneric<ProductsDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }

        public async Task ProductsAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<ProductsDTO>> api = new ObservableCollection<CheckBoxGeneric<ProductsDTO>>();
                var list = await _repository.ListProduct();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<ProductsDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                Products = new ObservableCollection<CheckBoxGeneric<ProductsDTO>>(api.OrderBy(e => e.value.Description));
                Filter = new ObservableCollection<CheckBoxGeneric<ProductsDTO>>(Products);
                OnPropertyChanged(nameof(Products));
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

            _hubConnection.On("UpdateProducts", async () => await ProductsAPI());

            await _hubConnection.StartAsync();
        }
    }
}
