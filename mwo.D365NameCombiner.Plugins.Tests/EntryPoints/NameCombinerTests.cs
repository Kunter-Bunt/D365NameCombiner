using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Plugins;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.EntryPoints.Tests
{
    [TestClass]
    public class NameCombinerTests : TestBase
    {
        [TestMethod]
        public void Execute_Test()
        {
            //Arrange
            var ctx = FakeEasyContext.GetDefaultPluginContext();
            ctx.InputParameters.Add(CRMPluginContext.TargetName, Target);

            //Act
            FakeEasyContext.ExecutePluginWithConfigurations<NameCombiner>(ctx, Config.Id.ToString(), null);

            //Assert
            Assert.AreEqual(Target[CombinedAttribute], StringValue);
        }
    }
}