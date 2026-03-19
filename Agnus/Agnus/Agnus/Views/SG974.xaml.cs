using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG974 : ContentPage
{
	public SG974()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG974ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SG974ViewModel>();
        else
            BindingContext = this.CreateViewModel<SG974ViewModel>(Convert.ToString(ID));

    }

    public int ID { get; set; }
    public SG974(int id)
	{
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SG974ViewModel>(Convert.ToString(id));
    }
}