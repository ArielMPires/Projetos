using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK991 : ContentPage
{
	public SK991()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SK991ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SK991ViewModel>();
    }
}