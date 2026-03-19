using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SP794 : ContentPage
{
	public SP794()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SP794ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SP794ViewModel>();
    }
}