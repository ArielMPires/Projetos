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
    public class SG842ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SG737");
        }

        #endregion

        #region Command NewCategory

        public ICommand _NewTypeCommand { get; }
        public bool CanExecuteNewType(object? parameter)
        {
            return true;
        }

        public async void ExecuteNewType(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Criando Tipo de Chamado...");
                _type.CreateBy = Convert.ToInt32(_credencial.ID);
                _type.DateCreate = DateTime.Now;
                _type.Category = _SelectedCategory.ID;
                _type.Priority = _SelectedPriority.Id;
                var response = await _repository.NewType(_type);
                if (response.Result)
                {
                    await _page.DisplayAlert("Tipo de Chamado", response.Message, "OK");

                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                        Application.Current?.CloseWindow(tela);
                    }
                    else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Shell.Current.GoToAsync("///SG737");
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

        public Service_Category _SelectedCategory { get; set; }
        public Priority _SelectedPriority { get; set; }

        private ObservableCollection<Service_Category> _category { get; set; }
        public ObservableCollection<Service_Category> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        private ObservableCollection<Priority> _priority { get; set; }
        public ObservableCollection<Priority> Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }
        public Service_Type _type { get; set; } = new Service_Type();
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

        private readonly Page _page;

        #endregion

        public SG842ViewModel(Page page, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            InitializeAsync();
            CategoryAPI();
            PriorityData();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewTypeCommand = new BaseCommand(ExecuteNewType, CanExecuteNewType);
        }

        public SG842ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            CategoryAPI();
            PriorityData();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _NewTypeCommand = new BaseCommand(ExecuteNewType, CanExecuteNewType);
        }

        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
            }
            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }
        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.ListAllCategory();
                Category = new ObservableCollection<Service_Category>(list);
                OnPropertyChanged(nameof(Category));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
        public async Task PriorityData()
        {
            try
            {
                Priority = new ObservableCollection<Priority>(Agnus.Models.Priority.Priorities);
                OnPropertyChanged(nameof(Priority));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
            }
        }
    }
}
