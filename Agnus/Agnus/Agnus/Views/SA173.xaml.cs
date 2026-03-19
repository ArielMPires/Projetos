using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA173 : ContentPage
{
	public SA173()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA173ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SA173ViewModel>();
        else
            BindingContext = this.CreateViewModel<SA173ViewModel>(Convert.ToString(ID));

    }
    public int ID { get; set; }
    public SA173(int id)
	{
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SA173ViewModel>(Convert.ToString(id));
	}
}