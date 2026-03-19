using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SK323 : ContentPage
{
    public SK323()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SK323ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            BindingContext = this.CreateViewModel<SK323ViewModel>(ID);
        }
        else
        {
            BindingContext = this.CreateViewModel<SK323ViewModel>();
        }
    }

    private string ID { get; set; }
    public SK323(int id)
    {
        InitializeComponent();
        ID = Convert.ToString(id);
        BindingContext = this.CreateViewModel<SK323ViewModel>(ID);
    }
}