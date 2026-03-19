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
    public class SP794ViewModel : ViewModelBase
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

        #region Command NewSupplier
        public ICommand _NewSupplierCommand { get; }
        public bool CanExecuteNewSupplier(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewSupplier(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Fornecedor...");
                _supplier.CreateBy = Convert.ToInt32(_credencial.ID);
                _supplier.DateCreate = DateTime.Now;
                var response = await _repository.NewSupplier(_supplier);
                if (response.Result)
                {
                    await _page.DisplayAlert("Fornecedores", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SP001");
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

        public Suppliers _supplier { get; set; } = new Suppliers();
        public Patrimony _SelectedPatrimnony { get; set; }
        public Credencial _credencial { get; set; }

        public IProducts _repository;

        private readonly Page _page;

        #endregion

        public SP794ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewSupplierCommand = new BaseCommand(ExecuteNewSupplier, CanExecuteNewSupplier);
        }

        public SP794ViewModel(Page page, IProducts repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewSupplierCommand = new BaseCommand(ExecuteNewSupplier, CanExecuteNewSupplier);
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
