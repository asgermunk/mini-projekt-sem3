using Microsoft.EntityFrameworkCore;
using Service;
using Data;
using model;


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

//Get

app.MapGet("/api/posts", (DataService dataService) =>
{
    var posts = dataService.GetPosts();
    return Results.Ok(posts);
});

app.MapGet("/api/posts/{id}", (int id, DataService dataService) =>
{
    var posts = dataService.GetPostById(id);
    return Results.Ok(posts);
});


// /api/posts/{id}/upvote
// This adds an upvote to a specific post
app.MapPut("/api/posts/{id}/upvote", (int id, DataService dataService) =>
{
    return dataService.UpvotePost(id);

});

// /api/posts/{id}/downvote
// This adds a downvote to a specific post
app.MapPut("/api/posts/{id}/downvote", (int id, DataService dataService) =>
{
    return dataService.DownvotePost(id);

});
// /api/posts/{postid}/comments/{commentid}/upvote
// This adds an upvote to a specific comment
app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (int postid, int commentid, DataService dataService) =>
{
    System.Console.WriteLine("jeg rammer api kald");
    return dataService.UpvoteComment(postid, commentid);

});
// /api/posts/{postid}/comments/{commentid}/downvote
// This adds a downvote to a specific comment
app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (int postid, int commentid, DataService dataService) =>
{

    return dataService.DownvoteComment(postid, commentid);
});

// POST:
// /api/posts
// This adds a new post
app.MapPost("/api/posts", (Post post, DataService dataService) =>
{
    dataService.AddPost(post);
    return Results.Ok(post);
});
// /api/posts/{id}/comments
// This adds a new comment to a specific post
app.MapPost("/api/posts/{id}/comments", (int id, Comment comment, DataService dataService) =>
{
    dataService.AddComment(id, comment);
    return Results.Ok(comment);
});
app.MapGet("/", () => "Hello World!");

app.Run();
