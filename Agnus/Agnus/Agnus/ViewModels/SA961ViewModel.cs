using Agnus.Commands;
using Agnus.Models.DB;
using Agnus.Views;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Agnus.Helpers;
using Agnus.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Agnus.DTO.Patrimony;

namespace Agnus.ViewModels
{
    public class SA961ViewModel : ViewModelBase
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

        #region Command SA264
        public ICommand _SA264Command { get; }
        public bool CanExecuteSA264(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA264"))
                return true;
            else return false;
        }

        public async void ExecuteSA264(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA264());
                Application.Current?.OpenWindow(secondWindow);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SA264");
            }
        }
        #endregion

        #region Command SA713
        public ICommand _SA713Command { get; }
        public bool CanExecuteSA713(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA713"))
                return true;
            else return false;
        }

        public async void ExecuteSA713(object? parameter)
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
                    LoadingService.ShowLoading(_page, "Criando Categoria de Patrimonio...");
                    var category = new Patrimony_Category();
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

        #region Commands SA173
        public ICommand _commandSA173 { get; set; }

        public bool CanExecuteSA173(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA173"))
                return true;
            else return false;
        }

        public async void ExecuteSA173(object? parameter)
        {
            var id = (int)parameter;
            var category = new Patrimony_Category();
            category = await _repository.Category_SearchBy(id);
            string result = await _page.DisplayPromptAsync("Categoria", "Digite o Nome da Categoria:", initialValue: category.Name);
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
                    LoadingService.ShowLoading(_page, "Editando Categoria de Patrimonio...");
                    category.Name = result;
                    category.CreateBy = Convert.ToInt32(_credencial.ID);
                    category.DateCreate = DateTime.Now;
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

        #region Commands SA337
        public ICommand _commandSA337 { get; set; }

        public bool CanExecuteSA337(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA337"))
                return true;
            else return false;
        }

        public async void ExecuteSA337(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SA337?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA337(id));
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Property
        private ObservableCollection<CheckBoxGeneric<PatrimonyDTO>> _patrimony { get; set; }
        public ObservableCollection<CheckBoxGeneric<PatrimonyDTO>> Patrimony
        {
            get { return _patrimony; }
            set
            {
                _patrimony = value;
                OnPropertyChanged(nameof(Patrimony));
            }
        }
        public ObservableCollection<CheckBoxGeneric<Patrimony_Category>> _catPatrimony { get; set; }

        public ObservableCollection<CheckBoxGeneric<Patrimony_Category>> CatPatrimony
        {
            get { return _catPatrimony; }
            set
            {
                _catPatrimony = value;
                OnPropertyChanged(nameof(CatPatrimony));
            }
        }

        public IPatrimony _repository;
        public Credencial _credencial { get; set; }

        private HubConnection _hubConnection;
        private readonly Page _page;
        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleExpandCommand2 { get; }

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
                OnPropertyChanged(nameof(IsPatrimony));
                OnPropertyChanged(nameof(IsCatPatrimony));
            }
        }

        public bool IsPatrimony => AbaSelecionada == "Patrimony";
        public bool IsCatPatrimony => AbaSelecionada == "CatPatrimony";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SA961ViewModel(IPatrimony repository)
        {
            _repository = repository;
            AbaSelecionada = "Patrimony";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SA264Command = new BaseCommand(ExecuteSA264, CanExecuteSA264);
            _SA713Command = new BaseCommand(ExecuteSA713, CanExecuteSA713);
            _commandSA173 = new BaseCommand(ExecuteSA173, CanExecuteSA173);
            _commandSA337 = new BaseCommand(ExecuteSA337, CanExecuteSA337);
            InitializeAsync();
            StartSignalR();
            CatPatrimonyAPI();
            PatrimonyAPI();
            ToggleExpandCommand = new Command<CheckBoxGeneric<PatrimonyDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Patrimony_Category>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });

        }

        public SA961ViewModel(Page page, IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            AbaSelecionada = "Patrimony";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SA264Command = new BaseCommand(ExecuteSA264, CanExecuteSA264);
            _SA713Command = new BaseCommand(ExecuteSA713, CanExecuteSA713);
            _commandSA173 = new BaseCommand(ExecuteSA173, CanExecuteSA173);
            _commandSA337 = new BaseCommand(ExecuteSA337, CanExecuteSA337);
            InitializeAsync();
            StartSignalR();
            CatPatrimonyAPI();
            PatrimonyAPI();
            ToggleExpandCommand = new Command<CheckBoxGeneric<PatrimonyDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Patrimony_Category>>((item) =>
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
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }

        public async Task CatPatrimonyAPI()
        {
            try
            {
                var list = await _repository.CategoryList();
                ObservableCollection<CheckBoxGeneric<Patrimony_Category>> api = new ObservableCollection<CheckBoxGeneric<Patrimony_Category>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Patrimony_Category>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                CatPatrimony = new ObservableCollection<CheckBoxGeneric<Patrimony_Category>>(api.OrderBy(e => e.value.Name));
                OnPropertyChanged(nameof(CatPatrimony));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar: {ex.Message}");
            }
        }
        public async Task PatrimonyAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<PatrimonyDTO>> api = new ObservableCollection<CheckBoxGeneric<PatrimonyDTO>>();
                var list = await _repository.PatrimonyList();
                foreach (var item in list)
                {
                    var lista = new CheckBoxGeneric<PatrimonyDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                };
                Patrimony = new ObservableCollection<CheckBoxGeneric<PatrimonyDTO>>(api);
                OnPropertyChanged(nameof(Patrimony));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar: {ex.Message}");
            }
        }
        private async void StartSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(Setting.BaseUrl + "Notification")
            .Build();

            _hubConnection.On("UpdatePCat", async () => await CatPatrimonyAPI());
            _hubConnection.On("UpdatePatrimony", async () => await PatrimonyAPI());

            await _hubConnection.StartAsync();
        }
    }
}
