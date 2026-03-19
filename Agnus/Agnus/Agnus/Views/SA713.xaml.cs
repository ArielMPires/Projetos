using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA713 : ContentPage
{
	public SA713()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA713ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA713ViewModel>();
    }
}