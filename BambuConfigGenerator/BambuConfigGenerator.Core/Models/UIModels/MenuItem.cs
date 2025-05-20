using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class MenuItem : MvxNotifyPropertyChanged
{
    private bool _isVisible;
    private IMvxViewModel _viewModel;
    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public IMvxViewModel ViewModel
    {
        get => _viewModel;
        set => SetProperty(ref _viewModel, value);
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }
}