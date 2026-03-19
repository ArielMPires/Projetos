using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA227 : ContentPage
{
	public SA227()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA221ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SA227ViewModel>();
        else
            BindingContext = this.CreateViewModel<SA227ViewModel>(Convert.ToString(ID));

    }

    public int ID { get; set; }
    public SA227(int id)
	{
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA227ViewModel>(Convert.ToString(id));
    }
}