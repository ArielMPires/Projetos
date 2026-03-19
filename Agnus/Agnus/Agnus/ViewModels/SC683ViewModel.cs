using Agnus.Commands;
using Agnus.DTO.Products;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    [QueryProperty(nameof(ID), "id")]
    public class SC683ViewModel : ViewModelBase
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

        #region Command EditOS
        public ICommand _EditOSCommand { get; }
        public bool CanExecuteEditOS(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditOS(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando OS...");
                OS.TypeFK = null;
                OS.ComputerFK = null;
                OS.RequestFK = null;
                OS.TechnicalFK = null;
                OS.FileFolderFK = null;
                OS.ChangedByFK = null;
                OS.CreateByFK = null;
                OS.ChangedBy = Convert.ToInt32(_credencial.ID);
                OS.DateChanged = DateTime.Now;

                var rand = new Random();

                try
                {
                    foreach (var item in _imagesNew)
                    {
                        var result = await _repositoryF.NewFiles(new Files
                        {
                            Name = $"{rand.Next(00000, 99999)}",
                            File = item,
                            Extension = "jpg",
                            Folder = Convert.ToInt32(OS.FileFolder),
                            CreateBy = Convert.ToInt32(_credencial.ID),
                            DateCreate = DateTime.Now
                        });
                    }
                }
                catch
                {

                }
                var response = await _repository.UpdateOrder(OS);
                if (response.Result)
                {
                    await _page.DisplayAlert("Chamados", response.Message, "OK");

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

        #region Property

        public int count { get; set; }
        public Credencial _credencial { get; set; }

        public IService_Order _repository;
        public IProducts _repositoryP;
        public IFiles _repositoryF;
        public ICheckList _repositoryC;

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
                    OSAPI();
            }
        }

        private Service_Order _OS { get; set; }
        public Service_Order OS
        {
            get { return _OS; }
            set
            {
                _OS = value;
                OnPropertyChanged(nameof(OS));
            }
        }
        private ICollection<ImageSource> _images { get; set; } = [];
        public ICollection<ImageSource> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        public Service_Items NewItem { get; set; } = new Service_Items();

        private ObservableCollection<Service_Items> _items = new ObservableCollection<Service_Items>();
        public ObservableCollection<Service_Items> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public Service_Execute NewService { get; set; } = new Service_Execute();

        private ObservableCollection<Service_Execute> _Services = new ObservableCollection<Service_Execute>();
        public ObservableCollection<Service_Execute> Services
        {
            get { return _Services; }
            set
            {
                _Services = value;
                OnPropertyChanged(nameof(Services));
            }
        }

        public Service_CheckList NewCheck { get; set; } = new Service_CheckList();

        private ObservableCollection<Service_CheckList> _CheckList = new ObservableCollection<Service_CheckList>();
        public ObservableCollection<Service_CheckList> CheckList
        {
            get { return _CheckList; }
            set
            {
                _CheckList = value;
                OnPropertyChanged(nameof(CheckList));
            }
        }

        public ProductsDTO _SelectedProduct { get; set; }
        public int _IndexProduct { get; set; }
        private ObservableCollection<ProductsDTO> _Products { get; set; }
        public ObservableCollection<ProductsDTO> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public Services _SelectedService { get; set; }
        public int _IndexService { get; set; }
        private ObservableCollection<Services> _Service { get; set; }
        public ObservableCollection<Services> Service
        {
            get { return _Service; }
            set
            {
                _Service = value;
                OnPropertyChanged(nameof(Service));
            }
        }

        public Checklist _SelectedCheck { get; set; }
        public int _IndexCheck { get; set; }
        private ObservableCollection<Checklist> _CheckLists { get; set; }
        public ObservableCollection<Checklist> CheckLists
        {
            get { return _CheckLists; }
            set
            {
                _CheckLists = value;
                OnPropertyChanged(nameof(CheckLists));
            }
        }

        private ObservableCollection<byte[]> _imagesNew = new ObservableCollection<byte[]>();

        public ObservableCollection<byte[]> ImagesNew
        {
            get => _imagesNew;
            set
            {
                _imagesNew = value;
                OnPropertyChanged(nameof(ImagesNew));
            }
        }
        #endregion

        #region Commands ADDItem
        public ICommand _commandADDItem { get; set; }

        public bool CanExecuteADDItem(object? parameter)
        {
            return true;
        }

        public async void ExecuteADDItem(object? parameter)
        {
            NewItem.Product = _SelectedProduct.ID;
            NewItem.ProductFK = null;
            var search = await _repository.ListAllItemsByOrder(OS.ID);
            var result = search.Find(e => e.Product == NewItem.Product);
            if (result == null)
            {
                NewItem.Order = OS.ID;
                NewItem.Product = _SelectedProduct.ID;
                await _repository.NewItemOrder(NewItem);
            }
            else
            {
                result.Amount = result.Amount + NewItem.Amount;
                await _repository.UpdateItem(result);
            }
            NewItem = new Service_Items();
            await _page.DisplayAlert("Item", "Item Adicionado com Sucesso!", "OK");
            _IndexProduct = -1;
            Items = new ObservableCollection<Service_Items>(await _repository.ListAllItemsByOrder(OS.ID));
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(NewItem));
            OnPropertyChanged(nameof(_IndexProduct));
            AbaSelecionada = "Information";
        }
        #endregion

        #region Commands DeleteItem
        public ICommand _commandDelItem { get; set; }

        public bool CanExecuteDelItem(object? parameter)
        {
            return true;
        }

        public async void ExecuteDelItem(object? parameter)
        {
            var product = (int)parameter;
            _repository.DeleteItem(product);
            await _page.DisplayAlert("Item", "Item Removido com Sucesso!", "OK");
            Items = new ObservableCollection<Service_Items>(await _repository.ListAllItemsByOrder(OS.ID));
            OnPropertyChanged(nameof(Items));
        }
        #endregion

        #region Commands ADDService
        public ICommand _commandADDService { get; set; }

        public bool CanExecuteADDService(object? parameter)
        {
            return true;
        }

        public async void ExecuteADDService(object? parameter)
        {
            NewService.Service = _SelectedService.ID;
            var search = await _repository.ListAllServiceByOrder(OS.ID);
            var result = search.Find(e => e.Service == NewService.Service);
            if (result == null)
            {
                NewService.Order = OS.ID;
                await _repository.NewExecute(NewService);
                await _page.DisplayAlert("Serviços", "Serviço Adicionado com Sucesso!", "OK");
            }
            else
            {
                await _page.DisplayAlert("Serviços", "Serviço já foi adicionado!", "OK");
            }
            NewService = new Service_Execute();
            _IndexCheck = -1;
            Services = new ObservableCollection<Service_Execute>(await _repository.ListAllServiceByOrder(OS.ID));
            OnPropertyChanged(nameof(Services));
            OnPropertyChanged(nameof(NewService));
            OnPropertyChanged(nameof(_IndexService));
            AbaSelecionada = "Services";
        }
        #endregion

        #region Commands DeleteService
        public ICommand _commandDelService { get; set; }

        public bool CanExecuteDelService(object? parameter)
        {
            return true;
        }

        public async void ExecuteDelService(object? parameter)
        {
            var id = (int)parameter;
            _repository.DeleteExecute(id);
            await _page.DisplayAlert("Serviços", "Serviço Removido com Sucesso!", "OK");
            Services = new ObservableCollection<Service_Execute>(await _repository.ListAllServiceByOrder(OS.ID));
            OnPropertyChanged(nameof(Services));
        }
        #endregion

        #region Commands ADDCheck
        public ICommand _commandADDCheck { get; set; }

        public bool CanExecuteADDCheck(object? parameter)
        {
            return true;
        }

        public async void ExecuteADDCheck(object? parameter)
        {
            NewCheck.Checklist = _SelectedCheck.ID;
            var search = await _repository.ListAllCheckByOrder(OS.ID);
            var result = search.Find(e => e.Checklist == NewCheck.Checklist);
            if (result == null)
            {
                NewCheck.Checked = false;
                NewCheck.Order = OS.ID;
                NewCheck.CreateBy = Convert.ToInt32(_credencial.ID);
                NewCheck.DateCreate = DateTime.Now;
                await _repository.NewChecklist(NewCheck);
                await _page.DisplayAlert("CheckList", "Checklist Adicionado com Sucesso!", "OK");
            }
            else
            {
                await _page.DisplayAlert("CheckList", "CheckList Já Ta Adicionado!", "OK");
            }
            NewCheck = new Service_CheckList();
            _IndexCheck = -1;
            CheckList = new ObservableCollection<Service_CheckList>(await _repository.ListAllCheckByOrder(OS.ID));
            OnPropertyChanged(nameof(CheckList));
            OnPropertyChanged(nameof(NewCheck));
            OnPropertyChanged(nameof(_IndexCheck));
            AbaSelecionada = "CheckList";

        }
        #endregion

        #region Commands DeleteCheck
        public ICommand _commandDelCheck { get; set; }

        public bool CanExecuteDelCheck(object? parameter)
        {
            return true;
        }

        public async void ExecuteDelCheck(object? parameter)
        {

            var id = (int)parameter;
            _repository.DeleteChecklist(id);
            await _page.DisplayAlert("CheckList", "CheckList Removido com Sucesso!", "OK");
            CheckList = new ObservableCollection<Service_CheckList>(await _repository.ListAllCheckByOrder(OS.ID));
            OnPropertyChanged(nameof(CheckList));

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
                        ImagesNew.Add(memoryStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }
        #endregion

        #region Commands Checked
        public ICommand _commandChecked { get; set; }

        public bool CanExecuteChecked(object? parameter)
        {
            return true;
        }

        public async void ExecuteChecked(object? parameter)
        {
            var id = (int)parameter;
            string result = await _page.DisplayPromptAsync("Observação", "Digite Observação(OPT)");
            var checklist = CheckList.FirstOrDefault(e => e.ID == id);
            checklist.Checked = true;
            checklist.ChangedBy = Convert.ToInt32(_credencial.ID);
            checklist.DateChanged = DateTime.Now;
            if (!String.IsNullOrEmpty(result))
                checklist.Note = result;

            var response = await _repository.UpdateChecklist(checklist);
            if (response.Result)
            {
                await _page.DisplayAlert("Checklist", response.Message, "OK");
                CheckList = new ObservableCollection<Service_CheckList>(await _repository.ListAllCheckByOrder(OS.ID));
                OnPropertyChanged(nameof(CheckList));
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }

        }
        #endregion

        #region ABAS

        private string _abaSelecionada;
        public string AbaSelecionada
        {
            get => _abaSelecionada;
            set
            {
                _abaSelecionada = value;
                OnPropertyChanged(nameof(AbaSelecionada));
                OnPropertyChanged(nameof(IsImages));
                OnPropertyChanged(nameof(IsInfo));
                OnPropertyChanged(nameof(IsItem));
                OnPropertyChanged(nameof(IsItemADD));
                OnPropertyChanged(nameof(IsService));
                OnPropertyChanged(nameof(IsServiceADD));
                OnPropertyChanged(nameof(IsCheckList));
                OnPropertyChanged(nameof(IsCheckListADD));
            }
        }

        public bool IsInfo => AbaSelecionada == "Information";
        public bool IsItem => AbaSelecionada == "Item";
        public bool IsItemADD => AbaSelecionada == "ItemAdd";
        public bool IsService => AbaSelecionada == "Services";
        public bool IsServiceADD => AbaSelecionada == "ServiceAdd";
        public bool IsCheckList => AbaSelecionada == "CheckList";
        public bool IsCheckListADD => AbaSelecionada == "CheckListAdd";
        public bool IsImages => AbaSelecionada == "Images";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SC683ViewModel(Page page, string id, IService_Order repository, IProducts repositoryP, IFiles repositoryF, ICheckList repositoryC)
        {
            _repository = repository;
            _repositoryP = repositoryP;
            _repositoryF = repositoryF;
            _repositoryC = repositoryC;
            _page = page;
            ID = id;
            AbaSelecionada = "Information";
            InitializeAsync();
            OSAPI();
            ProductAPI();
            ServicesAPI();
            CheckListAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditOSCommand = new BaseCommand(ExecuteEditOS, CanExecuteEditOS);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
            _commandADDService = new BaseCommand(ExecuteADDService, CanExecuteADDService);
            _commandDelService = new BaseCommand(ExecuteDelService, CanExecuteDelService);
            _commandADDCheck = new BaseCommand(ExecuteADDCheck, CanExecuteADDCheck);
            _commandDelCheck = new BaseCommand(ExecuteDelCheck, CanExecuteDelCheck);
            _commandChecked = new BaseCommand(ExecuteChecked, CanExecuteChecked);
        }

        public SC683ViewModel(IService_Order repository, IProducts repositoryP, IFiles repositoryF, ICheckList repositoryC)
        {
            _repository = repository;
            _repositoryP = repositoryP;
            _repositoryF = repositoryF;
            _repositoryC = repositoryC;
            AbaSelecionada = "Information";
            InitializeAsync();
            OSAPI();
            ProductAPI();
            ServicesAPI();
            CheckListAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditOSCommand = new BaseCommand(ExecuteEditOS, CanExecuteEditOS);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
            _commandADDService = new BaseCommand(ExecuteADDService, CanExecuteADDService);
            _commandDelService = new BaseCommand(ExecuteDelService, CanExecuteDelService);
            _commandADDCheck = new BaseCommand(ExecuteADDCheck, CanExecuteADDCheck);
            _commandDelCheck = new BaseCommand(ExecuteDelCheck, CanExecuteDelCheck);
            _commandChecked = new BaseCommand(ExecuteChecked, CanExecuteChecked);
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
                _repositoryC.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }

        public async Task OSAPI()
        {
            try
            {
                var list = await _repository.Order(Convert.ToInt32(ID));
                OS = list;

                if (_OS != null)
                {
                    var lista = await _repository.ListAllItemsByOrder(OS.ID);
                    Items = new ObservableCollection<Service_Items>(lista);
                    var listS = await _repository.ListAllServiceByOrder(OS.ID);
                    Services = new ObservableCollection<Service_Execute>(listS);
                    var listC = await _repository.ListAllCheckByOrder(OS.ID);
                    CheckList = new ObservableCollection<Service_CheckList>(listC);
                    OnPropertyChanged(nameof(OS));
                    OnPropertyChanged(nameof(Items));
                    OnPropertyChanged(nameof(Services));
                    OnPropertyChanged(nameof(CheckList));

                    var files = await _repositoryF.FilesList();
                    var files2 = files.Where(e => e.Folder == OS.FileFolder).ToList();
                    foreach (var item in files2)
                    {
                        Images.Add(ImageSource.FromStream(() => new MemoryStream(item.File)));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task ProductAPI()
        {
            try
            {
                var list = await _repositoryP.ListProduct();
                Products = new ObservableCollection<ProductsDTO>(list);
                OnPropertyChanged(nameof(Products));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task ServicesAPI()
        {
            try
            {
                var list = await _repository.ListAllService();
                Service = new ObservableCollection<Services>(list);
                OnPropertyChanged(nameof(Service));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task CheckListAPI()
        {
            try
            {
                var list = await _repositoryC.List();
                CheckLists = new ObservableCollection<Checklist>(list);
                OnPropertyChanged(nameof(CheckLists));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
