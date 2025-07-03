using backend.Model.DTO;
using backend.Repository;
using backend.Repository.InMemoryRepository;
using backend.Repository.SqLiteRepository;
using backend.Service;
using backend.Service.ColumnDataServices;
using backend.Service.TaskService;

var builder = WebApplication.CreateBuilder(args);

string dataName = builder.Configuration.GetConnectionString("SqLiteConnection");
var path = Path.Combine(AppContext.BaseDirectory, dataName);
string connectionString = $"Data Source={path}";
var result = Setup.SetupDatabase(connectionString);
builder.Services.AddSingleton<IRepository<ColumnsDataDTO>>(sp => new ColumnSqLiteRepository(connectionString));
builder.Services.AddSingleton<IService<ColumnsDataDTO>, ColumnDataService>();
builder.Services.AddSingleton<IRepository<TaskDTO>>(sp => new TaskSqLiteRepository(connectionString));
builder.Services.AddSingleton<IService<TaskDTO>, TaskService>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
