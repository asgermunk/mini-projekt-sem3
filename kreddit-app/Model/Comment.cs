namespace Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public int Score { get; set; }
        public string? Author { get; set; }

        public DateTime Date { get; set; }

        public Comment(string content, string author, int score, DateTime date)
        {
            Content = content;
            Author = author;
            Score = score;
            Date = date;

        }
        public Comment()
        {
        }

    }
}