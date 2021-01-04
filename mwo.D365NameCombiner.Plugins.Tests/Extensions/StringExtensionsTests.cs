using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mwo.D365NameCombiner.Plugins.Extensions.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void ToDictionary_SingleTest()
        {
            //Act
            var result = "a=b".ToDictionary();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey("a"));
        }

        [TestMethod()]
        public void ToDictionary_MultiTest()
        {
            //Act
            var result = "a=b;b=c".ToDictionary();

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("a"));
            Assert.IsTrue(result.ContainsKey("b"));
        }

        [TestMethod()]
        public void ToDictionary_EmptyTest()
        {
            //Act
            var result = "".ToDictionary();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod()]
        public void ToDictionary_MalformattedTest()
        {
            //Act
            var result = "abc+cvb.sdf".ToDictionary();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod()]
        public void ToDictionary_PartlyMalformattedTest()
        {
            //Act
            var result = "a=b;b;c".ToDictionary();

            //Assert
            Assert.AreEqual(1, result.Count);
        }
    }
}