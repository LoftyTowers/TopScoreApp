# TopScore Technical Test

This is a full-stack solution for a sentence validation and word scoring system. It was built to demonstrate clean architecture, robust validation, user feedback, and production-readiness (within scope). The project includes an ASP.NET Core Web API backend and an Angular frontend.

---

## Project Overview

Users submit a sentence, and the application extracts and stores the longest valid word based on a strict set of rules. The UI provides instant feedback, and a searchable, sortable, paginated table allows review of saved entries.

---

## Technologies Used

| Layer        | Tech Stack                                      |
|--------------|--------------------------------------------------|
| Frontend     | Angular 17, SCSS, TypeScript                    |
| Backend      | ASP.NET Core 9.0, Entity Framework Core (SQLite) |
| Logging      | Serilog (console and daily log files)           |
| Database     | SQLite                                          |
| Dev Tools    | Swagger, Visual Studio 2022 |

---

## Architecture Summary

- **Backend** is cleanly structured into `Core`, `Data`, and `Api` projects.
- **Frontend** uses Angular standalone components with SSR support.
- Validation rules are separated into extensions and constants for reuse.
- Logging is configured using Serilog and persisted to `Logs/log-<date>.txt`.
- API is RESTful and fully tested.

---

## Setup Instructions

> Prerequisites: .NET 9 SDK+, Node.js 18+, Visual Studio 2022, Angular CLI

### 📦 Backend (API)
1. Open solution in **Visual Studio 2022**
2. Set `TopScore.Api` as **startup project**
3. Run the API (F5 or `dotnet run` from `TopScore.Api`)
4. Swagger is available at `http://localhost:5210/swagger`

### 🌐 Frontend (Angular)
1. Open terminal in `top-score-ui/`
2. Run: `npm install`
3. Start app: `npx serve dist/top-score-ui/browser` (after build)
4. Or develop live: `ng serve`

> Ensure backend is running first before submitting from the frontend.

---

## Key Features

- Sentence validation based on strict rules
- Saves longest valid, unique word per sentence
- Search, sort, and pagination in frontend
- Submit feedback messages with saved word
- Full logging of all requests and validations
- Swagger support for API documentation

---

## Validation Rules

A word is considered **valid** if it:

- Is at least 8 characters long
- Contains at least **one uppercase** letter
- Contains at least **one lowercase** letter
- Contains at least **one digit**
- Has **no repeating characters**
- Contains **only letters and digits** (no punctuation)

The sentence itself must also be **no longer than 500 characters**.

---

## API Endpoints

| Method | Route            | Description                              |
|--------|------------------|------------------------------------------|
| POST   | `/api/words`     | Submits a sentence and stores valid word |
| GET    | `/api/words`     | Retrieves paginated list of saved words  |
| DELETE | `/api/words`     | Clears all words (via Swagger only)      |

---

## Developer Convenience Endpoint

The /api/words [DELETE] endpoint exists as a convenience method during development and testing. While not required in the spec, it allowed for quickly resetting the dataset while validating scenarios.

In a real production environment, this endpoint would either:

- Be removed entirely, or
- Be secured behind authentication and authorization controls.

It is intentionally excluded from the frontend UI.

---

## Testing Checklist

### 1. Sentence Validation Rules

| Input                                                                                                  | Expected Result                                  |
|--------------------------------------------------------------------------------------------------------|--------------------------------------------------|
| "TopScore M HJkl6789 Qwerty123 Test One"                                                               | ✅ Saves Qwerty123                                |
| "AbcdefghIjklmnoPqrstuvwxyz1234567890 this is valid"                                                   | ✅ Saves AbcdefghIjklmnoPqrstuvwxyz1234567890     |
| ""Try this valid word: XyZ19876"                                                                       | ✅ Saves: XyZ19876                                |
| "Mix: Abc123DE Yz9KlMn8"                                                                               | ✅ Saves: Yz9KlMn8                                |
| "Multiple: A1b2c3d4 W9xY7zT5"                                                                          | ✅ Saves: A1b2c3d4                                |
| "This has many: R8uT6eK3 A3l9NpO0 Z1xC5vBh"                                                            | ✅ Saves: R8uT6eK3                                |
| "short"                                                                                                | ❌ Error: “Must be at least 8 characters long.”   |
| "abcdefghij"                                                                                           | ❌ Error: “Must contain at least one uppercase letter.” + digit |
| "ABCDEFGH"                                                                                             | ❌ Error: “Must contain at least one lowercase letter.” + digit |
| "AaaaAAA111"                                                                                           | ❌ Error: “Must not contain repeating characters.” |
| "Valid123!"                                                                                            | ❌ Error: “Must only contain letters and digits.” |
| Long sentence > 500 chars                                                                              | ❌ Error: “Sentence must not exceed 500 characters.” |

### 2. Duplicate Word Check

| Input                                  | Expected Result                     |
|----------------------------------------|-------------------------------------|
| "Try word Alpha789"                    | ✅ Success                           |
| "This contains Alpha789"              | ❌ Error: Already submitted (409)    |

### 3. Case Sensitivity

| Input                                  | Expected Result                     |
|----------------------------------------|-------------------------------------|
| "Qwerty098"                            | ✅ Success                           |
| "qWERTy098"                            | ❌ Conflict: Already submitted       |

### 4. Case-Insensitive Search

| Search Term | Expected Match           |
|-------------|--------------------------|
| qwerty      | ✅ Qwerty123 and Qwerty098 |
| QWERTY      | ✅ Qwerty123 and Qwerty098 |

### 5. Paginated Display

| Action                        | Expected Outcome               |
|------------------------------|--------------------------------|
| Visit display page           | ✅ Table shows saved entries    |
| Submit new sentence          | ✅ Word appears in table        |
| Change page size             | ✅ Rows per page update         |
| Use pagination buttons       | ✅ Navigate pages (prev/next)   |

### 6. Sorting

| Action                   | Expected Outcome              |
|--------------------------|-------------------------------|
| Click “Word”             | ✅ Sort A-Z then Z-A           |
| Click “Created At”       | ✅ Newest → Oldest             |
| Click “Length”           | ✅ Shortest → Longest          |

### 7. Feedback Messages

| Action                        | Message                             |
|------------------------------|-------------------------------------|
| Submit valid                 | ✅ Green success + word shown        |
| Submit invalid               | ❌ Red failure + validation list     |

### 8. Multiple Valid Words (Same Length)

| Input                                                  | Result                     |
|--------------------------------------------------------|----------------------------|
| "ValidW1 ValidW2 ValidW3"                              | ✅ Adds first one not in DB |
| If ValidW1 already in DB                               | ✅ Tries ValidW2, ValidW3   |

---

## Design Notes

- Trailing punctuation **is not** stripped; words with it will fail validation.
- A max sentence length of 500 characters ensures performance and simplicity.
- Logging is written to `Logs/` daily and includes all errors and accepted/rejected words.

---

© Submitted by Peter Gunary for the TopScore Technical Test.