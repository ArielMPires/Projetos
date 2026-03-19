using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SG644 : ContentPage
{
	public SG644()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SG644ViewModel>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SG644ViewModel>(Convert.ToString(ID));
    }
    public int ID { get; set; }
    public SG644(int id)
    {
        ID = id;
        InitializeComponent();
        BindingContext = this.CreateViewModel<SG644ViewModel>(Convert.ToString(id));
    }
}