using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SF001 : ContentPage
{
	public SF001()
	{
		InitializeComponent();
        BindingContext = new SF001ViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SF001ViewModel();
    }
}