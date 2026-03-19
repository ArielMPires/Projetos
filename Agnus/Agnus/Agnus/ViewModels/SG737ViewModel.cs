using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Agnus.Views;
using Domus.DTO.Service_Type;
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
    public class SG737ViewModel : ViewModelBase
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
        public IService_Order _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleExpandCommand2 { get; }
        private ObservableCollection<CheckBoxGeneric<Service_Category>> _category { get; set; }
        public ObservableCollection<CheckBoxGeneric<Service_Category>> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Service_Category>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Service_Category>> Filter
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
        private ObservableCollection<CheckBoxGeneric<TypeDTO>> _type { get; set; }
        public ObservableCollection<CheckBoxGeneric<TypeDTO>> Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        #endregion

        #region Commands SG970
        public ICommand _commandSG970 { get; set; }

        public bool CanExecuteSG970(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG970"))
                return true;
            else return false;
        }

        public async void ExecuteSG970(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Categoria", "Digite o Nome da Categoria:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Categoria", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Categoria de Chamado...");
                    var category = new Service_Category();
                    category.Name = result;
                    category.CreateBy = Convert.ToInt32(_credencial.ID);
                    category.DateCreate = DateTime.Now;
                    var response = await _repository.NewCategory(category);
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

        #region Commands SG842
        public ICommand _commandSG842 { get; set; }

        public bool CanExecuteSG842(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG842"))
                return true;
            else return false;
        }

        public async void ExecuteSG842(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SG842");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SG842());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SG677
        public ICommand _commandSG677 { get; set; }

        public bool CanExecuteSG677(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG677"))
                return true;
            else return false;
        }

        public async void ExecuteSG677(object? parameter)
        {
            var id = (int)parameter;
            var category = new Service_Category();
            category = await _repository.CategoryById(id);
            string result = await _page.DisplayPromptAsync("Categoria", "Digite o Nome da Categoria:",initialValue:category.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Categoria", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Categoria de Chamado...");
                    category.Name = result;
                    category.ChangedBy = Convert.ToInt32(_credencial.ID);
                    category.DateChanged = DateTime.Now;
                    category.CreateByFK = null;
                    category.ChangedByFK = null;
                    var response = await _repository.UpdateCategory(category);
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

        #region Commands SG884
        public ICommand _commandSG884 { get; set; }

        public bool CanExecuteSG884(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SG884"))
                return true;
            else return false;
        }

        public async void ExecuteSG884(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SG884?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SG884(id));
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
                OnPropertyChanged(nameof(IsCategory));
                OnPropertyChanged(nameof(IsType));
            }
        }

        public bool IsCategory => AbaSelecionada == "Category";
        public bool IsType => AbaSelecionada == "Type";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SG737ViewModel(IService_Order repository)
        {
            _repository = repository;
            AbaSelecionada = "Category";
            InitializeAsync();
            StartSignalR();
            CategoryAPI();
            TypeAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG970 = new BaseCommand(ExecuteSG970, CanExecuteSG970);
            _commandSG842 = new BaseCommand(ExecuteSG842, CanExecuteSG842);
            _commandSG677 = new BaseCommand(ExecuteSG677, CanExecuteSG677);
            _commandSG884 = new BaseCommand(ExecuteSG884, CanExecuteSG884);

            ToggleExpandCommand = new Command<CheckBoxGeneric<Service_Category>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Service_Type>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SG737ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            AbaSelecionada = "Category";
            InitializeAsync();
            StartSignalR();
            CategoryAPI();
            TypeAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSG970 = new BaseCommand(ExecuteSG970, CanExecuteSG970);
            _commandSG842 = new BaseCommand(ExecuteSG842, CanExecuteSG842);
            _commandSG677 = new BaseCommand(ExecuteSG677, CanExecuteSG677);
            _commandSG884 = new BaseCommand(ExecuteSG884, CanExecuteSG884);

            ToggleExpandCommand = new Command<CheckBoxGeneric<Service_Category>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Service_Type>>((item) =>
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
        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.ListAllCategory();
                ObservableCollection<CheckBoxGeneric<Service_Category>> api = new ObservableCollection<CheckBoxGeneric<Service_Category>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Service_Category>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                Category = new ObservableCollection<CheckBoxGeneric<Service_Category>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Service_Category>>(Category);
                OnPropertyChanged(nameof(Category));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task TypeAPI()
        {
            try
            {
                var list = await _repository.ListAllType();
                ObservableCollection<CheckBoxGeneric<TypeDTO>> api = new ObservableCollection<CheckBoxGeneric<TypeDTO>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<TypeDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                Type = new ObservableCollection<CheckBoxGeneric<TypeDTO>>(api.OrderBy(e => e.value.Name));
                OnPropertyChanged(nameof(Type));
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

            _hubConnection.On("UpdateOSCategory", async () => await CategoryAPI());
            _hubConnection.On("UpdateType", async () => await TypeAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Service_Category>>(Category);
            }
            else
            {
                var filtrados = Category
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Service_Category>>(Category);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Service_Category>>(filtrados);
            }
        }
    }
}
