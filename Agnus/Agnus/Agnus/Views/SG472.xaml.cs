using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG472 : ContentPage
{
	public SG472()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG472ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG472ViewModel>();
    }
}