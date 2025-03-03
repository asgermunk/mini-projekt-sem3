using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using Data;
using model;
using System.Security.Cryptography.X509Certificates;

namespace Service;

public class DataService
{
    private PostContext db { get; }

    public DataService(PostContext db)
    {
        this.db = db;
    }
    /// <summary>
    /// Seeder noget nyt data i databasen hvis det er nødvendigt.
    /// </summary>
    public void SeedData()
    {

        Post Post = db.Posts.FirstOrDefault()!;
        if (Post == null)
        {
            Post = new Post("Title1", "Content", "Author", 0, DateTime.Now, new List<Comment> { new Comment("Comment", "Author", 0, DateTime.Now) });
            db.Posts.Add(Post);
            db.Posts.Add(new Post("Title2", "Content", "Author", 0, DateTime.Now, new List<Comment> { new Comment("Comment", "Author", 0, DateTime.Now) }));
            db.Posts.Add(new Post("Title3", "Content", "Author", 0, DateTime.Now, new List<Comment> { new Comment("Comment", "Author", 0, DateTime.Now) }));
        }
        db.SaveChanges();
    }
    public List<Post> GetPosts()
    {
        return db.Posts.Include(b => b.Comments).ToList();
    }
};