
using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA278 : ContentPage
{

    public SA278()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA278ViewModel>();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA278ViewModel>();
    }

}