// Класс реализует функционал перемещения файл, либо перемещаения и архивирования
using DigestLoader_Net6.Classes;

namespace DigestLoader_Net6.Repository
{
    public class FileMover
    {
        /// <summary>
        /// Создает архив, перемещает в указанную папку и удаляет исходный файл
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="destinationDirPath"></param>
        public static void MoveAsArchive(string fileFullName, string destinationDirPath)
        {
            string archiveFileName = Archiver.ZipFile(fileFullName);

            MoveFile(archiveFileName, destinationDirPath);

            File.Delete(fileFullName);
        }

        /// <summary>
        /// Перемещает файл в указанную папку. Если папка не сущесвтует, то метод создает ее.
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="destinationDirPath"></param>
        public static void MoveFile(string fileFullName, string destinationDirPath)
        {
            string fileName = Path.GetFileName(fileFullName);

            if (!Directory.Exists(destinationDirPath))
            {
                Directory.CreateDirectory(destinationDirPath);
            }

            File.Move(fileFullName, Path.Combine(destinationDirPath, fileName), true);
        }

    }
}
