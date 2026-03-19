using Agnus.Commands;
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
    public class SK472ViewModel : ViewModelBase
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
        private readonly Page _page;
        public Credencial _credencial { get; set; }
        public IProducts _repository;
        private HubConnection _hubConnection;
        public ICommand ToggleExpandCommand { get; }
        private ObservableCollection<CheckBoxGeneric<Product_Category>> _category { get; set; }
        public ObservableCollection<CheckBoxGeneric<Product_Category>> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        private ObservableCollection<CheckBoxGeneric<Product_Category>> _Filter { get; set; }
        public ObservableCollection<CheckBoxGeneric<Product_Category>> Filter
        {
            get { return _Filter; }
            set
            {
                _Filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }


        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                OnPropertyChanged(nameof(Search));
                ApplyFilter();
            }
        }

        #endregion

        #region Commands SK918
        public ICommand _commandSK918 { get; set; }

        public bool CanExecuteSK918(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK918"))
                return true;
            else return false;
        }

        public async void ExecuteSK918(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Categoria", "Digite o Nome da Categoria de Produto");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Categoria", "Obrigatorio colocar o Nome da Categoria", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Categoria de Produto...");
                    var category = new Product_Category()
                    {
                        Name = result,
                        CreateBy = Convert.ToInt32(_credencial.ID),
                        DateCreate = DateTime.Now
                    };
                    var response = await _repository.NewProductCategory(category);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Categoria", response.Message, "OK");

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

        }
        #endregion

        #region Commands SK522
        public ICommand _commandSK522 { get; set; }

        public bool CanExecuteSK522(object? parameter)
        {
            if (_credencial.Permissions.Any(e => e.Page == "SK522"))
                return true;
            else return false;
        }

        public async void ExecuteSK522(object? parameter)
        {
            var id = (int)parameter;
            var category = new Product_Category();
            category = await _repository.ProductCategoryById(id);

            string result = await _page.DisplayPromptAsync("Categoria", "Digite o Nome da Categoria de Produto", initialValue: category.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Categoria", "Obrigatorio colocar o Nome Categoria de Produto", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Categoria de Produto...");
                    category.Name = result;
                    category.ChangedBy = Convert.ToInt32(_credencial.ID);
                    category.DateChanged = DateTime.Now;
                    category.CreateByFK = null;
                    category.ChangedByFK = null;
                    var response = await _repository.UpdateProductCategory(category);
                    if (response.Result)
                    {
                        await _page.DisplayAlert("Categoria", response.Message, "OK");

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

        }
        #endregion

        public SK472ViewModel(Page page, IProducts repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            CategoryAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK918 = new BaseCommand(ExecuteSK918, CanExecuteSK918);
            _commandSK522 = new BaseCommand(ExecuteSK522, CanExecuteSK522);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Product_Category>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SK472ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            CategoryAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK918 = new BaseCommand(ExecuteSK918, CanExecuteSK918);
            _commandSK522 = new BaseCommand(ExecuteSK522, CanExecuteSK522);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Product_Category>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (!string.IsNullOrEmpty(userBasicInfoStr))
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
                _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
            }

        }

        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.ListProductCategory();
                ObservableCollection<CheckBoxGeneric<Product_Category>> api = new ObservableCollection<CheckBoxGeneric<Product_Category>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Product_Category>
                        {
                            IsChecked = false,
                            value = (Product_Category)item
                        };

                        api.Add(lista);
                    }
                }
                Category = new ObservableCollection<CheckBoxGeneric<Product_Category>>(api.OrderBy(e => e.value.Name));
                Filter = new ObservableCollection<CheckBoxGeneric<Product_Category>>(Category);
                OnPropertyChanged(nameof(Category));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }

        private async void StartSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(Setting.BaseUrl + "Notification")
            .Build();

            _hubConnection.On("UpdatePCategory", async () => await CategoryAPI());

            await _hubConnection.StartAsync();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Filter = new ObservableCollection<CheckBoxGeneric<Product_Category>>(Category);
            }
            else
            {
                var filtrados = Category
                    .Where(i => i.value.Name.StartsWith(Search, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtrados.Count == 0)
                    Filter = new ObservableCollection<CheckBoxGeneric<Product_Category>>(Category);
                else
                    Filter = new ObservableCollection<CheckBoxGeneric<Product_Category>>(filtrados);
            }
        }
    }
}
