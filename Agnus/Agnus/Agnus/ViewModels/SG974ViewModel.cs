using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    [QueryProperty(nameof(ID), "id")]
    public class SG974ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SS328");
        }

        #endregion

        #region Command EditUsage

        public ICommand _EditUseCommand { get; }
        public bool CanExecuteEditUse(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditUse(object? parameter)
        {
            var response = await _repository.UpdateUsage(_Usage);
            if (response.Result)
            {
                await _page.DisplayAlert("Usos", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SG897");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        private Request_Usage _Usage { get; set; }
        public Request_Usage Usage
        {
            get { return _Usage; }
            set
            {
                _Usage = value;
                OnPropertyChanged(nameof(Usage));
            }
        }
        public Credencial _credencial { get; set; }

        public IRequest _repository;

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
                    UsageAPI();
            }
        }

        #endregion

        public SG974ViewModel(Page page, string id, IRequest repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            UsageAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditUseCommand = new BaseCommand(ExecuteEditUse, CanExecuteEditUse);
        }

        public SG974ViewModel(IRequest repository)
        {
            _repository = repository;
            InitializeAsync();
            UsageAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditUseCommand = new BaseCommand(ExecuteEditUse, CanExecuteEditUse);
        }

        public async Task UsageAPI()
        {
            try
            {
                var list = await _repository.Usage_SearchByPc(Convert.ToInt32(ID));
                Usage = list;
                OnPropertyChanged(nameof(Usage));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
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
    }
}
