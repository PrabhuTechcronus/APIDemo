using NUnit.Framework;

namespace NUnitTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async System.Threading.Tasks.Task Test1Async()
        {
            JourneyManagerLamda.JourneyLamda journeyLamda = new JourneyManagerLamda.JourneyLamda();
            string result = await journeyLamda.Journeyfunction();
            Assert.Pass();
        }
    }
}