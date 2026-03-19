using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA001 : ContentPage
{
	public SA001()
	{
		InitializeComponent();
        BindingContext = new SA001ViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SA001ViewModel();
    }
}