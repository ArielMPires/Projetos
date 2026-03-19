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
    public class SK172ViewModel : ViewModelBase
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

        #region Command EditProduct
        public ICommand _EditProductCommand { get; }
        public bool CanExecuteEditProduct(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditProduct(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Produto...");
                _Product.CreateByFK = null;
                _Product.ChangedByFK = null;
                _Product.Mark = _SelectedBrand.ID;
                _Product.Category = _SelectedCat.ID;
                _Product.ChangedBy = Convert.ToInt32(_credencial.ID);
                _Product.DateChanged = DateTime.Now;

                var response = await _repository.UpdateProduct(_Product);
                if (response.Result)
                {
                    await _page.DisplayAlert("Produto", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SK538");
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

        public IProducts _repository;

        private readonly Page _page;

        public Brands _SelectedBrand { get; set; }
        public int? _selectedBrandIndex { get; set; }
        public int? _SelectedBrandIndex
        {
            get => _selectedBrandIndex;
            set
            {
                _selectedBrandIndex = value;
                OnPropertyChanged(nameof(_SelectedBrandIndex));
            }
        }

        public Product_Category _SelectedCat { get; set; }
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

        private ObservableCollection<Product_Category> _category { get; set; }
        public ObservableCollection<Product_Category> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private ObservableCollection<Brands> _brands { get; set; }
        public ObservableCollection<Brands> Brands
        {
            get { return _brands; }
            set
            {
                _brands = value;
                OnPropertyChanged(nameof(Brands));
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

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    ProductAPI();
            }
        }

        private Products _Product { get; set; }
        public Products Product
        {
            get { return _Product; }
            set
            {
                _Product = value;
                OnPropertyChanged(nameof(Product));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    ProductAPI();
            }
        }

        #endregion

        public SK172ViewModel(Page page, string id, IProducts repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            CategoryAPI();
            BrandsAPI();
            ProductAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _EditProductCommand = new BaseCommand(ExecuteEditProduct, CanExecuteEditProduct);
        }

        public SK172ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            CategoryAPI();
            BrandsAPI();
            ProductAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            _EditProductCommand = new BaseCommand(ExecuteEditProduct, CanExecuteEditProduct);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }

        public async Task ProductAPI()
        {
            try
            {
                var list = await _repository.ProductById(Convert.ToInt32(ID));
                Product = list;
                OnPropertyChanged(nameof(Product));

                if (Product != null)
                {
                    if (Product.MarkFK != null)
                    {
                        _SelectedBrand = Product.MarkFK;
                        _selectedBrandIndex = Brands.ToList().FindIndex(e => e.ID == _SelectedBrand.ID);
                        OnPropertyChanged(nameof(_SelectedBrandIndex));
                    }
                    else
                    {
                        _selectedBrandIndex = null;
                    }
                    if (Product.CategoryFK != null)
                    {
                        _SelectedCat = Product.CategoryFK;
                        _SelectedCatIndex = Category.ToList().FindIndex(e => e.ID == _SelectedCat.ID);
                        OnPropertyChanged(nameof(_SelectedCatIndex));
                    }
                    else
                    {
                        _selectedCatIndex = null;
                    }
                }
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
                var list = await _repository.ListProductCategory();
                Category = new ObservableCollection<Product_Category>(list);
                OnPropertyChanged(nameof(Products));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }

        public async Task BrandsAPI()
        {
            try
            {
                var list = await _repository.ListBrands();
                Brands = new ObservableCollection<Brands>(list);
                OnPropertyChanged(nameof(Products));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}

