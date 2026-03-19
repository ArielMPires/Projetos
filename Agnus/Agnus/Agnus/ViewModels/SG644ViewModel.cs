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
    public class SG644ViewModel : ViewModelBase
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

        #region Command EditService

        public ICommand _EditServCommand { get; }
        public bool CanExecuteEditServ(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditServ(object? parameter)
        {
            var response = await _repository.UpdateService(_service);
            if (response.Result)
            {
                await _page.DisplayAlert("Serviços", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SG429");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        private Services _service { get; set; }
        public Services Service
        {
            get { return _service; }
            set
            {
                _service = value;
                OnPropertyChanged(nameof(Service));
            }
        }
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

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
                    ServiceAPI();
            }
        }

        #endregion

        public SG644ViewModel(Page page, string id, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            ServiceAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditServCommand = new BaseCommand(ExecuteEditServ, CanExecuteEditServ);
        }

        public SG644ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            ServiceAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditServCommand = new BaseCommand(ExecuteEditServ, CanExecuteEditServ);
        }

        public async Task ServiceAPI()
        {
            try
            {
                var list = await _repository.ServiceById(Convert.ToInt32(ID));
                Service = list;
                OnPropertyChanged(nameof(Service));
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
            }
            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }
    }
}
