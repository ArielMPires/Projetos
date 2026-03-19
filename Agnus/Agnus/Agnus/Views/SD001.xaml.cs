using Agnus.Helpers;
using Agnus.ViewModels;
using System.Security.Cryptography;

namespace Agnus.Views;

public partial class SD001 : ContentPage
{
	public SD001()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SD001ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SD001ViewModel>();
    }
}