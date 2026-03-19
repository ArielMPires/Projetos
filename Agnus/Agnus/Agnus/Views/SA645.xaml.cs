using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA645 : ContentPage
{
	public SA645()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA645ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SA645ViewModel>();
        else
            BindingContext = this.CreateViewModel<SA645ViewModel>(Convert.ToString(ID));

    }

    public int ID { get; set; }
    public SA645(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA645ViewModel>(Convert.ToString(id));
    }
}