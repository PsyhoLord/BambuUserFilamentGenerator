using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class FilamentFileParamUIModel : MvxNotifyPropertyChanged
{
    private string _key;
    private string _value;

    public string Key
    {
        get => _key;
        set => SetProperty(ref _key, value);
    }

    public string Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
}