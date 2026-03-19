using Agnus.Commands;
using Agnus.DTO.Products;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
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
    public class SF136ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SD001");
        }
        #endregion

        #region Property
        public int count { get; set; }
        public Credencial _credencial { get; set; }
        public INF_Input _repository;
        public IProducts _repositoryP;
        public IFiles _repositoryF;
        private HubConnection _hubConnection;
        private readonly Page _page;

        public NF_Input Input { get; set; } = new NF_Input();

        public NF_Input_Items NewItem { get; set; } = new NF_Input_Items();
        private ObservableCollection<NF_Input_Items> _items = new ObservableCollection<NF_Input_Items>();
        public ObservableCollection<NF_Input_Items> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
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

        public Suppliers _SelectedSupplier { get; set; }
        private ObservableCollection<Suppliers> _supplier { get; set; }
        public ObservableCollection<Suppliers> Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
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
            NewItem.ProductFK = new Products() { ID = _SelectedProduct.ID, Description = _SelectedProduct.Description};
            var search = Items.FirstOrDefault(e => e.Product == NewItem.Product);
            if (search == null)
            {
                Items.Add(NewItem);
            }
            else
            {
                search.Amount = search.Amount + NewItem.Amount;
                Items.FirstOrDefault(e => e.Product == NewItem.Product).Amount = search.Amount;
            }
            var value = NewItem.Purchase_Value * Convert.ToDecimal(NewItem.Amount);
            Input.Total_Value = Input.Total_Value + value;
            NewItem = new NF_Input_Items();
            await _page.DisplayAlert("Item", "Item Adicionado com Sucesso!", "OK");
            _IndexProduct = -1;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Input));
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
            var item = Items.FirstOrDefault(e => e.Product == product);
            var value = item.Purchase_Value * Convert.ToDecimal(item.Amount);
            Input.Total_Value = Input.Total_Value - value;
            Items.Remove(item);
            await _page.DisplayAlert("Item", "Item Removido com Sucesso!", "OK");
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Input));
        }
        #endregion

        #region Commands ADDNF
        public ICommand _commandAddNF { get; set; }

        public bool CanExecuteAddNF(object? parameter)
        {
            return true;
        }

        public async void ExecuteAddNF(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando NF...");
                var rand = new Random();
            Input.DateIn = DateTime.Now;
            Input.DateCreate = DateTime.Now;
            Input.CreateBy = Convert.ToInt32(_credencial.ID);
            foreach (var item in Items)
            {
                item.NF = Input.Numero;
            }
            Input.NF_Input_ItemsFK = Items;
            Input.Supplier = _SelectedSupplier.ID;

            var res = await _repositoryF.NewFolder(new FileFolder
            {
                Name = $"NF {Input.Numero}",
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
                            Extension = "pdf",
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

            Input.FileFolder = Convert.ToInt32(res.Message);

            var response = await _repository.InputNew(Input);
            if (response.Result)
            {
                await _page.DisplayAlert("NF", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SF172");
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
                    PickerTitle = "Selecione o Pdf",
                    FileTypes = FilePickerFileType.Pdf
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
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar Pdf: {ex.Message}", "OK");
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
                OnPropertyChanged(nameof(IsInfo));
                OnPropertyChanged(nameof(IsItem));
                OnPropertyChanged(nameof(IsItemADD));
            }
        }

        public bool IsInfo => AbaSelecionada == "Information";
        public bool IsItem => AbaSelecionada == "Item";
        public bool IsItemADD => AbaSelecionada == "ItemAdd";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SF136ViewModel(INF_Input repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryP = repositoryP;
            _repositoryF = repositoryF;
            AbaSelecionada = "Information";
            InitializeAsync();
            ProductAPI();
            SupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandAddNF = new BaseCommand(ExecuteAddNF, CanExecuteAddNF);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
        }
        public SF136ViewModel(Page page, INF_Input repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryF = repositoryF;
            _page = page;
            AbaSelecionada = "Information";
            InitializeAsync();
            ProductAPI();
            SupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandAddNF = new BaseCommand(ExecuteAddNF, CanExecuteAddNF);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
        }
        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryP.SetHeader(_credencial?.tenantId, _credencial.Token);
            }

            _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
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
        public async Task SupplierAPI()
        {
            try
            {
                var list = await _repositoryP.ListSupplier();
                Supplier = new ObservableCollection<Suppliers>(list);
                OnPropertyChanged(nameof(Supplier));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }

    }
}
