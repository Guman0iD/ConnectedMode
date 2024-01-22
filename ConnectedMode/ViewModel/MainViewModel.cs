using System;
using System.Data;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ConnectedMode.Messages;
using ConnectedMode.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectedMode.ViewModel;

[INotifyPropertyChanged]
public partial class MainViewModel : BaseViewModel
{
    private readonly IConfiguration _config;
    private ErrorMessage _error;
    
    public MainViewModel(IConfiguration config, ErrorMessage error)
    {
        _config = config;
        _error = error;

        WeakReferenceMessenger.Default.Register<ChangeViewModelMessage>(this,
            (sender, message) => { CurrentViewModel = message.ViewModel; });

        var viewModel = App.ServiceProvider.GetService<ItemsViewModel>()!;

        var changeViewModelMessage = new ChangeViewModelMessage( viewModel);
        WeakReferenceMessenger.Default.Send(changeViewModelMessage);
        InitializeDb();
        CreateTable();
    }
    
    private void InitializeDb()
    {
        using var connection = new SqlConnection(_config["ConnectionString"]);
        
        connection.Open();

        var dbName = "Store";
        
        var existsDbCommand = new SqlCommand()
        {
            Connection = connection,
            CommandText = "IF (EXISTS(SELECT 1 FROM sys.databases WHERE name = @dbName)) SELECT 1 ELSE SELECT 0"
        };
        
        existsDbCommand.Parameters.AddWithValue("@dbName", dbName);

        int result = (int)existsDbCommand.ExecuteScalar();

        if (result != 1)
        {
           using var createDbcommand = new SqlCommand();
           createDbcommand.Connection = connection;
           createDbcommand.CommandType = CommandType.StoredProcedure;
           createDbcommand.CommandText = "sp_CreateDb";
           
           createDbcommand.ExecuteNonQuery();
        }
    }

    private void CreateTable()
    {
        using var connection = new SqlConnection(_config["ConnectionString"]);
        connection.Open();

        using var useCommand = new SqlCommand();
        useCommand.Connection = connection;
        useCommand.CommandText = "USE [Store]";
        useCommand.ExecuteNonQuery();
        
        try
        {
            using var createTableCommand = new SqlCommand();
            createTableCommand.Connection = connection;
            createTableCommand.CommandType = CommandType.StoredProcedure;
            createTableCommand.CommandText = "sp_CreateTable";
            createTableCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
           _error.Show("Failed to create table","Error");
           throw;
        }
    }
    
    [ObservableProperty]
    private BaseViewModel? _currentViewModel;
}