using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SH474 : ContentPage
{
	public SH474()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SH474ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SH474ViewModel>();
    }
}