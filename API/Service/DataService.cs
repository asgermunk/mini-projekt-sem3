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
    public Post UpvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == id);
        if (post != null)
        {
            post.Score++;
            db.SaveChanges();
        }
        return post;
    }
    public Post DownvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(b => b.PostId == id);
        if (post != null)
        {
            post.Score--;
            db.SaveChanges();
        }
        return post;
    }
    public Comment UpvoteComment(int postid, int commentid)
    {
        System.Console.WriteLine(postid + "something");
        System.Console.WriteLine(commentid + "something");
        var post = db.Posts.Include(p => p.Comments).FirstOrDefault(b => b.PostId == postid);
        if (post != null)
        {

            System.Console.WriteLine("we found a post");

            var comment = post.Comments.FirstOrDefault(b => b.CommentId == commentid);
            if (comment != null)
            {
                System.Console.WriteLine("we found a comment");
                System.Console.WriteLine(comment.Score);
                comment.Score++;

                db.SaveChanges();
            }
        }
        return post.Comments.FirstOrDefault(b => b.CommentId == commentid);
    }
    public Comment DownvoteComment(int postid, int commentid)
    {
        var post = db.Posts.Include(p => p.Comments).FirstOrDefault(b => b.PostId == postid);
        if (post != null)
        {
            var comment = post.Comments.FirstOrDefault(b => b.CommentId == commentid);
            if (comment != null)
            {
                comment.Score--;
                db.SaveChanges();
            }
        }
        return post.Comments.FirstOrDefault(b => b.CommentId == commentid);
    }
    public Post AddPost(Post post)
    {
        db.Posts.Add(post);
        db.SaveChanges();
        return post;
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