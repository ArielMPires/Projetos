using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG842 : ContentPage
{
	public SG842()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG842ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG842ViewModel>();
    }
}