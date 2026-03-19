using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK172 : ContentPage
{
    public SK172()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SK172ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SK172ViewModel>();
        else
            BindingContext = this.CreateViewModel<SK172ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }
    public SK172(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SK172ViewModel>(Convert.ToString(id));
    }
}