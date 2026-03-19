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
    [QueryProperty(nameof(ID), "id")]
    public class SK323ViewModel : ViewModelBase
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
        private Products _Product { get; set; }
        public Products Product
        {
            get { return _Product; }
            set
            {
                _Product = value;
                OnPropertyChanged(nameof(Product));
                ProductSupplierAPI();
            }
        }

        public Product_Supplier _PSupplier { get; set; } = new Product_Supplier();

        private ObservableCollection<Product_Supplier> _ProductSupplier { get; set; }
        public ObservableCollection<Product_Supplier> ProductSupplier
        {
            get { return _ProductSupplier; }
            set
            {
                _ProductSupplier = value;
                OnPropertyChanged(nameof(ProductSupplier));
            }
        }

        private ObservableCollection<Virtual_Stock> _VtStock { get; set; }
        public ObservableCollection<Virtual_Stock> VtStock
        {
            get { return _VtStock; }
            set
            {
                _VtStock = value;
                OnPropertyChanged(nameof(VtStock));
            }
        }

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    ProductAPI();
            }
        }
        public Suppliers _SelectSupplier { get; set; }
        private ObservableCollection<Suppliers> _supplier { get; set; }
        public ObservableCollection<Suppliers> Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }

        #endregion

        #region Commands SK669
        public ICommand _commandSK669 { get; set; }

        public bool CanExecuteSK669(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK669"))
                return true;
            else return false;
        }

        public async void ExecuteSK669(object? parameter)
        {
            _PSupplier.Product = _Product.ID;
            _PSupplier.Supplier = _SelectSupplier.ID;
            var response = await _repository.NewProductSupplier(_PSupplier);
            if (response.Result)
            {
                await _page.DisplayAlert("Fornecedores desse Produtos", response.Message, "OK");
                AbaSelecionada = "ProductSupplier";
                _SelectSupplier = null;
                _PSupplier = null;
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region ABAS

        private string _abaSelecionada;
        public string AbaSelecionada
        {
            get => _abaSelecionada;
            set
            {
                _abaSelecionada = value;
                OnPropertyChanged(nameof(AbaSelecionada));
                OnPropertyChanged(nameof(IsInfo));
                OnPropertyChanged(nameof(IsInfoAdd));
                OnPropertyChanged(nameof(IsVirtualStock));
                OnPropertyChanged(nameof(IsProductSupplier));
                OnPropertyChanged(nameof(IsAddProductSupplier));
            }
        }

        public bool IsInfo => AbaSelecionada == "Information";
        public bool IsInfoAdd => AbaSelecionada == "InfoAdd";
        public bool IsVirtualStock => AbaSelecionada == "VirtualStock";
        public bool IsProductSupplier => AbaSelecionada == "ProductSupplier";
        public bool IsAddProductSupplier => AbaSelecionada == "AddProductSupplier";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SK323ViewModel(IProducts repository)
        {
            _repository = repository;
            AbaSelecionada = "Information";
            InitializeAsync();
            StartSignalR();
            ProductAPI();
            SupplierAPI();
            ProductSupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK669 = new BaseCommand(ExecuteSK669, CanExecuteSK669);
        }
        public SK323ViewModel(Page page, string id, IProducts repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            AbaSelecionada = "Information";
            InitializeAsync();
            StartSignalR();
            ProductAPI();
            SupplierAPI();
            ProductSupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK669 = new BaseCommand(ExecuteSK669, CanExecuteSK669);
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
        public async Task ProductAPI()
        {
            try
            {
                var list = await _repository.ProductById(Convert.ToInt32(ID));
                Product = list;
                if (Product != null)
                {
                    VtStock = new ObservableCollection<Virtual_Stock>(Product.VirtualFK);
                }
                OnPropertyChanged(nameof(Product));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task SupplierAPI()
        {
            try
            {
                var list = await _repository.ListSupplier();
                Supplier = new ObservableCollection<Suppliers>(list);
                OnPropertyChanged(nameof(Supplier));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task ProductSupplierAPI()
        {
            try
            {
                if (Product != null)
                {
                    var list = await _repository.ListProductSupplierByProduct(_Product.ID);
                    ProductSupplier = new ObservableCollection<Product_Supplier>(list);
                    OnPropertyChanged(nameof(ProductSupplier));
                }
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

            _hubConnection.On("UpdateProducts", async () => await ProductAPI());
            _hubConnection.On("UpdatePS", async () => await ProductSupplierAPI());

            await _hubConnection.StartAsync();
        }
    }
}
