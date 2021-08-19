using FluentValidation.Results;
using System;
using System.Drawing;
using System.IO;

namespace Maxsys.Core.Helpers
{
    // TODO create docs
    public static class ImageHelper
    {
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

        public static Image ImageFromBytes(byte[] rawImage)
        {
            try
            {
                // Dispose MemoryStream ?????????
                using (var mStream = new MemoryStream(rawImage))
                {
                    return Image.FromStream(mStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                    result.AddFailure($"{nameof(ImageHelper)}.{nameof(SavePicture)}", ex.Message);
                }
            }

            return result;
        }

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