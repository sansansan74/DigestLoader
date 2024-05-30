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
        public void TestParse()
        {
            Digest digest = new();
            
            digest.Init(@"TestData\16.05.2024.xml");

            Assert.That(digest.Articles, Is.Not.Empty);
            Assert.That(digest.Articles.Count == 21);
        }
    }
}