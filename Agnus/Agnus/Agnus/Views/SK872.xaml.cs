using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK872 : ContentPage
{
	public SK872()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SK872ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SK872ViewModel>();
    }
}