using System;
using NUnit.Framework;

namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Primitives;

    public class TestCase_PrimitiveErroredResultTest
    {
        [Test(Description = "测试用例：ErroredResult")]
        public void TestSecurityErroredResult()
        {
            Assert.Multiple(() =>
            {
                Assert.That(ErroredResult.True == true, Is.True);
                Assert.That(ErroredResult.True == false, Is.False);
                Assert.That(ErroredResult.True != true, Is.False);
                Assert.That(ErroredResult.True != false, Is.True);
                Assert.That(true == ErroredResult.True , Is.True);
                Assert.That(false == ErroredResult.True, Is.False);
                Assert.That(true != ErroredResult.True, Is.False);
                Assert.That(false != ErroredResult.True, Is.True);

                Assert.That(ErroredResult.False == true, Is.False);
                Assert.That(ErroredResult.False == false, Is.True);
                Assert.That(ErroredResult.False != true, Is.True);
                Assert.That(ErroredResult.False != false, Is.False);
                Assert.That(true == ErroredResult.False, Is.False);
                Assert.That(false == ErroredResult.False, Is.True);
                Assert.That(true != ErroredResult.False, Is.True);
                Assert.That(false != ErroredResult.False, Is.False);

                Assert.That((bool)ErroredResult.True, Is.True);
                Assert.That((bool)ErroredResult.False, Is.False);

                Assert.That((bool?)ErroredResult.True, Is.True);
                Assert.That((bool?)ErroredResult.False, Is.False);
            });

            Assert.Multiple(() =>
            {
                Assert.That(ErroredResult.Ok(), Is.EqualTo(ErroredResult.True));
                Assert.That(ErroredResult.Fail(), Is.EqualTo(ErroredResult.False));
            });

            Assert.Multiple(() =>
            {
                Exception? mockError = new Exception();

                Assert.That(ErroredResult.Fail(mockError).Error, Is.EqualTo(mockError));
                Assert.That(new ErroredResult(false, mockError).Error, Is.EqualTo(mockError));
            });

            Assert.Throws<ArgumentException>(() =>
            {
                new ErroredResult(true, new Exception());
            });
        }
    }
}
