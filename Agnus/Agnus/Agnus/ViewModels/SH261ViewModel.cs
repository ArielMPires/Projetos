using Agnus.Commands;
using Agnus.DTO.Request;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Agnus.Views;
using Domus.DTO.Purchase_Order;
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
    public class SH261ViewModel : ViewModelBase
    {
        #region Property
        private readonly Page _page;
        public Credencial _credencial { get; set; }
        public IRequest _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleExpandCommand2 { get; }
        private ObservableCollection<CheckBoxGeneric<RequestDTO>> _request { get; set; }
        public ObservableCollection<CheckBoxGeneric<RequestDTO>> Request
        {
            get { return _request; }
            set
            {
                _request = value;
                OnPropertyChanged(nameof(Request));
            }
        }
        private ObservableCollection<CheckBoxGeneric<RequestDTO>> _requestPending { get; set; }
        public ObservableCollection<CheckBoxGeneric<RequestDTO>> RequestPending
        {
            get { return _requestPending; }
            set
            {
                _requestPending = value;
                OnPropertyChanged(nameof(RequestPending));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Purchase_OrderDTO>> _requestApproval { get; set; }
        public ObservableCollection<CheckBoxGeneric<Purchase_OrderDTO>> RequestApproval
        {
            get { return _requestApproval; }
            set
            {
                _requestApproval = value;
                OnPropertyChanged(nameof(RequestApproval));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Request>> _requestFailed { get; set; }
        public ObservableCollection<CheckBoxGeneric<Request>> RequestFailed
        {
            get { return _requestFailed; }
            set
            {
                _requestFailed = value;
                OnPropertyChanged(nameof(RequestFailed));
            }
        }
        private ObservableCollection<CheckBoxGeneric<RequestDTO>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<RequestDTO>> Filter
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

        #region Commands SH474
        public ICommand _commandSH474 { get; set; }

        public bool CanExecuteSH474(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SH474"))
                return true;
            else return false;
        }

        public async void ExecuteSH474(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SH474");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SH474());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SH908
        public ICommand _commandSH908 { get; set; }

        public bool CanExecuteSH908(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SH908"))
                return true;
            else return false;
        }

        public async void ExecuteSH908(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SH908?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SH908(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SH492
        public ICommand _commandSH492 { get; set; }

        public bool CanExecuteSH492(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SH492"))
                return true;
            else return false;
        }

        public async void ExecuteSH492(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SH492?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SH492(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Command Delivered

        public ICommand _DeliveredCommand { get; }
        public bool CanExecuteDelivered(object? parameter)
        {
            return true;
        }

        public async void ExecuteDelivered(object? parameter)
        {
            var id = (int)parameter;
            var _order = await _repository.Purchase_SearchByPc(id);
            _order.ChangedBy = Convert.ToInt32(_credencial.ID);
            _order.DateChanged = DateTime.Now;
            _order.Delivered = true;

            var response = await _repository.UpdateOrder(_order);
            if (response.Result)
            {
                await _page.DisplayAlert("Pedido de Compra", response.Message, "OK");

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
                OnPropertyChanged(nameof(IsApproval));
                OnPropertyChanged(nameof(IsPending));
                OnPropertyChanged(nameof(IsFailed));
                OnPropertyChanged(nameof(IsTotal));
            }
        }

        public bool IsApproval => AbaSelecionada == "Approval";
        public bool IsPending => AbaSelecionada == "Pending";
        public bool IsFailed => AbaSelecionada == "Failed";
        public bool IsTotal => AbaSelecionada == "Total";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SH261ViewModel(IRequest repository)
        {
            _repository = repository;
            AbaSelecionada = "Pending";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            InitializeAsync();
            StartSignalR();
            RequestAPI();
            PurchaseAPI();
            _commandSH474 = new BaseCommand(ExecuteSH474, CanExecuteSH474);
            _commandSH908 = new BaseCommand(ExecuteSH908, CanExecuteSH908);
            _commandSH492 = new BaseCommand(ExecuteSH492, CanExecuteSH492);
            _DeliveredCommand = new BaseCommand(ExecuteDelivered, CanExecuteDelivered);

            ToggleExpandCommand = new Command<CheckBoxGeneric<RequestDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Purchase_OrderDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SH261ViewModel(Page page, IRequest repository)
        {
            _repository = repository;
            _page = page;
            AbaSelecionada = "Pending";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            InitializeAsync();
            StartSignalR();
            RequestAPI();
            PurchaseAPI();
            _commandSH474 = new BaseCommand(ExecuteSH474, CanExecuteSH474);
            _commandSH908 = new BaseCommand(ExecuteSH908, CanExecuteSH908);
            _commandSH492 = new BaseCommand(ExecuteSH492, CanExecuteSH492);
            _DeliveredCommand = new BaseCommand(ExecuteDelivered, CanExecuteDelivered);

            ToggleExpandCommand = new Command<CheckBoxGeneric<RequestDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Purchase_OrderDTO>>((item) =>
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

        public async Task RequestAPI()
        {
            try
            {
                var list = await _repository.RequestList();
                ObservableCollection<CheckBoxGeneric<RequestDTO>> api = new ObservableCollection<CheckBoxGeneric<RequestDTO>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<RequestDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                ObservableCollection<CheckBoxGeneric<RequestDTO>> api2 = new ObservableCollection<CheckBoxGeneric<RequestDTO>>();
                var list2 = await _repository.RequestListPending();
                if (list2 != null)
                {
                    foreach (var item in list2)
                    {
                        var lista = new CheckBoxGeneric<RequestDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api2.Add(lista);
                    }
                }
                Request = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(api.OrderByDescending(e => e.value.ID));
                RequestPending = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(api2.OrderByDescending(e => e.value.ID));
                Filter = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(Request);
                OnPropertyChanged(nameof(Users));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task PurchaseAPI()
        {
            try
            {
                var list = await _repository.PurchaseListDelivered();
                ObservableCollection<CheckBoxGeneric<Purchase_OrderDTO>> api = new ObservableCollection<CheckBoxGeneric<Purchase_OrderDTO>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Purchase_OrderDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                RequestApproval = new ObservableCollection<CheckBoxGeneric<Purchase_OrderDTO>>(api.OrderBy(e => e.value.ID));

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

            _hubConnection.On("UpdateRequest", async () => await RequestAPI());
            _hubConnection.On("UpdateROrder", async () => await PurchaseAPI());

            await _hubConnection.StartAsync();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(Request);
            }
            else
            {
                var filtrados = Request
                    .Where(i => i.value.Department.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(Request);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<RequestDTO>>(filtrados);
            }
        }
    }
}
