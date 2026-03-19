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
    public class SS328ViewModel : ViewModelBase
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

        #region Command SS751
        public ICommand _SS751Command { get; }
        public bool CanExecuteSS751(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS751"))
                return true;
            else return false;
        }

        public async void ExecuteSS751(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Departamento", "Digite o Nome do Departamento:");
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
                    LoadingService.ShowLoading(_page, "Criando Departamento...");
                    _department.CreateBy = Convert.ToInt32(_credencial.ID);
                    _department.DateCreate = DateTime.Now;
                    _department.Name = result;
                    var response = await _repository.NewDepartment(_department);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Departamento", response.Message, "OK");
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

        #region Command SS617
        public ICommand _SS617Command { get; }
        public bool CanExecuteSS617(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SS617"))
                return true;
            else return false;
        }

        public async void ExecuteSS617(object? parameter)
        {
            var id = (int)parameter;
            var dept = new Department();
            dept = await _repository.DepartmentById(id);

            string result = await _page.DisplayPromptAsync("Departamento", "Digite o Nome do Departamento:",initialValue:dept.Name);
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
                    LoadingService.ShowLoading(_page, "Editando Departamento...");
                    dept.Name = result;
                    dept.ChangedBy = Convert.ToInt32(_credencial.ID);
                    dept.DateChanged = DateTime.Now;
                    dept.CreateByFK = null;
                    dept.ChangedByFK = null;
                    var response = await _repository.UpdateDepartment(dept);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Departamento", response.Message, "OK");
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
        private readonly Page _page;
        public Department _department { get; set; } = new Department();
        private ObservableCollection<CheckBoxGeneric<Department>> _departments { get; set; }
        public ObservableCollection<CheckBoxGeneric<Department>> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private ObservableCollection<CheckBoxGeneric<Department>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Department>> Filter
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
        public IUsers _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        #endregion

        public SS328ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SS751Command = new BaseCommand(ExecuteSS751, CanExecuteSS751);
            _SS617Command = new BaseCommand(ExecuteSS617, CanExecuteSS617);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Department>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SS328ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SS751Command = new BaseCommand(ExecuteSS751, CanExecuteSS751);
            _SS617Command = new BaseCommand(ExecuteSS617, CanExecuteSS617);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Department>>((item) =>
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

        public async Task DepartmentAPI()
        {
            try
            {
                var list = await _repository.ListDepartments();
                ObservableCollection<CheckBoxGeneric<Department>> api = new ObservableCollection<CheckBoxGeneric<Department>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Department>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                Departments = new ObservableCollection<CheckBoxGeneric<Department>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Department>>(Departments);
                OnPropertyChanged(nameof(Department));
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

            _hubConnection.On("UpdateDepartment", async () => await DepartmentAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Department>>(Departments);
            }
            else
            {
                var filtrados = Departments
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Department>>(Departments);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Department>>(filtrados);
            }
        }
    }
}
