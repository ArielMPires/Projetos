using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Agnus.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    public class SK872ViewModel : ViewModelBase
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
        public ICommand ToggleExpandCommand { get; }
        public Credencial _credencial { get; set; }
        public IProducts _repository;
        private HubConnection _hubConnection;
        private ObservableCollection<CheckBoxGeneric<Brands>> _brands { get; set; }
        public ObservableCollection<CheckBoxGeneric<Brands>> Brands
        {
            get { return _brands; }
            set
            {
                _brands = value;
                OnPropertyChanged(nameof(Brands));
            }
        }

        #endregion

        #region Commands SK771
        public ICommand _commandSK771 { get; set; }

        public bool CanExecuteSK771(object? parameter)
        {
            return true;
        }

        public async void ExecuteSK771(object? parameter)
        {
            string result = await _page.DisplayPromptAsync("Marca", "Digite o Nome da Marca");
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Marca", "Obrigatorio colocar o Nome Marca", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Criando Marca...");
                    var brand = new Brands()
                {
                    Name = result,
                    CreateBy = Convert.ToInt32(_credencial.ID),
                    DateCreate = DateTime.Now
                };
                var response = await _repository.NewBrands(brand);
                if (response.Result)
                {
                    await _page.DisplayAlert("Marcas", response.Message, "OK");

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

        #region Commands SK392
        public ICommand _commandSK392 { get; set; }

        public bool CanExecuteSK392(object? parameter)
        {
            return true;
        }

        public async void ExecuteSK392(object? parameter)
        {
            var id = (int)parameter;
            var mark = new Brands();
            mark = await _repository.BrandsById(id);

            string result = await _page.DisplayPromptAsync("Marca", "Digite o Nome da Marca", initialValue: mark.Name);
            if (String.IsNullOrEmpty(result))
            {
                await _page.DisplayAlert("Marca", "Obrigatorio colocar o Nome Marca", "OK");
            }
            else
            {
                if (IsBusy) return;

                try
                {
                    IsBusy = true;
                    LoadingService.ShowLoading(_page, "Editando Marca...");
                    mark.Name = result;
                mark.ChangedBy = Convert.ToInt32(_credencial.ID);
                mark.DateChanged = DateTime.Now;
                mark.CreateByFK = null;
                mark.ChangedByFK = null;
                var response = await _repository.UpdateBrands(mark);
                if (response.Result)
                {
                    await _page.DisplayAlert("Marcas", response.Message, "OK");

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

        public SK872ViewModel(Page page, IProducts repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            StartSignalR();
            BrandsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK771 = new BaseCommand(ExecuteSK771, CanExecuteSK771);
            _commandSK392 = new BaseCommand(ExecuteSK392, CanExecuteSK392);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Brands>>((item) =>
            {
                item.IsChecked = !item.IsChecked;
            });
        }

        public SK872ViewModel(IProducts repository)
        {
            _repository = repository;
            InitializeAsync();
            StartSignalR();
            BrandsAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _commandSK771 = new BaseCommand(ExecuteSK771, CanExecuteSK771);
            _commandSK392 = new BaseCommand(ExecuteSK392, CanExecuteSK392);
            ToggleExpandCommand = new Command<CheckBoxGeneric<Brands>>((item) =>
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

        public async Task BrandsAPI()
        {
            try
            {
                var list = await _repository.ListBrands();
                ObservableCollection<CheckBoxGeneric<Brands>> api = new ObservableCollection<CheckBoxGeneric<Brands>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var lista = new CheckBoxGeneric<Brands>
                        {
                            IsChecked = false,
                            value = item
                        };

                        api.Add(lista);
                    }
                }
                Brands = new ObservableCollection<CheckBoxGeneric<Brands>>(api.OrderBy(e => e.value.Name));
                OnPropertyChanged(nameof(Brands));
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

            _hubConnection.On("UpdateBrands", async () => await BrandsAPI());

            await _hubConnection.StartAsync();
        }
    }
}
