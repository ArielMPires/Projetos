using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SC001 : ContentPage
{
	public SC001()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SC001ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SC001ViewModel>();
    }
}