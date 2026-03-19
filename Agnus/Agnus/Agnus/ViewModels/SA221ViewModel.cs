using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Agnus.ViewModels
{
    public class SA221ViewModel : ViewModelBase
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
        public IProviderOrder _repository;
        private HubConnection _hubConnection;
        private ObservableCollection<Service_Providers> _providers { get; set; }
        public ObservableCollection<Service_Providers> Providers
        {
            get { return _providers; }
            set
            {
                _providers = value;
                OnPropertyChanged(nameof(Providers));
            }
        }

        #endregion
        public SA221ViewModel(IProviderOrder repository)
        {
            _repository = repository;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            InitializeAsync();
            ProvidersAPI();
            StartSignalR();
        }
        public SA221ViewModel(Page page,IProviderOrder repository)
        {
            _page = page;
            _repository = repository;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            InitializeAsync();
            ProvidersAPI();
            StartSignalR();
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

        public async Task ProvidersAPI()
        {
            try
            {
                var list = await _repository.ListServices();
                Providers = new ObservableCollection<Service_Providers>(list);
                OnPropertyChanged(nameof(Providers));
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

            _hubConnection.On("UpdateProvider", async () => await ProvidersAPI());

            await _hubConnection.StartAsync();
        }





    }
}
