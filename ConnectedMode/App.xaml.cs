using System.Windows;
using ConnectedMode.Messages;
using ConnectedMode.Model;
using ConnectedMode.Services;
using ConnectedMode.View;
using ConnectedMode.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectedMode
{
    public partial class App : Application
    {
        public static ServiceCollection ServiceCollection = null!;
        public static ServiceProvider ServiceProvider = null!;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            

            ServiceCollection = new ServiceCollection();
            
            ServiceCollection.AddSingleton<MainViewModel>();
            ServiceCollection.AddTransient<AddItemViewModel>();
            ServiceCollection.AddSingleton<ItemsViewModel>();
            ServiceCollection.AddSingleton<UpdateItemViewModel>();
            ServiceCollection.AddSingleton<ItemsInfoViewModel>();
            
            ServiceCollection.AddSingleton<ViewModelFactory>();
            
            ServiceCollection.AddSingleton<MainView>();

            ServiceCollection.AddSingleton<ChangeViewModelMessage>();
            ServiceCollection.AddSingleton<AddItemMessage>();
            ServiceCollection.AddSingleton<UpdateItemMessage>();
            ServiceCollection.AddSingleton<IConfiguration>(configuration);
            ServiceCollection.AddTransient<ErrorMessage>();
            ServiceCollection.AddTransient<DataBaseService>();
            
            ServiceProvider = ServiceCollection.BuildServiceProvider();
            var view = ServiceProvider.GetService<MainView>()!;
            view.Show();
        }
    }
}