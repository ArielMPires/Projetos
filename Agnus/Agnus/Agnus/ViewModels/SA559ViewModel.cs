using Agnus.Commands;
using Agnus.DTO.Passwords;
using Agnus.DTO.Patrimony;
using Agnus.Helpers;
using Agnus.Interfaces;
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
    public class SA559ViewModel : ViewModelBase
    {
        #region Property
        public IPassword _repository;

        public readonly Page _page;

        private HubConnection _hubConnection;

        public Credencial _credencial {  get; set; }

        private ObservableCollection<CheckBoxGeneric<PasswordDTO>> _PasswordDTO { get; set; }
        public ObservableCollection<CheckBoxGeneric<PasswordDTO>> PasswordDTO {
            get => _PasswordDTO;
            set {
                _PasswordDTO = value;
                OnPropertyChanged(nameof(PasswordDTO));
                }
                 
        }
        private ObservableCollection<CheckBoxGeneric<Type_Passwords>> _Type { get; set; }
        public ObservableCollection<CheckBoxGeneric<Type_Passwords>> Type
        {
            get => _Type;
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }

        }
        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleExpandCommand2 { get; }


        #endregion

        #region Command SA465
        public ICommand _SA465Command { get; }
        public bool CanExecuteSA465(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA465"))
                return true;
            else return false;
        }

        public async void ExecuteSA465(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Tipo", "Digite o Nome do Tipo:");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Tipo", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Tipo de Senha...");
                    var tipo = new Type_Passwords();
                    tipo.Name = result;
                    var response = await _repository.NewType(tipo);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Tipo de Senha", response.Message, "OK");
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

        #region Commands SA328
        public ICommand _commandSA328 { get; set; }

        public bool CanExecuteSA328(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA328"))
                return true;
            else return false;
        }

        public async void ExecuteSA328(object? parameter)
        {
            var id = (int)parameter;
            var tipo = new Type_Passwords();
            tipo = await _repository.Type_SearchBy(id);
            string result = await _page.DisplayPromptAsync("Tipo", "Digite o Nome do Tipo:", initialValue: tipo.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Tipo", "Obrigatorio colocar um Nome", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Tipo de Senha...");
                    tipo.Name = result;
                    var response = await _repository.UpdateType(tipo);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Tipo;", response.Message, "OK");
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

        #region Command SA568
        public ICommand _SA568Command { get; }
        public bool CanExecuteSA568(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA568"))
                return true;
            else return false;
        }

        public async void ExecuteSA568(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA568());
                Application.Current?.OpenWindow(secondWindow);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SA568");
            }
        }
        #endregion

        #region Commands SA645
        public ICommand _commandSA645 { get; set; }

        public bool CanExecuteSA645(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SA645"))
                return true;
            else return false;
        }

        public async void ExecuteSA645(object? parameter)
        {
            var id = (int)parameter;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SA645?id={id}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SA645(id));
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
                OnPropertyChanged(nameof(IsPasswords));
                OnPropertyChanged(nameof(IsPassType));
            }
        }

        public bool IsPasswords => AbaSelecionada == "Passwords";
        public bool IsPassType => AbaSelecionada == "PassType";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion
        public SA559ViewModel(IPassword repository)
        {
            _repository = repository;
            AbaSelecionada = "Passwords";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _SA465Command = new BaseCommand(ExecuteSA465, CanExecuteSA465);
            _commandSA328 = new BaseCommand(ExecuteSA328, CanExecuteSA328);
            _SA568Command = new BaseCommand(ExecuteSA568, CanExecuteSA568);
            _commandSA645 = new BaseCommand(ExecuteSA645, CanExecuteSA645);
            InitializeAsync();
            PasswordAPI();
            PasswordTypeAPI();  
            StartSignalR();

            ToggleExpandCommand = new Command<CheckBoxGeneric<PasswordDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Type_Passwords>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });

        }
        public SA559ViewModel(Page page,IPassword repository)
        {
            _page = page;
            _repository = repository;
            AbaSelecionada = "Passwords";
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _SA465Command = new BaseCommand(ExecuteSA465,CanExecuteSA465);
            _commandSA328 = new BaseCommand(ExecuteSA328,CanExecuteSA328);
            _commandSA645 = new BaseCommand(ExecuteSA645,CanExecuteSA645);
            _SA568Command = new BaseCommand(ExecuteSA568, CanExecuteSA568);
            InitializeAsync();
            PasswordAPI();
            PasswordTypeAPI();
            StartSignalR();

            ToggleExpandCommand = new Command<CheckBoxGeneric<PasswordDTO>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<Type_Passwords>>((item) =>
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

        public async Task PasswordAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<PasswordDTO>> api = new ObservableCollection<CheckBoxGeneric<PasswordDTO>>();
                var passwords = await _repository.PasswordsList();
                foreach (var item in passwords)
                {
                    var lista = new CheckBoxGeneric<PasswordDTO>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                PasswordDTO = new ObservableCollection<CheckBoxGeneric<PasswordDTO>>(api);
                OnPropertyChanged(nameof(PasswordDTO));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar Passwords: {ex.Message}");
            }
        }
        public async Task PasswordTypeAPI()
        {
            try
            {
                ObservableCollection<CheckBoxGeneric<Type_Passwords>> api = new ObservableCollection<CheckBoxGeneric<Type_Passwords>>();
                var type = await _repository.TypeList();
                foreach (var item in type)
                {
                    var lista = new CheckBoxGeneric<Type_Passwords>
                    {
                        IsChecked = false,
                        value = item
                    };

                    api.Add(lista);
                }
                Type = new ObservableCollection<CheckBoxGeneric<Type_Passwords>>(api);
                OnPropertyChanged(nameof(Type));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar Passwords: {ex.Message}");
            }
        }
        private async void StartSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(Setting.BaseUrl + "Notification")
            .Build();

            _hubConnection.On("UpdatePasswords", async () => await PasswordAPI());
            _hubConnection.On("UpdatePassType", async () => await PasswordTypeAPI());

            await _hubConnection.StartAsync();
        }
    }
}
