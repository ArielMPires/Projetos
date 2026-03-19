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
    class SS573ViewModel : ViewModelBase
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

        #region Command SS634
        public ICommand _SS634Command { get; }
        public bool CanExecuteSS634(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS634"))
                return true;
            else return false;
        }

        public async void ExecuteSS634(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Função", "Digite o Nome do Função:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Departamento", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Função...");
                    var roles = new Roles();
                    roles.Name = result;
                    roles.CreateBy = Convert.ToInt32(_credencial.ID);
                    roles.DateCreate = DateTime.Now;
                    var response = await _repository.NewRole(roles);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Função", response.Message, "OK");
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

        #region Command SS582
        public ICommand _SS582Command { get; }
        public bool CanExecuteSS582(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS582"))
                return true;
            else return false;
        }

        public async void ExecuteSS582(object? parameter)
        {
            var id = (int)parameter;
            var roles = new Roles();
            roles = await _repository.RoleById(id);
            string result = await _page.DisplayPromptAsync("Função", "Digite o Nome do Função:", initialValue: roles.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Departamento", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Função...");
                    roles.Name = result;
                    roles.ChangedBy = Convert.ToInt32(_credencial.ID);
                    roles.DateChanged = DateTime.Now;
                    var response = await _repository.UpdateRole(roles);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Função", response.Message, "OK");
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

        #region Property
        public Credencial _credencial { get; set; }
        public IUsers _repository;
        private HubConnection _hubConnection;
        private readonly Page _page;
        public ICommand ToggleExpandCommand { get; }

        private ObservableCollection<CheckBoxGeneric<Roles>> _roles { get; set; }
        public ObservableCollection<CheckBoxGeneric<Roles>> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Roles>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Roles>> Filter
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

        public SS573ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            RolesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SS634Command = new BaseCommand(ExecuteSS634, CanExecuteSS634);
            _SS582Command = new BaseCommand(ExecuteSS582, CanExecuteSS582);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Roles>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SS573ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            RolesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SS634Command = new BaseCommand(ExecuteSS634, CanExecuteSS634);
            _SS582Command = new BaseCommand(ExecuteSS582, CanExecuteSS582);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Roles>>((item) =>
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

        public async Task RolesAPI()
        {
            try
            {
                var list = await _repository.ListRoles();
                ObservableCollection<CheckBoxGeneric<Roles>> api = new ObservableCollection<CheckBoxGeneric<Roles>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Roles>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
          

                Roles = new ObservableCollection<CheckBoxGeneric<Roles>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Roles>>(Roles);
                OnPropertyChanged(nameof(Roles));
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

            _hubConnection.On("UpdateRole", async () => await RolesAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Roles>>(Roles);
            }
            else
            {
                var filtrados = Roles
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Roles>>(Roles);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Roles>>(filtrados);
            }
        }
    }
}