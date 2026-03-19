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
    public class SS634ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SS573");
        }

        #endregion

        #region Command NewRole

        public ICommand _NewRoleCommand { get; }
        public bool CanExecuteNewRole(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewRole(object? parameter)
        {
            _roles.CreateBy = Convert.ToInt32(_credencial.ID);
            _roles.DateCreate = DateTime.Now;
            var response = await _repository.NewRole(_roles);
            if (response.Result)
            {
                await _page.DisplayAlert("Função", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SS573");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        public Roles _roles { get; set; } = new Roles();
        public Credencial _credencial { get; set; }

        public IUsers _repository;

        private readonly Page _page;

        #endregion

        public SS634ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewRoleCommand = new BaseCommand(ExecuteNewRole, CanExecuteNewRole);
        }

        public SS634ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewRoleCommand = new BaseCommand(ExecuteNewRole, CanExecuteNewRole);
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
