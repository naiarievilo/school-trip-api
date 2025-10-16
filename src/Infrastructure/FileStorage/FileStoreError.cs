using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Infrastructure.FileStorage;

public class FileStoreError(string code, string description) : Error(code, description)
{
    private const string FileNotFoundCode = "FileStorageError.FileNotFound";
    private const string FailedToSaveFileCode = "FileStorageError.SaveFileFailed";
    private const string FailedToDeleteFileCode = "FileStorageError.FailedToDeleteFile";
    private const string FailedToRenameFileCode = "FileStorageError.FailedToRenameFile";

    public static Error FileNotFound(string fileName)
    {
        return new FileStoreError(FileNotFoundCode, $"File '{fileName}' was not found.");
    }

    public static Error FailedToSaveFile(string fileName)
    {
        return new FileStoreError(FailedToSaveFileCode, $"Failed to save file '{fileName}'.");
    }

    public static Error FailedToDeleteFile(string fileName)
    {
        return new FileStoreError(FailedToDeleteFileCode, $"Failed to delete file '{fileName}'.");
    }

    public static Error FailedToRenameFile(string fileName)
    {
        return new FileStoreError(FailedToRenameFileCode, $"Failed to rename file '{fileName}'.");
    }
}