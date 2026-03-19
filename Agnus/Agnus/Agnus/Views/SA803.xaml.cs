using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA803 : ContentPage
{
	public SA803()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA803ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA803ViewModel>();
    }
}