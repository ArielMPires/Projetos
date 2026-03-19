using Agnus.Commands;
using Agnus.Interfaces;
using Agnus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Agnus.Interfaces.Repositories;
using Agnus.Helpers;
using CommunityToolkit.Maui.Views;
using Agnus.Views;
using System.Collections.ObjectModel;
using Agnus.Models.DB;

namespace Agnus.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Property
        public Log_In _users { get; set; }
        public string tenantId { get; set; }
        public IUsers _repository;
        public Tenant _SelectedTenant { get; set; }
        private ObservableCollection<Tenant> _tenant { get; set; }
        public ObservableCollection<Tenant> Tenant
        {
            get { return _tenant; }
            set
            {
                _tenant = value;
                OnPropertyChanged(nameof(Tenant));
            }
        }

        private readonly Page _page;
        #endregion

        #region Command Log_In
        public ICommand _logarCommand { get; }
        public bool CanExecuteLog_In(object? parameter)
        {
            return true;
        }

        public async void ExecuteLog_In(object? parameter)
        {
            tenantId = Convert.ToString(_SelectedTenant.Id);
            _repository.SetTenantHeader(tenantId);

            var response = await _repository.Log_In(_users);
            if (response.Result)
            {
                _repository.SetHeader(tenantId, response.UserToken.Token);
                var lista = await _repository.ListPermissionsByUser(Convert.ToInt32(_users.Id));
                List <Agnus.Models.DB.Permissions> permissions = new List<Agnus.Models.DB.Permissions>(lista);

                TokenProcessor.ProcessToken(response.UserToken.Token, response.UserToken.Expiration, tenantId, permissions);

                if (Application.Current.MainPage is AppShell shell)
                {
                    shell.UpdateCredencial();
                }
                await Task.Delay(500);
                if (Routing.GetOrCreateContent("SD001") != null)
                {
                    await Shell.Current.GoToAsync("//SD001");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Tente novamente", "OK");
                }

            }
            else
            {
                await _page.DisplayAlert("Log In", "ID ou Senha estão errada!.", "OK");
            }

        }
        #endregion
        public LoginViewModel(IUsers repository)
        {
            _users = new Log_In();
            _logarCommand = new BaseCommand(ExecuteLog_In, CanExecuteLog_In);
            _repository = repository;
            TenantData();
        }

        public LoginViewModel(Page page, IUsers repository)
        {
            _page = page;
            _users = new Log_In();
            _logarCommand = new BaseCommand(ExecuteLog_In, CanExecuteLog_In);
            _repository = repository;
            TenantData();
        }

        public async Task TenantData()
        {
            try
            {
                Tenant = new ObservableCollection<Tenant>(Agnus.Models.Tenant.Tenants);
                OnPropertyChanged(nameof(Tenant));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
