using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS159 : ContentPage
{
	public SS159()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS159ViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS159ViewModel>();
    }
}