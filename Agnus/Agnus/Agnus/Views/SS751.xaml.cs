using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS751 : ContentPage
{
	public SS751()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS751ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS751ViewModel>();
    }
}