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
    public class SG970ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SG737");
        }

        #endregion

        #region Command NewCategory

        public ICommand _NewCatCommand { get; }
        public bool CanExecuteNewCat(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewCat(object? parameter)
        {
            _category.CreateBy = Convert.ToInt32(_credencial.ID);
            _category.DateCreate = DateTime.Now;
            var response = await _repository.NewCategory(_category);
            if (response.Result)
            {
                await _page.DisplayAlert("Categoria de Chamado", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SG737");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        public Service_Category _category { get; set; } = new Service_Category();
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

        private readonly Page _page;

        #endregion

        public SG970ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewCatCommand = new BaseCommand(ExecuteNewCat, CanExecuteNewCat);
        }

        public SG970ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewCatCommand = new BaseCommand(ExecuteNewCat, CanExecuteNewCat);
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
