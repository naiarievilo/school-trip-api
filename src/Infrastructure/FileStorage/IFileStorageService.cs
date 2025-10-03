using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Infrastructure.FileStorage;

public interface IFileStorageService
{
    public Task<Result> SaveFileAsync(byte[] file, string fileName);
    public Result DeleteFileAsync(string fileName);
    public Task<Result<byte[]>> GetFileAsync(string fileName);
    public Result RenameFileAsync(string oldFileName, string newFileName);
    public string GetFilePath(string fileName);
    public string GetFilesBasePath();
}