namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.UnitTests
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    public partial class TestCase_ActivatorHelperTest
    {
        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance`")]
        public void TestActivatorHelper()
        {
            Assert.Multiple(() =>
            {
                var instance = ActivatorHelper.CreateInitializedInstance<Mocks.MockClass>();

                Assert.True(instance.PropertyAsBool);
                Assert.Equal(int.MaxValue, instance.PropertyAsInt32);
                Assert.Equal(long.MaxValue, instance.PropertyAsInt64);
                Assert.Equal(60118.1332M, instance.PropertyAsDecimal);
                Assert.Equal("SKIT.FlurlHttpClient", instance.PropertyAsString);
                Assert.Equal(DateTimeOffset.Parse("2006/01/02 15:04:05"), instance.PropertyAsDateTime);
                Assert.Equal("SKIT.FlurlHttpClient", Assert.Single(instance.PropertyAsStringArray!));
                Assert.IsType<Mocks.MockClass>(Assert.Single(instance.PropertyAsArray!));
                Assert.IsType<Mocks.MockClass>(Assert.Single(instance.PropertyAsList!));
            });

            Assert.Multiple(() =>
            {
                var instance = (Mocks.MockClass)ActivatorHelper.CreateInitializedInstance(typeof(Mocks.MockClass));

                Assert.True(instance.PropertyAsBool);
                Assert.Equal(int.MaxValue, instance.PropertyAsInt32);
                Assert.Equal(long.MaxValue, instance.PropertyAsInt64);
                Assert.Equal(60118.1332M, instance.PropertyAsDecimal);
                Assert.Equal("SKIT.FlurlHttpClient", instance.PropertyAsString);
                Assert.Equal(DateTimeOffset.Parse("2006/01/02 15:04:05"), instance.PropertyAsDateTime);
                Assert.Equal("SKIT.FlurlHttpClient", Assert.Single(instance.PropertyAsStringArray!));
                Assert.IsType<Mocks.MockClass>(Assert.Single(instance.PropertyAsArray!));
                Assert.IsType<Mocks.MockClass>(Assert.Single(instance.PropertyAsList!));
            });
        }

        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance` 之基元与特殊类型")]
        public void TestActivatorHelper_PrimitiveAndSpecialTypes()
        {
            Assert.Multiple(() =>
            {
                Assert.True(ActivatorHelper.CreateInitializedInstance<bool>());
                Assert.Equal('z', ActivatorHelper.CreateInitializedInstance<char>());
                Assert.Equal(sbyte.MaxValue, ActivatorHelper.CreateInitializedInstance<sbyte>());
                Assert.Equal(byte.MaxValue, ActivatorHelper.CreateInitializedInstance<byte>());
                Assert.Equal(short.MaxValue, ActivatorHelper.CreateInitializedInstance<short>());
                Assert.Equal(ushort.MaxValue, ActivatorHelper.CreateInitializedInstance<ushort>());
                Assert.Equal(int.MaxValue, ActivatorHelper.CreateInitializedInstance<int>());
                Assert.Equal(uint.MaxValue, ActivatorHelper.CreateInitializedInstance<uint>());
                Assert.Equal(long.MaxValue, ActivatorHelper.CreateInitializedInstance<long>());
                Assert.Equal(ulong.MaxValue, ActivatorHelper.CreateInitializedInstance<ulong>());
                Assert.Equal(float.MaxValue, ActivatorHelper.CreateInitializedInstance<float>());
                Assert.Equal(double.MaxValue, ActivatorHelper.CreateInitializedInstance<double>());
                Assert.Equal(60118.1332M, ActivatorHelper.CreateInitializedInstance<decimal>());
                Assert.Equal("SKIT.FlurlHttpClient", ActivatorHelper.CreateInitializedInstance<string>());
                Assert.Equal(DateTimeOffset.Parse("2006/01/02 15:04:05"), ActivatorHelper.CreateInitializedInstance<DateTimeOffset>());
                Assert.Equal(int.MaxValue, ActivatorHelper.CreateInitializedInstance<int?>());
            });
        }

        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance` 之数组与集合")]
        public void TestActivatorHelper_ArraysAndCollections()
        {
            Assert.Multiple(() =>
            {
                var array = ActivatorHelper.CreateInitializedInstance<int[]>();

                Assert.Equal(int.MaxValue, Assert.Single(array));
            });

            Assert.Multiple(() =>
            {
                var list = ActivatorHelper.CreateInitializedInstance<IList<int>>();

                Assert.IsType<List<int>>(list);
                Assert.Equal(int.MaxValue, Assert.Single(list));
            });

            Assert.Multiple(() =>
            {
                var dict = ActivatorHelper.CreateInitializedInstance<IDictionary<string, int>>();

                Assert.IsType<Dictionary<string, int>>(dict);

                var dictEntry = Assert.Single(dict);

                Assert.Equal("SKIT.FlurlHttpClient", dictEntry.Key);
                Assert.Equal(int.MaxValue, dictEntry.Value);
            });
        }

        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance` 之只初始化公开赋值器属性")]
        public void TestActivatorHelper_PublicSettersOnly()
        {
            var instance = ActivatorHelper.CreateInitializedInstance<Mocks.MockClassWithPropertyAccess>();

            Assert.Equal(int.MaxValue, instance.PublicSetter);
            Assert.Equal(0, instance.PrivateSetter);
            Assert.Equal(42, instance.GetterOnly);
        }

        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance` 之缺少无参构造函数")]
        public void TestActivatorHelper_TypeWithoutParameterlessConstructor()
        {
            Assert.Null(ActivatorHelper.CreateInitializedInstance<Mocks.MockClassWithNoDefaultConstructor>());
            Assert.Null(ActivatorHelper.CreateInitializedInstance(typeof(Mocks.MockClassWithNoDefaultConstructor)));
        }

        [Fact(DisplayName = "测试用例：工具类 `ActivatorHelper.CreateInitializedInstance` 之限制递归深度")]
        public void TestActivatorHelper_RecursiveObjectDepth()
        {
            var instance = ActivatorHelper.CreateInitializedInstance<Mocks.MockClassWithRecursive>();
            var depth = 1;

            while (instance.Next is not null)
            {
                instance = instance.Next;
                depth++;
            }

            Assert.Equal(8, depth);
        }
    }

    partial class TestCase_ActivatorHelperTest
    {
        private static class Mocks
        {
            public sealed class MockClass
            {
                public bool? PropertyAsBool { get; set; }

                public int? PropertyAsInt32 { get; set; }

                public long? PropertyAsInt64 { get; set; }

                public decimal? PropertyAsDecimal { get; set; }

                public string? PropertyAsString { get; set; }

                public DateTimeOffset? PropertyAsDateTime { get; set; }

                public string[]? PropertyAsStringArray { get; set; }

                public MockClass? PropertyAsObject { get; set; }

                public MockClass[]? PropertyAsArray { get; set; }

                public IList<MockClass>? PropertyAsList { get; set; }

                public IDictionary<string, MockClass>? PropertyAsDictionary { get; set; }
            }

            public sealed class MockClassWithPropertyAccess
            {
                public int PublicSetter { get; set; }

                public int PrivateSetter { get; private set; }

                public int GetterOnly => 42;
            }

            public sealed class MockClassWithNoDefaultConstructor
            {
                public MockClassWithNoDefaultConstructor(int value)
                {
                }
            }

            public sealed class MockClassWithRecursive
            {
                public MockClassWithRecursive? Next { get; set; }
            }
        }
    }
}
