using Newtonsoft.Json;
using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
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
    public class SH492ViewModel : ViewModelBase
    {
        #region Command Return

        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SH261");
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

        #region Command EditOrder

        public ICommand _EditRequestCommand { get; }
        public bool CanExecuteRequest(object? parameter)
        {
            return true;
        }

        public async void ExecuteRequest(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Pedido de Compra...");
                _order.ChangedBy = Convert.ToInt32(_credencial.ID);
                _order.DateChanged = DateTime.Now;

                var response = await _repository.UpdateOrder(_order);
                if (response.Result)
                {
                    await _page.DisplayAlert("Pedido de Compra", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SH261");
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

        #region Commands EntryNF
        public ICommand _commandEntry { get; set; }

        public bool CanExecuteEntry(object? parameter)
        {
            return true;
        }

        public async void ExecuteEntry(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando NF...");
                var rand = new Random();
                NF.DateIn = DateTime.Now;
                NF.DateCreate = DateTime.Now;
                NF.CreateBy = Convert.ToInt32(_credencial.ID);
                _ItemsSelected = new ObservableCollection<CheckBoxGeneric<Request_Items>>(Items.Where(e => e.IsChecked == true));
                foreach (var item in _ItemsSelected)
                {
                    NFItems.Add(new NF_Input_Items { NF = NF.Numero, Product = item.value.Product, Amount = item.value.Amount, Purchase_Value = item.value.Unit_Value });
                    var total = item.value.Unit_Value * Convert.ToDecimal(item.value.Amount);
                    NF.Total_Value = NF.Total_Value + total;
                }
                NF.NF_Input_ItemsFK = NFItems;
                NF.Supplier = _SelectedSupplier.ID;

                var res = await _repositoryF.NewFolder(new FileFolder
                {
                    Name = $"NF {NF.Numero}",
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

                NF.FileFolder = Convert.ToInt32(res.Message);

                var response = await _repositoryN.InputNew(NF);
                if (response.Result)
                {

                    foreach (var item in _ItemsSelected)
                    {
                        item.value.purchase = true;
                        var api = await _repository.UpdateItems(item.value);
                    }

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

        #region Property

        public int count { get; set; }
        public Request Request { get; set; }
        private Purchase_Order _order { get; set; }
        public Purchase_Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged(nameof(Order));
            }
        }
        public ObservableCollection<CheckBoxGeneric<Request_Items>> Items { get; set; }
        public ObservableCollection<CheckBoxGeneric<Request_Items>> _ItemsSelected { get; set; }
        public Suppliers _SelectedSupplier { get; set; }
        public NF_Input NF { get; set; } = new NF_Input();
        public ObservableCollection<NF_Input_Items> NFItems { get; set; } = new ObservableCollection<NF_Input_Items>();
        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    OrderAPI();
            }
        }
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

        public Credencial _credencial { get; set; }

        public IRequest _repository;
        public INF_Input _repositoryN;
        public IFiles _repositoryF;
        public IProducts _repositoryP;

        private readonly Page _page;

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
                OnPropertyChanged(nameof(IsInfoAdd));
                OnPropertyChanged(nameof(IsApproved));
            }
        }

        public bool IsInfo => AbaSelecionada == "Information";
        public bool IsInfoAdd => AbaSelecionada == "InfoAdd";
        public bool IsApproved => AbaSelecionada == "Approved";

        public ICommand TrocarAbaCommand { get; }

        private void TrocarAba(string aba)
        {
            AbaSelecionada = aba;
        }

        #endregion

        public SH492ViewModel(INF_Input repositoryN, IRequest repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryN = repositoryN;
            _repositoryF = repositoryF;
            AbaSelecionada = "Information";
            InitializeAsync();
            OrderAPI();
            SupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditRequestCommand = new BaseCommand(ExecuteRequest, CanExecuteRequest);
            _commandEntry = new BaseCommand(ExecuteEntry, CanExecuteEntry);
        }

        public SH492ViewModel(Page page, string id, INF_Input repositoryN, IRequest repository, IProducts repositoryP, IFiles repositoryF)
        {
            _repositoryP = repositoryP;
            _repository = repository;
            _repositoryN = repositoryN;
            _repositoryF = repositoryF;
            _page = page;
            ID = id;
            AbaSelecionada = "Information";
            InitializeAsync();
            OrderAPI();
            SupplierAPI();
            TrocarAbaCommand = new Command<string>(TrocarAba);
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditRequestCommand = new BaseCommand(ExecuteRequest, CanExecuteRequest);
            _commandEntry = new BaseCommand(ExecuteEntry, CanExecuteEntry);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repositoryN.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryP.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
                _repositoryF.SetHeader(_credencial?.tenantId, _credencial.Token);
            }
        }
        public async Task OrderAPI()
        {
            try
            {
                Order = await _repository.Purchase_SearchByPc(Convert.ToInt32(ID));
                if (Order != null)
                {
                    Request = Order.RequestFK;
                    var items = await _repository.ItemsList();
                    Request.ItemsFK = new ObservableCollection<Request_Items>(items.Where(e => e.Request == Request.ID && e.purchase == false));
                    ObservableCollection<CheckBoxGeneric<Request_Items>> api = new ObservableCollection<CheckBoxGeneric<Request_Items>>();
                    if (Request.ItemsFK != null)
                    {
                        foreach (var item in Request.ItemsFK)
                        {
                            bool Checked;
                            if (item.purchase)
                                Checked = true;
                            else
                                Checked = false;

                            var lista = new CheckBoxGeneric<Request_Items>
                            {
                                IsChecked = Checked,
                                value = item
                            };

                            api.Add(lista);
                        }
                    }
                    Items = new ObservableCollection<CheckBoxGeneric<Request_Items>>(api);
                    OnPropertyChanged(nameof(Request));
                    OnPropertyChanged(nameof(Items));
                }
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
