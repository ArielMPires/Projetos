using Agnus.Commands;
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
    [QueryProperty(nameof(ID), "id")]
    public class SA227ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SA961");
        }

        #endregion

        #region Command EditPC
        public ICommand _EditComputerCommand { get; }
        public bool CanExecuteEditComputer(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditComputer(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Computador...");
                _Computer.CreateByFK = null;
            _Computer.ChangedByFK = null;
            _Computer.ChangedBy = Convert.ToInt32(_credencial.ID);
            _Computer.DateChanged = DateTime.Now;

            var response = await _repository.UpdateComputer(_Computer);
            if (response.Result)
            {
                await _page.DisplayAlert("Patrimonio", response.Message, "OK");

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

        public Credencial _credencial { get; set; }

        public IPatrimony _repository;

        private readonly Page _page;

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    ComputerAPI();
            }
        }

        private Computer _Computer { get; set; }
        public Computer Computers
        {
            get { return _Computer; }
            set
            {
                _Computer = value;
                OnPropertyChanged(nameof(Computers));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    ComputerAPI();
            }
        }

        #endregion

        public SA227ViewModel(Page page, string id, IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            ComputerAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditComputerCommand = new BaseCommand(ExecuteEditComputer, CanExecuteEditComputer);
        }

        public SA227ViewModel(IPatrimony repository)
        {
            _repository = repository;
            InitializeAsync();
            ComputerAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditComputerCommand = new BaseCommand(ExecuteEditComputer, CanExecuteEditComputer);
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

        public async Task ComputerAPI()
        {
            try
            {
                var list = await _repository.Computer_SearchByPc(Convert.ToInt32(ID));
                Computers = list;
                OnPropertyChanged(nameof(Computers));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
