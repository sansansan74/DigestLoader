/* Класс хранит данные статьи из сводки новостей
 * 
 * Данные получаются парсингом XML структуры указанного типа

 <new date="03.03.2022">
    <title>РЖД переходит на 100%-ную предоплату грузовых перевозок</title>
    <sources>
    <source authors="Ксения Потаева" name="Ведомости" date="03.03.2022"/>
    </sources>
    <classifies>
    <classify name="Отрасль">Железнодорожный транспорт</classify>
    <classify name="Компании">Российские железные дороги (РЖД)</classify>
    <classify name="Страна">Россия</classify>
    <classify name="Тема">Производственная деятельность</classify>
    </classifies>
    <body>
    <para>Банковские гарантии временно не принимаются</para>
    <para>"Российские железные дороги" (РЖД) прекращает работу с банковскими гарантиями, предоставляемыми в качестве обеспечения обязательств по оплате за грузовые перевозки, и переходит на 100%-ную предоплату, рассказал "Ведомостям" источник, близкий к монополии, и подтвердил представитель РЖД.</para>
    <para>"Практика приема груза к перевозке под банковские гарантии применяется по инициативе РЖД для поддержки грузоотправителей", – говорит представитель компании. "Такой порядок не является обязательным: в соответствии со статьей 30 Устава железнодорожного транспорта (федеральный закон. – "Ведомости") грузовые перевозки осуществляются на условиях 100%-ной предоплаты", – пояснил он.</para>
    <para>С учетом высокой волатильности на финансовых рынках отсрочка платежа для грузоотправителей будет временно недоступна, добавил представитель РЖД. Компания рассматривает возможности предоставления клиентам иных дополнительных мер поддержки, но каких конкретно, он не уточнил.</para>
    <para>Зампредседателя Ассоциации операторов железнодорожного подвижного состава Денис Семенкин рассказал, что банковские гарантии уже несколько лет применяются при оплате перевозок. "Эту схему использовали не все, потому что там есть ряд условий, например нельзя предъявлять претензии к перевозчику. Если сопоставить с краткосрочным кредитованием, то выгоды особой нет. Поэтому малый и средний бизнес не используют этот механизм. Но может пострадать часть крупных грузоотправителей", – пояснил он.</para>
    <para>Молочный союз и Союз экспортеров зерна подтвердили, что отправители этих грузов работают по предоплате. "Ведомости" направили запросы крупным грузоотправителям.</para>
    </body>
</new>
 */

using System.Xml.Linq;

namespace DigestLoader_Net6.Classes
{

    public class Article(XElement xnode)
    {
        public DateTime Date { get; set; } = DateTime.Parse(xnode.Attribute("date")?.Value ?? string.Empty);
        public string? Sources { get; set; } = ProcessSources(xnode?.Element("sources")!);
        public string? Classifies { get; set; } = ProcessClassifies(xnode?.Element("classifies")!);
        public List<string> Body { get; set; } = xnode
                ?.Element("body")
                ?.Elements("para")
                ?.Select(p => p.Value)
                .ToList() ?? [];
        public string? Title { get; set; } = xnode?.Element("title")?.Value!;

        const int MaxAnnotationLength = 300;
        public string GetAnnotation()
        {
            var strBody = CreateArticleBody();
            if (strBody == string.Empty)
                return string.Empty;

            if (strBody.Length >= MaxAnnotationLength)
                strBody = strBody.Substring(0, MaxAnnotationLength);

            int annotationLength = GetFirstIntry(strBody, ".!?"); // По концу предложения

            if (annotationLength <= 0)
            {
                annotationLength = GetFirstIntry(strBody, "\t\n\r "); // Предолжений нет - по пробелам
            }

            if (annotationLength > 0)
                return strBody.Substring(0, checked(annotationLength + 1));

            return strBody.Trim();
        }

        private string CreateArticleBody() => 
            string.Join("", Body.Select(RemoveTags));

        static readonly string[] TagsToRemove = { 
            "<para>",
            "</para>",
            "<p>",
            "</p>"
        };

        private static string RemoveTags(string body)
        {
            string result = body.Trim();
            foreach (var tag in TagsToRemove)
            {
                result = body.Replace(tag, string.Empty);
            }

            return result;
        }

        private static int GetFirstIntry(string strBody, string findSymbols) => 
            findSymbols.Select(ch => strBody.LastIndexOf(ch)).Min();

        public static string ProcessSources(XElement sources)
        {
            string str = "";
            if (sources != null)
            {
                foreach (XElement source in sources.Elements("source"))
                {
                    str += String.Format(
                        "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                        source?.Attribute("name")?.Value ?? String.Empty,
                        source?.Attribute("date")?.Value ?? String.Empty,
                        source?.Value); 
                }
            }
            if (str != "")
                str = "<table class=gr-tb border=0 cellpadding=0 cellspacing=2><tr><th>"
                        + "Источник</th><th>Дата</th><th>Наименование материала</th></tr>" 
                        + str + "</table>";
            return str;
        }

        public static string ProcessClassifies(XElement xmlnode)
        {
            string str = "";
            foreach (XElement childNode in xmlnode.Elements())
            {
                if (childNode.Name == "classify")
                {
                    if (str != "")
                        str += "|#|";
                    str = str + $"{childNode.Attribute("name")?.Value}={childNode.Value}";
                }
            }
            return str;
        }

        /// <summary>
        /// Проверка, отрасли, если ОТРАСЛЬ = "ЖЕЛЕЗНОДОРОЖНЫЙ ТРАНСПОРТ",
        /// то отправляем запись на Интернет-портал
        /// </summary>
        /// <returns></returns>
        public bool HasRailTransport()
        {
            string[] classifies = Classifies?.Split("|#|", StringSplitOptions.RemoveEmptyEntries) ?? [];

            foreach (var classify in classifies)
            {
                string[] clSubstrings = classify.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (clSubstrings[0].Trim().Equals("ОТРАСЛЬ", StringComparison.CurrentCultureIgnoreCase)
                    && clSubstrings[1].Trim().Equals("ЖЕЛЕЗНОДОРОЖНЫЙ ТРАНСПОРТ", StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public string GetBodyWithTag(string tagName)
        {
            IEnumerable<string>? paragrafs = this.Body?.Select(
                b => $"<{tagName}>{b}</{tagName}>") ?? new List<string>();
            return string.Concat(paragrafs);
            // + "<p>&nbsp;</p>" + strSources
        }

        public string GetBodyForNbktrans()
        {
            IEnumerable<string>? paragrafs = this.Body?.Select(
                b => $"<p>{b}</p>") ?? new List<string>();
            string body = string.Concat(paragrafs); 
            return string.Concat(body, "<p>&nbsp;</p>", Sources);
            // + "<p>&nbsp;</p>" + strSources
        }
    }
}
