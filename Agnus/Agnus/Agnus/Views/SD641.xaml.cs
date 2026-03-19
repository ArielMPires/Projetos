using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SD641 : ContentPage
{
	public SD641()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SD641ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SD641ViewModel>();
    }
}