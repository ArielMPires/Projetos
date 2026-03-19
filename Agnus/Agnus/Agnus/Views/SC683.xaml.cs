using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SC683 : ContentPage
{
	public SC683()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SC683ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        BindingContext = this.CreateViewModel<SC683ViewModel>();
        else
        BindingContext = this.CreateViewModel<SC683ViewModel>(Convert.ToString(ID));

    }

    public int ID { get; set; }

    public SC683(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SC683ViewModel>(Convert.ToString(id));
    }
}