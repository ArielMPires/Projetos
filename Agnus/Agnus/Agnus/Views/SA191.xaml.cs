using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA191 : ContentPage
{
	public SA191()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA191ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA191ViewModel>();
    }
}