namespace TopScore.Core.Models;

public class ValidationResult
{
    public List<string> ValidCandidates { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}
