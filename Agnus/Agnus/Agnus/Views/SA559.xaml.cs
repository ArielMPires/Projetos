using Agnus.Helpers;
using Agnus.ViewModels;

namespace Agnus.Views;

public partial class SA559 : ContentPage
{
	public SA559()
	{
		InitializeComponent();
        BindingContext = this.CreateViewModel<SA559ViewModel>();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = this.CreateViewModel<SA559ViewModel>();
    }

    private bool _rotated = false;

    private async void FabBtn_Clicked(object sender, EventArgs e)
    {
        await ((Button)sender).RotateTo(_rotated ? 0 : -90);

        //FabBtnsContainer.Margin = new Thickness(0, 0, _rotated ? 0 : -100, 50);
        FabBtnsContainer.Animate<Thickness>("fab_btns",
            value =>
            {
                int factor = Convert.ToInt32(value * 10);

                var rightMargin = !_rotated
                ? (factor * 10) - 100
                : (factor * 10) * -2;

                return new Thickness(0, 0, rightMargin, 60);
            },
            newthickness => FabBtnsContainer.Margin = newthickness
            , length: 250
            , finished: (_, __) => _rotated = !_rotated);

    }
}