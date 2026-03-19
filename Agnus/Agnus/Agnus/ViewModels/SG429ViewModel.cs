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
    public class SG429ViewModel : ViewModelBase
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

        private ObservableCollection<CheckBoxGeneric<Services>> _services { get; set; }
        public ObservableCollection<CheckBoxGeneric<Services>> Services

        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged(nameof(Services));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Services>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Services>> Filter
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

        public Credencial _credencial { get; set; }
        public IService_Order _repository;
        private HubConnection _hubConnection;
        private readonly Page _page;
        public ICommand ToggleExpandCommand { get; }

        #endregion

        #region Commands SG472
        public ICommand _commandSG472 { get; set; }

        public bool CanExecuteSG472(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG472"))
                return true;
            else return false;
        }

        public async void ExecuteSG472(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Serviços", "Digite o Nome do Serviço:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Serviço", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Tipo de Serviço...");
                    var service = new Services();
                    service.Name = result;
                    var response = await _repository.NewService(service);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Serviço", response.Message, "OK");
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
        }
        #endregion

        #region Commands SG644
        public ICommand _commandSG644 { get; set; }

        public bool CanExecuteSG644(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG644"))
                return true;
            else return false;
        }

        public async void ExecuteSG644(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Tipo de Serviço...");
                var id = (int)parameter;
                var service = new Services();
                service = await _repository.ServiceById(id);
                string result = await _page.DisplayPromptAsync("Serviços", "Digite o Nome do Serviço:", initialValue: service.Name);
                if (String.IsNullOrEmpty(result))
                {
                    await _page.DisplayAlert("Serviço", "Obrigatorio colocar um Nome", "OK");
                }
                else
                {
                    service.Name = result;
                    var response = await _repository.UpdateService(service);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Serviço", response.Message, "OK");
                    }
                    else
                    {
                        await _page.DisplayAlert(response.Message, response.Error, "OK");
                    }
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
        public SG429ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            ServicesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG472 = new BaseCommand(ExecuteSG472, CanExecuteSG472);
            _commandSG644 = new BaseCommand(ExecuteSG644, CanExecuteSG644);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Services>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SG429ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            ServicesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG472 = new BaseCommand(ExecuteSG472, CanExecuteSG472);
            _commandSG644 = new BaseCommand(ExecuteSG644, CanExecuteSG644);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Services>>((item) =>
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
            }

            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }
        public async Task ServicesAPI()
        {
            try
            {
                var list = await _repository.ListAllService();
                ObservableCollection<CheckBoxGeneric<Services>> api = new ObservableCollection<CheckBoxGeneric<Services>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Services>
                        {
                            IsChecked = false,
                            value = (Services)item
                        };

                        api.Add(lista);
                    }
                }

                Services = new ObservableCollection<CheckBoxGeneric<Services>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Services>>(Services);
                OnPropertyChanged(nameof(Services));
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

            _hubConnection.On("UpdateServices", async () => await ServicesAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Services>>(Services);
            }
            else
            {
                var filtrados = Services
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Services>>(Services);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Services>>(filtrados);
            }
        }
    }
}
