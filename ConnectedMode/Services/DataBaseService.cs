using System;
using System.Data;
using ConnectedMode.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ConnectedMode.Services;

public class DataBaseService
{
    private readonly IConfiguration _configuration;
    private ErrorMessage _error;

    public DataBaseService(IConfiguration config)
    {
        _configuration = config;
        _error = new ErrorMessage();
    }

    public SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(_configuration["ConnectionString"]);
        connection.Open();
        return connection;
    }

    public void InitializeDb()
    {
        using var connection = OpenConnection();

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

    public void CreateTable()
    {
        using (var connection = OpenConnection())
        {
            using var useCommand = new SqlCommand("USE [Store]", connection);
            useCommand.ExecuteNonQuery();

            using (var checkTable = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE name = 'Items'", connection))
            {
                int tablesCount = (int)checkTable.ExecuteScalar();
                if (tablesCount == 0)
                {
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
                        _error.Show("Failed to create table", "Error");
                    }
                }
            }
        }
    }

    public int AddItem(Items item, string tableName)
    {
        int generatedId;
        using var connection = OpenConnection();
        using var addCommand = new SqlCommand();
        addCommand.Connection = connection;
        addCommand.CommandText = $" INSERT INTO {tableName}" +
                                 $" OUTPUT INSERTED.Id" +
                                 $" VALUES (@ItemName, @Quantity, @Category, @Price, @Description, @AddedDate)";
        addCommand.Parameters.AddWithValue("@ItemName", item.ItemName);
        addCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
        addCommand.Parameters.AddWithValue("@Category", item.Category);
        addCommand.Parameters.AddWithValue("@Price", item.Price);
        addCommand.Parameters.AddWithValue("@Description", item.Description);
        addCommand.Parameters.AddWithValue("@AddedDate", item.AddedDate);
        generatedId = (int)addCommand.ExecuteScalar();
        return generatedId;
    }

    public void RemoveFromDb(int id)
    {
        using var connection = OpenConnection();
        using var removeCommand = new SqlCommand();
        removeCommand.Connection = connection;
        removeCommand.CommandType = CommandType.StoredProcedure;
        removeCommand.CommandText = "sp_Remove";
        removeCommand.Parameters.AddWithValue("@Id", id);
        removeCommand.ExecuteNonQuery();
    }

    public void UpdateItemDb(Items item)
    {
        using var connection = OpenConnection();
        using var updateCommand = new SqlCommand();
        updateCommand.Connection = connection;
        updateCommand.CommandType = CommandType.StoredProcedure;
        updateCommand.CommandText = "sp_updateItem";
        updateCommand.Parameters.AddWithValue("@ItemId", item.Id);
        updateCommand.Parameters.AddWithValue("@NewName", item.ItemName);
        updateCommand.Parameters.AddWithValue("@NewQuantity", item.Quantity);
        updateCommand.Parameters.AddWithValue("@NewCategory", item.Category);
        updateCommand.Parameters.AddWithValue("@NewPrice", item.Price);
        updateCommand.Parameters.AddWithValue("@NewDescription", item.Description);
        updateCommand.Parameters.AddWithValue("@NewDate", DateTime.Now);
        updateCommand.ExecuteNonQuery();
    }

    public int GetCount()
    {
        using var connection = OpenConnection();
        using var removeCommand = new SqlCommand();
        removeCommand.Connection = connection;
        removeCommand.CommandType = CommandType.StoredProcedure;
        removeCommand.CommandText = "sp_getItemsCount";
        var result = removeCommand.ExecuteScalar();
        if (result != null && result != System.DBNull.Value)
        {
            return (int)result;
        }
        return 0;
    }
}