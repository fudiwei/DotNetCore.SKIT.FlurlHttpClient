using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using System.Collections.Concurrent;
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers.Internal;

    /// <summary>
    /// 源文件质量分析器。该分析器用于分析 SDK 中的源码或测试样例问题。
    /// </summary>
    public sealed partial class SourceFileAnalyzer : IAnalyzer
    {
        private const string REGEX_DOCCOMMENT_REQUEST_MODEL = @"表示\s\[([a-zA-Z]+)\]\s(\S*)\s接口的请求。";
        private const string REGEX_DOCCOMMENT_RESPONSE_MODEL = @"表示\s\[([a-zA-Z]+)\]\s(\S*)\s接口的响应。";
        private const string REGEX_DOCCOMMENT_EXECUTING_METHOD = @"异步调用\s\[([a-zA-Z]+)\]\s(\S*)\s接口。";

        private static bool IsJsonFile(FileInfo fileInfo) => fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase);
        private static bool IsXmlFile(FileInfo fileInfo) => fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase);

        private bool _disposed;

        private readonly Assembly _sdkAssembly;
        private readonly string _sdkRequestModelDeclarationNamespace;
        private readonly string _sdkResponseModelDeclarationNamespace;
        private readonly string _sdkWebhookEventDeclarationNamespace;
        private readonly string _projectSourceRootDirectory;
        private readonly string _projectSourceRequestModelClassCodeSubDirectory;
        private readonly string _projectSourceResponseModelClassCodeSubDirectory;
        private readonly string _projectSourceExecutingExtensionClassCodeSubDirectory;
        private readonly string? _projectSourceExecutingExtensionClassCodeFileNameRegex;
        private readonly string _projectSourceWebhookEventClassCodeSubDirectory;
        private readonly string _projectTestRootDirectory;
        private readonly string _projectTestRequestModelSerializationSampleSubDirectory;
        private readonly string _projectTestResponseModelSerializationSampleSubDirectory;
        private readonly string _projectTestWebhookEventSerializationSampleSubDirectory;
        private readonly Func<FileInfo, bool>? _ignoreRequestModelClassCodeFiles;
        private readonly Func<FileInfo, bool>? _ignoreResponseModelClassCodeFiles;
        private readonly Func<FileInfo, bool>? _ignoreExecutingExtensionClassCodeFiles;
        private readonly Func<FileInfo, bool>? _ignoreWebhookEventClassCodeFiles;
        private readonly Func<FileInfo, bool>? _ignoreRequestModelSerializationSampleFiles;
        private readonly Func<FileInfo, bool>? _ignoreResponseModelSerializationSampleFiles;
        private readonly Func<FileInfo, bool>? _ignoreWebhookEventSerializationSampleFiles;
        private readonly bool _throwOnNotFoundRequestModelClassCodeFiles;
        private readonly bool _throwOnNotFoundResponseModelClassCodeFiles;
        private readonly bool _throwOnNotFoundExecutingExtensionClassCodeFiles;
        private readonly bool _throwOnNotFoundWebhookEventClassCodeFiles;
        private readonly bool _throwOnNotFoundRequestModelSerializationSampleFiles;
        private readonly bool _throwOnNotFoundResponseModelSerializationSampleFiles;
        private readonly bool _throwOnNotFoundWebhookEventSerializationSampleFiles;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ConcurrentDictionary<string, string> _filePathToContentMap;
        private readonly List<SourceFileAnalyzerRule> _customRules;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customRequestModelClassCodeFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customResponseModelClassCodeFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customExecutingExtensionClassCodeFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customWebhookEventClassCodeFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customRequestModelSerializationSampleFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customResponseModelSerializationSampleFilesScanner;
        private Func<DirectoryInfo, IEnumerable<FileInfo>>? _customWebhookEventSerializationSampleFilesScanner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SourceFileAnalyzer(SourceFileAnalyzerOptions options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _sdkAssembly = options.SdkAssembly;
            _sdkRequestModelDeclarationNamespace = options.SdkRequestModelDeclarationNamespace;
            _sdkResponseModelDeclarationNamespace = options.SdkResponseModelDeclarationNamespace;
            _sdkWebhookEventDeclarationNamespace = options.SdkWebhookEventDeclarationNamespace;
            _projectSourceRootDirectory = options.ProjectSourceRootDirectory;
            _projectSourceRequestModelClassCodeSubDirectory = options.ProjectSourceRequestModelClassCodeSubDirectory;
            _projectSourceResponseModelClassCodeSubDirectory = options.ProjectSourceResponseModelClassCodeSubDirectory;
            _projectSourceExecutingExtensionClassCodeSubDirectory = options.ProjectSourceExecutingExtensionClassCodeSubDirectory;
            _projectSourceExecutingExtensionClassCodeFileNameRegex = options.ProjectSourceExecutingExtensionClassCodeFileNameRegex;
            _projectSourceWebhookEventClassCodeSubDirectory = options.ProjectSourceWebhookEventClassCodeSubDirectory;
            _projectTestRootDirectory = options.ProjectTestRootDirectory;
            _projectTestRequestModelSerializationSampleSubDirectory = options.ProjectTestRequestModelSerializationSampleSubDirectory;
            _projectTestResponseModelSerializationSampleSubDirectory = options.ProjectTestResponseModelSerializationSampleSubDirectory;
            _projectTestWebhookEventSerializationSampleSubDirectory = options.ProjectTestWebhookEventSerializationSampleSubDirectory;
            _ignoreRequestModelClassCodeFiles = options.IgnoreRequestModelClassCodeFiles;
            _ignoreResponseModelClassCodeFiles = options.IgnoreResponseModelClassCodeFiles;
            _ignoreExecutingExtensionClassCodeFiles = options.IgnoreExecutingExtensionClassCodeFiles;
            _ignoreWebhookEventClassCodeFiles = options.IgnoreWebhookEventClassCodeFiles;
            _ignoreRequestModelSerializationSampleFiles = options.IgnoreRequestModelSerializationSampleFiles;
            _ignoreResponseModelSerializationSampleFiles = options.IgnoreResponseModelSerializationSampleFiles;
            _ignoreWebhookEventSerializationSampleFiles = options.IgnoreWebhookEventSerializationSampleFiles;
            _throwOnNotFoundRequestModelClassCodeFiles = options.ThrowOnNotFoundRequestModelClassCodeFiles;
            _throwOnNotFoundResponseModelClassCodeFiles = options.ThrowOnNotFoundResponseModelClassCodeFiles;
            _throwOnNotFoundExecutingExtensionClassCodeFiles = options.ThrowOnNotFoundExecutingExtensionClassCodeFiles;
            _throwOnNotFoundWebhookEventClassCodeFiles = options.ThrowOnNotFoundWebhookEventClassCodeFiles;
            _throwOnNotFoundRequestModelSerializationSampleFiles = options.ThrowOnNotFoundRequestModelSerializationSampleFiles;
            _throwOnNotFoundResponseModelSerializationSampleFiles = options.ThrowOnNotFoundResponseModelSerializationSampleFiles;
            _throwOnNotFoundWebhookEventSerializationSampleFiles = options.ThrowOnNotFoundWebhookEventSerializationSampleFiles;

            _customRules = new List<SourceFileAnalyzerRule>();
            _filePathToContentMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            UseRule_ReqAndRespModelOrWebhhookEventCodeFileNameCase();
            UseRule_ReqAndRespModelCodeFileXmlDocumentationComment();
            UseRule_RequestModelCodeFilePropertiesInitialization();
            UseRule_ResponseModelCodeFilePropertiesInitialization();
            UseRule_ExecutingExtensionCodeFileXmlDocumentationComment();
            UseRule_ReqAndRespModelSerializationSampleCanDesarialized();
            UseRule_WebhhookEventSerializationSampleCanDesarialized();
        }

        /// <summary>
        /// 添加自定义规则。
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public SourceFileAnalyzer AddRule(SourceFileAnalyzerRule rule)
        {
            if (rule is null) throw new ArgumentNullException(nameof(rule));
            if (_disposed) throw new ObjectDisposedException(nameof(TypeDeclarationAnalyzer));

            _customRules.Add(rule);

            return this;
        }

        /// <summary>
        /// 设置特定文件内容类型的扫描方式，在断言时将替换默认的扫描方式。
        /// </summary>
        /// <param name="contentKind"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public SourceFileAnalyzer SetFileScanner(SourceFileContentKinds contentKind, Func<DirectoryInfo, IEnumerable<FileInfo>> scanner)
        {
            if (scanner is null) throw new ArgumentNullException(nameof(scanner));
            if (_disposed) throw new ObjectDisposedException(nameof(TypeDeclarationAnalyzer));

            switch (contentKind)
            {
                case SourceFileContentKinds.RequestModelClassCode:
                    _customRequestModelClassCodeFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceRequestModelClassCodeSubDirectory)));
                    break;

                case SourceFileContentKinds.ResponseModelClassCode:
                    _customResponseModelClassCodeFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceResponseModelClassCodeSubDirectory)));
                    break;

                case SourceFileContentKinds.ExecutingExtensionClassCode:
                    _customExecutingExtensionClassCodeFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceExecutingExtensionClassCodeSubDirectory)));
                    break;

                case SourceFileContentKinds.WebhookEventClassCode:
                    _customWebhookEventClassCodeFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceWebhookEventClassCodeSubDirectory)));
                    break;

                case SourceFileContentKinds.RequestModelSerializationSample:
                    _customRequestModelSerializationSampleFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestRequestModelSerializationSampleSubDirectory)));
                    break;

                case SourceFileContentKinds.ResponseModelSerializationSample:
                    _customResponseModelSerializationSampleFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestResponseModelSerializationSampleSubDirectory)));
                    break;

                case SourceFileContentKinds.WebhookEventSerializationSample:
                    _customWebhookEventSerializationSampleFilesScanner = (_) => scanner.Invoke(new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestWebhookEventSerializationSampleSubDirectory)));
                    break;
            }

            return this;
        }

        /// <inheritdoc/>
        public void AssertNoIssues()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(TypeDeclarationAnalyzer));

            var units = ScanAnalyzerRuleUnits();
            var options = ExtractAnalyzerOptions();
            ParallelHelper.ForEachOrdered(units, (unit) =>
            {
                IList<Exception> issues = new List<Exception>();
                foreach (var rule in _customRules)
                {
                    try
                    {
                        rule.Invoke(options, units, unit);
                    }
                    catch (Exception ex)
                    {
                        issues.Add(ex);
                    }
                }

                if (issues.Any())
                    throw new AggregateException(issues);
            }, _cancellationTokenSource.Token);
        }

        private SourceFileAnalyzerOptions ExtractAnalyzerOptions()
        {
            return new SourceFileAnalyzerOptions()
            {
                SdkAssembly = _sdkAssembly,
                SdkRequestModelDeclarationNamespace = _sdkRequestModelDeclarationNamespace,
                SdkResponseModelDeclarationNamespace = _sdkResponseModelDeclarationNamespace,
                SdkWebhookEventDeclarationNamespace = _sdkWebhookEventDeclarationNamespace,
                ProjectSourceRootDirectory = _projectSourceRootDirectory,
                ProjectSourceRequestModelClassCodeSubDirectory = _projectSourceRequestModelClassCodeSubDirectory,
                ProjectSourceResponseModelClassCodeSubDirectory = _projectSourceResponseModelClassCodeSubDirectory,
                ProjectSourceExecutingExtensionClassCodeSubDirectory = _projectSourceExecutingExtensionClassCodeSubDirectory,
                ProjectSourceWebhookEventClassCodeSubDirectory = _projectSourceWebhookEventClassCodeSubDirectory,
                ProjectTestRootDirectory = _projectTestRootDirectory,
                ProjectTestRequestModelSerializationSampleSubDirectory = _projectTestRequestModelSerializationSampleSubDirectory,
                ProjectTestResponseModelSerializationSampleSubDirectory = _projectTestResponseModelSerializationSampleSubDirectory,
                ProjectTestWebhookEventSerializationSampleSubDirectory = _projectTestWebhookEventSerializationSampleSubDirectory,
                IgnoreRequestModelClassCodeFiles = _ignoreRequestModelClassCodeFiles,
                IgnoreResponseModelClassCodeFiles = _ignoreResponseModelClassCodeFiles,
                IgnoreExecutingExtensionClassCodeFiles = _ignoreExecutingExtensionClassCodeFiles,
                IgnoreWebhookEventClassCodeFiles = _ignoreWebhookEventClassCodeFiles,
                IgnoreRequestModelSerializationSampleFiles = _ignoreRequestModelSerializationSampleFiles,
                IgnoreResponseModelSerializationSampleFiles = _ignoreResponseModelSerializationSampleFiles,
                IgnoreWebhookEventSerializationSampleFiles = _ignoreWebhookEventSerializationSampleFiles,
                ThrowOnNotFoundRequestModelClassCodeFiles = _throwOnNotFoundRequestModelClassCodeFiles,
                ThrowOnNotFoundResponseModelClassCodeFiles = _throwOnNotFoundResponseModelClassCodeFiles,
                ThrowOnNotFoundExecutingExtensionClassCodeFiles = _throwOnNotFoundExecutingExtensionClassCodeFiles,
                ThrowOnNotFoundWebhookEventClassCodeFiles = _throwOnNotFoundWebhookEventClassCodeFiles,
                ThrowOnNotFoundRequestModelSerializationSampleFiles = _throwOnNotFoundRequestModelSerializationSampleFiles,
                ThrowOnNotFoundResponseModelSerializationSampleFiles = _throwOnNotFoundResponseModelSerializationSampleFiles,
                ThrowOnNotFoundWebhookEventSerializationSampleFiles = _throwOnNotFoundWebhookEventSerializationSampleFiles
            };
        }

        private IEnumerable<SourceFileAnalyzerRuleUnit> ScanAnalyzerRuleUnits()
        {
            DirectoryInfo scannedRequestModelClassDirectory = new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceRequestModelClassCodeSubDirectory));
            FileInfo[] scannedRequestModelClassCodeFiles = _customRequestModelClassCodeFilesScanner is not null
                ? _customRequestModelClassCodeFilesScanner.Invoke(scannedRequestModelClassDirectory).ToArray()
                : scannedRequestModelClassDirectory.GetAllFiles("*.cs")
                    .Where(f =>
                        Path.GetFileNameWithoutExtension(f.Name).EndsWith(AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX)
                    )
                    .Where(f => _ignoreRequestModelClassCodeFiles is null || !_ignoreRequestModelClassCodeFiles.Invoke(f))
                    .ToArray();
            if (!scannedRequestModelClassCodeFiles.Any() && _throwOnNotFoundRequestModelClassCodeFiles)
                throw new AnalysisException($"未在目录 {scannedRequestModelClassDirectory.FullName} 下扫描到表示 API 请求模型类的代码文件。");

            DirectoryInfo scannedResponseModelClassDirectory = new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceResponseModelClassCodeSubDirectory));
            FileInfo[] scannedResponseModelClassCodeFiles = _customResponseModelClassCodeFilesScanner is not null
                ? _customResponseModelClassCodeFilesScanner.Invoke(scannedResponseModelClassDirectory).ToArray()
                : scannedResponseModelClassDirectory.GetAllFiles("*.cs")
                    .Where(f =>
                        Path.GetFileNameWithoutExtension(f.Name).EndsWith(AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX)
                    )
                    .Where(f => _ignoreResponseModelClassCodeFiles is null || !_ignoreResponseModelClassCodeFiles.Invoke(f))
                    .ToArray();
            if (!scannedResponseModelClassCodeFiles.Any() && _throwOnNotFoundResponseModelClassCodeFiles)
                throw new AnalysisException($"未在目录 {scannedResponseModelClassDirectory.FullName} 下扫描到表示 API 响应模型类的代码文件。");

            DirectoryInfo scannedExecutingExtensionClassDirectory = new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceExecutingExtensionClassCodeSubDirectory));
            FileInfo[] scannedExecutingExtensionClassCodeFiles = _customExecutingExtensionClassCodeFilesScanner is not null
                ? _customExecutingExtensionClassCodeFilesScanner.Invoke(scannedExecutingExtensionClassDirectory).ToArray()
                : scannedExecutingExtensionClassDirectory.GetAllFiles("*.cs")
                    .Where(f =>
                        new Regex(_projectSourceExecutingExtensionClassCodeFileNameRegex ?? AnalyzerDefaults.DEFAULT_EXECUTING_EXTENSION_NAME_REGEX).IsMatch(Path.GetFileNameWithoutExtension(f.Name))
                    )
                    .Where(f => _ignoreExecutingExtensionClassCodeFiles is null || !_ignoreExecutingExtensionClassCodeFiles.Invoke(f))
                    .ToArray();
            if (!scannedExecutingExtensionClassCodeFiles.Any() && _throwOnNotFoundExecutingExtensionClassCodeFiles)
                throw new AnalysisException($"未在目录 {scannedExecutingExtensionClassDirectory.FullName} 下扫描到表示 API 接口方法类的代码文件。");

            DirectoryInfo scannedWebhookEventClassDirectory = new DirectoryInfo(Path.Combine(_projectSourceRootDirectory, _projectSourceWebhookEventClassCodeSubDirectory));
            FileInfo[] scannedWebhookEventClassCodeFiles = _customWebhookEventClassCodeFilesScanner is not null
                ? _customWebhookEventClassCodeFilesScanner.Invoke(scannedWebhookEventClassDirectory).ToArray()
                : scannedWebhookEventClassDirectory.GetAllFiles("*.cs")
                    .Where(f =>
                        Path.GetFileNameWithoutExtension(f.Name).EndsWith(AnalyzerDefaults.NAMING_WEBHOOK_EVENT_SUFFIX)
                    )
                    .Where(f => _ignoreWebhookEventClassCodeFiles is null || !_ignoreWebhookEventClassCodeFiles.Invoke(f))
                    .ToArray();
            if (!scannedWebhookEventClassCodeFiles.Any() && _throwOnNotFoundWebhookEventClassCodeFiles)
                throw new AnalysisException($"未在目录 {scannedWebhookEventClassDirectory.FullName} 下扫描到表示回调通知事件模型类的代码文件。");

            DirectoryInfo scannedRequestModelSerializationSampleDirectory = new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestRequestModelSerializationSampleSubDirectory));
            FileInfo[] scannedRequestModelSerializationSampleFiles = _customRequestModelSerializationSampleFilesScanner is not null
                ? _customRequestModelSerializationSampleFilesScanner.Invoke(scannedRequestModelSerializationSampleDirectory).ToArray()
                : scannedRequestModelSerializationSampleDirectory.GetAllFiles("*.*")
                    .Where(f =>
                        (IsJsonFile(f) || IsXmlFile(f)) &&
                        Path.GetFileNameWithoutExtension(f.Name).Split('.').First().EndsWith(AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX)
                    )
                    .Where(f => _ignoreRequestModelSerializationSampleFiles is null || !_ignoreRequestModelSerializationSampleFiles.Invoke(f))
                    .ToArray();
            if (!scannedRequestModelSerializationSampleFiles.Any() && _throwOnNotFoundRequestModelSerializationSampleFiles)
                throw new AnalysisException($"未在目录 {scannedRequestModelSerializationSampleDirectory.FullName} 下扫描到表示 API 请求模型序列化后的样例文件。");

            DirectoryInfo scannedResponseModelSerializationSampleDirectory = new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestResponseModelSerializationSampleSubDirectory));
            FileInfo[] scannedResponseModelSerializationSampleFiles = _customResponseModelSerializationSampleFilesScanner is not null
                ? _customResponseModelSerializationSampleFilesScanner.Invoke(scannedResponseModelSerializationSampleDirectory).ToArray()
                : scannedResponseModelSerializationSampleDirectory.GetAllFiles("*.*")
                    .Where(f =>
                        (IsJsonFile(f) || IsXmlFile(f)) &&
                        Path.GetFileNameWithoutExtension(f.Name).Split('.').First().EndsWith(AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX)
                    )
                    .Where(f => _ignoreResponseModelSerializationSampleFiles is null || !_ignoreResponseModelSerializationSampleFiles.Invoke(f))
                    .ToArray();
            if (!scannedResponseModelSerializationSampleFiles.Any() && _throwOnNotFoundResponseModelSerializationSampleFiles)
                throw new AnalysisException($"未在目录 {scannedResponseModelSerializationSampleDirectory.FullName} 下扫描到表示 API 响应模型序列化后的样例文件。");

            DirectoryInfo scannedWebhookEventSerializationSampleDirectory = new DirectoryInfo(Path.Combine(_projectTestRootDirectory, _projectTestWebhookEventSerializationSampleSubDirectory));
            FileInfo[] scannedWebhookEventSerializationSampleFiles = _customWebhookEventSerializationSampleFilesScanner is not null
                ? _customWebhookEventSerializationSampleFilesScanner.Invoke(scannedWebhookEventSerializationSampleDirectory).ToArray()
                : scannedWebhookEventSerializationSampleDirectory.GetAllFiles("*.*")
                    .Where(f =>
                        (IsJsonFile(f) || IsXmlFile(f)) &&
                        Path.GetFileNameWithoutExtension(f.Name).Split('.').First().EndsWith(AnalyzerDefaults.NAMING_WEBHOOK_EVENT_SUFFIX)
                    )
                    .Where(f => _ignoreWebhookEventSerializationSampleFiles is null || !_ignoreWebhookEventSerializationSampleFiles.Invoke(f))
                    .ToArray();
            if (!scannedWebhookEventSerializationSampleFiles.Any() && _throwOnNotFoundWebhookEventSerializationSampleFiles)
                throw new AnalysisException($"未在目录 {scannedWebhookEventSerializationSampleDirectory.FullName} 下扫描到表示回调通知事件模型序列化后的样例文件。");

            return Array.Empty<SourceFileAnalyzerRuleUnit>()
                .Concat(scannedRequestModelClassCodeFiles.Select(f => new SourceFileAnalyzerRuleUnit(SourceFileKinds.CSharp, SourceFileContentKinds.RequestModelClassCode, f)))
                .Concat(scannedResponseModelClassCodeFiles.Select(f => new SourceFileAnalyzerRuleUnit(SourceFileKinds.CSharp, SourceFileContentKinds.ResponseModelClassCode, f)))
                .Concat(scannedExecutingExtensionClassCodeFiles.Select(f => new SourceFileAnalyzerRuleUnit(SourceFileKinds.CSharp, SourceFileContentKinds.ExecutingExtensionClassCode, f)))
                .Concat(scannedWebhookEventClassCodeFiles.Select(f => new SourceFileAnalyzerRuleUnit(SourceFileKinds.CSharp, SourceFileContentKinds.WebhookEventClassCode, f)))
                .Concat(scannedRequestModelSerializationSampleFiles.Select(f => new SourceFileAnalyzerRuleUnit(IsJsonFile(f) ? SourceFileKinds.Json : IsXmlFile(f) ? SourceFileKinds.Xml : SourceFileKinds.Unknown, SourceFileContentKinds.RequestModelSerializationSample, f)))
                .Concat(scannedResponseModelSerializationSampleFiles.Select(f => new SourceFileAnalyzerRuleUnit(IsJsonFile(f) ? SourceFileKinds.Json : IsXmlFile(f) ? SourceFileKinds.Xml : SourceFileKinds.Unknown, SourceFileContentKinds.ResponseModelSerializationSample, f)))
                .Concat(scannedWebhookEventSerializationSampleFiles.Select(f => new SourceFileAnalyzerRuleUnit(IsJsonFile(f) ? SourceFileKinds.Json : IsXmlFile(f) ? SourceFileKinds.Xml : SourceFileKinds.Unknown, SourceFileContentKinds.WebhookEventSerializationSample, f)))
                .ToImmutableArray();
        }

        private string ReadFileContent(FileInfo fileInfo)
        {
            return _filePathToContentMap.GetOrAdd(fileInfo.FullName, static (filePath) => File.ReadAllText(filePath, Encoding.UTF8));
        }

        private void UseRule_ReqAndRespModelOrWebhhookEventCodeFileNameCase()
        {
            /**
             * 目标：
             *   API 请求或响应、回调通知事件模型类的代码。
             * 
             * 规则：
             *   代码中声明的类名，必须与源文件名相同。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.RequestModelClassCode &&
                    cur.ContentKind != SourceFileContentKinds.ResponseModelClassCode &&
                    cur.ContentKind != SourceFileContentKinds.WebhookEventClassCode)
                    return;

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadFileContent(cur.FileInfo)).WithFilePath(cur.FileInfo.FullName);
                CompilationUnitSyntax syntaxRootNode = syntaxTree.GetCompilationUnitRoot();
                ClassDeclarationSyntax[] syntaxClassDeclarationNodes = syntaxRootNode.Members
                    .Where(s => SourceFileCodeSyntaxKinds.BaseNamespaceDeclaration.Contains(s.Kind()))
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .SelectMany(s => s.Members
                        .Where(m => m.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                    )
                    .Where(s => s.TypeParameterList is null)
                    .ToArray();
                if (syntaxClassDeclarationNodes.Length != 1)
                    throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 中未识别到唯一的类声明语法结构。");

                string className = syntaxClassDeclarationNodes.Single().Identifier.ToFullString().Trim();
                string fileName = Path.GetFileNameWithoutExtension(cur.FileInfo.Name);
                if (!string.Equals(fileName, className))
                    throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 中定义的类名与源文件名不匹配。不匹配。（Expected: \"{fileName}\", Actual: \"{className}\"）");
            });
        }

        private void UseRule_ReqAndRespModelCodeFileXmlDocumentationComment()
        {
            /**
             * 目标：
             *   API 请求或响应模型类的代码。
             * 
             * 规则：
             *   必须包含符合规范的 XML 文档注释。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.RequestModelClassCode &&
                    cur.ContentKind != SourceFileContentKinds.ResponseModelClassCode)
                    return;

                GetUrlFromReqAndRespModelCodeComment(cur.FileInfo, cur.ContentKind, throwOnError: true);
            });
        }

        private void UseRule_RequestModelCodeFilePropertiesInitialization()
        {
            /**
             * 目标：
             *   API 请求模型类的代码。
             * 
             * 规则：
             *   请求模型中的属性，如果有初始赋值，值不能为 `null` 或 `default`。
             *   请求模型中 Nullable 类型的属性，不能有初始赋值。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.RequestModelClassCode)
                    return;

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadFileContent(cur.FileInfo)).WithFilePath(cur.FileInfo.FullName);
                CompilationUnitSyntax syntaxRootNode = syntaxTree.GetCompilationUnitRoot();
                ClassDeclarationSyntax syntaxClassDeclarationNode = syntaxRootNode.Members
                    .Where(s => SourceFileCodeSyntaxKinds.BaseNamespaceDeclaration.Contains(s.Kind()))
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .SelectMany(s => s.Members
                        .Where(m => m.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                    )
                    .Where(s => s.TypeParameterList is null)
                    .Single(s => s.Identifier.ValueText == cur.FileInfo.Name.Split('.')[0]);

                DeepCheck(cur.FileInfo.FullName, syntaxClassDeclarationNode);
            });

            static void DeepCheck(string filePath, ClassDeclarationSyntax node)
            {
                PropertyDeclarationSyntax[] propertyDeclarationSyntaxNodes = node.ChildNodes()
                    .Where(s => s.IsKind(SyntaxKind.PropertyDeclaration))
                    .OfType<PropertyDeclarationSyntax>()
                    .ToArray();
                foreach (PropertyDeclarationSyntax propertyDeclarationSyntaxNode in propertyDeclarationSyntaxNodes)
                {
                    if (propertyDeclarationSyntaxNode.AttributeLists.Any(a => a.ToFullString().Contains("JsonIgnore") || a.ToFullString().Contains("XmlIgnore")))
                        continue; // 跳过不可序列化的字段

                    if (propertyDeclarationSyntaxNode.Type is PredefinedTypeSyntax predefinedTypeSyntaxNode)
                    {
                        string keyword = predefinedTypeSyntaxNode.Keyword.ToFullString().Trim();
                        bool isNullable = keyword.EndsWith("?");
                        bool isInitialized = propertyDeclarationSyntaxNode.Initializer is not null;

                        if (isInitialized && propertyDeclarationSyntaxNode.Initializer!.ToFullString().Contains("default"))
                            throw new AnalysisException($"文件 \"{filePath}\" 应是一个 API 请求模型的代码文件，但其中存在不合理的属性初始赋值语句：\"{propertyDeclarationSyntaxNode.Identifier.ValueText}\" 不应初始化为 `default`。");
                        if (isInitialized && propertyDeclarationSyntaxNode.Initializer!.ToFullString().Contains("null"))
                            throw new AnalysisException($"文件 \"{filePath}\" 应是一个 API 请求模型的代码文件，但其中存在不合理的属性初始赋值语句：\"{propertyDeclarationSyntaxNode.Identifier.ValueText}\" 不应初始化为 `null`。");
                        if (isNullable && isInitialized)
                            throw new AnalysisException($"文件 \"{filePath}\" 应是一个 API 请求模型的代码文件，但其中存在不合理的属性初始赋值语句：\"{propertyDeclarationSyntaxNode.Identifier.ValueText}\" 是可空的。");
                    }
                }

                string identifier = node.Identifier.ToFullString().Trim();
                if (string.Equals(identifier, "Types"))
                {
                    ClassDeclarationSyntax[] classDeclarationSyntaxNodes = node.ChildNodes()
                        .Where(s => s.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                        .ToArray();
                    foreach (ClassDeclarationSyntax classDeclarationSyntaxNode in classDeclarationSyntaxNodes)
                    {
                        DeepCheck(filePath, classDeclarationSyntaxNode);
                    }
                }
            }
        }

        private void UseRule_ResponseModelCodeFilePropertiesInitialization()
        {
            /**
             * 目标：
             *   API 响应模型类的代码。
             * 
             * 规则：
             *   响应模型中的属性，如果有初始赋值，值只能为 `default`。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.ResponseModelClassCode)
                    return;

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadFileContent(cur.FileInfo)).WithFilePath(cur.FileInfo.FullName);
                CompilationUnitSyntax syntaxRootNode = syntaxTree.GetCompilationUnitRoot();
                ClassDeclarationSyntax syntaxClassDeclarationNode = syntaxRootNode.Members
                    .Where(s => SourceFileCodeSyntaxKinds.BaseNamespaceDeclaration.Contains(s.Kind()))
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .SelectMany(s => s.Members
                        .Where(m => m.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                    )
                    .Where(s => s.TypeParameterList is null)
                    .Single(s => s.Identifier.ValueText == cur.FileInfo.Name.Split('.')[0]);

                DeepCheck(cur.FileInfo.FullName, syntaxClassDeclarationNode);
            });

            void DeepCheck(string filePath, ClassDeclarationSyntax node)
            {
                PropertyDeclarationSyntax[] propertyDeclarationSyntaxNodes = node.ChildNodes()
                    .Where(s => s.IsKind(SyntaxKind.PropertyDeclaration))
                    .Select(s => (PropertyDeclarationSyntax)s)
                    .ToArray();
                foreach (PropertyDeclarationSyntax propertyDeclarationSyntaxNode in propertyDeclarationSyntaxNodes)
                {
                    if (propertyDeclarationSyntaxNode.AttributeLists.Any(a => a.ToFullString().Contains("JsonIgnore") || a.ToFullString().Contains("XmlIgnore")))
                        continue; // 跳过不可序列化的字段

                    if (propertyDeclarationSyntaxNode.Type is PredefinedTypeSyntax predefinedTypeSyntaxNode)
                    {
                        bool isInitialized = propertyDeclarationSyntaxNode.Initializer is not null;
                        if (isInitialized && !propertyDeclarationSyntaxNode.Initializer!.ToFullString().Contains("default!"))
                            throw new AnalysisException($"文件 '{filePath}' 应是一个 API 响应模型的代码文件，但其中存在不合理的属性初始赋值语句：属性 \"{propertyDeclarationSyntaxNode.Identifier.ValueText}\" 不应初始化、或应初始化为 `default!`。");
                    }
                }

                string identifier = node.Identifier.ToFullString().Trim();
                if (string.Equals(identifier, "Types"))
                {
                    ClassDeclarationSyntax[] classDeclarationSyntaxNodes = node.ChildNodes()
                        .Where(s => s.IsKind(SyntaxKind.ClassDeclaration))
                        .Select(s => (ClassDeclarationSyntax)s)
                        .ToArray();
                    foreach (ClassDeclarationSyntax classDeclarationSyntaxNode in classDeclarationSyntaxNodes)
                    {
                        DeepCheck(filePath, classDeclarationSyntaxNode);
                    }
                }
            }
        }

        private void UseRule_ExecutingExtensionCodeFileXmlDocumentationComment()
        {
            /**
             * 目标：
             *   API 接口方法类的代码。
             * 
             * 规则：
             *   必须包含符合规范的 XML 文档注释。
             *   与对应的请求/响应模型类的代码中的文档注释中描述的 URL 必须匹配。
             */

            AddRule((_, agg, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.ExecutingExtensionClassCode)
                    return;

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadFileContent(cur.FileInfo)).WithFilePath(cur.FileInfo.FullName);
                CompilationUnitSyntax syntaxRootNode = syntaxTree.GetCompilationUnitRoot();
                MethodDeclarationSyntax[] syntaxMethodDeclarationNodes = syntaxRootNode.Members
                    .Where(s => SourceFileCodeSyntaxKinds.BaseNamespaceDeclaration.Contains(s.Kind()))
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .SelectMany(s => s.Members
                        .Where(s => s.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                    )
                    .SelectMany(s => s.Members
                        .Where(s => s.IsKind(SyntaxKind.MethodDeclaration))
                        .OfType<MethodDeclarationSyntax>()
                    )
                    .Where(s =>
                    {
                        string methodName = s.Identifier.ToFullString().Trim();
                        return methodName.StartsWith(AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX) &&
                               methodName.EndsWith(AnalyzerDefaults.NAMING_EXECUTING_METHOD_SUFFIX);
                    })
                    .ToArray();
                if (syntaxMethodDeclarationNodes.Length == 0)
                    throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 中未识别到函数声明语法结构。");

                foreach (MethodDeclarationSyntax syntaxMethodDeclarationNode in syntaxMethodDeclarationNodes)
                {
                    string methodName = syntaxMethodDeclarationNode.Identifier.ToFullString().Trim();
                    string methodNameWithoutAffix = methodName.Substring(AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX.Length, methodName.Length - AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX.Length - AnalyzerDefaults.NAMING_EXECUTING_METHOD_SUFFIX.Length);

                    SyntaxTrivia? syntaxDocumentationCommentTrivia = syntaxMethodDeclarationNode
                        .GetLeadingTrivia()
                        .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                    if (syntaxDocumentationCommentTrivia is null)
                        throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 下的函数 \"{methodName}\" 中未识别到 XML 文档注释语法结构。");

                    string commentSummaryContent = syntaxDocumentationCommentTrivia.Value.GetStructure()!
                        .DescendantNodes()
                        .OfType<XmlElementSyntax>()
                        .Where(node => node.StartTag.Name.ToString() == "summary")
                        .Select(n => n.Content.ToString().Trim(' ', '\t', '\r', '\n', '/'))
                        .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s))!;
                    if (!Regex.IsMatch(commentSummaryContent, REGEX_DOCCOMMENT_EXECUTING_METHOD))
                        throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 中未识别到有效的 XML 文档注释语法结构。（形如：“异步调用 [VERB] /path/to/url 接口。”）");

                    Match commentSummaryMatch = new Regex(REGEX_DOCCOMMENT_EXECUTING_METHOD).Match(commentSummaryContent);
                    string expectedUrlVerb = commentSummaryMatch.Groups[1].Value;
                    string expectedUrlPath = commentSummaryMatch.Groups[2].Value;

                    if (expectedUrlVerb is not null)
                    {
                        ExecutingMethodUrlVerbSyntaxWalker syntaxWalker = new ExecutingMethodUrlVerbSyntaxWalker();
                        syntaxWalker.Reset().Visit(syntaxMethodDeclarationNode);
                        if (!string.Equals(expectedUrlVerb, syntaxWalker.Result, StringComparison.OrdinalIgnoreCase))
                            throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其 XML 文档注释中描述的 URL 谓词与实际的代码实现不一致。（Expected: \"{expectedUrlVerb}\", Actual: \"{syntaxWalker.Result}\"）");
                    }

                    if (expectedUrlPath is not null)
                    {
                        ExecutingMethodUrlPathSegmentsSyntaxWalker syntaxWalker = new ExecutingMethodUrlPathSegmentsSyntaxWalker();
                        syntaxWalker.Reset().Visit(syntaxMethodDeclarationNode);

                        string[] expectedUrlPathSegments = expectedUrlPath.TrimStart('/').Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] actualUrlPathSegments = syntaxWalker.Result ?? Array.Empty<string>();
                        for (int i = 0, len = Math.Max(expectedUrlPathSegments.Length, actualUrlPathSegments.Length); i < len; i++)
                        {
                            string? s1 = expectedUrlPathSegments.Length > i ? expectedUrlPathSegments[i] : null;
                            string? s2 = actualUrlPathSegments.Length > i ? actualUrlPathSegments[i] : null;
                            if (s2 is not null && s2.StartsWith("{") && s2.EndsWith("}"))
                            {
                                if (s1 is null || !s1.StartsWith("{") || !s1.EndsWith("}"))
                                    throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其 XML 文档注释中描述的 URL 路径与实际的代码实现不一致。（Expected: \"{"/" + string.Join("/", expectedUrlPathSegments)}\", Actual: \"{"/" + string.Join("/", actualUrlPathSegments)}\"）");
                            }
                            else if (!string.Equals(s1, s2))
                            {
                                if (i == len - 1 && s2 is not null && s2.EndsWith("/") && !string.Equals(s1, s2.TrimEnd('/')))
                                    throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其 XML 文档注释中描述的 URL 路径与实际的代码实现不一致。（Expected: \"{"/" + string.Join("/", expectedUrlPathSegments)}\", Actual: \"{"/" + string.Join("/", actualUrlPathSegments)}\"）");
                            }
                        }
                    }

                    if (expectedUrlVerb is not null && expectedUrlPath is not null)
                    {
                        SourceFileAnalyzerRuleUnit? reqUnit = agg.FirstOrDefault(e =>
                            e.FileKind == SourceFileKinds.CSharp &&
                            e.ContentKind == SourceFileContentKinds.RequestModelClassCode &&
                            e.FileInfo.Name.Split('.')[0] == methodNameWithoutAffix + AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX
                        );
                        if (reqUnit is null)
                            throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其对应的请求模型类的代码文件未找到。");

                        SourceFileAnalyzerRuleUnit? respUnit = agg.FirstOrDefault(e =>
                            e.FileKind == SourceFileKinds.CSharp &&
                            e.ContentKind == SourceFileContentKinds.ResponseModelClassCode &&
                            e.FileInfo.Name.Split('.')[0] == methodNameWithoutAffix + AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX
                        );
                        if (respUnit is null)
                            throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其对应的响应模型类的代码文件未找到。");

                        var reqCommentUrl = GetUrlFromReqAndRespModelCodeComment(reqUnit.FileInfo, reqUnit.ContentKind, throwOnError: false);
                        var respCommentUrl = GetUrlFromReqAndRespModelCodeComment(respUnit.FileInfo, respUnit.ContentKind, throwOnError: false);
                        if (!string.Equals(expectedUrlVerb, reqCommentUrl?.Verb) || !string.Equals(expectedUrlPath, reqCommentUrl?.Path))
                            throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其 XML 文档注释中描述的 URL 与对应的请求模型类的代码文件不匹配。（Expected: \"[{expectedUrlVerb}] {expectedUrlPath}\", Actual: \"[{reqCommentUrl?.Verb}] {reqCommentUrl?.Path}\"）");
                        if (!string.Equals(expectedUrlVerb, respCommentUrl?.Verb) || !string.Equals(expectedUrlPath, respCommentUrl?.Path))
                            throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\"下的函数 \"{methodName}\" 应是一个 API 接口方法，但其 XML 文档注释中描述的 URL 与对应的响应模型类的代码文件不匹配。（Expected: \"[{expectedUrlVerb}] {expectedUrlPath}\", Actual: \"[{respCommentUrl?.Verb}] {respCommentUrl?.Path}\"）");
                    }
                }
            });
        }

        private void UseRule_ReqAndRespModelSerializationSampleCanDesarialized()
        {
            /**
             * 目标：
             *   API 请求或响应模型类的序列化后的样例。
             * 
             * 规则：
             *   可使用该样例文件反序列化得到对应的请求或响应模型类实例。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.RequestModelSerializationSample &&
                    cur.ContentKind != SourceFileContentKinds.ResponseModelSerializationSample)
                    return;

                string fileContent = ReadFileContent(cur.FileInfo);
                string typeName = Path.GetFileNameWithoutExtension(cur.FileInfo.Name).Split('.').First();
                string typeFullName = $"{_sdkRequestModelDeclarationNamespace}.{typeName}";
                Type type = _sdkAssembly.GetType(typeFullName, throwOnError: true, ignoreCase: true)!;

                switch (cur.FileKind)
                {
                    case SourceFileKinds.Json:
                        {
                            if (!Helpers.JsonHelper.TryDeserialize(fileContent, type, out Exception ex))
                                throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：{ex.InnerException?.Message ?? ex.Message}。", ex);
                        }
                        break;

                    case SourceFileKinds.Xml:
                        {
                            if (!Helpers.XmlHelper.TryDeserialize(fileContent, type, out Exception ex))
                                throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：{ex.InnerException?.Message ?? ex.Message}。", ex);
                        }
                        break;

                    default:
                        throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：未知格式。");
                }
            });
        }

        private void UseRule_WebhhookEventSerializationSampleCanDesarialized()
        {
            /**
             * 目标：
             *   回调通知事件模型类的序列化后的样例。
             * 
             * 规则：
             *   可使用该样例文件反序列化得到对应的回调通知事件模型类实例。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.ContentKind != SourceFileContentKinds.WebhookEventSerializationSample)
                    return;

                string fileContent = ReadFileContent(cur.FileInfo);
                string typeName = Path.GetFileNameWithoutExtension(cur.FileInfo.Name).Split('.').First();
                string typeFullName = $"{_sdkWebhookEventDeclarationNamespace}.{typeName}";
                Type type = _sdkAssembly.GetType(typeFullName, throwOnError: true, ignoreCase: true)!;

                switch (cur.FileKind)
                {
                    case SourceFileKinds.Json:
                        {
                            if (!Helpers.JsonHelper.TryDeserialize(fileContent, type, out Exception ex))
                                throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：{ex.InnerException?.Message ?? ex.Message}。", ex);
                        }
                        break;

                    case SourceFileKinds.Xml:
                        {
                            if (!Helpers.XmlHelper.TryDeserialize(fileContent, type, out Exception ex))
                                throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：{ex.InnerException?.Message ?? ex.Message}。", ex);
                        }
                        break;

                    default:
                        throw new AnalysisException($"文件 \"{cur.FileInfo.FullName}\" 尝试反序列化失败：未知格式。");
                }
            });
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();

                    _customRules.Clear();
                    _customRequestModelClassCodeFilesScanner = null;
                    _customResponseModelClassCodeFilesScanner = null;
                    _customExecutingExtensionClassCodeFilesScanner = null;
                    _customWebhookEventClassCodeFilesScanner = null;
                    _customRequestModelSerializationSampleFilesScanner = null;
                    _customResponseModelSerializationSampleFilesScanner = null;
                    _customWebhookEventSerializationSampleFilesScanner = null;

                    _filePathToContentMap.Clear();

                    _cancellationTokenSource.Dispose();
                }

                _disposed = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    partial class SourceFileAnalyzer
    {
        private (string Verb, string Path)? GetUrlFromReqAndRespModelCodeComment(FileInfo fileInfo, SourceFileContentKinds fileContentKind, bool throwOnError = true)
        {
            try
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(ReadFileContent(fileInfo)).WithFilePath(fileInfo.FullName);
                CompilationUnitSyntax syntaxRootNode = syntaxTree.GetCompilationUnitRoot();
                ClassDeclarationSyntax[] syntaxClassDeclarationNodes = syntaxRootNode.Members
                    .Where(s => SourceFileCodeSyntaxKinds.BaseNamespaceDeclaration.Contains(s.Kind()))
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .SelectMany(s => s.Members
                        .Where(m => m.IsKind(SyntaxKind.ClassDeclaration))
                        .OfType<ClassDeclarationSyntax>()
                    )
                    .Where(s => s.TypeParameterList is null)
                    .Where(s => s.Identifier.ValueText == fileInfo.Name.Split('.')[0])
                    .ToArray();
                if (syntaxClassDeclarationNodes.Length != 1)
                    throw new AnalysisException($"文件 \"{fileInfo.FullName}\" 中未识别到唯一的类声明语法结构。");

                SyntaxTrivia? syntaxDocumentationCommentTrivia = syntaxClassDeclarationNodes.Single()
                    .GetLeadingTrivia()
                    .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                if (syntaxDocumentationCommentTrivia is null)
                    throw new AnalysisException($"文件 \"{fileInfo.FullName}\" 中未识别到 XML 文档注释语法结构。");

                string commentSummaryContent = syntaxDocumentationCommentTrivia.Value.GetStructure()!
                    .DescendantNodes()
                    .OfType<XmlElementSyntax>()
                    .Where(n => n.StartTag.Name.ToString() == "summary")
                    .Select(n => n.Content.ToString().Trim(' ', '\t', '\r', '\n', '/'))
                    .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s))!;
                commentSummaryContent = Regex.Replace(commentSummaryContent, @"<.*?>", "");

                switch (fileContentKind)
                {
                    case SourceFileContentKinds.RequestModelClassCode:
                        {
                            if (!Regex.IsMatch(commentSummaryContent, REGEX_DOCCOMMENT_REQUEST_MODEL))
                                throw new AnalysisException($"文件 \"{fileInfo.FullName}\" 中未识别到有效的 XML 文档注释语法结构。（形如：“表示 [VERB] /path/to/url 接口的请求。”）");

                            Match match = new Regex(REGEX_DOCCOMMENT_REQUEST_MODEL).Match(commentSummaryContent);
                            return (match.Groups[1].Value, match.Groups[2].Value);
                        }

                    case SourceFileContentKinds.ResponseModelClassCode:
                        {
                            if (!Regex.IsMatch(commentSummaryContent, REGEX_DOCCOMMENT_RESPONSE_MODEL))
                                throw new AnalysisException($"文件 \"{fileInfo.FullName}\" 中未识别到有效的 XML 文档注释语法结构。（形如：“表示 [VERB] /path/to/url 接口的响应。”）");

                            Match match = new Regex(REGEX_DOCCOMMENT_RESPONSE_MODEL).Match(commentSummaryContent);
                            return (match.Groups[1].Value, match.Groups[2].Value);
                        }

                    default:
                        throw new NotSupportedException();
                }
            }
            catch
            {
                if (throwOnError)
                    throw;
            }

            return null;
        }

        private class ExecutingMethodUrlVerbSyntaxWalker : CSharpSyntaxWalker
        {
            public string? Result { get; private set; }

            public ExecutingMethodUrlVerbSyntaxWalker Reset()
            {
                Result = null;
                return this;
            }

            public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                /**
                 * 适用于 `new HttpMethod("VERB")`
                 */

                if (node.Type.ToString() == nameof(HttpMethod))
                {
                    if (node.ArgumentList?.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literalExpressionSyntaxNode)
                    {
                        Result = literalExpressionSyntaxNode.Token.ValueText;
                    }
                }

                base.VisitObjectCreationExpression(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                /**
                 * 适用于 `HttpMethod.VERB`
                 */

                if (node.Expression is IdentifierNameSyntax identifierNameSyntaxNode &&
                    identifierNameSyntaxNode.Identifier.Text == nameof(HttpMethod))
                {
                    Result = node.Name.Identifier.ValueText;
                }

                base.VisitMemberAccessExpression(node);
            }
        }

        private class ExecutingMethodUrlPathSegmentsSyntaxWalker : CSharpSyntaxWalker
        {
            public string[]? Result { get; private set; }

            public ExecutingMethodUrlPathSegmentsSyntaxWalker Reset()
            {
                Result = null;
                return this;
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (node.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntaxNode &&
                    node.ArgumentList?.Arguments.Count >= 3 &&
                    node.ToFullString().Contains("Create") &&
                    node.ToFullString().Contains("Request"))
                {
                    string arg1 = node.ArgumentList.Arguments[0].ToFullString();
                    string arg2 = node.ArgumentList.Arguments[1].ToFullString();
                    if (arg1 is not null && arg2.Contains(nameof(HttpMethod)))
                    {
                        Result = node.ArgumentList.Arguments
                            .Skip(2)
                            .Select(n => n.ToFullString())
                            .Select(s =>
                            {
                                if (s.StartsWith("\"") && s.EndsWith("\""))
                                    return s.Trim('\"');

                                if (s == "string.Empty")
                                    return string.Empty;

                                return "{" + s + "}";
                            })
                            .ToArray();
                    }
                }

                base.VisitInvocationExpression(node);
            }
        }
    }
}
