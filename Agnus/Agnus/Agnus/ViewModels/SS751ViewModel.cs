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
    public class SS751ViewModel
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

        #region Command NewDepartment

        public ICommand _NewDeptCommand { get; }
        public bool CanExecuteNewDept(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewDept(object? parameter)
        {
            _department.CreateBy = Convert.ToInt32(_credencial.ID);
            _department.DateCreate = DateTime.Now;
            var response = await _repository.NewDepartment(_department);
            if (response.Result)
            {
                await _page.DisplayAlert("Departamento", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SS328");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        public Department _department { get; set; } = new Department();
        public Credencial _credencial { get; set; }

        public IUsers _repository;

        private readonly Page _page;

        #endregion

        public SS751ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewDeptCommand = new BaseCommand(ExecuteNewDept, CanExecuteNewDept);
        }

        public SS751ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewDeptCommand = new BaseCommand(ExecuteNewDept, CanExecuteNewDept);
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
