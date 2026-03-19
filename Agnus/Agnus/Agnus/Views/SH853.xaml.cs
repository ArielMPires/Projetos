using Agnus.Helpers;
using Agnus.Models;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SH853 : ContentPage
{
	public SH853()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SH853ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SH853ViewModel>();
    }
}