namespace shared
{
    public class Post
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public int Score { get; set; }

        public string? Author { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public DateTime Date { get; set; }

        public Post(string title, string content, string author, int score, DateTime date, List<Comment> comments)
        {
            Title = title;
            Content = content;
            Author = author;
            Score = score;
            Date = date;
            Comments = comments;
        }
        public Post()
        {
        }
    }
}