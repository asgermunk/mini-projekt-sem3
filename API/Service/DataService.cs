using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using Data;
using shared.Model;
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
    public List<Post> UpvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == id);
        if (post != null)
        {
            post.Score++;
            db.SaveChanges();
        }
        return db.Posts.Include(b => b.Comments).ToList();
    }
    public List<Post> DownvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == id);
        if (post != null)
        {
            post.Score--;
            db.SaveChanges();
        }
        return db.Posts.Include(b => b.Comments).ToList();
    }
    public List<Comment> UpvoteComment(int postid, int commentid)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == postid);
        if (post != null)
        {
            var comment = post.Comments.FirstOrDefault(b => b.CommentId == commentid);
            if (comment != null)
            {
                comment.Score++;
                db.SaveChanges();
            }
        }
        return db.Comment.ToList();
    }
    public List<Comment> DownvoteComment(int postid, int commentid)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == postid);
        if (post != null)
        {
            var comment = post.Comments.FirstOrDefault(b => b.CommentId == commentid);
            if (comment != null)
            {
                comment.Score--;
                db.SaveChanges();
            }
        }
        return db.Comment.ToList();
    }
    public List<Post> AddPost(Post post)
    {
        db.Posts.Add(post);
        db.SaveChanges();
        return db.Posts.Include(b => b.Comments).ToList();
    }
    public List<Comment> AddComment(int id, Comment comment)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == id);
        if (post != null)
        {
            post.Comments.Add(comment);
            db.SaveChanges();
        }
        return db.Comment.ToList();
    }

    public List<Post> GetPosts()
    {
        return db.Posts.Include(b => b.Comments).ToList();
    }

    public Post GetPostById(int id)
    {
        return db.Posts.Include(b => b.Comments).FirstOrDefault(p => p.PostId == id)!;
    }
};