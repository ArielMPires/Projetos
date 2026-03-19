using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SS582 : ContentPage
{
	public SS582()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SS582ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
        BindingContext = this.CreateViewModel<SS582ViewModel>(Convert.ToString(ID));
        }
        else
        {
        BindingContext = this.CreateViewModel<SS582ViewModel>();
        }
    }
    public int ID { get; set; }
    public SS582(int id)
	{
        ID = id;
		InitializeComponent();
		BindingContext = this.CreateViewModel<SS582ViewModel>(Convert.ToString(id));
	}
}