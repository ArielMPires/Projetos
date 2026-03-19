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
    public class SG472ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SG429");
        }

        #endregion

        #region Command NewService

        public ICommand _NewServCommand { get; }
        public bool CanExecuteNewServ(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewServ(object? parameter)
        {

            var response = await _repository.NewService(_service);
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

        public Services _service { get; set; } = new Services();
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

        private readonly Page _page;

        #endregion

        public SG472ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewServCommand = new BaseCommand(ExecuteNewServ, CanExecuteNewServ);
        }

        public SG472ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewServCommand = new BaseCommand(ExecuteNewServ, CanExecuteNewServ);
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
