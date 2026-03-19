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
    public class SA264ViewModel : ViewModelBase
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

        #region Command NewPatrimony
        public ICommand _NewPatrimonyCommand { get; }
        public bool CanExecuteNewPatrimony(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewPatrimony(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Patrimonio...");
                var rand = new Random();
                _patrimony.Current_Owner = _SelectedUser.ID;
                _patrimony.Department = _SelectedDept.ID;
                _patrimony.Category = _SelectedCat.ID;
                _patrimony.ID = Convert.ToInt32($"{DateTime.Now:ff}{DateTime.Now:ss}{rand.Next(00000, 99999)}");
                _patrimony.CreateBy = Convert.ToInt32(_credencial.ID);
                _patrimony.DateCreate = DateTime.Now;

                var res = await _repositoryF.NewFolder(new FileFolder
                {
                    Name = $"Patrimonio {_patrimony.ID}",
                    CreateBy = Convert.ToInt32(_credencial.ID),
                    DateCreate = DateTime.Now
                });
                if (res.Result)
                {
                    try
                    {
                        foreach (var item in _images)
                        {
                            var result = await _repositoryF.NewFiles(new Files
                            {
                                Name = $"{rand.Next(00000, 99999)}",
                                File = item,
                                Extension = "jpg",
                                Folder = Convert.ToInt32(res.Message),
                                CreateBy = Convert.ToInt32(_credencial.ID),
                                DateCreate = DateTime.Now
                            });
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    await _page.DisplayAlert(res.Message, res.Error, "OK");
                }

                _patrimony.FileFolder = Convert.ToInt32(res.Message);

                var response = await _repository.NewPatrimony(_patrimony);
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
                        await Shell.Current.GoToAsync("///SA961");
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

        #region Command SelectImages

        public ICommand _SelectImagesCommand { get; }
        public bool CanExecuteSelectImages(object? parameter)
        {
            return true;
        }

        public async void ExecuteSelectImages(object? parameter)
        {
            try
            {
                var result = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Selecione as imagens",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    count = count + result.Count();
                    OnPropertyChanged(nameof(count));
                    foreach (var file in result)
                    {
                        using var stream = await file.OpenReadAsync();
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        Images.Add(memoryStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }
        #endregion

        #region Property

        public Patrimony _patrimony { get; set; } = new Patrimony();
        public UserDTO _SelectedUser { get; set; }
        public Department _SelectedDept { get; set; }
        public Patrimony_Category _SelectedCat { get; set; }

        public int count { get; set; }

        private ObservableCollection<byte[]> _images = new ObservableCollection<byte[]>();

        public ObservableCollection<byte[]> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
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
        private ObservableCollection<UserDTO> _users { get; set; }
        public ObservableCollection<UserDTO> User
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private ObservableCollection<Patrimony_Category> _category { get; set; }
        public ObservableCollection<Patrimony_Category> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public Credencial _credencial { get; set; }

        public IPatrimony _repository;
        public IUsers _repositoryU;
        public IFiles _repositoryF;

        private readonly Page _page;

        #endregion

        public SA264ViewModel(IUsers repositoryU, IPatrimony repository,IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryF = repositoryF;
            InitializeAsync();
            CategoryAPI();
            UserAPI();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _NewPatrimonyCommand = new BaseCommand(ExecuteNewPatrimony, CanExecuteNewPatrimony);
        }

        public SA264ViewModel(Page page, IUsers repositoryU, IPatrimony repository, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryF = repositoryF;
            _page = page;
            InitializeAsync();
            CategoryAPI();
            UserAPI();
            DepartmentAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _NewPatrimonyCommand = new BaseCommand(ExecuteNewPatrimony, CanExecuteNewPatrimony);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryU.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }
        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.CategoryList();
                Category = new ObservableCollection<Patrimony_Category>(list);
                OnPropertyChanged(nameof(Category));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task UserAPI()
        {
            try
            {
                var list = await _repositoryU.ListUsers();
                User = new ObservableCollection<UserDTO>(list);
                OnPropertyChanged(nameof(User));
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
                var list = await _repositoryU.ListDepartments();
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
