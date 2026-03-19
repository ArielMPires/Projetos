using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SM001 : ContentPage
{
	public SM001()
	{
		InitializeComponent();
        BindingContext = new SM001ViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SM001ViewModel();
    }
}