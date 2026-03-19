using Agnus.Commands;
using Agnus.DTO.Products;
using Agnus.DTO.Users;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
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
    public class SH474ViewModel : ViewModelBase
    {
        #region Command Return

        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SC001");
        }

        #endregion

        #region Command NewRequest

        public ICommand _NewRequestCommand { get; }
        public bool CanExecuteRequest(object? parameter)
        {
            return true;
        }

        public async void ExecuteRequest(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Solicitação...");
                _request.Department = _SelectedDept.ID;
                _request.Requester = _SelectedUser.ID;
                _request.Use = _SelectedUse.ID;
                _request.Status = false;
                _request.CreateBy = Convert.ToInt32(_credencial.ID);
                _request.DateCreate = DateTime.Now;
                _request.ItemsFK = Items;

                var response = await _repository.NewRequest(_request);
                if (response.Result)
                {
                    await _page.DisplayAlert("Solicitação", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SH261");
                    }
                }
                else
                {
                    await _page.DisplayAlert(response.Message, response.Error, "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Erro inesperado", ex.Message, "OK");
            }
            finally
            {
                LoadingService.HideLoading(_page);
                IsBusy = false;
            }
        }
        #endregion

        #region Commands ADDItem
        public ICommand _commandADDItem { get; set; }

        public bool CanExecuteADDItem(object? parameter)
        {
            return true;
        }

        public async void ExecuteADDItem(object? parameter)
        {
            NewItem.Product = _SelectedProduct.ID;
            NewItem.ProductFK = new Products() { ID = _SelectedProduct.ID, Description = _SelectedProduct.Description };
            NewItem.purchase = false;
            var result = Items.FirstOrDefault(e => e.Product == NewItem.Product);
            if (result == null)
            {
                NewItem.Product = _SelectedProduct.ID;
                Items.Add(NewItem);
            }
            else
            {
                result.Amount = result.Amount + NewItem.Amount;
            }
            var value = NewItem.Unit_Value * Convert.ToDecimal(NewItem.Amount);
            _request.Total_Value = _request.Total_Value + value;
            NewItem = new Request_Items();
            await _page.DisplayAlert("Item", "Item Adicionado com Sucesso!", "OK");
            _IndexProduct = -1;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(NewItem));
            OnPropertyChanged(nameof(_IndexProduct));
            AbaSelecionada = "Information";

        }
        #endregion

        #region Commands DeleteItem
        public ICommand _commandDelItem { get; set; }

        public bool CanExecuteDelItem(object? parameter)
        {
            return true;
        }

        public async void ExecuteDelItem(object? parameter)
        {
            var product = (int)parameter;
            var item = Items.FirstOrDefault(e => e.Product == product);
            var value = item.Unit_Value * Convert.ToDecimal(item.Amount);
            _request.Total_Value = _request.Total_Value - value;
            Items.Remove(item);
            await _page.DisplayAlert("Item", "Item Removido com Sucesso!", "OK");
            OnPropertyChanged(nameof(Items));
        }
        #endregion

        #region Property

        public Request _request { get; set; } = new Request();
        public Request_Items NewItem { get; set; } = new Request_Items();

        private ObservableCollection<Request_Items> _items = new ObservableCollection<Request_Items>();
        public ObservableCollection<Request_Items> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public UserDTO _SelectedUser { get; set; }
        public Department _SelectedDept { get; set; }
        public Request_Usage _SelectedUse { get; set; }
        private ObservableCollection<Department> _departments { get; set; }
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }
        private ObservableCollection<UserDTO> _users { get; set; }
        public ObservableCollection<UserDTO> User
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private ObservableCollection<Request_Usage> _use { get; set; }
        public ObservableCollection<Request_Usage> Use
        {
            get { return _use; }
            set
            {
                _use = value;
                OnPropertyChanged(nameof(Use));
            }
        }

        public ProductsDTO _SelectedProduct { get; set; }
        public int _IndexProduct { get; set; }
        private ObservableCollection<ProductsDTO> _Products { get; set; }
        public ObservableCollection<ProductsDTO> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public Credencial _credencial { get; set; }

        public IRequest _repository;
        public IUsers _repositoryU;
        public IProducts _repositoryP;

        private readonly Page _page;

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
            }
        }

        public bool IsInfo => AbaSelecionada == "Information";
        public bool IsInfoAdd => AbaSelecionada == "InfoAdd";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SH474ViewModel(IUsers repositoryU, IRequest repository, IProducts repositoryP)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryU = repositoryU;
            AbaSelecionada = "Information";
            InitializeAsync();
            DepartmentAPI();
            UsageAPI();
            UserAPI();
            ProductAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewRequestCommand = new BaseCommand(ExecuteRequest, CanExecuteRequest);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
        }

        public SH474ViewModel(Page page, IUsers repositoryU, IRequest repository, IProducts repositoryP)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryU = repositoryU;
            _page = page;
            AbaSelecionada = "Information";
            InitializeAsync();
            DepartmentAPI();
            UsageAPI();
            UserAPI();
            ProductAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewRequestCommand = new BaseCommand(ExecuteRequest, CanExecuteRequest);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryU.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryP.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }
        public async Task DepartmentAPI()
        {
            try
            {
                var list = await _repositoryU.ListDepartments();
                Departments = new ObservableCollection<Department>(list);
                OnPropertyChanged(nameof(Departments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task UserAPI()
        {
            try
            {
                var list = await _repositoryU.ListUsers();
                User = new ObservableCollection<UserDTO>(list.OrderBy(e => e.Name));
                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task UsageAPI()
        {
            try
            {
                var list = await _repository.UsageList();
                Use = new ObservableCollection<Request_Usage>(list);
                OnPropertyChanged(nameof(Use));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task ProductAPI()
        {
            try
            {
                var list = await _repositoryP.ListProduct();
                Products = new ObservableCollection<ProductsDTO>(list);
                OnPropertyChanged(nameof(Products));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
