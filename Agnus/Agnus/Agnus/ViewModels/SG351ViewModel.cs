using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Windows.Input;
using Agnus.Models;
using Agnus.Interfaces;
using Agnus.Models.DB;
using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces.Repositories;


namespace Agnus.ViewModels
{
    public class SG351ViewModel : ViewModelBase
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
        public ICheckList _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }


        private ObservableCollection<CheckBoxGeneric<Checklist>> _check { get; set; }
        public ObservableCollection<CheckBoxGeneric<Checklist>> Check
        {
            get { return _check; }
            set
            {
                _check = value;
                OnPropertyChanged(nameof(Check));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Checklist>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Checklist>> Filter
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

        #region Commands SG741
        public ICommand _commandSG741 { get; set; }

        public bool CanExecuteSG741(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG741"))
                return true;
            else return false;
        }

        public async void ExecuteSG741(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Check-List", "Digite o Nome do Check-List");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Check-List", "Obrigatorio colocar o Nome do Check-List", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando CheckList...");
                    var check = new Checklist()
                    {
                        Check = result,
                        CreateBy = Convert.ToInt32(_credencial.ID),
                        DateCreate = DateTime.Now
                    };
                    var response = await _repository.NewCheckList(check);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Check-List", response.Message, "OK");

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

        #region Commands SG211
        public ICommand _commandSG211 { get; set; }

        public bool CanExecuteSG211(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG211"))
                return true;
            else return false;
        }

        public async void ExecuteSG211(object? parameter)
        {
            var id = (int)parameter;
            var check = new Checklist();
            check = await _repository.CheckList_SearchByPc(id);

            string result = await _page.DisplayPromptAsync("Check-List", "Digite o Nome do Check-List", initialValue: check.Check);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Check-List", "Obrigatorio colocar o Nome do Check-List", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando CheckList...");
                    check.Check = result;
                    check.ChangedBy = Convert.ToInt32(_credencial.ID);
                    check.DateChanged = DateTime.Now;
                    check.CreateByFK = null;
                    check.ChangedByFK = null;
                    var response = await _repository.UpdateCheckList(check);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Check-List", response.Message, "OK");

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

        public SG351ViewModel(Page page, ICheckList repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            CheckAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG741 = new BaseCommand(ExecuteSG741, CanExecuteSG741);
            _commandSG211 = new BaseCommand(ExecuteSG211, CanExecuteSG211);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Checklist>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SG351ViewModel(ICheckList repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            CheckAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG741 = new BaseCommand(ExecuteSG741, CanExecuteSG741);
            _commandSG211 = new BaseCommand(ExecuteSG211, CanExecuteSG211);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Checklist>>((item) =>
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

        public async Task CheckAPI()
        {
            try
            {
                var list = await _repository.List();
                ObservableCollection<CheckBoxGeneric<Checklist>> api = new ObservableCollection<CheckBoxGeneric<Checklist>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Checklist>
                        {
                            IsChecked = false,
                            value = (Checklist)item
                        };

                        api.Add(lista);
                    }
                }

                Check = new ObservableCollection<CheckBoxGeneric<Checklist>>(api.OrderBy(e => e.value.Check));
                Filter = new ObservableCollection<CheckBoxGeneric<Checklist>>(Check);
                OnPropertyChanged(nameof(Check));
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

            _hubConnection.On("UpdateCheck", async () => await CheckAPI());

            await _hubConnection.StartAsync();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Checklist>>(Check);
            }
            else
            {
                var filtrados = Check
                    .Where(i => (i.value.Check?.ToString() ?? "")
                        .StartsWith(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                ;
                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Checklist>>(Check);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Checklist>>(filtrados);
            }
        }
    }
}
