using FluentValidation.Results;
using System;
using System.Drawing;
using System.IO;

#if NET5_0_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Maxsys.Core.Helpers
{
    /// <summary>
    /// Contains static methods for Image handling.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Converts an <see cref="Image"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> to convert.</param>
        /// <returns>A <see cref="byte"/> array that represents the <paramref name="image"/> object.</returns>
        public static byte[] ImageToBytes(this Image image)
        {
            // ImageConverter is not present on .netstandart
            //return (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));

            try
            {
                using (var mStream = new System.IO.MemoryStream())
                {
                    image.Save(mStream, image.RawFormat);
                    return mStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a <see cref="byte"/> array image into an <see cref="Image"/> object.
        /// </summary>
        /// <param name="rawImage">The <see cref="byte"/> array to convert.</param>
        /// <returns>An <see cref="Image"/> object converted from the <paramref name="rawImage"/> array.</returns>
        public static Image ImageFromBytes(byte[] rawImage)
        {
            try
            {
                var image = Image.FromStream(new MemoryStream(rawImage));

                return image;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Obsolete("Uses SaveImageIntoJpgFile() method. This method will be removed in next release.")]
        /// <summary>
        /// Saves an image to a new file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="image"></param>
        /// <param name="overrideFile"></param>
        /// <returns></returns>
        public static ValidationResult SavePicture(
            this Image image,
            string filePath,
            bool overrideFile = true)
        {
            var result = new ValidationResult();

            if (overrideFile) result = IOHelper.DeleteFile(filePath);
            if (result.IsValid)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    image.Save(filePath);
                }
                catch (Exception ex)
                {
                    result.AddFailure(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Saves an image represented by the <see cref="byte"/> array to a new file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static ValidationResult SaveByteArrayImageIntoJpgFile(
            byte[] imageBytes,
            string targetFile)
        {
            var result = new ValidationResult();

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                File.WriteAllBytes(targetFile, imageBytes);
            }
            catch (Exception ex)
            {
                result.AddFailure(ex);
            }

            return result;
        }

        /// <summary>
        /// Saves an image represented by the <see cref="byte"/> array to a new file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static ValidationResult SaveImageIntoJpgFile(
            Image image,
            string targetFile)
        {
            var imageBytes = image.ImageToBytes();

            var result = SaveByteArrayImageIntoJpgFile(imageBytes, targetFile);

            return result;
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Asynchronously saves an image represented by the <see cref="byte"/> array to a new file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static async Task<ValidationResult> SaveByteArrayImageIntoJpgFileAsync(
            byte[] imageBytes,
            string targetFile)
        {
            var result = new ValidationResult();

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                await File.WriteAllBytesAsync(targetFile, imageBytes);
            }
            catch (Exception ex)
            {
                result.AddFailure(ex);
            }

            return result;
        }

        /// <summary>
        /// Asynchronously saves an image to a new file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static async Task<ValidationResult> SaveImageIntoJpgFileAsync(
            Image image,
            string targetFile)
        {
            var imageBytes = image.ImageToBytes();

            var result = await SaveByteArrayImageIntoJpgFileAsync(imageBytes, targetFile);

            return result;
        }

#endif

        /* Look this

        private static void SalvaArrayBytesEmArquivo(byte[] array, string arquivo)
        {
            try
            {
                using (var ms = new MemoryStream(array))
                {
                    //escreve para arquivo
                    var fs = new FileStream(arquivo, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(fs);
                    fs.Close();
                    ms.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] ConverteArquivoBytes(string arquivo)
        {
            try
            {
                using (var fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read))
                {
                    // cria um array de byte do arquivo
                    byte[] bytes = File.ReadAllBytes(arquivo);
                    //le o bloco de bytes do stream
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    return bytes;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        */
    }
}