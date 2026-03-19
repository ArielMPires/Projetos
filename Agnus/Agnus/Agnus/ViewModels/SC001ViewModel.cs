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
using System.ComponentModel;
using Agnus.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using Agnus.Views;
using Domus.DTO.Service_Order;
using Agnus.DTO.Service_Order;

namespace Agnus.ViewModels
{
    class SC001ViewModel : ViewModelBase
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

        #region Command SC273
        public ICommand _SC273Command { get; }
        public bool CanExecuteSC273(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SC273"))
                return true;
            else return false;
        }

        public async void ExecuteSC273(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SC273());
                Application.Current?.OpenWindow(secondWindow);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SC273");
            }
        }
        #endregion

        #region Command CatchOS
        public ICommand _CatchOSCommand { get; }
        public bool CanExecuteCatchOS(object? parameter)
        {
            return true;
        }

        public async void ExecuteCatchOS(object? parameter)
        {
            var id = (int)parameter;
            try
            {
                var os = new CatchOrderDTO()
                {
                    Technical = Convert.ToInt32(_credencial.ID),
                    ChangedBy = Convert.ToInt32(_credencial.ID),
                    DateChanged = DateTime.Now
                };
                var response = await _repository.CatchOrder(id, os);
                if (response.Result)
                {
                    await _page.DisplayAlert("Chamado", response.Message, "OK");
                }
                else
                {
                    await _page.DisplayAlert(response.Message, response.Error, "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Chamado", ex.Message, "OK");
            }
        }
        #endregion

        #region Command Contact
        public ICommand _ContactCommand { get; }
        public bool CanExecuteContact(object? parameter)
        {
            return true;
        }

        public async void ExecuteContact(object? parameter)
        {
            try
            {
                var id = (int)parameter;
                var response = await _repository.ContactOrder(id);
                if (response.Result)
                {
                    await _page.DisplayAlert("Chamado", response.Message, "OK");
                }
                else
                {
                    await _page.DisplayAlert(response.Message, response.Error, "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Chamado", ex.Message, "OK");
            }
        }
        #endregion

        #region Command EndOS
        public ICommand _EndOSCommand { get; }
        public bool CanExecuteEndOS(object? parameter)
        {
            return true;
        }

        public async void ExecuteEndOS(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Finalizar OS", "Qual o Motivo para a Finalização do Chamado?");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Chamados", "Obrigatorio colocar um Motivo", "OK");
            }
            else
            {
                var id = (int)parameter;
                try
                {
                    var order = new EndOrderDTO()
                    {
                        Status = true,
                        ChangedBy = Convert.ToInt32(_credencial.ID),
                        DateChanged = DateTime.Now,
                        Reason = result
                    };
                    var response = await _repository.EndOrder(id, order);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Chamado", response.Message, "OK");
                        TechnicalAPI();
                        OrdersAPI();
                        PendingAPI();
                    }
                    else
                    {
                        await _page.DisplayAlert(response.Message, response.Error, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await _page.DisplayAlert("Chamado", ex.Message, "OK");
                }
            }
        }
        #endregion

        #region Command SC683
        public ICommand _commandSC683 { get; set; }

        public bool CanExecuteSC683(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SC683"))
                return true;
            else return false;
        }

        public async void ExecuteSC683(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SC683?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SC683(id));
                Application.Current?.OpenWindow(secondWindow);
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
                OnPropertyChanged(nameof(IsOrders));
                OnPropertyChanged(nameof(IsPending));
                OnPropertyChanged(nameof(IsTechnical));
            }
        }

        public bool IsOrders => AbaSelecionada == "Orders";
        public bool IsPending => AbaSelecionada == "Pending";
        public bool IsTechnical => AbaSelecionada == "ByTechnical";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        #region Property
        private HubConnection _hubConnection;
        private readonly Page _page;
        private ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> _orders { get; set; }
        public ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }
        public ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> _ordersPending { get; set; }

        public ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> OrdersPending
        {
            get { return _ordersPending; }
            set
            {
                _ordersPending = value;
                OnPropertyChanged(nameof(OrdersPending));
            }
        }
        public ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> _ordersByTechnical { get; set; }

        public ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> OrdersByTechnical
        {
            get { return _ordersByTechnical; }
            set
            {
                _ordersByTechnical = value;
                OnPropertyChanged(nameof(OrdersByTechnical));
            }
        }
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

        public ICommand ToggleExpandCommand { get; }
        #endregion

        public SC001ViewModel(IService_Order repository)
        {
            _repository = repository;
            AbaSelecionada = "Pending";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SC273Command = new BaseCommand(ExecuteSC273, CanExecuteSC273);
            _CatchOSCommand = new BaseCommand(ExecuteCatchOS, CanExecuteCatchOS);
            _ContactCommand = new BaseCommand(ExecuteContact, CanExecuteContact);
            _EndOSCommand = new BaseCommand(ExecuteEndOS, CanExecuteEndOS);
            _commandSC683 = new BaseCommand(ExecuteSC683, CanExecuteSC683);
            InitializeAsync();
            OrdersAPI();
            PendingAPI();
            TechnicalAPI();
            StartSignalR();

            ToggleExpandCommand = new Command<CheckBoxGeneric<ServiceOrderDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SC001ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            AbaSelecionada = "Pending";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SC273Command = new BaseCommand(ExecuteSC273, CanExecuteSC273);
            _CatchOSCommand = new BaseCommand(ExecuteCatchOS, CanExecuteCatchOS);
            _ContactCommand = new BaseCommand(ExecuteContact, CanExecuteContact);
            _EndOSCommand = new BaseCommand(ExecuteEndOS, CanExecuteEndOS);
            _commandSC683 = new BaseCommand(ExecuteSC683, CanExecuteSC683);
            InitializeAsync();
            OrdersAPI();
            PendingAPI();
            TechnicalAPI();
            StartSignalR();

            ToggleExpandCommand = new Command<CheckBoxGeneric<ServiceOrderDTO>>((item) =>
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
            }
            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            OrdersAPI();
            PendingAPI();
            TechnicalAPI();
        }
        public async Task OrdersAPI()
        {
            try
            {
                if (_credencial.Permissions.Any(e => e.Page == "SC023"))
                {
                    var list = await _repository.ListAllOrder();
                    ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> api = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>();
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<ServiceOrderDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                    Orders = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>(api.OrderByDescending(e => e.value.Requested_Date));
                    OnPropertyChanged(nameof(Orders));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task PendingAPI()
        {
            try
            {
                var list = await _repository.ListPendingOrder();
                ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> api = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<ServiceOrderDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                OrdersPending = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>(api.OrderBy(e => e.value.Priority));
                OnPropertyChanged(nameof(OrdersPending));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task TechnicalAPI()
        {
            try
            {
                var list = await _repository.ListTechnicalOrder(Convert.ToInt32(_credencial.ID));
                ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>> api = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<ServiceOrderDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                OrdersByTechnical = new ObservableCollection<CheckBoxGeneric<ServiceOrderDTO>>(api.OrderBy(e => e.value.Requested_Date));
                OnPropertyChanged(nameof(OrdersByTechnical));
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

            _hubConnection.On("UpdateOS", async () => await OrdersAPI());
            _hubConnection.On("UpdateOS", async () => await PendingAPI());
            _hubConnection.On("UpdateOS", async () => await TechnicalAPI());

            await _hubConnection.StartAsync();
        }
    }
}
