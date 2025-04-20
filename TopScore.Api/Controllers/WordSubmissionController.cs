using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopScore.Core.Interfaces;
using TopScore.Core.Models;
using TopScore.Data.Context;
using TopScore.Api.Models;

namespace TopScore.Api.Controllers;

/// <summary>
/// Handles endpoints related to word submission, retrieval, and clearing.
/// </summary>
[ApiController]
[Route("api/words")]
public class WordSubmissionController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<WordSubmissionController> _logger;
    private readonly IWordValidator _validator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordSubmissionController"/> class.
    /// </summary>
    public WordSubmissionController(AppDbContext db, ILogger<WordSubmissionController> logger, IWordValidator validator, IMapper mapper)
    {
        _db = db;
        _logger = logger;
        _validator = validator;
        _mapper = mapper;
    }

    /// <summary>
    /// Accepts a sentence and stores the longest valid word based on business rules.
    /// </summary>
    /// <param name="input">The sentence input.</param>
    /// <returns>Returns the created word entry or an error if validation fails.</returns>
    [HttpPost]
    public async Task<IActionResult> SubmitSentence([FromBody] SentenceInputModel input)
    {
        try
        {
            var sentence = input.Sentence;
            if (string.IsNullOrWhiteSpace(sentence))
            {
                _logger.LogWarning("Received an empty or whitespace-only sentence.");
                return BadRequest(new { errors = new[] { "Sentence cannot be empty." } });
            }

            _logger.LogInformation("Received sentence: {Sentence}", sentence);
            var validationResult = _validator.ValidateSentence(sentence);

            if (validationResult.ValidCandidates == null || !validationResult.ValidCandidates.Any())
            {
                _logger.LogWarning("No valid word found in: {Sentence}", sentence);
                return BadRequest(new { errors = validationResult.Errors });
            }

            foreach (var word in validationResult.ValidCandidates)
            {
                if (!await _db.WordEntries.AnyAsync(w => w.Word.ToLower() == word.ToLower()))
                {
                    var wordEntry = new WordEntry { Word = word };
                    _db.WordEntries.Add(wordEntry);
                    await _db.SaveChangesAsync();

                    _logger.LogInformation("New word saved: {Word}", word);
                    var result = _mapper.Map<WordEntryModel>(wordEntry);
                    return Ok(new { word = result.Word });
                }
            }

            _logger.LogInformation("All valid words already exist in the database.");
            return Conflict("That word has already been submitted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing sentence submission.");
            return StatusCode(500, "Something went wrong.");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of saved words optionally filtered by search term.
    /// </summary>
    /// <param name="query">The query parameters including search term, page, and page size.</param>
    /// <returns>A list of matching word entries with pagination metadata.</returns>
    [HttpGet]
    public async Task<IActionResult> GetWords([FromQuery] WordQueryParameters query)
    {
        try
        {
            var dbQuery = _db.WordEntries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                dbQuery = dbQuery.Where(w => w.Word.ToLower().Contains(query.Search.ToLower()));
            }

            var totalCount = await dbQuery.CountAsync();

            var words = await dbQuery
                .OrderByDescending(w => w.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var results = _mapper.Map<List<WordEntryModel>>(words);

            return Ok(new
            {
                total = totalCount,
                page = query.Page,
                pageSize = query.PageSize,
                items = results
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving word list.");
            return StatusCode(500, "Something went wrong.");
        }
    }

    /// <summary>
    /// Deletes all word entries from the database.
    /// </summary>
    /// <returns>Returns 200 OK if successful or 204 No Content if the database was already empty.</returns>
    [HttpDelete]
    public async Task<IActionResult> ClearAllWords()
    {
        try
        {
            _logger.LogWarning("Clearing all words from the database...");

            var allWords = await _db.WordEntries.ToListAsync();

            if (!allWords.Any())
            {
                return NoContent();
            }

            _db.WordEntries.RemoveRange(allWords);
            await _db.SaveChangesAsync();

            _logger.LogInformation("All words deleted.");
            return Ok("All words deleted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing the word database.");
            return StatusCode(500, "Something went wrong.");
        }
    }
}
