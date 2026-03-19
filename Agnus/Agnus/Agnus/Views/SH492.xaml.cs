using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SH492 : ContentPage
{
	public SH492()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SH492ViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SH492ViewModel>();
        else
            BindingContext = this.CreateViewModel<SH492ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }
    public SH492(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SH492ViewModel>(Convert.ToString(id));
    }
}