using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Agnus.Views;
using Microsoft.AspNetCore.SignalR.Client;
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
    public class SS603ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SD001");
        }
        #endregion

        #region Property
        public Credencial _credencial { get; set; }
        public IUsers _repository;
        private HubConnection _hubConnection;
        private readonly Page _page;

        private ObservableCollection<CheckBoxGeneric<Pages>> _pages { get; set; }
        public ObservableCollection<CheckBoxGeneric<Pages>> pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                OnPropertyChanged(nameof(pages));
            }
        }

        private List<Agnus.Models.DB.Permissions> _permissionsADD { get; set; } = new List<Models.DB.Permissions>();
        private List<Agnus.Models.DB.Permissions> _permissionsDEL { get; set; } = new List<Models.DB.Permissions>();

        private ObservableCollection<Agnus.Models.DB.Permissions> _Permissions { get; set; } = new ObservableCollection<Agnus.Models.DB.Permissions>();
        public ObservableCollection<Agnus.Models.DB.Permissions> Permissions
        {
            get { return _Permissions; }
            set
            {
                _Permissions = value;
                OnPropertyChanged(nameof(Permissions));
            }
        }

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        #endregion

        #region Commands NewPermission
        public ICommand _commandNew { get; set; }

        public bool CanExecuteNew(object? parameter)
        {
            return true;
        }

        public async void ExecuteNew(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Atualizando Permissões...");
                foreach (var item in pages)
                {
                    if (item.IsChecked)
                    {

                        var checke = Permissions.FirstOrDefault(e => e.Page == item.value.Codigo);
                        if (checke == null)
                        {
                            _permissionsADD.Add(new Models.DB.Permissions
                            {
                                Page = item.value.Codigo,
                                User = Convert.ToInt32(ID),
                                CreateBy = Convert.ToInt32(_credencial.ID),
                                DateCreate = DateTime.Now
                            });
                        }
                    }
                }

                foreach (var item in pages)
                {
                    if (!item.IsChecked)
                    {

                        var checke = Permissions.FirstOrDefault(e => e.Page == item.value.Codigo);
                        if (checke != null)
                        {
                            _permissionsDEL.Add(checke);
                        }
                    }
                }
                if(_permissionsDEL.Count > 0)
                {
                await _repository.MultiDeletePermission(_permissionsDEL);
                }

                var response = await _repository.MultiNewPermission(_permissionsADD);
                if (response.Result)
                {
                    await _page.DisplayAlert("Usuarios", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("//SS001");
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

        public SS603ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            PermissionsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandNew = new BaseCommand(ExecuteNew, CanExecuteNew);
        }
        public SS603ViewModel(Page page, string id, IUsers repository)
        {
            _repository = repository;
            ID = id;
            _page = page;
            InitializeAsync();
            PermissionsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandNew = new BaseCommand(ExecuteNew, CanExecuteNew);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
            }

            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }

        public async Task PermissionsAPI()
        {
            try
            {
                var list = await _repository.ListPermissionsByUser(Convert.ToInt32(ID));
                Permissions = new ObservableCollection<Models.DB.Permissions>(list);
                OnPropertyChanged(nameof(Permissions));

                var list2 = new ObservableCollection<Pages>(Agnus.Models.Pages.pages);
                var api = new ObservableCollection<CheckBoxGeneric<Pages>>();
                if (list2 != null)
                {
                    foreach (var item in list2)
                    {
                        bool IsChecked;
                        var checke = Permissions.FirstOrDefault(e => e.Page == item.Codigo);
                        if (checke != null)
                        {
                            IsChecked = true;
                        }
                        else
                        {
                            IsChecked = false;
                        }

                        var lista = new CheckBoxGeneric<Pages>
                        {
                            IsChecked = IsChecked,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                pages = new ObservableCollection<CheckBoxGeneric<Pages>>(api.OrderBy(e => e.value.Description));
                OnPropertyChanged(nameof(pages));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }

    }
}
