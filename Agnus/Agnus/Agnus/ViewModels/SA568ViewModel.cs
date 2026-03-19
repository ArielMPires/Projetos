using Agnus.Commands;
using Agnus.DTO.Passwords;
using Agnus.DTO.Users;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Models;
using Agnus.Models.DB;
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
    public class SA568ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SA559");
        }

        #endregion

        #region Command NewPassword
        public ICommand _NewPasswordCommand { get; }
        public bool CanExecuteNewPassword(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewPassword(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Senha...");
                _NewPassword.CreateBy = Convert.ToInt32(_credencial.ID);
                _NewPassword.DateCreate = DateTime.Now;
                _NewPassword.Owner = _SelectedUser.ID;
                _NewPassword.Type = _Selectedtype.ID;
                var response = await _repository.NewPassoword(_NewPassword);
                if (response.Result)
                {
                    await _page.DisplayAlert("Senha", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SA559");
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
        public IPassword _repository;
        public IUsers _repositoryU;

        public readonly Page _page;

        public Credencial _credencial { get; set; }

        public NewPasswordDTO _NewPassword {get; set;} = new NewPasswordDTO();

        private ObservableCollection<Type_Passwords> _Type { get; set; }
        public ObservableCollection<Type_Passwords> Type
        {
            get => _Type;
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }

        }
        private ObservableCollection<UserDTO> _user { get; set; }
        public ObservableCollection<UserDTO> User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }

        }
        private UserDTO _selectedUser { get; set; }
        public UserDTO _SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(_SelectedUser));
            }
        }
        private Type_Passwords _selectedtype { get; set; }
        public Type_Passwords _Selectedtype
        {
            get { return _selectedtype; }
            set
            {
                _selectedtype = value;
                OnPropertyChanged(nameof(_Selectedtype));
            }
        }
        #endregion
        public SA568ViewModel(IPassword repository, IUsers repositoryU)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            InitializeAsync();
            PassTypeAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewPasswordCommand = new BaseCommand(ExecuteNewPassword, CanExecuteNewPassword);
        }
        public SA568ViewModel(IPassword repository, IUsers repositoryU, Page page)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _page = page;
            InitializeAsync();
            PassTypeAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewPasswordCommand = new BaseCommand(ExecuteNewPassword, CanExecuteNewPassword);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryU.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }

        public async Task PassTypeAPI()
        {
            try
            {
            var type = await _repository.TypeList();
            Type = new ObservableCollection<Type_Passwords>(type);
            OnPropertyChanged(nameof(Type));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar Passwords: {ex.Message}");
            }
        }

        public async Task UserAPI()
        {
            try
            {
                var users = await _repositoryU.ListUsers();
                User = new ObservableCollection<UserDTO>(users);
                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar Passwords: {ex.Message}");
            }
        }
    }
}
