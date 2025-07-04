using backend.Model.DTO;
using backend.Service; // Supondo que a interface IRepository esteja aqui
using Microsoft.Data.Sqlite;
using System.Data;

namespace backend.Repository.SqliteDynamicRepository
{
    public class ColumnSqLiteRepository : IRepository<ColumnsDataDTO>
    {
        private readonly string _connectionString;

        public ColumnSqLiteRepository(string roomName)
        {
            _connectionString = DynamicSetup.SetupDatabase(roomName);
        }

        // O método CreateConnection permanece o mesmo, pois já usa Microsoft.Data.Sqlite
        private IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public async Task<ColumnsDataDTO> GetByIdAsync(int id)
        {
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT ColumnId AS Id, Title FROM Columns WHERE ColumnId = @Id";
            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ColumnsDataDTO
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Tasks = new List<TaskDTO>() // Inicializa a lista de tarefas
                };
            }

            // Se o reader não encontrar nenhuma linha, o item não existe.
            throw new KeyNotFoundException($"Column with ID {id} not found.");
        }

        public async Task<List<ColumnsDataDTO>> GetAll()
        {
            var columnDictionary = new Dictionary<int, ColumnsDataDTO>();

            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    c.ColumnId,
                    c.Title AS ColumnTitle,
                    t.TaskId,
                    t.Title AS TaskTitle,
                    t.Description,
                    t.Status,
                    t.DueDate,
                    t.ColumnId AS TaskColumnId
                FROM Columns c
                LEFT JOIN Tasks t ON t.ColumnId = c.ColumnId
                ORDER BY c.ColumnId, t.TaskId;";

            // Correção: Usar SqliteCommand em vez de SqlCommand
            await using var command = new SqliteCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var columnId = reader.GetInt32(reader.GetOrdinal("ColumnId"));

                // Se a coluna ainda não foi adicionada ao dicionário, adicione-a.
                if (!columnDictionary.TryGetValue(columnId, out var column))
                {
                    column = new ColumnsDataDTO
                    {
                        Id = columnId,
                        Title = reader.GetString(reader.GetOrdinal("ColumnTitle")),
                        Tasks = new List<TaskDTO>()
                    };
                    columnDictionary.Add(columnId, column);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("TaskId")))
                {
                    var task = new TaskDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("TaskId")),
                        Title = reader.GetString(reader.GetOrdinal("TaskTitle")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                        DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        ColumnId = reader.IsDBNull(reader.GetOrdinal("TaskColumnId")) ? null : reader.GetInt32(reader.GetOrdinal("TaskColumnId"))
                    };
                    column.Tasks.Add(task);
                }
            }

            return columnDictionary.Values.ToList();
        }

        public async Task<ColumnsDataDTO> PostAsync(ColumnsDataDTO objectToPost)
        {
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = "INSERT INTO Columns (Title) VALUES (@Title); SELECT last_insert_rowid();";

            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", objectToPost.Title);

            
            var newId = (long)await command.ExecuteScalarAsync();
            objectToPost.Id = (int)newId;

            return objectToPost;
        }

        public async Task<ColumnsDataDTO> PutAsync(ColumnsDataDTO objectToPut)
        {
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            
            var checkSql = "SELECT COUNT(1) FROM Columns WHERE ColumnId = @Id";
            await using var checkCommand = new SqliteCommand(checkSql, connection);
            checkCommand.Parameters.AddWithValue("@Id", objectToPut.Id);

            var exists = (long)await checkCommand.ExecuteScalarAsync() > 0;
            if (!exists)
            {
                throw new KeyNotFoundException($"Column with ID {objectToPut.Id} not found.");
            }

            var updateSql = "UPDATE Columns SET Title = @Title WHERE ColumnId = @Id";
            await using var updateCommand = new SqliteCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@Title", objectToPut.Title);
            updateCommand.Parameters.AddWithValue("@Id", objectToPut.Id);

            await updateCommand.ExecuteNonQueryAsync();

            return objectToPut;
        }

        public async Task<ColumnsDataDTO> DeleteAsync(int id)
        {
            
            var itemToDelete = await GetByIdAsync(id);

            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            

            var deleteColumnSql = "DELETE FROM Columns WHERE ColumnId = @Id";
            await using var command = new SqliteCommand(deleteColumnSql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();

            return itemToDelete;
        }
    }
}