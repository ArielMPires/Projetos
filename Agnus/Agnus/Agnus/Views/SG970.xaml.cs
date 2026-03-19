using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG970 : ContentPage
{
	public SG970()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG970ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG970ViewModel>();
    }
}