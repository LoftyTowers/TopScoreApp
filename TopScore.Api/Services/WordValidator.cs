using Microsoft.Extensions.Logging;
using TopScore.Core.Extensions;
using TopScore.Core.Interfaces;
using TopScore.Core.Models;
using TopScore.Core.Constants;

namespace TopScore.Api.Services;

/// <summary>
/// Provides logic for validating a sentence and extracting the longest valid word
/// based on defined business rules.
/// </summary>
public class WordValidator : IWordValidator
{
    private readonly ILogger<WordValidator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordValidator"/> class.
    /// </summary>
    /// <param name="logger">Logger for capturing validation diagnostics.</param>
    public WordValidator(ILogger<WordValidator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Validates a sentence to extract the longest valid word based on the following criteria:
    /// <list type="bullet">
    /// <item>Word must be between 8 and 500 characters long.</item>
    /// <item>Must include at least one uppercase letter, one lowercase letter, and one digit.</item>
    /// <item>Must not contain repeating characters.</item>
    /// <item>Must only include letters and digits (no punctuation or symbols).</item>
    /// </list>
    /// </summary>
    /// <param name="sentence">The input sentence to evaluate.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> containing a list of valid candidate words,
    /// or a list of validation rules that must be satisfied if no valid words were found.
    /// </returns>
    public ValidationResult ValidateSentence(string sentence)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(sentence))
        {
            _logger.LogWarning("Received an empty or whitespace-only sentence.");
            result.Errors.Add("Sentence must not be empty.");
            return result;
        }

        if (sentence.Length > 500)
        {
            _logger.LogWarning("Received sentence exceeds max length.");
            result.Errors.Add("Sentence must not exceed 500 characters.");
            return result;
        }

        var words = sentence
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .OrderByDescending(w => w.Length)
            .ToList();

        int? validLength = null;

        foreach (var word in words)
        {
            if (validLength != null && word.Length < validLength)
                break;

            var errors = word.GetValidationErrors();
            if (!errors.Any())
            {
                result.ValidCandidates.Add(word);
                validLength ??= word.Length; // set only once, when first valid word is found
            }
            else
            {
                _logger.LogInformation("Rejected word: {Word}\n{Errors}", word, string.Join("\n", errors));
            }
        }

        if (!result.ValidCandidates.Any())
        {
            result.Errors = ValidationMessages.AllRules.ToList();
            _logger.LogInformation("No valid words found in sentence.");
        }

        return result;
    }
}
