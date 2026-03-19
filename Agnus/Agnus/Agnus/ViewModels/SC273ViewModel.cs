using Agnus.Commands;
using Agnus.DTO.Computer;
using Agnus.DTO.Users;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Domus.DTO.Service_Type;
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
    public class SC273ViewModel : ViewModelBase
    {
        #region Command Return

        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SC001");
        }

        #endregion

        #region Command NewOS

        public ICommand _NewOSCommand { get; }
        public bool CanExecuteOSPatrimony(object? parameter)
        {
            return true;
        }

        public async void ExecuteOSPatrimony(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando OS...");
                var rand = new Random();
                _order.Request = _SelectedUser.ID;
                _order.Computer = _SelectedPC.ID;
                _order.Type = _SelectedType.ID;
                _order.ID = Convert.ToInt32($"{DateTime.Now:MM}{DateTime.Now:yy}{rand.Next(00000, 99999)}");
                _order.CreateBy = Convert.ToInt32(_credencial.ID);
                _order.DateCreate = DateTime.Now;
                _order.Requested_Date = DateTime.Now;
                _order.Status = false;

                var res = await _repositoryF.NewFolder(new FileFolder
                {
                    Name = $"OS {_order.ID}",
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

                _order.FileFolder = Convert.ToInt32(res.Message);

                var response = await _repository.NewOrder(_order);
                if (response.Result)
                {
                    await _page.DisplayAlert("Chamado", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SC001");
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

        public Service_Order _order { get; set; } = new Service_Order();
        private UserDTO _selectedUser { get; set; }
        public UserDTO _SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(_SelectedUser));
                ComputerAPI();
            }
        }
        private Service_Category _selectedCat { get; set; }
        public Service_Category _SelectedCat
        {
            get { return _selectedCat; }
            set
            {
                _selectedCat = value;
                OnPropertyChanged(nameof(_SelectedCat));
                TypeAPI();
            }
        }
        public TypeDTO _SelectedType { get; set; }
        public PCDTO _SelectedPC { get; set; }

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

        private ObservableCollection<TypeDTO> _type { get; set; } = new ObservableCollection<TypeDTO>();
        public ObservableCollection<TypeDTO> Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
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
        private ObservableCollection<Service_Category> _category { get; set; }
        public ObservableCollection<Service_Category> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        private ObservableCollection<PCDTO> _pc { get; set; } = new ObservableCollection<PCDTO>();
        public ObservableCollection<PCDTO> PC
        {
            get { return _pc; }
            set
            {
                _pc = value;
                OnPropertyChanged(nameof(PC));
            }
        }

        public Credencial _credencial { get; set; }

        public IService_Order _repository;
        public IPatrimony _repositoryP;
        public IFiles _repositoryF;
        public IUsers _repositoryU;

        private readonly Page _page;

        #endregion

        public SC273ViewModel(IUsers repositoryU, IService_Order repository, IPatrimony repositoryP, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryP = repositoryP;
            _repositoryF = repositoryF;
            InitializeAsync();
            CategoryAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _NewOSCommand = new BaseCommand(ExecuteOSPatrimony, CanExecuteOSPatrimony);
        }

        public SC273ViewModel(Page page, IUsers repositoryU, IService_Order repository, IPatrimony repositoryP, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryU = repositoryU;
            _repositoryP = repositoryP;
            _repositoryF = repositoryF;
            _page = page;
            InitializeAsync();
            CategoryAPI();
            UserAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _NewOSCommand = new BaseCommand(ExecuteOSPatrimony, CanExecuteOSPatrimony);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryP.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryU.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }
        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.ListAllCategory();
                Category = new ObservableCollection<Service_Category>(list.OrderBy(e => e.Name));
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
                User = new ObservableCollection<UserDTO>(list.OrderBy(e => e.Name));
                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task ComputerAPI()
        {
            try
            {
                _pc.Clear();
                var list = await _repositoryP.ComputerListByOwner(_selectedUser.ID);
                PC = new ObservableCollection<PCDTO>(list);
                OnPropertyChanged(nameof(PC));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task TypeAPI()
        {
            try
            {
                _type.Clear();
                var list = await _repository.ListAllTypeByCategory(_selectedCat.ID);
                Type = new ObservableCollection<TypeDTO>(list.OrderBy(e => e.Name));
                OnPropertyChanged(nameof(Type));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
