using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this.CreateViewModel<LoginViewModel>();
        }


    }

}
