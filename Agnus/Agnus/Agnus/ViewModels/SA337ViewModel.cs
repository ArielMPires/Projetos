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
    [QueryProperty(nameof(ID), "id")]
    public class SA337ViewModel : ViewModelBase
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

        #region Command EditPatrimony

        public ICommand _EditPatrimonyCommand { get; }
        public bool CanExecuteEditPatrimony(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditPatrimony(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Patrimonio...");

                var rand = new Random();
                _Patrimony.Category = _SelectedCat.ID;
                _Patrimony.Department = _SelectedDept.ID;
                _Patrimony.Current_Owner = _SelectedUser.ID;
                _Patrimony.CategoryFK = null;
                _Patrimony.DepartmentFK = null;
                _Patrimony.Current_OwnerFK = null;
                _Patrimony.CreateByFK = null;
                _Patrimony.ChangedByFK = null;
                _Patrimony.ChangedBy = Convert.ToInt32(_credencial.ID);
                _Patrimony.DateChanged = DateTime.Now;

                try
                {
                    foreach (var item in _images)
                    {
                        var result = await _repositoryF.NewFiles(new Files
                        {
                            Name = $"{rand.Next(00000, 99999)}",
                            File = item,
                            Extension = "jpg",
                            Folder = _Patrimony.FileFolder,
                            CreateBy = Convert.ToInt32(_credencial.ID),
                            DateCreate = DateTime.Now
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                var response = await _repository.UpdatePatrimony(_Patrimony);
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

        #region Property

        public int count { get; set; }
        public Credencial _credencial { get; set; }
        public UserDTO _SelectedUser { get; set; }
        public int? _selectedUserIndex { get; set; }
        public int? _SelectedUserIndex
        {
            get => _selectedUserIndex;
            set
            {
                _selectedUserIndex = value;
                OnPropertyChanged(nameof(_SelectedUserIndex));
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

        public Patrimony_Category _SelectedCat { get; set; }
        public int? _selectedCatIndex { get; set; }
        public int? _SelectedCatIndex
        {
            get => _selectedCatIndex;
            set
            {
                _selectedCatIndex = value;
                OnPropertyChanged(nameof(_SelectedCatIndex));
            }
        }

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

        public IPatrimony _repository;
        public IUsers _repositoryU;
        public IFiles _repositoryF;

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
                    PatrimonyAPI();
            }
        }

        private Patrimony _Patrimony { get; set; }
        public Patrimony Patrimony
        {
            get { return _Patrimony; }
            set
            {
                _Patrimony = value;
                OnPropertyChanged(nameof(Patrimony));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    PatrimonyAPI();
            }
        }

        private ObservableCollection<UserDTO> _user { get; set; }
        public ObservableCollection<UserDTO> User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
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

        #endregion

        public SA337ViewModel(Page page, string id, IUsers repositoryU, IPatrimony repository, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryF = repositoryF;
            _page = page;
            ID = id;
            InitializeAsync();
            CategoryAPI();
            DepartmentAPI();
            UsersAPI();
            PatrimonyAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _EditPatrimonyCommand = new BaseCommand(ExecuteEditPatrimony, CanExecuteEditPatrimony);
        }

        public SA337ViewModel(IUsers repositoryU, IPatrimony repository, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryF = repositoryF;
            InitializeAsync();
            CategoryAPI();
            DepartmentAPI();
            UsersAPI();
            PatrimonyAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _EditPatrimonyCommand = new BaseCommand(ExecuteEditPatrimony, CanExecuteEditPatrimony);
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

        public async Task PatrimonyAPI()
        {
            try
            {
                var _patrimony = await _repository.Patrimony_SearchBy(Convert.ToInt32(ID));
                Patrimony = _patrimony;
                OnPropertyChanged(nameof(Patrimony));

                if (Patrimony != null)
                {
                    if (Patrimony.DepartmentFK != null)
                    {
                        _SelectedDept = Patrimony.DepartmentFK;
                        _SelectedDeptIndex = Departments.ToList().FindIndex(e => e.ID == _SelectedDept.ID);
                    }
                    else
                    {
                        _selectedDeptIndex = null;
                    }
                    if (Patrimony.CategoryFK != null)
                    {
                        _SelectedCat = Patrimony.CategoryFK;
                        _SelectedCatIndex = Category.ToList().FindIndex(e => e.ID == _SelectedCat.ID);
                    }
                    else
                    {
                        _selectedCatIndex = null;
                    }
                    if (Patrimony.Current_OwnerFK != null)
                    {
                        _SelectedUser = User.FirstOrDefault( e => e.ID == Patrimony.Current_OwnerFK.ID);
                        _SelectedUserIndex = User.ToList().FindIndex(e => e.ID == _SelectedUser.ID);
                    }
                    else
                    {
                        _selectedUserIndex = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public async Task UsersAPI()
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

        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.CategoryList();
                Category = new ObservableCollection<Patrimony_Category>(list);
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
