using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Provides static methods for file operations like Copy, Move and Delete
/// </summary>
public static class IOHelper
{
    #region Attibutes

    public static void InsertAttribute(ref FileAttributes attributesFlags, FileAttributes insert)
    {
        attributesFlags |= insert;
    }

    public static void RemoveAttribute(ref FileAttributes attributesFlags, FileAttributes remove)
    {
        attributesFlags &= ~remove;
    }

    /// <summary>
    /// Removes the <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see> from the file
    /// </summary>
    /// <param name="filePath">Is the path of the file</param>
    public static void RemoveReadOnlyAttribute(string filePath)
    {
        var attributes = File.GetAttributes(filePath);

        RemoveAttribute(ref attributes, FileAttributes.ReadOnly);

        File.SetAttributes(filePath, attributes);
    }

    /// <summary>
    /// Inserts the <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see> from the file
    /// </summary>
    /// <param name="filePath">Is the path of the file</param>
    public static void InsertReadOnlyAttribute(string filePath)
    {
        var attributes = File.GetAttributes(filePath);

        InsertAttribute(ref attributes, FileAttributes.ReadOnly);

        File.SetAttributes(filePath, attributes);
    }

    #endregion Attibutes

    #region File Operations

    /// <summary>
    /// Moves an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static ValidationResult MoveFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        var validationResult = new ValidationResult();

        try
        {
            RemoveReadOnlyAttribute(sourceFileName);

            var copyResult = CopyFile(sourceFileName, destFileName, setAsReadOnly);
            if (copyResult.IsValid)
                _ = DeleteFile(sourceFileName);
            else
                validationResult.AddError($"Error moving file: {copyResult.Errors[0].ErrorMessage}");
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex);
        }

        return validationResult;
    }

    /// <summary>
    /// Moves or overwrite an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// If destination file exists, will be deleted.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static ValidationResult MoveOrOverwriteFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        var deleteResult = DeleteFile(destFileName);

        return deleteResult.IsValid
            ? MoveFile(sourceFileName, destFileName, setAsReadOnly)
            : deleteResult;
    }

    /// <summary>
    /// Deletes the specified file if exists. Ignores <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted. Wildcard characters are not supported.</param>
    /// <returns></returns>
    public static ValidationResult DeleteFile(string fileName)
    {
        var validationResult = new ValidationResult();
        try
        {
            if (File.Exists(fileName))
            {
                RemoveReadOnlyAttribute(fileName);
                File.Delete(fileName);
            }
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex);
        }

        return validationResult;
    }

    /// <summary>
    /// Copies an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static ValidationResult CopyFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        var validationResult = new ValidationResult();

        if (!File.Exists(destFileName))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                File.Copy(sourceFileName, destFileName);
                if (setAsReadOnly) InsertReadOnlyAttribute(destFileName);
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex);
            }
        }
        else
        {
            validationResult.AddError("Destination file already exists.");
        }

        return validationResult;
    }

    #endregion File Operations

    #region Async File Operations

    /// <summary>
    /// Asynchronously moves an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static async ValueTask<ValidationResult> MoveFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true, CancellationToken cancellationToken = default)
    {
        var validationResult = new ValidationResult();

        try
        {
            RemoveReadOnlyAttribute(sourceFileName);

            var copyResult = await CopyFileAsync(sourceFileName, destFileName, setAsReadOnly, cancellationToken);

            if (copyResult.IsValid)
            {
                validationResult = await DeleteFileAsync(sourceFileName, cancellationToken);
            }
            else
            {
                validationResult.AddError($"Error moving file: {copyResult}");
            }
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex);
        }

        return validationResult;
    }

    /// <summary>
    /// Asynchronously moves or overwrite an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// If destination file exists, will be deleted.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static async ValueTask<ValidationResult> MoveOrOverwriteFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        var deleteResult = await DeleteFileAsync(destFileName);

        return deleteResult.IsValid
            ? await MoveFileAsync(sourceFileName, destFileName, setAsReadOnly)
            : deleteResult;
    }

    /// <summary>
    /// Asynchronously copies an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="ValidationResult"/> of the operation.</returns>
    public static async ValueTask<ValidationResult> CopyFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true, CancellationToken cancellationToken = default)
    {
        var validationResult = new ValidationResult();

        if (!File.Exists(destFileName))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));

                await InternalCopyFileAsync(sourceFileName, destFileName, cancellationToken);

                if (setAsReadOnly) InsertReadOnlyAttribute(destFileName);
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex);
            }
        }
        else
        {
            validationResult.AddError("Destination file already exists.");
        }

        return validationResult;
    }

    /// <summary>
    /// Asynchronously deletes the specified file if exists. Ignores <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted. Wildcard characters are not supported.</param>
    /// <returns></returns>
    public static async ValueTask<ValidationResult> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var validationResult = new ValidationResult();
        try
        {
            if (File.Exists(fileName))
            {
                RemoveReadOnlyAttribute(fileName);

                await InternalDeleteAsync(fileName, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex);
        }

        return validationResult;
    }

    #region Internal operations

    private static async Task InternalDeleteAsync(string fileName, CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(() =>
        {
            using (_ = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 1, FileOptions.DeleteOnClose | FileOptions.Asynchronous)) ;
        }, cancellationToken);
    }

    private static async Task InternalCopyFileAsync(string sourceFileName, string destFileName, CancellationToken cancellationToken)
    {
        var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        var bufferSize = 4096;

        using (var srcStream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions))

        using (var dstStream = new FileStream(destFileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, fileOptions))

            await srcStream.CopyToAsync(dstStream, bufferSize, cancellationToken);
    }

    #endregion Internal operations

    #endregion Async File Operations

    #region Path Operations

    public static string RemoveInvalidDirectoryChars(string directoryPath)
    {
        var invalidPathChars = Path.GetInvalidPathChars();
        foreach (char invalidChar in invalidPathChars)
            directoryPath = directoryPath.Replace(invalidChar.ToString(), "");

        return directoryPath;
    }

    public static string ReplaceInvalidDirectoryChars(string directoryPath)
    {
        directoryPath = directoryPath
            .Replace("<", "(")
            .Replace(">", ")")
            .Replace("|", "-")
            .Replace("/", "-")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(":", " - ");

        return directoryPath;
    }

    public static string ReplaceAndRemoveInvalidDirectoryChars(string directoryPath)
    {
        directoryPath = RemoveInvalidDirectoryChars(ReplaceInvalidDirectoryChars(directoryPath));

        return directoryPath;
    }

    public static string RemoveInvalidFileNameChars(string filePath)
    {
        var invalidFileChars = Path.GetInvalidFileNameChars();
        foreach (char invalidChar in invalidFileChars)
            filePath = filePath.Replace(invalidChar.ToString(), "");

        return filePath;
    }

    public static string ReplaceInvalidFileNameChars(string filePath)
    {
        filePath = filePath
            .Replace("<", "(")
            .Replace(">", ")")
            .Replace("|", "-")
            .Replace("/", "-")
            .Replace("\\", "-")
            .Replace("\"", "-")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(":", " ");

        return filePath;
    }

    public static string ReplaceAndRemoveInvalidFileNameChars(string filePath)
    {
        filePath = RemoveInvalidFileNameChars(ReplaceInvalidFileNameChars(filePath));

        return filePath;
    }

    #endregion Path Operations
}