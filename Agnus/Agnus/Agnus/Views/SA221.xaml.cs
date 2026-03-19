using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA221 : ContentPage
{
    public SA221()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA221ViewModel>();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA221ViewModel>();

    }
}