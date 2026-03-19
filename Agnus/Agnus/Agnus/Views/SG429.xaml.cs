using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG429 : ContentPage
{
	public SG429()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SG429ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG429ViewModel>();
    }
}