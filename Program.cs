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



// /api/posts/{id}/upvote
// This adds an upvote to a specific post
app.MapPut("/api/posts/{id}/upvote", (int id, DataService dataService) =>
{
    dataService.UpvotePost(id);
    return Results.Ok($"Upvoted post {id}");
});

// /api/posts/{id}/downvote
// This adds a downvote to a specific post
app.MapPut("/api/posts/{id}/downvote", (int id, DataService dataService) =>
{
    dataService.DownvotePost(id);
    return Results.Ok($"Downvoted post {id}");
});
// /api/posts/{postid}/comments/{commentid}/upvote
// This adds an upvote to a specific comment
app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (int postid, int commentid, DataService dataService) =>
{
    dataService.UpvoteComment(postid, commentid);
    return Results.Ok($"Upvoted comment {commentid} on post {postid}");
});
// /api/posts/{postid}/comments/{commentid}/downvote
// This adds a downvote to a specific comment
app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (int postid, int commentid, DataService dataService) =>
{
    dataService.DownvoteComment(postid, commentid);
    return Results.Ok($"Downvoted comment {commentid} on post {postid}");
});

// POST:
// /api/posts
// This adds a new post
app.MapPost("/api/posts", (Post post, DataService dataService) =>
{
    dataService.AddPost(post);
    return Results.Ok($"Added post {post.Title}");
});
// /api/posts/{id}/comments
// This adds a new comment to a specific post
app.MapPost("/api/posts/{id}/comments", (int id, Comment comment, DataService dataService) =>
{
    dataService.AddComment(id, comment);
    return Results.Ok($"Added comment {comment.Content} to post {id}");
});
app.MapGet("/", () => "Hello World!");

app.Run();
