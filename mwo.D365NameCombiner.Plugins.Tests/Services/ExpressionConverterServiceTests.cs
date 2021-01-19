using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.Services.Tests
{
    [TestClass]
    public class ExpressionConverterServiceTests : TestBase
    {
        private const string complexExpression = @"_ => 
            EntityHelper.GetValue(_.Subject, ""StringAttribute"") 
            + (EntityHelper.HasValue(_.Subject, ""StringAttribute"") && EntityHelper.HasValue(_.Subject, ""IntAttribute"") ? ""/"" : null) 
            + EntityHelper.GetValue(_.Subject, ""IntAttribute"")";

        [DataTestMethod]
        [DataRow(SimpleExpression, SimpleExpressionExpected)]
        [DataRow(@"_ => string(EntityHelper.GetValue(_.Subject, ""IntAttribute""))", IntValue)]
        [DataRow(@"_ => ""Hello""", "Hello")]
        [DataRow(@"_ => _.Target.LogicalName", EntityName)]
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