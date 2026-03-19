using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS573 : ContentPage
{
	public SS573()
	{
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS573ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SS573ViewModel>();
    }
}