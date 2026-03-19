using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG351 : ContentPage
{
    public SG351()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SG351ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG351ViewModel>();
    }
}