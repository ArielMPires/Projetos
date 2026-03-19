

using Agnus.Helpers;
using Agnus.ViewModels;
namespace Agnus.Views;

public partial class SG897 : ContentPage
{
	public SG897()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SG897ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG897ViewModel>();
    }
}