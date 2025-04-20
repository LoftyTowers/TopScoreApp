using System.Text.RegularExpressions;

namespace TopScore.Core.Extensions
{
    /// <summary>
    /// Extension methods for validating words based on business rules.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Validates a word against the current business rules.
        /// </summary>
        /// <param name="word">The word to validate.</param>
        /// <returns>A list of validation error messages. If empty, the word is valid.</returns>
        /// <remarks>
        /// IMPORTANT: If you update any of the validation rules here,
        /// you must also update the corresponding messages in ValidationMessages.AllRules
        /// to keep user-facing feedback in sync.
        /// </remarks>
        public static List<string> GetValidationErrors(this string word)
        {
            var errors = new List<string>();

            if (word.Length < 8)
            {
                errors.Add("Must be at least 8 characters long.");
            }

            if (!word.Any(char.IsUpper))
            {
                errors.Add("Must contain at least one uppercase letter.");
            }

            if (!word.Any(char.IsLower))
            {
                errors.Add("Must contain at least one lowercase letter.");
            }

            if (!word.Any(char.IsDigit))
            {
                errors.Add("Must contain at least one digit.");
            }

            if (HasRepeatingCharacters(word))
            {
                errors.Add("Must not contain repeating characters.");
            }

            if (!word.All(char.IsLetterOrDigit))
            {
                errors.Add("Must only contain letters and digits.");
            }

            return errors;
        }

        /// <summary>
        /// Checks whether a word contains any repeating characters.
        /// </summary>
        /// <param name="word">The word to inspect.</param>
        /// <returns>True if the word has duplicate characters; otherwise, false.</returns>
        private static bool HasRepeatingCharacters(string word)
        {
            var seen = new HashSet<char>();
            foreach (var ch in word.ToLower())
            {
                if (!seen.Add(ch))
                    return true;
            }
            return false;
        }
    }
}
