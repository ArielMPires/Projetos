using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SF852 : ContentPage
{
	public SF852()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SF852ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SF852ViewModel>();
    }
}