using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA264 : ContentPage
{
	public SA264()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA264ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA264ViewModel>();
    }
}