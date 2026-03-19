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
    public class SS815ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SS001");
        }

        #endregion

        #region Command SelectImagePerfil

        public ICommand _SelectImagePerfilCommand { get; }
        public bool CanExecuteSelectImagePerfil(object? parameter)
        {
            return true;
        }

        public async void ExecuteSelectImagePerfil(object? parameter)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione uma imagem",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    // Atualiza a imagem para exibição
                    _user.photo = memoryStream.ToArray();
                    ImagemPerfil = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }
        #endregion

        #region Command SelectImageAss

        public ICommand _SelectImageAssCommand { get; }
        public bool CanExecuteSelectImageAss(object? parameter)
        {
            return true;
        }

        public async void ExecuteSelectImageAss(object? parameter)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione uma imagem",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    // Atualiza a imagem para exibição
                    _user.Signature = memoryStream.ToArray();
                    ImagemAss = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }
        #endregion

        #region Command EditUser

        public ICommand _EditUserCommand { get; }
        public bool CanExecuteEditUser(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditUser(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Usuario...");
                _user.Role = _SelectedRole.ID;
                _user.Department = _SelectedDept.ID;
                _user.RoleFK = null;
                _user.DepartmentFK = null;
                var response = await _repository.UpdateUser(_user);
                if (response.Result)
                {
                    await _page.DisplayAlert("Usuario", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SS001");
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
        public Roles _SelectedRole { get; set; }
        public int? _selectedRoleIndex { get; set; }
        public int? _SelectedRoleIndex
        {
            get => _selectedRoleIndex;
            set
            {
                _selectedRoleIndex = value;
                OnPropertyChanged(nameof(_SelectedRoleIndex));
            }
        }
        public Department _SelectedDept { get; set; }
        public int? _selectedDeptIndex { get; set; }
        public int? _SelectedDeptIndex
        {
            get => _selectedDeptIndex;
            set
            {
                _selectedDeptIndex = value;
                OnPropertyChanged(nameof(_SelectedDeptIndex));
            }
        }

        private ImageSource _imagemPerfil;
        public ImageSource ImagemPerfil
        {
            get => _imagemPerfil;
            set
            {
                _imagemPerfil = value;
                OnPropertyChanged(nameof(ImagemPerfil));
            }
        }

        private ImageSource _imagemAss;
        public ImageSource ImagemAss
        {
            get => _imagemAss;
            set
            {
                _imagemAss = value;
                OnPropertyChanged(nameof(ImagemAss));
            }
        }

        private Users _user { get; set; }
        public Users User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

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
                    UserAPI();
            }
        }
        private ObservableCollection<Department> _departments { get; set; }
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }
        private ObservableCollection<Roles> _roles { get; set; }
        public ObservableCollection<Roles> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        #endregion

        public SS815ViewModel(Page page, string id, IUsers repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            RolesAPI();
            DepartmentAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
            _SelectImageAssCommand = new BaseCommand(ExecuteSelectImageAss, CanExecuteSelectImageAss);
            _EditUserCommand = new BaseCommand(ExecuteEditUser, CanExecuteEditUser);
        }

        public SS815ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            RolesAPI();
            DepartmentAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
            _SelectImageAssCommand = new BaseCommand(ExecuteSelectImageAss, CanExecuteSelectImageAss);
            _EditUserCommand = new BaseCommand(ExecuteEditUser, CanExecuteEditUser);
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

        public async Task UserAPI()
        {
            try
            {
                var _user = await _repository.UserByID(Convert.ToInt32(ID));
                User = _user;
                OnPropertyChanged(nameof(User));

                if (User != null)
                {
                    if (User.DepartmentFK != null)
                    {
                        _SelectedDept = _user.DepartmentFK;
                        _SelectedDeptIndex = Departments.ToList().FindIndex(e => e.ID == _SelectedDept.ID);
                    }
                    else
                    {
                        _selectedDeptIndex = null;
                    }
                    if (User.RoleFK != null)
                    {
                        _SelectedRole = _user.RoleFK;
                        _SelectedRoleIndex = Roles.ToList().FindIndex(e => e.ID == _SelectedRole.ID);
                    }
                    else
                    {
                        _selectedRoleIndex = null;
                    }
                    if (User.photo != null)
                        ImagemPerfil = ImageSource.FromStream(() => new MemoryStream(_user.photo));
                    else
                        ImagemPerfil = null;
                    if (User.Signature != null)
                        ImagemAss = ImageSource.FromStream(() => new MemoryStream(_user.Signature));
                    else
                        ImagemAss = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task RolesAPI()
        {
            try
            {
                var list = await _repository.ListRoles();
                Roles = new ObservableCollection<Roles>(list);
                OnPropertyChanged(nameof(Roles));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task DepartmentAPI()
        {
            try
            {
                var list = await _repository.ListDepartments();
                Departments = new ObservableCollection<Department>(list);
                OnPropertyChanged(nameof(Departments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
