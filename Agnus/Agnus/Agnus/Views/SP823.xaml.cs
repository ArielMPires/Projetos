using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SP823 : ContentPage
{
	public SP823()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SP823ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SP823ViewModel>();
        else
            BindingContext = this.CreateViewModel<SP823ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }

    public SP823(int id)
	{

        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SP823ViewModel>(Convert.ToString(id));
    }
}