<div align="center">

# 📁 FileManager API

**An educational ASP.NET Core Web API project for practicing secure file upload, storage, and management.**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF%20Core-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![FluentValidation](https://img.shields.io/badge/FluentValidation-enabled-6DB33F)](https://docs.fluentvalidation.net/)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-3.1-85EA2D?logo=swagger&logoColor=black)](https://swagger.io/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](#license)

</div>

---

## Overview

**FileManager API** is a practice project built to apply core backend concepts in ASP.NET Core: file handling, request validation, EF Core persistence, and clean layering between controllers, services, and data access.

The focus of this project is **secure file handling** — every uploaded file is checked for size, name format, and binary signature before it's saved to disk.

---

## ✨ Key Features

| Capability | Description |
|---|---|
| 🔐 **Secure Uploads** | File size limits, name pattern checks, and binary signature blocklisting (blocks `.exe`, `.js`, `.msi` disguised as other types) |
| 📤 **Single & Bulk Upload** | Upload one file or a full batch in a single request |
| 🖼️ **Image Upload** | Dedicated endpoint restricted to safe image extensions (`.jpg`, `.jpeg`, `.png`) |
| ⬇️ **Download & Streaming** | Full download for small files, range-enabled streaming for large media (supports seeking) |
| 🔎 **Search, Sort & Paginate** | List endpoint with dynamic sorting, filtering, and pagination, with safe fallbacks on invalid input |
| 🔁 **Replace In-Place** | Swap a file's content while keeping its Id, with automatic cleanup of the old physical file |
| 🧾 **Metadata API** | Retrieve file details (name, size, type, upload date) without downloading the content |

---

## 🧱 Tech Stack

- **ASP.NET Core Web API** (.NET 10)
- **Entity Framework Core** + SQL Server — file metadata persistence
- **FluentValidation** — declarative, composable request validation
- **OpenAPI 3.1 / Swagger UI** — interactive API documentation

---

## 🏗️ Architecture & Design Decisions

The project follows a layered architecture with a clear separation of responsibilities:

- **Controllers** — thin HTTP layer; only route requests and shape responses, no business logic
- **Services** (`IFileService` / `FileService`) — own the actual file-handling logic (saving, reading, deleting, replacing) and coordinate between disk storage and the database
- **Validation** — request DTOs are validated declaratively via **FluentValidation** *before* they ever reach the service layer, keeping cross-cutting rules (size, name, signature) reusable and testable in isolation
- **Persistence** — EF Core with per-entity `IEntityTypeConfiguration` classes, keeping the `DbContext` clean and configuration colocated per entity

**Notable decisions:**

| Decision | Rationale |
|---|---|
| Store files under a **randomized name** on disk | Prevents name collisions and avoids exposing the original file name/path externally |
| Validate the **binary signature**, not just the extension | A renamed `.exe` still has to pass content inspection — extension alone isn't trustworthy |
| Reusable `AbstractValidator<IFormFile>` rules (`FileSizeValidator`, `FileNameValidator`, `BlockedSignaturesValidator`) | Same validation logic is composed across single-file, multi-file, and image upload requests without duplication |
| `Stream` endpoint uses range-enabled `FileStreamResult` | Supports partial content requests (e.g. seeking in audio/video) instead of loading the whole file into memory |
| Sort/pagination inputs fall back to safe defaults instead of throwing | Prevents invalid query strings from breaking the list endpoint |

---

## 📡 API Reference

### Files

| Method | Endpoint | Description |
|:---:|---|---|
| `POST` | `/api/files/upload` | Upload a single file |
| `POST` | `/api/files/upload-many` | Upload multiple files in one request |
| `POST` | `/api/files/upload-image` | Upload an image (extension-restricted) |
| `GET` | `/api/files/download/{id}` | Download a file by Id |
| `GET` | `/api/files/stream/{id}` | Stream a file by Id (range-enabled) |
| `GET` | `/api/files/{id}` | Get file metadata by Id |
| `GET` | `/api/files` | List files — supports search, sort, and pagination |
| `PUT` | `/api/files/{id}` | Replace a file's content |
| `DELETE` | `/api/files/{id}` | Delete a file by Id |

---

## 🛡️ Validation Rules

Configured centrally in `FileSettings`:

| Rule | Value |
|---|---|
| Max file size | `1 MB` |
| Blocked binary signatures | `4D-5A` (.exe), `2F-2A` (.js), `D0-CF` (.msi) |
| Allowed image extensions | `.jpg`, `.jpeg`, `.png` |
| File name pattern | Letters, digits, spaces, `_ - ( )`, followed by a 2–10 character extension |

Every upload passes through:
1. **Size validation** — rejects files above the configured limit
2. **Signature validation** — inspects the first bytes of the file to block disguised executables
3. **Name validation** — enforces a safe, predictable naming pattern

Files are stored on disk under a **randomly generated name** (`Path.GetRandomFileName()`), decoupling the physical file from the user-supplied name — preventing collisions and avoiding exposure of the original file name on the server.

---

## 🗄️ Data Model

**`Files` table** (`UploadedFiles` entity):

| Column | Type | Description |
|---|---|---|
| `Id` | `Guid` | Primary key (`Guid.CreateVersion7()`) |
| `FileName` | `string(250)` | Original file name (display only) |
| `StoredFileName` | `string(250)` | Random name used on disk |
| `ContentType` | `string(50)` | MIME type |
| `FileExtension` | `string(10)` | File extension |
| `FileSize` | `long` | Size in bytes |
| `UploadedAt` | `DateTime` | UTC upload timestamp |

---

## 🚀 Getting Started

**Prerequisites:** .NET 10 SDK, SQL Server

```bash
# 1. Configure your connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=FileManagerDb;Trusted_Connection=True;"
}

# 2. Apply EF Core migrations
dotnet ef database update

# 3. Run the API
dotnet run
```

Once running in Development, browse to `/swagger` for the interactive API console.

---

## 🔭 Roadmap / Suggested Improvements

- [ ] Guard `Download` / `Stream` / `Delete` against a missing physical file when the DB record still exists
- [ ] Ensure `wwwroot/uploads` and `wwwroot/images` are created on startup
- [ ] Persist `UploadImage` metadata to the database and apply the same random-naming strategy used elsewhere
- [ ] Add authentication/authorization before exposing the API publicly

---

## 📄 License

This project is licensed under the MIT License.

---

<div align="center">

**Made with ♥ By Eng:-Abdalla Aboaziz**

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Abdalla%20Aboaziz-0A66C2?logo=linkedin&logoColor=white)](https://www.linkedin.com/in/abdalla-aboaziz-13a513331/)
[![Gmail](https://img.shields.io/badge/Gmail-abdallaaboaziz%40gmail.com-D14836?logo=gmail&logoColor=white)](mailto:abdallaaboaziz@gmail.com)

</div>
