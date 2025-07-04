using System;
using Dapper;
using Microsoft.Data.Sqlite;

public static class DynamicSetup
{
    public static string SetupDatabase(string roomName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roomName))
                throw new ArgumentException("roomName não pode ser vazio");

            string dataName = $"{roomName}.db";
            var path = Path.Combine(AppContext.BaseDirectory, dataName);
            string connectionString = $"Data Source={path}";

            // ✅ Se o arquivo já existe, retorna direto
            if (File.Exists(path))
                return connectionString;

            // ✅ Se ainda não existe, cria e inicializa
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Ativa constraints de foreign key
            connection.Execute("PRAGMA foreign_keys = ON;");

            // Criação da tabela Columns
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Columns (
                    ColumnId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL
                );
            ");

            // Criação da tabela Tasks
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status TEXT,
                    DueDate TEXT,
                    ColumnId INTEGER NOT NULL,
                    FOREIGN KEY(ColumnId) REFERENCES Columns(ColumnId) ON DELETE CASCADE
                );
            ");

            connection.Close();

            return connectionString;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar banco de dados: {ex.Message}");
            return "";
        }
    }

    public static bool FileExists(string roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
            throw new ArgumentException("roomName não pode ser vazio");

        string dataName = $"{roomName}.db";
        var path = Path.Combine(AppContext.BaseDirectory, dataName);
        string connectionString = $"Data Source={path}";

        return File.Exists(path); 
    }
}
