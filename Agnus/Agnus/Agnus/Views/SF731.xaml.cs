using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SF731 : ContentPage
{
	public SF731()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SF731ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SF731ViewModel>();
    }
}