using Agnus.Commands;
using Agnus.DTO.NF;
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
    public class SF852ViewModel : ViewModelBase
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
        public INF_Input _repository;
        private HubConnection _hubConnection;

        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleExpandCommand2 { get; }

        private ObservableCollection<CheckBoxGeneric<InputDTO>> _NFInput { get; set; }
        public ObservableCollection<CheckBoxGeneric<InputDTO>> NFInput
        {
            get { return _NFInput; }
            set
            {
                _NFInput = value;
                OnPropertyChanged(nameof(NF_Input));
            }
        }

        private ObservableCollection<CheckBoxGeneric<OutputDTO>> _NFOutput { get; set; }
        public ObservableCollection<CheckBoxGeneric<OutputDTO>> NFOutput
        {
            get { return _NFOutput; }
            set
            {
                _NFOutput = value;
                OnPropertyChanged(nameof(NFOutput));
            }
        }

        #endregion

        #region Commands SF136
        public ICommand _commandSF136 { get; set; }

        public bool CanExecuteSF136(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SF136"))
                return true;
            else return false;
        }

        public async void ExecuteSF136(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync("///SF136");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SF136());
                Application.Current?.OpenWindow(secondWindow);
            }
        }
        #endregion

        #region Commands SF731
        public ICommand _commandSF731 { get; set; }

        public bool CanExecuteSF731(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SF731"))
                return true;
            else return false;
        }

        public async void ExecuteSF731(object? parameter)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Shell.Current.GoToAsync($"///SF731");
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Window secondWindow = new Window(new SF731());
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
                OnPropertyChanged(nameof(IsInput));
                OnPropertyChanged(nameof(IsOutput));
            }
        }

        public bool IsInput => AbaSelecionada == "Input";
        public bool IsOutput => AbaSelecionada == "Output";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SF852ViewModel(INF_Input repository)
        {
            _repository = repository;
            AbaSelecionada = "Input";
            InitializeAsync();
            StartSignalR();
            InputAPI();
            OutputAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSF136 = new BaseCommand(ExecuteSF136, CanExecuteSF136);
            _commandSF731 = new BaseCommand(ExecuteSF731, CanExecuteSF731);

            ToggleExpandCommand = new Command<CheckBoxGeneric<NF_Input>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<NF_Output>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }
        public SF852ViewModel(Page page,INF_Input repository)
        {
            _page = page;
            _repository = repository;
            AbaSelecionada = "Input";
            InitializeAsync();
            StartSignalR();
            InputAPI();
            OutputAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSF136 = new BaseCommand(ExecuteSF136, CanExecuteSF136);
            _commandSF731 = new BaseCommand(ExecuteSF731, CanExecuteSF731);

            ToggleExpandCommand = new Command<CheckBoxGeneric<NF_Input>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
            ToggleExpandCommand2 = new Command<CheckBoxGeneric<NF_Output>>((item) =>
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
        public async Task InputAPI()
        {
            try
            {
                var list = await _repository.InputList();
                ObservableCollection<CheckBoxGeneric<InputDTO>> api = new ObservableCollection<CheckBoxGeneric<InputDTO>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<InputDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                NFInput = new ObservableCollection<CheckBoxGeneric<InputDTO>>(api.OrderByDescending(e => e.value.DateIn));
                OnPropertyChanged(nameof(NFInput));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task OutputAPI()
        {
            try
            {
                var list = await _repository.OutputList();
                ObservableCollection<CheckBoxGeneric<OutputDTO>> api = new ObservableCollection<CheckBoxGeneric<OutputDTO>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<OutputDTO>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                NFOutput = new ObservableCollection<CheckBoxGeneric<OutputDTO>>(api.OrderByDescending(e => e.value.DateOut));
                OnPropertyChanged(nameof(NFOutput));
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

            _hubConnection.On("UpdateNFI", async () => await InputAPI());
            _hubConnection.On("UpdateNFO", async () => await OutputAPI());

            await _hubConnection.StartAsync();
        }
    }
}
