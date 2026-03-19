using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SF136 : ContentPage
{
	public SF136()
	{
		InitializeComponent();
		BindingContext = this.CreateViewModel<SF136ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SF136ViewModel>();
    }
}