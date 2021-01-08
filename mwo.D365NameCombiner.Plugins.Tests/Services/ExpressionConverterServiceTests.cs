using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.Services.Tests
{
    [TestClass()]
    public class ExpressionConverterServiceTests : TestBase
    {
        private const string complexExpression = @"_ => 
            EntityHelper.GetValue(_.PostImage, ""StringAttribute"") 
            + (EntityHelper.HasValue(_.PostImage, ""StringAttribute"") && EntityHelper.HasValue(_.PostImage, ""IntAttribute"") ? ""/"" : null) 
            + EntityHelper.GetValue(_.PostImage, ""IntAttribute"")";

        [DataTestMethod]
        [DataRow(@"_ => EntityHelper.GetValue(_.PostImage, ""StringAttribute"")", StringValue)]
        [DataRow(@"_ => EntityHelper.GetValue(_.PostImage, ""IntAttribute"")", IntValue)]
        [DataRow(complexExpression, "StringValue/123")]
        public void Convert_Test(string expression, object expected)
        {
            //Arrange
            var service = new ExpressionConverterService(Context);

            //Act
            var result = service.Convert(expression);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}