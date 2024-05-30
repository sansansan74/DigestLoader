// Класс - сводка новостей, хранит список статей, получается парсингом из XML-файла
using System.Text;
using System.Xml.Linq;

namespace DigestLoader_Net6.Classes
{
    public class Digest
    {
        public List<Article> Articles = [];

        public Digest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary> Парсинг списка новостей из файла
        /// </summary>
        /// <param name="fileName">Путь к xml-файлу</param>
        public void Init(string fileName)
        {
            // Тут падает по кодировке XML
            // Под отладчиком на моей машине работает
            XDocument xdoc = XDocument.Load(fileName);
            var xmlArticles = xdoc.Element("digest")?.Elements("new").ToList() ?? [];
            Articles = xmlArticles.Select(a => new Article(a)).ToList();
        }
    }
}
