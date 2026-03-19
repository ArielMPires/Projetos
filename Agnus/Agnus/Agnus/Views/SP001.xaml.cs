using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SP001 : ContentPage
{
	public SP001()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SP001ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SP001ViewModel>();
    }
}