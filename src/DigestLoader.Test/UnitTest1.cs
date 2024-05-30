using DigestLoader_Net6.Classes;

namespace DigestLoader.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Digest digest = new();
            
            digest.Init(@"D:\Projects\DigestLoader\src\Data\new\13.11.2023.xml");


            var titles = digest.Articles.Select(a => a.Title).ToList();
            var rt = digest.Articles.Select(a => a.HasRailTransport()).ToList();

            Assert.That(digest.Articles, Is.Not.Empty);
        }
    }
}