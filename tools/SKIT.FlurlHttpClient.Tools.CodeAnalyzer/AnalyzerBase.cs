using System;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 代码质量分析器接口。
    /// </summary>
    public interface IAnalyzer : IDisposable
    {
        /// <summary>
        /// 使用当前分析器分析代码质量，断言是否没有代码质量问题。如果存在任何问题，将会抛出异常。
        /// </summary>
        public void AssertNoIssues();
    }

    /// <summary>
    /// 代码质量分析器选项接口。
    /// </summary>
    public interface IAnalyzerOptions
    {
        /// <summary>
        /// 获取或设置待分析的 SDK 程序集。
        /// </summary>
        public Assembly SdkAssembly { get; set; }
    }
}
