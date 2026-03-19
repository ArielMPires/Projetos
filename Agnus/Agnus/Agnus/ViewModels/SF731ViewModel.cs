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
    public class SF731ViewModel : ViewModelBase
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

        public NF_Output Output { get; set; } = new NF_Output();

        public NF_Output_Items NewItem { get; set; } = new NF_Output_Items();
        private ObservableCollection<NF_Output_Items> _items = new ObservableCollection<NF_Output_Items>();
        public ObservableCollection<NF_Output_Items> Items
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
            NewItem.ProductFK = new Products() { ID = _SelectedProduct.ID, Description = _SelectedProduct.Description };
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
            var value = NewItem.Usage_Value * Convert.ToDecimal(NewItem.Amount);
            Output.Total_Value = Output.Total_Value + value;
            NewItem = new NF_Output_Items();
            await _page.DisplayAlert("Item", "Item Adicionado com Sucesso!", "OK");
            _IndexProduct = -1;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Output));
            OnPropertyChanged(nameof(NewItem));
            OnPropertyChanged(nameof(_IndexProduct));
            AbaSelecionada = "Item";
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
            var value = item.Usage_Value * Convert.ToDecimal(item.Amount);
            Output.Total_Value = Output.Total_Value - value;
            Items.Remove(item);
            await _page.DisplayAlert("Item", "Item Removido com Sucesso!", "OK");
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Output));
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
                Output.DateOut = DateTime.Now;
                Output.DateCreate = DateTime.Now;
                Output.CreateBy = Convert.ToInt32(_credencial.ID);
                foreach (var item in Items)
                {
                    item.NF = Output.ID;
                    item.ProductFK = null;
                }
                Output.NF_Output_ItemsFK = Items;

                var res = await _repositoryF.NewFolder(new FileFolder
                {
                    Name = $"NF Saida",
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

                Output.FileFolder = Convert.ToInt32(res.Message);

                var response = await _repository.OutputNew(Output);
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
                    PickerTitle = "Selecione o PDF",
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
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
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
                OnPropertyChanged(nameof(IsItem));
                OnPropertyChanged(nameof(IsItemADD));
            }
        }

        public bool IsItem => AbaSelecionada == "Item";
        public bool IsItemADD => AbaSelecionada == "ItemAdd";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SF731ViewModel(INF_Input repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryF = repositoryF;
            AbaSelecionada = "Item";
            InitializeAsync();
            ProductAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandAddNF = new BaseCommand(ExecuteAddNF, CanExecuteAddNF);
            _commandADDItem = new BaseCommand(ExecuteADDItem, CanExecuteADDItem);
            _commandDelItem = new BaseCommand(ExecuteDelItem, CanExecuteDelItem);
            _SelectImagesCommand = new BaseCommand(ExecuteSelectImages, CanExecuteSelectImages);
        }
        public SF731ViewModel(Page page, INF_Input repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryF = repositoryF;
            _page = page;
            AbaSelecionada = "Item";
            InitializeAsync();
            ProductAPI();
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
                _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
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
    }
}
