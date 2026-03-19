using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS001 : ContentPage
{
	public SS001()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS001ViewModel>();
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS001ViewModel>();
    }
}