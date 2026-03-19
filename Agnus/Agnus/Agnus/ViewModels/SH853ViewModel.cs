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
    public class SH853ViewModel : ViewModelBase
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
        public Credencial _credencial { get; set; }
        public IProducts _repository;
        private HubConnection _hubConnection;
        private readonly Page _page;

        private ObservableCollection<CheckBoxGeneric<Products>> _products { get; set; } = new ObservableCollection<CheckBoxGeneric<Products>>();
        public ObservableCollection<CheckBoxGeneric<Products>> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Request_Items> _Selectedproducts { get; set; } = new ObservableCollection<Request_Items>();
        public ObservableCollection<Request_Items> SelectedProducts
        {
            get { return _Selectedproducts; }
            set
            {
                _Selectedproducts = value;
                OnPropertyChanged(nameof(SelectedProducts));
            }
        }

        #endregion

        #region Commands Checkbox
        public ICommand _commandCheck { get; set; }

        public bool CanExecuteCheck(object? parameter)
        {
            return true;
        }

        public async void ExecuteCheck(object? parameter)
        {
            var value = (int)parameter;
            string result = await _page.DisplayPromptAsync("Reposição", "Digite a quantidade para repor", keyboard: Keyboard.Numeric);

            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Reposição", "Obrigatorio colocar a quantidade", "OK");
            }
            else
            {
                var product = Products.FirstOrDefault(e => e.value.ID == value);

                if (product.IsChecked)
                {

                    SelectedProducts.Add(new Request_Items()
                    {
                        Product = product.value.ID,
                        Amount = Convert.ToInt32(result),
                        purchase = false,
                        Unit_Value = 0
                    });
                }
                else
                {
                    var delete = SelectedProducts.FirstOrDefault(e => e.Product == Convert.ToInt32(product.value.ID));
                    SelectedProducts.Remove(delete);
                }
            }
        }
        #endregion

        #region Commands SK172
        public ICommand _commandSK172 { get; set; }

        public bool CanExecuteSK172(object? parameter)
        {
            return true;
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
            return true;
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

        public SH853ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            ProductsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandCheck = new BaseCommand(ExecuteCheck, CanExecuteCheck);
            _commandSK172 = new BaseCommand(ExecuteSK172, CanExecuteSK172);
            _commandSK323 = new BaseCommand(ExecuteSK323, CanExecuteSK323);
        }
        public SH853ViewModel(Page page, IProducts repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            ProductsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandCheck = new BaseCommand(ExecuteCheck, CanExecuteCheck);
            _commandSK172 = new BaseCommand(ExecuteSK172, CanExecuteSK172);
            _commandSK323 = new BaseCommand(ExecuteSK323, CanExecuteSK323);
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
                var list = await _repository.ReplacementList();
                if (list != null)
                {
                    foreach (var product in list)
                    {
                        var lista = new CheckBoxGeneric<Products>
                        {
                            IsChecked = false,
                            value = product
                        };

                        Products.Add(lista);
                    }
                }

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
