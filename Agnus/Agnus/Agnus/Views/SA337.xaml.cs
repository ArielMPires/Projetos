using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA337 : ContentPage
{
	public SA337()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA337ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
		if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
			BindingContext = this.CreateViewModel<SA337ViewModel>();
        else
			BindingContext = this.CreateViewModel<SA337ViewModel>(Convert.ToString(ID));

    }

	public int ID { get; set; }
	public SA337(int id)
	{
		ID = id;
		InitializeComponent();
		BindingContext = this.CreateViewModel<SA337ViewModel>(Convert.ToString(id));
    }
}