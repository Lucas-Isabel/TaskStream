using backend.Model.DTO;
using backend.Repository;
using backend.Repository.InMemoryRepository;
using backend.Repository.RoomRepository;
using backend.Repository.SqliteDynamicRepository;
using backend.Service;
using backend.Service.ColumnDataServices;
using backend.Service.RoomsServices;
using backend.Service.TaskService;

var builder = WebApplication.CreateBuilder(args);

string dataName = builder.Configuration.GetConnectionString("SqLiteConnection");
var path = Path.Combine(AppContext.BaseDirectory, dataName);
string connectionString = $"Data Source={path}";
var result = Setup.SetupDatabase(connectionString);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRepository<TaskDTO>>(sp =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var roomName = httpContext?.Request.RouteValues["roomName"]?.ToString();

    if (string.IsNullOrWhiteSpace(roomName))
        throw new Exception("roomName não foi fornecido na URL.");

    return new TaskSqLiteRepository(roomName);
});

builder.Services.AddScoped<IService<TaskDTO>, TaskService>();


builder.Services.AddScoped<IRepository<ColumnsDataDTO>>(sp =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var roomName = httpContext?.Request.RouteValues["roomName"]?.ToString();

    if (string.IsNullOrWhiteSpace(roomName))
        throw new Exception("roomName não foi fornecido na URL.");

    return new ColumnSqLiteRepository(roomName);
});

builder.Services.AddScoped<IService<ColumnsDataDTO>, ColumnDataService>();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomServices, RoomService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
