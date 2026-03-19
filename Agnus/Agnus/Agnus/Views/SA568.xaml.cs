using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA568 : ContentPage
{
	public SA568()
	{
		InitializeComponent();
		BindingContext = this.CreateViewModel<SA568ViewModel>();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		BindingContext = this.CreateViewModel<SA568ViewModel>();
    }
}