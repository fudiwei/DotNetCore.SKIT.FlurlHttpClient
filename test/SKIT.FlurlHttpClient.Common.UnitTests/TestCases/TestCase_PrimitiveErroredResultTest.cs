namespace SKIT.FlurlHttpClient.UnitTests.TestCases
{
    using SKIT.FlurlHttpClient.Primitives;

    public class TestCase_PrimitiveErroredResultTest
    {
        [Fact(DisplayName = "测试用例：元类型 `ErroredResult`")]
        public void TestErroredResult()
        {
            Assert.Multiple(() =>
            {
                Assert.True(ErroredResult.True == true);
                Assert.False(ErroredResult.True == false);
                Assert.False(ErroredResult.True != true);
                Assert.True(ErroredResult.True != false);
                Assert.True(true == ErroredResult.True);
                Assert.False(false == ErroredResult.True);
                Assert.False(true != ErroredResult.True);
                Assert.True(false != ErroredResult.True);

                Assert.False(ErroredResult.False == true);
                Assert.True(ErroredResult.False == false);
                Assert.True(ErroredResult.False != true);
                Assert.False(ErroredResult.False != false);
                Assert.False(true == ErroredResult.False);
                Assert.True(false == ErroredResult.False);
                Assert.True(true != ErroredResult.False);
                Assert.False(false != ErroredResult.False);

                Assert.True((bool)ErroredResult.True);
                Assert.False((bool)ErroredResult.False);

                Assert.True((bool?)ErroredResult.True);
                Assert.False((bool?)ErroredResult.False);
            });

            Assert.Multiple(() =>
            {
                Assert.Equal(ErroredResult.True, ErroredResult.Ok());
                Assert.Equal(ErroredResult.False, ErroredResult.Fail());
            });

            Assert.Multiple(() =>
            {
                Exception? mockError = new Exception();

                Assert.Equal(mockError, ErroredResult.Fail(mockError).Error);
                Assert.Equal(mockError, new ErroredResult(false, mockError).Error);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                new ErroredResult(true, new Exception());
            });
        }
    }
}
