using Agnus.Commands;
using Agnus.DTO.Users;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    public class SD641ViewModel : ViewModelBase
    {
        #region Property
        public Credencial _credencial { get; set; }
        private readonly Page _page;
        public IUsers _repository;
        private ImageSource _Photo { get; set; }
        public ImageSource Photo
        {
            get { return _Photo; }
            set
            {
                _Photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }
        private NewPhotoDTO _NewPhotoDTO { get; set; } = new NewPhotoDTO();
        public NewPhotoDTO NewPhotoDTO
        {
            get { return _NewPhotoDTO; }
            set
            {
                _NewPhotoDTO = value;
                OnPropertyChanged(nameof(NewPhotoDTO));
            }
        }
        private ThemeDTO _ThemeDTO { get; set; } = new ThemeDTO();
        public ThemeDTO ThemeDTO
        {
            get { return _ThemeDTO; }
            set
            {
                _ThemeDTO = value;
                OnPropertyChanged(nameof(ThemeDTO));
            }
        }


        #endregion

        #region Command Theme

        public ICommand _ThemeCommand { get; }
        public bool CanExecuteTheme(object? parameter)
        {
            return true;
        }

        public async void ExecuteTheme(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Tema...");
                _ThemeDTO.User = Convert.ToInt32(_credencial.ID);
                var response = await _repository.SwitchTheme(_ThemeDTO);
                if (response.Result)
                {
                    await _page.DisplayAlert("Thema", response.Message, "OK");

                    var theme = await _repository.ThemeByUser(Convert.ToInt32(_credencial.ID));
                    if (theme != null)
                        ThemeManager.ApplyTheme(theme);

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SD001");
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
                    NewPhotoDTO.photo = memoryStream.ToArray();
                    NewPhotoDTO.ID = Convert.ToInt32(_credencial.ID);

                    var response = await _repository.NewPhoto(_NewPhotoDTO);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Foto", response.Message, "OK");
                    }
                    else
                    {
                        await _page.DisplayAlert(response.Message, response.Error, "OK");
                    }
                    Photo = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }
        #endregion
        public SD641ViewModel(IUsers repository)
        {
            _repository = repository;
            InitializeAsync(repository);
            PhotoAPI();
            ThemeAPI();
            _ThemeCommand = new BaseCommand(ExecuteTheme, CanExecuteTheme);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
        }
        public SD641ViewModel(Page page, IUsers repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync(repository);
            PhotoAPI();
            ThemeAPI();
            _ThemeCommand = new BaseCommand(ExecuteTheme, CanExecuteTheme);
            _SelectImagePerfilCommand = new BaseCommand(ExecuteSelectImagePerfil, CanExecuteSelectImagePerfil);
        }

        private async void InitializeAsync(IUsers repository)
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository = repository;
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }

        }

        public async Task ThemeAPI()
        {
            try
            {
                var theme = await _repository.ThemeByUser(Convert.ToInt32(_credencial.ID));
                if (theme != null)
                    ThemeDTO = new ThemeDTO()
                    {
                        ID = theme.ID,
                        User = theme.User,
                        Primary = theme.Primary,
                        Secondary = theme.Secondary,
                        Tertiary = theme.Tertiary,
                        SecondaryDarkText = theme.SecondaryDarkText,
                        PrimaryDarkText = theme.PrimaryDarkText,
                        PrimaryDark = theme.PrimaryDark
                    };
                OnPropertyChanged(nameof(ThemeDTO));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task PhotoAPI()
        {
            try
            {
                var photo = await _repository.Photo(Convert.ToInt32(_credencial.ID));
                if (photo != null)
                    Photo = ImageSource.FromStream(() => new MemoryStream(photo));
                else
                    Photo = ImageSource.FromFile("Logo.png");
                OnPropertyChanged(nameof(Photo));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
