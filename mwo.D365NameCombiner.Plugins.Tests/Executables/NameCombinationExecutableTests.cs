using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Services;
using mwo.D365NameCombiner.Plugins.Tests;
using System;

namespace mwo.D365NameCombiner.Plugins.Executables.Tests
{
    [TestClass]
    public class NameCombinationExecutableTests : TestBase
    {
        private NameCombinationExecutable Executable;

        [TestInitialize]
        public void Initialize()
        {
            var attributeService = new AttributeConverterService(Context);
            var expressionService = new ExpressionConverterService(Context);
            var combinerService = new CombinerService(Target, attributeService, expressionService, Context);

            Executable = new NameCombinationExecutable(combinerService, Context);
        }

        [TestMethod]
        public void Execute_HappyTest()
        {
            //Act
            Executable.Execute(Config.Id.ToString(), Target);

            //Assert
            Assert.AreEqual(Target[CombinedAttribute], StringValue);
        }


        [TestMethod]
        public void Execute_NotExistingTest()
        {
            //Act
            Executable.Execute(Guid.NewGuid().ToString(), Target);

            //Assert
            Assert.IsFalse(Target.Contains(CombinedAttribute));
        }


        [TestMethod]
        public void Execute_NoConfigTest()
        {
            //Act
            Executable.Execute(null, Target);

            //Assert
            Assert.IsFalse(Target.Contains(CombinedAttribute));
        }

        [TestMethod]
        public void Execute_InvalidConfigTest()
        {
            //Act
            Executable.Execute("Nope", Target);

            //Assert
            Assert.IsFalse(Target.Contains(CombinedAttribute));
        }
    }
}