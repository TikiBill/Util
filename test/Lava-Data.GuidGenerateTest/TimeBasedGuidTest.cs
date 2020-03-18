using System;
using LavaData.Util.GuidGenerate;
using Xunit;

// spell-checker:ignore xunit
namespace LavaData.Util.GuidGenerateTest
{
    public class TimeBasedGuidTest
    {
        [Fact]
        public void Test1()
        {
            var guid1 = GuidGenerator.GenerateTimeBasedGuid();
            var guid2 = GuidGenerator.GenerateTimeBasedGuid();
            Assert.True(string.Compare(guid2.ToString(), guid1.ToString()) > 0);
        }

        [Fact]
        public void Test2()
        {
            var guid1 = GuidGenerator.GenerateTimeBasedGuid();
            System.Threading.Thread.Sleep(1000);
            var guid2 = GuidGenerator.GenerateTimeBasedGuid();
            Assert.True(string.Compare(guid2.ToString(), guid1.ToString()) > 0);
        }
    }
}
