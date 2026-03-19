using Agnus.Commands;
using Agnus.Helpers;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Agnus.Models;
using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Agnus.ViewModels
{
    [QueryProperty(nameof(ID), "id")]
    public class SA173ViewModel : ViewModelBase
    {
        #region Command Return
        public ICommand _ReturnCommand { get; }
        public bool CanExecuteReturn(object? parameter)
        {
            return true;
        }

        public async void ExecuteReturn(object? parameter)
        {
            await Shell.Current.GoToAsync("///SA961");
        }

        #endregion

        #region Command EditCategory

        public ICommand _EditCatCommand { get; }
        public bool CanExecuteEditCat(object? parameter)
        {
            return true;
        }

        public async void ExecuteEditCat(object? parameter)
        {
            _Category.ChangedBy = Convert.ToInt32(_credencial.ID);
            _Category.DateChanged = DateTime.Now;
            var response = await _repository.UpdateCategory(_Category);
            if (response.Result)
            {
                await _page.DisplayAlert("Categoria", response.Message, "OK");

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    var tela = Application.Current.Windows.OfType<Window>().FirstOrDefault(e => e.Page == _page);
                    Application.Current?.CloseWindow(tela);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await Shell.Current.GoToAsync("///SG961");
                }
            }
            else
            {
                await _page.DisplayAlert(response.Message, response.Error, "OK");
            }
        }
        #endregion

        #region Property

        private Patrimony_Category _Category { get; set; }
        public Patrimony_Category Category
        {
            get { return _Category; }
            set
            {
                _Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public Credencial _credencial { get; set; }

        public IPatrimony _repository;

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
                    CategoryAPI();
            }
        }

        #endregion

        public SA173ViewModel(Page page, string id,IPatrimony repository)
        {
            _repository = repository;
            _page = page;
            ID = id;
            InitializeAsync();
            CategoryAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditCatCommand = new BaseCommand(ExecuteEditCat, CanExecuteEditCat);
        }

        public SA173ViewModel(IPatrimony repository)
        {
            _repository = repository;
            InitializeAsync();
            CategoryAPI();
            _ReturnCommand = new BaseCommand(ExecuteReturn, CanExecuteReturn);
            _EditCatCommand = new BaseCommand(ExecuteEditCat, CanExecuteEditCat);
        }

        public async Task CategoryAPI()
        {
            try
            {
                var list = await _repository.Category_SearchBy(Convert.ToInt32(ID));
                Category = list;
                OnPropertyChanged(nameof(Category));
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
