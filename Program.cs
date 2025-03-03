using Microsoft.EntityFrameworkCore;
using Service;
using Data;



var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddDbContext<Data.PostContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=posts.db"));

// Register DataService
builder.Services.AddScoped<DataService>();

var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder =>
    {
        builder.AllowAnyOrigin()
             .AllowAnyHeader()
             .AllowAnyMethod();
    });
});
// ...
var app = builder.Build();
app.UseCors(AllowSomeStuff);
//Seed data
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}
// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


// /api/posts/{id}/upvote
// This adds an upvote to a specific post
// /api/posts/{id}/upvote
// This adds an upvote to a specific post
app.MapPost("/api/posts/{id}/upvote", (int id, DataService dataService) =>
{
    dataService.UpvotePost(id);
    return Results.Ok($"Upvoted post {id}");
});
// /api/posts/{id}/downvote
// /api/posts/{postid}/comments/{commentid}/upvote
// /api/posts/{postid}/comments/{commentid}/downvote

app.MapGet("/", () => "Hello World!");

app.Run();
