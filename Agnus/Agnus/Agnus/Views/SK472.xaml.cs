using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK472 : ContentPage
{
	public SK472()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SK472ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SK472ViewModel>();
    }
}