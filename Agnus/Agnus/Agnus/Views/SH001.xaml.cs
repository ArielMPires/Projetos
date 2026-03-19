using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SH001 : ContentPage
{
	public SH001()
	{
		InitializeComponent();
        BindingContext = new SH001ViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new SH001ViewModel();
    }
}