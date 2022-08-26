var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDIServices();

var app = builder.Build();

app.UseOpenApi();
app.UseEndpoints();
app.Run();

public partial class Program { }