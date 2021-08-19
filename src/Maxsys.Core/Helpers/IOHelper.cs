﻿using FluentValidation.Results;
using System;
using System.IO;

namespace Maxsys.Core.Helpers
{
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
                    validationResult.AddFailure(nameof(MoveFile)
                        , $"Error moving file: {copyResult.Errors[0].ErrorMessage}");
            }
            catch (Exception ex)
            {
                validationResult.AddFailure(nameof(MoveFile), ex.Message);
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
                validationResult.AddFailure(nameof(DeleteFile), ex.Message);
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
                    validationResult.AddFailure(nameof(CopyFile), ex.Message);
                }
            }
            else
            {
                validationResult.AddFailure(destFileName, "Destination file already exists");
            }

            return validationResult;
        }

        #endregion File Operations

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
}