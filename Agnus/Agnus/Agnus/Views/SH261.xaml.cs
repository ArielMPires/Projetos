using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SH261 : ContentPage
{
	public SH261()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SH261ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SH261ViewModel>();
    }
}