using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    public class TestCase_ActivatorHelperTest
    {
        private sealed class MockObject
        {
            public bool? PropertyAsBool { get; set; }

            public int? PropertyAsInt32 { get; set; }

            public long? PropertyAsInt64 { get; set; }

            public decimal? PropertyAsDecimal { get; set; }

            public string? PropertyAsString { get; set; }

            public DateTimeOffset? PropertyAsDateTime { get; set; }

            public string[]? PropertyAsStringArray { get; set; } 

            public MockObject? PropertyAsObject { get; set; }

            public MockObject[]? PropertyAsArray { get; set; }

            public IList<MockObject>? PropertyAsList { get; set; }

            public IDictionary<string, MockObject>? PropertyAsDictionary { get; set; }
        }

        [Test(Description = "测试用例：工具类之 ActivatorHelper")]
        public void TestActivatorHelper()
        {
            Assert.Multiple(() =>
            {
                var actualObj = ActivatorHelper.CreateInitializedInstance<MockObject>();

                Assert.That(actualObj.PropertyAsBool, Is.True);
                Assert.That(actualObj.PropertyAsInt32, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsInt64, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsDecimal, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsString, Is.Not.Null);
                Assert.That(actualObj.PropertyAsDateTime, Is.Not.Null);
                Assert.That(actualObj.PropertyAsStringArray, Is.Not.Null);
                Assert.That(actualObj.PropertyAsObject, Is.Not.Null);
                Assert.That(actualObj.PropertyAsArray, Is.Not.Null);
                Assert.That(actualObj.PropertyAsList, Is.Not.Null);
                Assert.That(actualObj.PropertyAsDictionary, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                var actualObj = (MockObject)ActivatorHelper.CreateInitializedInstance(typeof(MockObject));

                Assert.That(actualObj.PropertyAsBool, Is.True);
                Assert.That(actualObj.PropertyAsInt32, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsInt64, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsDecimal, Is.GreaterThan(0));
                Assert.That(actualObj.PropertyAsString, Is.Not.Null);
                Assert.That(actualObj.PropertyAsDateTime, Is.Not.Null);
                Assert.That(actualObj.PropertyAsStringArray, Is.Not.Null);
                Assert.That(actualObj.PropertyAsObject, Is.Not.Null);
                Assert.That(actualObj.PropertyAsArray, Is.Not.Null);
                Assert.That(actualObj.PropertyAsList, Is.Not.Null);
                Assert.That(actualObj.PropertyAsDictionary, Is.Not.Null);
            });
        }
    }
}
