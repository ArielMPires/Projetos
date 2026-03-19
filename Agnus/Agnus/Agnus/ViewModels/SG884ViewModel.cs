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
    public class SG884ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SS328");
        }

        #endregion

        #region Command EditType

        public ICommand _EditTypeCommand { get; }
        public bool CanExecuteEditType(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditType(object? parameter)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                LoadingService.ShowLoading(_page, "Editando Tipo de Chamado...");
                _type.ChangedBy = Convert.ToInt32(_credencial.ID);
                _type.DateChanged = DateTime.Now;
                _type.CategoryFK = null;
                _type.Category = _SelectedCategory.ID;
                _type.Priority = _SelectedPriority.Id;
                var response = await _repository.UpdateType(_type);
                if (response.Result)
                {
                    await _page.DisplayAlert("Serviços", response.Message, "OK");

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
        private int? _selectedPriorityIndex { get; set; }
        public int? _SelectedPriorityIndex
        {
            get => _selectedPriorityIndex;
            set
            {
                _selectedPriorityIndex = value;
                OnPropertyChanged(nameof(_SelectedPriorityIndex));
            }
        }
        private int? _selectedCatIndex { get; set; }
        public int? _SelectedCatIndex
        {
            get => _selectedCatIndex;
            set
            {
                _selectedCatIndex = value;
                OnPropertyChanged(nameof(_SelectedCatIndex));
            }
        }
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
        private Service_Type _type { get; set; }
        public Service_Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public Credencial _credencial { get; set; }

        public IService_Order _repository;

        private readonly Page _page;

        private string _Id { get; set; }
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(ID));
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    TypeAPI();
            }
        }

        #endregion

        public SG884ViewModel(Page page, string id, IService_Order repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            CategoryAPI();
            PriorityData();
            TypeAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditTypeCommand = new BaseCommand(ExecuteEditType, CanExecuteEditType);
        }
        public SG884ViewModel(IService_Order repository)
        {
            _repository = repository;
            InitializeAsync();
            CategoryAPI();
            PriorityData();
            TypeAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditTypeCommand = new BaseCommand(ExecuteEditType, CanExecuteEditType);
        }
        public async Task TypeAPI()
        {
            try
            {
                var list = await _repository.TypeById(Convert.ToInt32(ID));
                Type = list;
                OnPropertyChanged(nameof(Type));

                if (Type != null)
                {
                    if (Type.CategoryFK != null)
                    {
                        _SelectedCategory = Type.CategoryFK;
                        _SelectedCatIndex = Category.ToList().FindIndex(e => e.ID == _SelectedCategory.ID);
                    }
                    else
                    {
                        _selectedCatIndex = null;
                    }
                    if (Type.Priority != null)
                    {
                        _SelectedPriority = Priority.FirstOrDefault(e => e.Id == Type.Priority);
                        _SelectedPriorityIndex = Priority.ToList().FindIndex(e => e.Id == _SelectedPriority.Id);
                    }
                    else
                    {
                        _selectedPriorityIndex = null;
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
        private async void InitializeAsync()
        {
            string userBasicInfoStr = TokenProcessor.LoadFromSecureStorageAsync(nameof(Credencial));
            if (userBasicInfoStr != null)
            {
                _credencial = JsonConvert.DeserializeObject<Credencial>(userBasicInfoStr);
            }
            _repository.SetHeader(_credencial?.tenantId, _credencial.Token);
        }
    }
}
