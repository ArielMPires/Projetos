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
    public class SG897ViewModel : ViewModelBase
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

        private ObservableCollection<CheckBoxGeneric<Request_Usage>> _usage { get; set; }
        public ObservableCollection<CheckBoxGeneric<Request_Usage>> Usage
        {
            get { return _usage; }
            set
            {
                _usage = value;
                OnPropertyChanged(nameof(Usage));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Request_Usage>> _Filter { get; set; }

        public ObservableCollection<CheckBoxGeneric<Request_Usage>> Filter
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
        public IRequest _repository;
        private HubConnection _hubConnection;
        private readonly Page _page;
        public ICommand ToggleExpandCommand { get; }


        #endregion

        #region Commands SG699
        public ICommand _commandSG699 { get; set; }

        public bool CanExecuteSG699(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG699"))
                return true;
            else return false;
        }

        public async void ExecuteSG699(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Usos", "Digite o Nome do Uso:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Usos", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Uso...");
                    var use = new Request_Usage();
                    use.Name = result;
                    var response = await _repository.New_Usage(use);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Usos", response.Message, "OK");
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

        #region Commands SG974
        public ICommand _commandSG974 { get; set; }

        public bool CanExecuteSG974(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG974"))
                return true;
            else return false;
        }

        public async void ExecuteSG974(object? parameter)
        {
            var id = (int)parameter;
            var use = new Request_Usage();
            use = await _repository.Usage_SearchByPc(id);
            string result = await _page.DisplayPromptAsync("Usos", "Digite o Nome do Uso:", initialValue: use.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Usos", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Uso...");
                    use.Name = result;
                    var response = await _repository.UpdateUsage(use);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Usos", response.Message, "OK");
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

        public SG897ViewModel(IRequest repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            UsageAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG699 = new BaseCommand(ExecuteSG699, CanExecuteSG699);
            _commandSG974 = new BaseCommand(ExecuteSG974, CanExecuteSG974);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Request_Usage>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SG897ViewModel(Page page, IRequest repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            UsageAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG699 = new BaseCommand(ExecuteSG699, CanExecuteSG699);
            _commandSG974 = new BaseCommand(ExecuteSG974, CanExecuteSG974);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Request_Usage>>((item) =>
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

        public async Task UsageAPI()
        {
            try
            {
                var list = await _repository.UsageList();
                ObservableCollection<CheckBoxGeneric<Request_Usage>> api = new ObservableCollection<CheckBoxGeneric<Request_Usage>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Request_Usage>
                        {
                            IsChecked = false,
                            value = (Request_Usage)item
                        };

                        api.Add(lista);
                    }
                }

                Usage = new ObservableCollection<CheckBoxGeneric<Request_Usage>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Request_Usage>>(Usage);
                OnPropertyChanged(nameof(Request_Usage));
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

            _hubConnection.On("UpdateUse", async () => await UsageAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Request_Usage>>(Usage);
            }
            else
            {
                var filtrados = Usage
                  .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Request_Usage>>(Usage);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Request_Usage>>(filtrados);
            }
        }
    }
}
