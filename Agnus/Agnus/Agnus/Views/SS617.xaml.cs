using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS617 : ContentPage
{
	public SS617()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS617ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SS617ViewModel>();
        else
            BindingContext = this.CreateViewModel<SS617ViewModel>(Convert.ToString(ID));
    }

    public int ID { get; set; }
    public SS617(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SS617ViewModel>(Convert.ToString(id));
    }
}