using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS815 : ContentPage
{
    public SS815()
    {
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS815ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SS815ViewModel>();
        else
            BindingContext = this.CreateViewModel<SS815ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }

    public SS815(int id)
	{
        ID = id;
        InitializeComponent();
		BindingContext = this.CreateViewModel<SS815ViewModel>(Convert.ToString(id));
	}
}