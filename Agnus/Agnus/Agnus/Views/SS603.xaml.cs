using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS603 : ContentPage
{
	public SS603()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS603ViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SS603ViewModel>();
        else
            BindingContext = this.CreateViewModel<SS603ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }
    public SS603(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS603ViewModel>(Convert.ToString(id));
    }
}