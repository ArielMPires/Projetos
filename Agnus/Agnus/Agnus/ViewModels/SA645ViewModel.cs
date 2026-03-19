using Agnus.Commands;
using Agnus.DTO.Passwords;
using Agnus.DTO.Users;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    [QueryProperty(nameof(ID), "id")]
    class SA645ViewModel : ViewModelBase
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

        #region Command EditPassword
        public ICommand _EditPasswordCommand { get; }
        public bool CanExecuteEditPassword(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditPassword(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Senha...");
                _EditPassword.ChangedBy = Convert.ToInt32(_credencial.ID);
                _EditPassword.DateChanged = DateTime.Now;
                _EditPassword.Owner = _SelectedUser.ID;
                _EditPassword.Type = _Selectedtype.ID;
                _EditPassword.User = Password.User;
                _EditPassword.Password = Password.Password;
                _EditPassword.ID = Password.ID;
                var response = await _repository.UpdatePassoword(_EditPassword);
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

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    PasswordAPI();
            }
        }
        public EditPasswordDTO _EditPassword { get; set; } = new EditPasswordDTO();

        public PasswordByDTO _Password { get; set; }
        public PasswordByDTO Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    PasswordAPI();
            }
        }
        private int? _selectedTypeIndex { get; set; }
        public int? _SelectedTypeIndex
        {
            get => _selectedTypeIndex;
            set
            {
                _selectedTypeIndex = value;
                OnPropertyChanged(nameof(_SelectedTypeIndex));
            }
        }
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
        private int? _selectedUserIndex { get; set; }
        public int? _SelectedUserIndex
        {
            get => _selectedUserIndex;
            set
            {
                _selectedUserIndex = value;
                OnPropertyChanged(nameof(_SelectedUserIndex));
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
        public SA645ViewModel(IPassword repository, IUsers repositoryU)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            InitializeAsync();
            PassTypeAPI();
            UserAPI();
            PasswordAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditPasswordCommand = new BaseCommand(ExecuteEditPassword, CanExecuteEditPassword);
        }
        public SA645ViewModel(IPassword repository, IUsers repositoryU, Page page, string id)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _page = page;
            ID = id;
            InitializeAsync();
            PassTypeAPI();
            UserAPI();
            PasswordAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditPasswordCommand = new BaseCommand(ExecuteEditPassword, CanExecuteEditPassword);
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

        public async Task PasswordAPI()
        {
            var pass = await _repository.Password_SearchBy(Convert.ToInt32(ID));
            Password = pass;
            OnPropertyChanged(nameof(Password));

            if (Password != null)
            {
                if (Password.Owner != null)
                {
                    _SelectedUser = User.FirstOrDefault(e => e.ID == Password.Owner);
                    _selectedUserIndex = User.ToList().FindIndex(e => e.ID == _SelectedUser.ID);
                }
                else
                {
                    _selectedUserIndex = null;
                }
                if (Password.Type != null)
                {
                    _Selectedtype = Type.FirstOrDefault(e => e.ID == Password.Type);
                    _selectedTypeIndex = Type.ToList().FindIndex(e => e.ID == _Selectedtype.ID);
                }
                else
                {
                    _SelectedTypeIndex = null;
                }
            }
        }
    }
}
