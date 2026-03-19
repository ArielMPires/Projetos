using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK538 : ContentPage
{
	public SK538()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SK538ViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SK538ViewModel>();
    }
}