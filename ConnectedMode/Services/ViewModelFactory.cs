using ConnectedMode.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectedMode.Services;
public class ViewModelFactory
{
    public BaseViewModel Create(int index)
    {
        return index switch
        {
            0 => App.ServiceProvider.GetService<MainViewModel>(),
            1 => App.ServiceProvider.GetService<AddItemViewModel>(),
            2 => App.ServiceProvider.GetService<ItemsViewModel>(),
            3 => App.ServiceProvider.GetService<UpdateItemViewModel>(),
            4 => App.ServiceProvider.GetService<ItemsInfoViewModel>()
        };
    }
}