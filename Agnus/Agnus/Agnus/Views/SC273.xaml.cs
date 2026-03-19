using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SC273 : ContentPage
{
	public SC273()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SC273ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SC273ViewModel>();
    }
}