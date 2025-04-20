using TopScore.Core.Models;

namespace TopScore.Core.Interfaces;

public interface IWordValidator
{
    /// <summary>
    /// Returns the longest valid word in the sentence, or null if none with a list of reasons why validation failed.
    /// </summary>
    ValidationResult ValidateSentence(string sentence);

}
