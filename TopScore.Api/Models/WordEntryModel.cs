namespace TopScore.Api.Models;

public class WordEntryModel
{
    public int Id { get; set; }
    public string Word { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
