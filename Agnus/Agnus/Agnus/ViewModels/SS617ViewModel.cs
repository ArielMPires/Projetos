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
    public class SS617ViewModel : ViewModelBase
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

        public ICommand _EditDeptCommand { get; }
        public bool CanExecuteEditDept(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditDept(object? parameter)
        {
            _department.ChangedBy = Convert.ToInt32(_credencial.ID);
            _department.DateChanged = DateTime.Now;
            var response = await _repository.UpdateDepartment(_department);
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

        private Department _department { get; set; }
        public Department Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
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
                    DepartmentAPI();
            }
        }

        #endregion

        public SS617ViewModel(Page page, string id, IUsers repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditDeptCommand = new BaseCommand(ExecuteEditDept, CanExecuteEditDept);
        }

        public SS617ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditDeptCommand = new BaseCommand(ExecuteEditDept, CanExecuteEditDept);
        }

        public async Task DepartmentAPI()
        {
            try
            {
                var list = await _repository.DepartmentById(Convert.ToInt32(ID));
                Department = list;
                OnPropertyChanged(nameof(Department));
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
