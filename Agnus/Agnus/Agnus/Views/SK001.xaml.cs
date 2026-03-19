using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK001 : ContentPage
{
	public SK001()
	{
		InitializeComponent();
        BindingContext = new SK001ViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SK001ViewModel();
    }
}