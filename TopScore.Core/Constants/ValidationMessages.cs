namespace TopScore.Core.Constants;

/// <summary>
/// These are the current validation rules required for a word to be considered valid.
/// 
/// They are stored in code rather than external configuration because:
/// - The rules are closely tied to validation logic and unlikely to change often.
/// - Any rule change will likely require code changes anyway.
/// - Keeping them here keeps things simple and transparent for now.
/// 
/// If the validation rules grow significantly or need to be editable without redeployment,
/// consider moving them to a config file or localization resource.
/// </summary>
public static class ValidationMessages
{
    public static readonly string[] AllRules = new[]
    {
        "Must be at least 8 characters long.",
        "Must contain at least one uppercase letter.",
        "Must contain at least one lowercase letter.",
        "Must contain at least one digit.",
        "Must not contain repeating characters.",
        "Must only contain letters and digits."
    };
}
