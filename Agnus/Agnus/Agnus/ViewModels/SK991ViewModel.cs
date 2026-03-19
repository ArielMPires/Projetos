using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces.Repositories;
using Agnus.Interfaces;
using Agnus.Models.DB;
using Agnus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Agnus.ViewModels
{
    public class SK991ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SA803");
        }

        #endregion

        #region Command NewProduct
        public ICommand _NewProductCommand { get; }
        public bool CanExecuteNewProduct(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewProduct(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Produto...");
                var rand = new Random();
                _product.ID = Convert.ToInt32($"{DateTime.Now:ffff}{rand.Next(00000, 99999)}");
                _product.CreateBy = Convert.ToInt32(_credencial.ID);
                _product.DateCreate = DateTime.Now;
                _product.Mark = _Selectbrand.ID;
                _product.Category = _SelectCategory.ID;
                var idpaste = $"{rand.Next(000000, 999999)}";

                var res = await _repositoryF.NewFolderAllTenant(new FileFolder
                {
                    ID = Convert.ToInt32(idpaste),
                    Name = $"Produto {_product.ID}",
                    CreateBy = Convert.ToInt32(_credencial.ID),
                    DateCreate = DateTime.Now
                });
                if (res.Result)
                {
                    try
                    {
                        foreach (var item in _images)
                        {
                            var result = await _repositoryF.NewFilesAllTenant(new Files
                            {
                                Name = $"{rand.Next(00000, 99999)}",
                                File = item,
                                Extension = "jpg",
                                Folder = Convert.ToInt32(idpaste),
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

                _product.FolderPicture = Convert.ToInt32(idpaste);


                var response = await _repository.NewProduct(_product);
                if (response.Result)
                {
                    await _page.DisplayAlert("Produtos", response.Message, "OK");

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

        public int count { get; set; }
        public Products _product { get; set; } = new Products();
        public Brands _Selectbrand { get; set; }
        public Product_Category _SelectCategory { get; set; }
        public Credencial _credencial { get; set; }

        public IProducts _repository;
        public IFiles _repositoryF;

        private readonly Page _page;

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

        #endregion

        public SK991ViewModel(IProducts repository, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryF = repositoryF;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewProductCommand = new BaseCommand(ExecuteNewProduct, CanExecuteNewProduct);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            CategoryAPI();
            BrandsAPI();
        }

        public SK991ViewModel(Page page, IProducts repository, IFiles repositoryF)
        {
            _repository = repository;
            _repositoryF = repositoryF;
            _page = page;
            InitializeAsync();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewProductCommand = new BaseCommand(ExecuteNewProduct, CanExecuteNewProduct);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
            CategoryAPI();
            BrandsAPI();
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
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
