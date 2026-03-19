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
    [QueryProperty(nameof(ID), "id")]
    public class SS582ViewModel : ViewModelBase
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

        #region Command EditDepartment

        public ICommand _EditRoleCommand { get; }
        public bool CanExecuteEditRole(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditRole(object? parameter)
        {
            _role.ChangedBy = Convert.ToInt32(_credencial.ID);
            _role.DateChanged = DateTime.Now;
            var response = await _repository.UpdateRole(_role);
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

        private Roles _role { get; set; }
        public Roles Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public Credencial _credencial { get; set; }

        public IUsers _repository;

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
                    RolesAPI();
            }
        }

        #endregion

        public SS582ViewModel(Page page, string id, IUsers repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            RolesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditRoleCommand = new BaseCommand(ExecuteEditRole, CanExecuteEditRole);
        }

        public SS582ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            RolesAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditRoleCommand = new BaseCommand(ExecuteEditRole, CanExecuteEditRole);
        }

        public async Task RolesAPI()
        {
            try
            {
                var list = await _repository.RoleById(Convert.ToInt32(ID));
                Role = list;
                OnPropertyChanged(nameof(Roles));
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
