var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
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
app.UseCors(AllowSomeStuff);



// /api/posts/{id}/upvote
app.MapPost
// /api/posts/{id}/downvote
// /api/posts/{postid}/comments/{commentid}/upvote
// /api/posts/{postid}/comments/{commentid}/downvote

app.MapGet("/", () => "Hello World!");

app.Run();
