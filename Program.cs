var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
options.AddPolicy(name: AllowSomeStuff, builder => {
builder.AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod();
    });
});
// ...
app.UseCors(AllowSomeStuff);





app.MapGet("/", () => "Hello World!");

app.Run();
