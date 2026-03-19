using Agnus.Commands;
using Agnus.DTO.Patrimony;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
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
    public class SA191ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SA803");
        }

        #endregion

        #region Command NewComputer
        public ICommand _NewComputerCommand { get; }
        public bool CanExecuteNewComputer(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewComputer(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Computador...");
                _computer.CreateBy = Convert.ToInt32(_credencial.ID);
                _computer.DateCreate = DateTime.Now;
                _computer.ID = _SelectedPatrimnony.ID;
                var response = await _repository.NewComputer(_computer);
                if (response.Result)
                {
                    await _page.DisplayAlert("Computadores", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SA803");
                    }
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
        #endregion

        #region Property

        public Computer _computer { get; set; } = new Computer();
        public PatrimonyDTO _SelectedPatrimnony { get; set; }
        private ObservableCollection<PatrimonyDTO> _patrimony { get; set; }
        public ObservableCollection<PatrimonyDTO> Patrimony
        {
            get { return _patrimony; }
            set
            {
                _patrimony = value;
                OnPropertyChanged(nameof(Patrimony));
            }
        }
        public Credencial _credencial { get; set; }

        public IPatrimony _repository;

        private readonly Page _page;

        #endregion

        public SA191ViewModel(IPatrimony repository)
        {
            _repository = repository;
            InitializeAsync();
            PatrimonyAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewComputerCommand = new BaseCommand(ExecuteNewComputer, CanExecuteNewComputer);
        }

        public SA191ViewModel(Page page, IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            PatrimonyAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewComputerCommand = new BaseCommand(ExecuteNewComputer, CanExecuteNewComputer);
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
        public async Task PatrimonyAPI()
        {
            try
            {
                var list = await _repository.PCRegister();
                Patrimony = new ObservableCollection<PatrimonyDTO>(list);
                OnPropertyChanged(nameof(Patrimony));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
