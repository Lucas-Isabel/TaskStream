using System;
using Microsoft.Data.Sqlite;

public static class Setup
{
    public static bool SetupDatabase(string connectionString)
    {
        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Criação da tabela Columns com ColumnId auto-incrementado
            var createColumnsCmd = connection.CreateCommand();
            createColumnsCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Columns (
                    ColumnId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL
                );
            ";
            createColumnsCmd.ExecuteNonQuery();

            // Faz um insert fake para garantir que sqlite_sequence será criada
            var tempInsertCmd = connection.CreateCommand();
            tempInsertCmd.CommandText = "INSERT INTO Columns (Title) VALUES ('__seed__');";
            tempInsertCmd.ExecuteNonQuery();

            // Atualiza o valor do autoincremento para começar em 1000
            var setSeqCmd = connection.CreateCommand();
            setSeqCmd.CommandText = "UPDATE sqlite_sequence SET seq = 999 WHERE name = 'Columns';";
            setSeqCmd.ExecuteNonQuery();

            // Remove o insert fake
            var deleteTempCmd = connection.CreateCommand();
            deleteTempCmd.CommandText = "DELETE FROM Columns WHERE Title = '__seed__';";
            deleteTempCmd.ExecuteNonQuery();

            // Criação da tabela Tasks com TaskId auto-incrementado
            var createTasksCmd = connection.CreateCommand();
            createTasksCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status TEXT,
                    DueDate TEXT,
                    ColumnId INTEGER NOT NULL,
                    FOREIGN KEY(ColumnId) REFERENCES Columns(ColumnId) ON DELETE CASCADE
                );
            ";
            createTasksCmd.ExecuteNonQuery();

            // Mesmo processo para Tasks (seed = 1000)
            var tempTaskInsertCmd = connection.CreateCommand();
            tempTaskInsertCmd.CommandText = "INSERT INTO Tasks (Title, Description, Status, DueDate, ColumnId) VALUES ('__seed__', '', '', '', 1000);";
            tempTaskInsertCmd.ExecuteNonQuery();

            var setTaskSeqCmd = connection.CreateCommand();
            setTaskSeqCmd.CommandText = "UPDATE sqlite_sequence SET seq = 999 WHERE name = 'Tasks';";
            setTaskSeqCmd.ExecuteNonQuery();

            var deleteTempTaskCmd = connection.CreateCommand();
            deleteTempTaskCmd.CommandText = "DELETE FROM Tasks WHERE Title = '__seed__';";
            deleteTempTaskCmd.ExecuteNonQuery();

            connection.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar banco de dados: {ex.Message}");
            return false;
        }
    }
}