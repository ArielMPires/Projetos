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
    public class SP001ViewModel : ViewModelBase
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
        private ObservableCollection<CheckBoxGeneric<Suppliers>> _supplier { get; set; }
        public ObservableCollection<CheckBoxGeneric<Suppliers>> Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Suppliers>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Suppliers>> Filter
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

        #region Commands SP794
        public ICommand _commandSP794 { get; set; }

        public bool CanExecuteSP794(object? parameter)
        {
            return true;
        }

        public async void ExecuteSP794(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SP794");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SP794());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SP823
        public ICommand _commandSP823 { get; set; }

        public bool CanExecuteSP823(object? parameter)
        {
            return true;
        }

        public async void ExecuteSP823(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SP823?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SP823(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        public SP001ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            SupplierAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSP794 = new BaseCommand(ExecuteSP794, CanExecuteSP794);
            _commandSP823 = new BaseCommand(ExecuteSP823, CanExecuteSP823);

            ToggleExpandCommand = new Command<CheckBoxGeneric<Suppliers>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SP001ViewModel(Page page,IProducts repository)
        {
            _page = page;
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            SupplierAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSP794 = new BaseCommand(ExecuteSP794, CanExecuteSP794);
            _commandSP823 = new BaseCommand(ExecuteSP823, CanExecuteSP823);

            ToggleExpandCommand = new Command<CheckBoxGeneric<Suppliers>>((item) =>
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

        public async Task SupplierAPI()
        {
            try
            {
                var list = await _repository.ListSupplier();
                ObservableCollection<CheckBoxGeneric<Suppliers>> api = new ObservableCollection<CheckBoxGeneric<Suppliers>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Suppliers>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                Supplier = new ObservableCollection<CheckBoxGeneric<Suppliers>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Suppliers>>(Supplier);
                OnPropertyChanged(nameof(Supplier));
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

            _hubConnection.On("UpdateSupplier", async () => await SupplierAPI());

            await _hubConnection.StartAsync();

        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Suppliers>>(Supplier);
            }
            else
            {
                var filtrados = Supplier
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Suppliers>>(Supplier);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Suppliers>>(filtrados);
            }
        }
    }
}
