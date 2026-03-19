using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS328 : ContentPage
{
	public SS328()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS328ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS328ViewModel>();
    }
}