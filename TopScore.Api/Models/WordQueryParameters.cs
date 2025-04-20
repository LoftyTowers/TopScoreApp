namespace TopScore.Api.Models;

public class WordQueryParameters
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
