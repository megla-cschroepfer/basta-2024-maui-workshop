using DontLetMeExpire.ViewModels;

namespace DontLetMeExpire.Views;

[QueryProperty(nameof(DisplayMode), nameof(DisplayMode))]
[QueryProperty(nameof(LocationId), nameof(LocationId))]
public partial class ItemsPage : ContentPage
{
	private ItemsViewModel _viewModel;
    public ItemsPage(ItemsViewModel viewModel)
	{
        BindingContext = _viewModel = viewModel;
        InitializeComponent();
	}

    public string DisplayMode { get; set; }
    public string LocationId { get; set; }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.InitializeAsync(DisplayMode, LocationId);
        base.OnNavigatedTo(args);
    }
}