// Класс архивирует файл
using System.IO.Compression;

namespace DigestLoader_Net6.Classes
{
    internal class Archiver
    {
        /// <summary>
        /// Создает архив файла, сохраняет его в исходную папку с аналогичным именем и расширением .zip
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static string ZipFile(string fileFullName)
        {
            string zipPath = fileFullName.Replace(".xml", ".zip");

            string fileName = Path.GetFileName(fileFullName);

            using (ZipArchive archive = System.IO.Compression.ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(fileFullName, fileName);
            }

            return zipPath;
        }
    }
}
