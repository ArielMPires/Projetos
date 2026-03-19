using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG677 : ContentPage
{
	public SG677()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG677ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG677ViewModel>(Convert.ToString(ID));
    }

	public int ID { get; set; }
	public SG677(int id)
	{
		ID = id;
		InitializeComponent();
		BindingContext = this.CreateViewModel<SG677ViewModel>(Convert.ToString(id));
	}
}