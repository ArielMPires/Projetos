using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG884 : ContentPage
{
	public SG884()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG884ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            BindingContext = this.CreateViewModel<SG884ViewModel>();
        else
            BindingContext = this.CreateViewModel<SG884ViewModel>(Convert.ToString(ID));

    }

    public int ID { get; set; }

	public SG884(int id)
	{
        ID = id;
		InitializeComponent();
		BindingContext = this.CreateViewModel<SC683ViewModel>(Convert.ToString(id));
	}
}