using backend.Model.DTO;
using backend.Service;
using Microsoft.Data.Sqlite;
using System.Data;

namespace backend.Repository.SqLiteRepository
{
    public class TaskSqLiteRepository : IRepository<TaskDTO>
    {
        private readonly string _connectionString;

        public TaskSqLiteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        /// <summary>
        /// Mapeia um SqliteDataReader para um TaskDTO.
        /// </summary>
        private TaskDTO MapReaderToTask(SqliteDataReader reader)
        {
            return new TaskDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? null : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                ColumnId = reader.GetInt32(reader.GetOrdinal("ColumnId"))
            };
        }

        public async Task<TaskDTO> GetByIdAsync(int id)
        {
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT TaskId AS Id, Title, Description, Status, DueDate, ColumnId FROM Tasks WHERE TaskId = @Id";
            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToTask(reader);
            }

            throw new KeyNotFoundException($"Task with ID {id} not found.");
        }

        public async Task<List<TaskDTO>> GetAll()
        {
            var tasks = new List<TaskDTO>();
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT TaskId AS Id, Title, Description, Status, DueDate, ColumnId FROM Tasks ORDER BY TaskId";
            await using var command = new SqliteCommand(sql, connection);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tasks.Add(MapReaderToTask(reader));
            }

            return tasks;
        }

        public async Task<TaskDTO> PostAsync(TaskDTO objectToPost)
        {
            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            // Boa prática: Verificar se a ColumnId fornecida existe antes de inserir a task.
            var checkColumnSql = "SELECT COUNT(1) FROM Columns WHERE ColumnId = @ColumnId";
            await using var checkCmd = new SqliteCommand(checkColumnSql, connection);
            checkCmd.Parameters.AddWithValue("@ColumnId", objectToPost.ColumnId);
            if ((long)await checkCmd.ExecuteScalarAsync() == 0)
            {
                throw new KeyNotFoundException($"Cannot create task because Column with ID {objectToPost.ColumnId} does not exist.");
            }

            var sql = @"
                INSERT INTO Tasks (Title, Description, Status, DueDate, ColumnId) 
                VALUES (@Title, @Description, @Status, @DueDate, @ColumnId);
                SELECT last_insert_rowid();";

            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", objectToPost.Title);
            command.Parameters.AddWithValue("@Description", objectToPost.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Status", objectToPost.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DueDate", objectToPost.DueDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ColumnId", objectToPost.ColumnId);

            var newId = (long)await command.ExecuteScalarAsync();
            objectToPost.Id = (int)newId;

            return objectToPost;
        }

        public async Task<TaskDTO> PutAsync(TaskDTO objectToPut)
        {
            // Primeiro, garante que a task que se quer atualizar realmente existe.
            await GetByIdAsync(objectToPut.Id);

            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            // Também verificamos se a nova ColumnId é válida.
            var checkColumnSql = "SELECT COUNT(1) FROM Columns WHERE ColumnId = @ColumnId";
            await using var checkCmd = new SqliteCommand(checkColumnSql, connection);
            checkCmd.Parameters.AddWithValue("@ColumnId", objectToPut.ColumnId);
            if ((long)await checkCmd.ExecuteScalarAsync() == 0)
            {
                throw new KeyNotFoundException($"Cannot update task because Column with ID {objectToPut.ColumnId} does not exist.");
            }

            var sql = @"
                UPDATE Tasks SET 
                    Title = @Title, 
                    Description = @Description, 
                    Status = @Status, 
                    DueDate = @DueDate, 
                    ColumnId = @ColumnId 
                WHERE TaskId = @Id";

            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", objectToPut.Id);
            command.Parameters.AddWithValue("@Title", objectToPut.Title);
            command.Parameters.AddWithValue("@Description", objectToPut.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Status", objectToPut.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@DueDate", objectToPut.DueDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ColumnId", objectToPut.ColumnId);

            await command.ExecuteNonQueryAsync();

            return objectToPut;
        }

        public async Task<TaskDTO> DeleteAsync(int id)
        {
            // Busca a task primeiro para poder retorná-la e para verificar se ela existe.
            var taskToDelete = await GetByIdAsync(id);

            using var connection = (SqliteConnection)CreateConnection();
            await connection.OpenAsync();

            var sql = "DELETE FROM Tasks WHERE TaskId = @Id";
            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();

            return taskToDelete;
        }
    }
}