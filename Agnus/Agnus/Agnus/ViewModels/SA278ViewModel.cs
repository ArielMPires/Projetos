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
    public class SA278ViewModel : ViewModelBase
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
        public IManuals _repository;
        private HubConnection _hubConnection;
        private ObservableCollection<Manuals> _manuals { get; set; }
        public ObservableCollection<Manuals> Manuals
        {
            get { return _manuals; }
            set
            {
                _manuals = value;
                OnPropertyChanged(nameof(ManualsAPI));
            }
        }

        #endregion
        public SA278ViewModel(IManuals repository)
        {
            _repository = repository;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            InitializeAsync();
            ManualsAPI();
            StartSignalR();
        }
        public SA278ViewModel(Page page,IManuals repository)
        {
            _page = page;
            _repository = repository;
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            InitializeAsync();
            ManualsAPI();
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

        public async Task ManualsAPI()
        {
            try
            {
                var list = await _repository.ListManuals();
                Manuals = new ObservableCollection<Manuals>(list);
                OnPropertyChanged(nameof(ManualsAPI));
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

            _hubConnection.On("UpdateManuals", async () => await ManualsAPI());

            await _hubConnection.StartAsync();
        }

    }
}

