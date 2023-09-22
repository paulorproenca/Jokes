namespace Jokes.Models;

public class Joke
{
    public long Id { get; set; }
    public Category? Category {get; set;}
    public string? Text { get; set; }
}