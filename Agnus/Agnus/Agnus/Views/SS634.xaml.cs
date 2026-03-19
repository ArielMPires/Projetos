using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS634 : ContentPage
{
	public SS634()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS634ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS634ViewModel>();
    }
}