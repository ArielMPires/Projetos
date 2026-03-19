using Agnus.Commands;
using Agnus.DTO.Users;
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
    public class SS159ViewModel : ViewModelBase
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

        #region Command NewUser

        public ICommand _NewUserCommand { get; }
        public bool CanExecuteNewUser(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewUser(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Usuario...");
                _user.Role = _SelectedRole.ID;
                _user.Department = _SelectedDept.ID;
                var response = await _repository.NewUser(_user);
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

        #region Property

        public CreateUserDTO _user { get; set; } = new CreateUserDTO();

        public Roles _SelectedRole { get; set; }
        public Department _SelectedDept { get; set; }

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
        public Credencial _credencial { get; set; }

        public IUsers _repository;

        private readonly Page _page;

        #endregion

        public SS159ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            RolesAPI();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewUserCommand = new BaseCommand(ExecuteNewUser, CanExecuteNewUser);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
            _SelectImageAssCommand = new BaseCommand(ExecuteSelectImageAss, CanExecuteSelectImageAss);
        }

        public SS159ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync();
            RolesAPI();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewUserCommand = new BaseCommand(ExecuteNewUser, CanExecuteNewUser);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
            _SelectImageAssCommand = new BaseCommand(ExecuteSelectImageAss, CanExecuteSelectImageAss);
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
