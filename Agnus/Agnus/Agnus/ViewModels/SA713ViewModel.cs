using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Microsoft.Maui.Controls;
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
    public class SA713ViewModel : ViewModelBase
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

        #region Command NewCategory

        public ICommand _NewCategoryCommand { get; }
        public bool CanExecuteNewCategory(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewCategory(object? parameter)
        {
            _newCategory.CreateBy = Convert.ToInt32(_credencial.ID);
            _newCategory.DateCreate = DateTime.Now;
            var response = await _repository.NewCategory(_newCategory);
            if (response.Result)
            {
                await _page.DisplayAlert("Categoria", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SA961");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }

        #endregion

        #region Property

        public Patrimony_Category _newCategory { get; set; }

        public Credencial _credencial { get; set; }

        public IPatrimony _repository;

        private readonly Page _page;

        #endregion

        public SA713ViewModel(Page page, IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewCategoryCommand = new BaseCommand(ExecuteNewCategory, CanExecuteNewCategory);
            _newCategory = new Patrimony_Category();
        }

        public SA713ViewModel(IPatrimony repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewCategoryCommand = new BaseCommand(ExecuteNewCategory, CanExecuteNewCategory);
            _newCategory = new Patrimony_Category();
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
