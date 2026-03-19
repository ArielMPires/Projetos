using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG699 : ContentPage
{
	public SG699()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG699ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG699ViewModel>();
    }
}