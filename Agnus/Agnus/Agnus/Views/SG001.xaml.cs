using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG001 : ContentPage
{
	public SG001()
	{
        BindingContext = new SG001ViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SG001ViewModel();
    }
}