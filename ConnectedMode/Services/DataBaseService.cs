using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    
    public ICollection<Items> LoadItemsFromDb()
    {
        ObservableCollection<Items> students = new ObservableCollection<Items>();

        using var connection = OpenConnection();
        using var loadCommand = new SqlCommand();
        loadCommand.Connection = connection;
        loadCommand.CommandType = CommandType.StoredProcedure;
        loadCommand.CommandText = "sp_GetAllItems";

        using var reader = loadCommand.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Items()
            {
                Id = reader.GetInt32(0),
                ItemName = reader.GetString(1),
                Quantity = reader.GetInt32(2),
                Category = reader.GetString(3),
                Price = (int)reader.GetDecimal(4), //
                Description = reader.GetString(5),
                AddedDate = reader.GetDateTime(6)
            });
        }

        return students;
    }

    
    public int AddItem(Items item)
    {
        int generatedId;
        using var connection = OpenConnection();
        using var addCommand = new SqlCommand();
        addCommand.Connection = connection;
        addCommand.CommandType = CommandType.StoredProcedure;
        addCommand.CommandText = "sp_InsertItem";
        addCommand.Parameters.AddWithValue("@ItemName", item.ItemName);
        addCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
        addCommand.Parameters.AddWithValue("@Category", item.Category);
        addCommand.Parameters.AddWithValue("@Price", item.Price);
        addCommand.Parameters.AddWithValue("@Description", item.Description);
        addCommand.Parameters.AddWithValue("@AddedDate", item.AddedDate);
        SqlParameter insertedIdParameter = new SqlParameter
        {
            ParameterName = "@InsertedId",
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Output
        };
        addCommand.Parameters.Add(insertedIdParameter);
        addCommand.ExecuteNonQuery();
        generatedId = (int)insertedIdParameter.Value;
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