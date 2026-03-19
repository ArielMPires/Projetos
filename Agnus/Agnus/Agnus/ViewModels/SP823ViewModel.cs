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
    public class SP823ViewModel : ViewModelBase
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

        #region Command EditSupplier
        public ICommand _EditSupplierCommand { get; }
        public bool CanExecuteEditSupplier(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditSupplier(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Fornecedor...");
                _Supplier.CreateByFK = null;
                _Supplier.ChangedByFK = null;
                _Supplier.ChangedBy = Convert.ToInt32(_credencial.ID);
                _Supplier.DateChanged = DateTime.Now;

                var response = await _repository.UpdateSupplier(_Supplier);
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

        public Credencial _credencial { get; set; }

        public IProducts _repository;

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
                    SupplierAPI();
            }
        }

        private Suppliers _Supplier { get; set; }
        public Suppliers Supplier
        {
            get { return _Supplier; }
            set
            {
                _Supplier = value;
                OnPropertyChanged(nameof(Supplier));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    SupplierAPI();
            }
        }

        #endregion

        public SP823ViewModel(Page page, string id, IProducts repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            SupplierAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditSupplierCommand = new BaseCommand(ExecuteEditSupplier, CanExecuteEditSupplier);
        }

        public SP823ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            SupplierAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditSupplierCommand = new BaseCommand(ExecuteEditSupplier, CanExecuteEditSupplier);
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

        public async Task SupplierAPI()
        {
            try
            {
                var list = await _repository.SupplierById(Convert.ToInt32(ID));
                Supplier = list;
                OnPropertyChanged(nameof(Supplier));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
