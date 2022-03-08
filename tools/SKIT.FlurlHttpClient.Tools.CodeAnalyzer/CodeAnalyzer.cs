using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public class CodeAnalyzer
    {
        private const string MEMBER_NAME_SUFFIX_REQUEST_MODEL = "Request";
        private const string MEMBER_NAME_SUFFIX_RESPONSE_MODEL = "Response";
        private const string MEMBER_NAME_PREFIX_API_METHOD = "Execute";
        private const string MEMBER_NAME_SUFFIX_API_METHOD = "Async";

        private const string REGEX_DOCCOMMENT_REQUEST = @"表示\s\[([a-zA-Z]+)\]\s(\S*)\s接口的请求。";
        private const string REGEX_DOCCOMMENT_RESPONSE = @"表示\s\[([a-zA-Z]+)\]\s(\S*)\s接口的响应。";
        private const string REGEX_DOCCOMMENT_API = @"异步调用\s\[([a-zA-Z]+)\]\s(\S*)\s接口。";

        private readonly object _lockObj;
        private readonly string _dirSourceCode;
        private readonly string _dirSourceCodeForApiModels;
        private readonly string _dirSourceCodeForApiEvents;
        private readonly string _dirSourceCodeForApiMethods;
        private readonly string _dirTestSample;
        private readonly string _dirTestSampleForApiModels;
        private readonly string _dirTestSampleForApiEvents;
        private readonly string _targetSdkApiModelInnerNestedTypesWrapperIdentifier;
        private readonly string _targetSdkApiModelNamespaceUnderAssemblyIdentifier;
        private readonly string _targetSdkApiEventNamespaceUnderAssemblyIdentifier;
        private readonly string _targetSdkApiMethodNamespaceUnderAssemblyIdentifier;
        private readonly string _targetSdkApiMethodInnerFlurlRequestInitializerIdentifier;
        private readonly bool _allowNotFoundModelTypes;
        private readonly bool _allowNotFoundEventTypes;
        private readonly bool _allowNotFoundModelSamples;
        private readonly bool _allowNotFoundEventSamples;

        protected Assembly TargetAssembly { get; }

        protected string TargetAssemblyNamespaceForApiModels { get { return $"{TargetAssembly.GetName().Name}.{_targetSdkApiModelNamespaceUnderAssemblyIdentifier}".TrimEnd('.'); } }
        
        protected string TargetAssemblyNamespaceForApiEvents { get { return $"{TargetAssembly.GetName().Name}.{_targetSdkApiEventNamespaceUnderAssemblyIdentifier}".TrimEnd('.'); } }

        protected string TargetAssemblyNamespaceForApiMethods { get { return $"{TargetAssembly.GetName().Name}.{_targetSdkApiMethodNamespaceUnderAssemblyIdentifier}".TrimEnd('.'); } }

        protected IList<string> Errors { get; }

        public CodeAnalyzer(CodeAnalyzerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _lockObj = new object();
            _dirSourceCode = options.WorkDirectoryForSourceCode;
            _dirSourceCodeForApiModels = options.WorkSubDirectoryForApiModels;
            _dirSourceCodeForApiEvents = options.WorkSubDirectoryForApiEvents;
            _dirSourceCodeForApiMethods = options.WorkSubDirectoryForApiMethods;
            _dirTestSample = options.WorkDirectoryForTestSample;
            _dirTestSampleForApiModels = options.WorkSubDirectoryForApiModelSamples;
            _dirTestSampleForApiEvents = options.WorkSubDirectoryForApiEventSamples;
            _targetSdkApiModelInnerNestedTypesWrapperIdentifier = options.TargetSdkApiModelInnerNestedTypesWrapperIdentifier;
            _targetSdkApiModelNamespaceUnderAssemblyIdentifier = options.TargetSdkApiModelNamespaceUnderAssemblyIdentifier;
            _targetSdkApiEventNamespaceUnderAssemblyIdentifier = options.TargetSdkApiEventNamespaceUnderAssemblyIdentifier;
            _targetSdkApiMethodNamespaceUnderAssemblyIdentifier = options.TargetSdkApiMethodNamespaceUnderAssemblyIdentifier;
            _targetSdkApiMethodInnerFlurlRequestInitializerIdentifier = options.TargetSdkApiMethodInnerFlurlRequestInitializerIdentifier;
            _allowNotFoundModelTypes = options.AllowNotFoundModelTypes;
            _allowNotFoundEventTypes = options.AllowNotFoundEventTypes;
            _allowNotFoundModelSamples = options.AllowNotFoundModelSamples;
            _allowNotFoundEventSamples = options.AllowNotFoundEventSamples;

            TargetAssembly = Assembly.Load(options.AssemblyName);
            Errors = new List<string>();
        }

        public void Start()
        {
            lock (_lockObj)
            {
                AnalyzeAssembly();
                AnalyzeSourceCode();
                AnalyzeTestSample();
            }
        }

        public void Assert()
        {
            lock (_lockObj)
            {
                if (Errors.Any())
                {
                    throw new AggregateException($"扫描到共 {Errors.Count} 个代码质量问题。", Errors.Select(e => new Exception(e)));
                }
            }
        }

        public void Flush()
        {
            lock (_lockObj)
            {
                Errors.Clear();
            }
        }

        private void AnalyzeAssembly()
        {
            Type[] modelTypes = TargetAssembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.Equals(TargetAssemblyNamespaceForApiModels) &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsNested
                )
                .ToArray();
            Type[] eventTypes = TargetAssembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.Equals(TargetAssemblyNamespaceForApiEvents) &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsNested
                )
                .ToArray();
            Type[] apiTypes = TargetAssembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.Equals(TargetAssemblyNamespaceForApiMethods) &&
                    t.IsClass &&
                    t.IsAbstract &&
                    t.IsSealed &&
                    new Regex("[a-zA-Z0-9]+ClientExecute[a-zA-Z0-9]+Extensions$").IsMatch(t.Name)
                )
                .ToArray();
            Trace.Assert(modelTypes.Any() || _allowNotFoundModelTypes, $"程序集下不存在 API 模型类型，请检查 \"{_targetSdkApiModelNamespaceUnderAssemblyIdentifier}\" 命名空间是否为空。");
            Trace.Assert(eventTypes.Any() || _allowNotFoundEventTypes, $"程序集下不存在 API 事件类型，请检查 \"{_targetSdkApiEventNamespaceUnderAssemblyIdentifier}\" 命名空间是否为空。");
            Trace.Assert(apiTypes.Any() || _allowNotFoundModelTypes, $"程序集下不存在 API 扩展方法。");

            /* 校验 API 模型程序集类型 */
            Parallel.ForEach(modelTypes, AnalyzeAssembly_ModelTypeNameCase);
            Parallel.ForEach(modelTypes, AnalyzeAssembly_ModelTypeCollectionPropertyDeclaration);
            Parallel.ForEach(modelTypes, AnalyzeAssembly_ModelTypeJsonPropertyNameCase);

            /* 校验 API 事件程序集类型 */
            Parallel.ForEach(eventTypes, AnalyzeAssembly_EventTypeJsonPropertyNameCase);

            /* 校验 API 接口程序集类型 */
            Parallel.ForEach(apiTypes, AnalyzeAssembly_ApiTypeNameCase);
        }

        private void AnalyzeAssembly_ModelTypeNameCase(Type type)
        {
            WrapExceptionalAction(() =>
            {
                string typeName = type.GetNameWithoutGenerics();

                if (typeName.EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                {
                    if (!typeof(ICommonRequest).IsAssignableFrom(type))
                        throw new CodeAnalyzerException($"类型 '{type}' 应是一个请求模型，但却未实现接口类型 '{nameof(ICommonRequest)}'。");

                    if (!TargetAssembly.GetTypes().Any(t => string.Equals(t.GetNameWithoutGenerics(), $"{typeName.Substring(0, typeName.Length - MEMBER_NAME_SUFFIX_REQUEST_MODEL.Length)}{MEMBER_NAME_SUFFIX_RESPONSE_MODEL}")))
                        throw new CodeAnalyzerException($"类型 `{type}` 应是一个请求模型，但找不到对应的响应模型。");
                }
                else if (typeName.EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                {
                    if (!typeof(ICommonResponse).IsAssignableFrom(type))
                        throw new CodeAnalyzerException($"类型 '{type}' 应是一个响应模型，但却未实现接口类型 '{nameof(ICommonResponse)}'。");

                    if (!TargetAssembly.GetTypes().Any(t => string.Equals(t.GetNameWithoutGenerics(), $"{typeName.Substring(0, typeName.Length - MEMBER_NAME_SUFFIX_RESPONSE_MODEL.Length)}{MEMBER_NAME_SUFFIX_REQUEST_MODEL}")))
                        throw new CodeAnalyzerException($"类型 `{type}` 应是一个响应模型，但找不到对应的请求模型。");
                }
            });
        }

        private void AnalyzeAssembly_ApiTypeNameCase(Type type)
        {
            WrapExceptionalAction(() =>
            {
                MethodInfo[] apiMethods = type.GetMethods()
                    .Where(m =>
                        m.IsPublic &&
                        m.IsStatic &&
                        m.GetParameters().Any() &&
                        typeof(ICommonClient).IsAssignableFrom(m.GetParameters().First().ParameterType)
                    )
                    .ToArray();

                foreach (MethodInfo apiMethod in apiMethods)
                {
                    if (!apiMethod.ReturnType.IsGenericType || apiMethod.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但返回值类型不是 'Task'。");

                    ParameterInfo[] apiMethodParams = apiMethod.GetParameters();
                    if (apiMethodParams.Length != 3)
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但参数个数无效。");
                    Type param1Type = apiMethodParams[0].ParameterType;
                    Type param2Type = apiMethodParams[1].ParameterType;
                    Type param3Type = apiMethodParams[2].ParameterType;
                    Type returnType = apiMethod.ReturnType.GetGenericArguments().Single();

                    if (!typeof(ICommonClient).IsAssignableFrom(param1Type))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但第一个参数类型不是 '{nameof(ICommonClient)}'。");
                    if (!typeof(ICommonRequest).IsAssignableFrom(param2Type))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但第二个参数类型不是 '{nameof(ICommonRequest)}'。");
                    if (!typeof(CancellationToken).IsAssignableFrom(param3Type))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但第三个参数类型不是 'CancellationToken'。");
                    if (!typeof(ICommonResponse).IsAssignableFrom(returnType))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但返回值类型不是 'Task<{nameof(ICommonResponse)}>'。");
                    if (!param2Type.GetNameWithoutGenerics().EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但第二个参数类型命名不是以 '{MEMBER_NAME_SUFFIX_REQUEST_MODEL}' 结尾。");
                    if (!returnType.GetNameWithoutGenerics().EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但返回值类型命名不是以 '{MEMBER_NAME_SUFFIX_RESPONSE_MODEL}' 结尾。");
                    if (!apiMethod.Name.EndsWith(MEMBER_NAME_SUFFIX_API_METHOD))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但命名不是以 '{MEMBER_NAME_SUFFIX_API_METHOD}' 结尾。");
                    if (!apiMethod.Name.StartsWith(MEMBER_NAME_PREFIX_API_METHOD))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但命名不是以 '{MEMBER_NAME_PREFIX_API_METHOD}' 开头。");

                    string nameOfRequest = param2Type.GetNameWithoutGenerics().Substring(0, param2Type.GetNameWithoutGenerics().Length - MEMBER_NAME_SUFFIX_REQUEST_MODEL.Length);
                    string nameOfResponse = returnType.GetNameWithoutGenerics().Substring(0, returnType.GetNameWithoutGenerics().Length - MEMBER_NAME_SUFFIX_RESPONSE_MODEL.Length);
                    string nameOfApi = apiMethod.Name.Substring(MEMBER_NAME_PREFIX_API_METHOD.Length, apiMethod.Name.Length - MEMBER_NAME_PREFIX_API_METHOD.Length - MEMBER_NAME_SUFFIX_API_METHOD.Length);
                    if (!string.Equals(nameOfRequest, nameOfApi))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但命名与传入的请求模型命名不匹配。 (Expected: '{nameOfApi}', Actual: '{nameOfRequest}')");
                    if (!string.Equals(nameOfResponse, nameOfApi))
                        throw new CodeAnalyzerException($"类型 '{type}' 中的方法 '{apiMethod.Name}' 应是一个 API，但命名与返回的响应模型命名不匹配。 (Expected: '{nameOfApi}', Actual: '{nameOfResponse}')");
                }
            });
        }

        private void AnalyzeAssembly_ModelTypeCollectionPropertyDeclaration(Type type)
        {
            void DeepAnalyze(Type innerType)
            {
                PropertyInfo[] properties = innerType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() is not null)
                        continue;
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() is not null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true) is null)
                        continue;
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true) is null)
                        continue;

                    if (innerType.GetNameWithoutGenerics().EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                    {
                        if (property.PropertyType.IsArray && property.PropertyType.GetElementType() != typeof(byte))
                            throw new CodeAnalyzerException($"类型 {type} 上的属性 '{property.Name}' 是列表类型，不应在响应模型中声明。");
                    }
                    else if (innerType.GetNameWithoutGenerics().EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                    {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                            throw new CodeAnalyzerException($"类型 {type} 上的属性 '{property.Name}' 是数组类型，不应在请求模型中声明。");
                    }
                }

                Type[] nestedTypes = innerType.GetNestedType(_targetSdkApiModelInnerNestedTypesWrapperIdentifier)?.GetNestedTypes() ?? Array.Empty<Type>();
                foreach (Type nestedType in nestedTypes)
                {
                    DeepAnalyze(nestedType);
                }
            }

            WrapExceptionalAction(() =>
            {
                DeepAnalyze(type);
            });
        }

        private void AnalyzeAssembly_ModelTypeJsonPropertyNameCase(Type type)
        {
            void DeepAnalyze(Type innerType)
            {
                PropertyInfo[] properties = innerType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    Newtonsoft.Json.JsonPropertyAttribute attr1 = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true);
                    System.Text.Json.Serialization.JsonPropertyNameAttribute attr2 = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true);
                    if (!string.Equals(attr1?.PropertyName, attr2?.Name))
                        throw new CodeAnalyzerException($"类型 {type} 上的属性 '{property.Name}' 包含两个不同的 JSON 字段名：'{attr1?.PropertyName}' 和 '{attr2?.Name}'。");
                    if (!string.Equals(attr1?.PropertyName, attr1?.PropertyName?.Trim()))
                        throw new CodeAnalyzerException($"类型 {type} 上的属性 '{property.Name}' 包含无效的 JSON 字段名：'{attr1?.PropertyName}'。");
                    if (!string.Equals(attr2?.Name, attr2?.Name?.Trim()))
                        throw new CodeAnalyzerException($"类型 {type} 上的属性 '{property.Name}' 包含无效的 JSON 字段名：'{attr2?.Name}'。");
                }

                Type[] nestedTypes = innerType.GetNestedTypes();
                foreach (Type nestedType in nestedTypes)
                {
                    if (nestedType.IsClass && !nestedType.IsAbstract)
                        DeepAnalyze(nestedType);
                }
            }

            WrapExceptionalAction(() =>
            {
                DeepAnalyze(type);
            });
        }

        private void AnalyzeAssembly_EventTypeJsonPropertyNameCase(Type type)
        {
            AnalyzeAssembly_ModelTypeJsonPropertyNameCase(type);
        }

        private void AnalyzeSourceCode()
        {
            DirectoryInfo dir = new DirectoryInfo(_dirSourceCode);
            Trace.Assert(dir.Exists, $"工作目录 \"{dir.FullName}\" 不存在。");

            FileInfo[] modelDefinationFiles = dir.GetAllFiles($"{_dirSourceCodeForApiModels}/*.cs");
            FileInfo[] eventDefinationFiles = dir.GetAllFiles($"{_dirSourceCodeForApiEvents}/*.cs");
            FileInfo[] apiDefinationFiles = dir.GetAllFiles($"{_dirSourceCodeForApiMethods}/*ClientExecute*Extensions.cs");
            Trace.Assert(modelDefinationFiles.Any() || _allowNotFoundModelTypes, $"工作目录下不存在 API 模型源代码文件，请检查 \"{_dirSourceCodeForApiModels}\" 目录是否为空。");
            Trace.Assert(apiDefinationFiles.Any() || _allowNotFoundModelTypes, $"工作目录下不存在 API 接口源代码文件，请检查 \"{_dirSourceCodeForApiMethods}\" 目录是否为空。");

            /* 校验 API 模型源代码文件 */
            Parallel.ForEach(modelDefinationFiles, AnalyzeSourceCode_ModelDefinationFileDocumatationComment);
            Parallel.ForEach(modelDefinationFiles, AnalyzeSourceCode_ModelDefinationFileTypePropertyInitialization);

            /* 校验 API 事件源代码文件 */
            Parallel.ForEach(eventDefinationFiles, AnalyzeSourceCode_EventDefinationFileDocumatationComment);

            /* 校验 API 接口源代码文件 */
            Parallel.ForEach(apiDefinationFiles, AnalyzeSourceCode_ApiDefinationFileDocumatationComment);
        }

        private void AnalyzeSourceCode_ModelDefinationFileDocumatationComment(FileInfo file)
        {
            WrapExceptionalAction(() =>
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                string typeName = $"{TargetAssemblyNamespaceForApiModels}.{fileName}";
                Type? targetType = TargetAssembly.GetType(typeName, throwOnError: false);

                if (targetType == null)
                    throw new CodeAnalyzerException($"目标程序集中找不到类型 '{typeName}'。");

                if (targetType.IsAbstract || targetType.IsInterface)
                    return;

                if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(targetType), targetType, out Exception ex))
                    throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 JSON 序列化失败。", ex);
            });

            WrapExceptionalAction(() =>
            {
                using Stream fileStream = file.OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();
                if (!LanguageNames.CSharp.Equals(syntaxRoot.Language))
                    throw new NotSupportedException($"文件 '{file.FullName}' 无法以 C# 源码的形式加载。");

                ClassDeclarationSyntax[] classDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .ToArray();
                foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarationSyntaxes)
                {
                    string className = classDeclarationSyntax.Identifier.ToFullString().Trim();

                    SyntaxTrivia? syntaxTrivia = classDeclarationSyntax.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                    if (syntaxTrivia == null)
                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到文档注释结构。");

                    string comment = syntaxTrivia.Value.ToFullString();
                    if (className.EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                    {
                        if (!new Regex(REGEX_DOCCOMMENT_REQUEST).IsMatch(comment))
                            throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到有效的文档注释结构（形如：“表示 [FOO] /bar 接口的请求。”）。");
                    }
                    else if (className.EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                    {
                        if (!new Regex(REGEX_DOCCOMMENT_RESPONSE).IsMatch(comment))
                            throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到有效的文档注释结构（形如：“表示 [FOO] /bar 接口的响应。”）。");
                    }
                }
            });

            WrapExceptionalAction(() =>
            {
                using Stream fileStream = file.OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();
                if (!LanguageNames.CSharp.Equals(syntaxRoot.Language))
                    throw new NotSupportedException($"文件 '{file.FullName}' 无法以 C# 源码的形式加载。");

                ClassDeclarationSyntax[] classDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .ToArray();
                foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarationSyntaxes)
                {
                    string className = classDeclarationSyntax.Identifier.ToFullString().Trim();

                    SyntaxTrivia? syntaxTrivia = classDeclarationSyntax.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                    if (syntaxTrivia == null)
                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到文档注释结构。");

                    string comment = syntaxTrivia.Value.ToFullString();
                    if (className.EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                    {
                        if (!new Regex(REGEX_DOCCOMMENT_REQUEST).IsMatch(comment))
                            throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到有效的文档注释结构（形如：“表示 [FOO] /bar 接口的请求。”）。");
                    }
                    else if (className.EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                    {
                        if (!new Regex(REGEX_DOCCOMMENT_RESPONSE).IsMatch(comment))
                            throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点找不到有效的文档注释结构（形如：“表示 [FOO] /bar 接口的响应。”）。");
                    }
                }
            });
        }

        private void AnalyzeSourceCode_ModelDefinationFileTypePropertyInitialization(FileInfo file)
        {
            WrapExceptionalAction(() =>
            {
                using Stream fileStream = file.OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();
                if (!LanguageNames.CSharp.Equals(syntaxRoot.Language))
                    throw new NotSupportedException($"文件 '{file.FullName}' 无法以 C# 源码的形式加载。");

                ClassDeclarationSyntax[] classDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .ToArray();
                foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarationSyntaxes)
                {
                    string className = classDeclarationSyntax.Identifier.ToFullString().Trim();
                    if (className.EndsWith(MEMBER_NAME_SUFFIX_REQUEST_MODEL))
                    {
                        void DeepAnalyzeSyntaxNode(ClassDeclarationSyntax syntaxNode)
                        {
                            PropertyDeclarationSyntax[] propertyDeclarationSyntaxes = classDeclarationSyntax.ChildNodes()
                                .Where(n => n.IsKind(SyntaxKind.PropertyDeclaration))
                                .Select(n => (PropertyDeclarationSyntax)n)
                                .ToArray();
                            foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in propertyDeclarationSyntaxes)
                            {
                                if (propertyDeclarationSyntax.AttributeLists.Any(a => a.ToFullString().Contains("JsonIgnore")))
                                    continue;

                                if (propertyDeclarationSyntax.Type is PredefinedTypeSyntax predefinedTypeSyntax)
                                {
                                    string keyword = predefinedTypeSyntax.Keyword.ToFullString().Trim();
                                    bool isNullable = keyword.EndsWith("?");
                                    bool isInitialized = propertyDeclarationSyntax.Initializer != null;
                                    if (isNullable && isInitialized)
                                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点存在不合理的属性初始赋值，可空属性不应初始化。 ({propertyDeclarationSyntax})");
                                    if (isInitialized && propertyDeclarationSyntax.Initializer!.ToFullString().Contains("default!"))
                                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点存在不合理的属性初始赋值，请求模型的属性不应初始化为 'default!'。 ({propertyDeclarationSyntax})");
                                }
                            }

                            string identifier = syntaxNode.Identifier.ToFullString().Trim();
                            if (string.Equals(identifier, _targetSdkApiModelInnerNestedTypesWrapperIdentifier))
                            {
                                ClassDeclarationSyntax[] childClassDeclarationSyntaxes = syntaxNode.ChildNodes()
                                    .Where(n => n.IsKind(SyntaxKind.ClassDeclaration))
                                    .Select(n => (ClassDeclarationSyntax)n)
                                    .ToArray();
                                foreach (ClassDeclarationSyntax childSyntaxNode in childClassDeclarationSyntaxes)
                                {
                                    DeepAnalyzeSyntaxNode(childSyntaxNode);
                                }
                            }
                        }
                        DeepAnalyzeSyntaxNode(classDeclarationSyntax);
                    }
                    else if (className.EndsWith(MEMBER_NAME_SUFFIX_RESPONSE_MODEL))
                    {
                        void DeepAnalyzeSyntaxNode(ClassDeclarationSyntax syntaxNode)
                        {
                            PropertyDeclarationSyntax[] propertyDeclarationSyntaxes = classDeclarationSyntax.ChildNodes()
                                .Where(n => n.IsKind(SyntaxKind.PropertyDeclaration))
                                .Select(n => (PropertyDeclarationSyntax)n)
                                .ToArray();
                            foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in propertyDeclarationSyntaxes)
                            {
                                if (propertyDeclarationSyntax.AttributeLists.Any(a => a.ToFullString().Contains("JsonIgnore")))
                                    continue;

                                if (propertyDeclarationSyntax.Type is PredefinedTypeSyntax predefinedTypeSyntax)
                                {
                                    bool isInitialized = propertyDeclarationSyntax.Initializer != null;
                                    if (isInitialized && !propertyDeclarationSyntax.Initializer!.ToFullString().Contains("default!"))
                                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{className}' 节点存在不合理的属性初始赋值，响应模型的属性应初始化为 'default!'。 ({propertyDeclarationSyntax})");
                                }
                            }

                            string identifier = syntaxNode.Identifier.ToFullString().Trim();
                            if (string.Equals(identifier, _targetSdkApiModelInnerNestedTypesWrapperIdentifier))
                            {
                                ClassDeclarationSyntax[] childClassDeclarationSyntaxes = syntaxNode.ChildNodes()
                                    .Where(n => n.IsKind(SyntaxKind.ClassDeclaration))
                                    .Select(n => (ClassDeclarationSyntax)n)
                                    .ToArray();
                                foreach (ClassDeclarationSyntax childSyntaxNode in childClassDeclarationSyntaxes)
                                {
                                    DeepAnalyzeSyntaxNode(childSyntaxNode);
                                }
                            }
                        }

                        DeepAnalyzeSyntaxNode(classDeclarationSyntax);
                    }
                }
            });
        }

        private void AnalyzeSourceCode_EventDefinationFileDocumatationComment(FileInfo file)
        {
            WrapExceptionalAction(() =>
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                string typeName = $"{TargetAssemblyNamespaceForApiEvents}.{fileName}";
                Type? targetType = TargetAssembly.GetType(typeName, throwOnError: false);

                if (targetType == null)
                    throw new CodeAnalyzerException($"目标程序集中找不到类型 '{typeName}'。");

                if (targetType.IsAbstract || targetType.IsInterface)
                    return;

                if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(targetType), targetType, out Exception ex))
                    throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 JSON 序列化失败。", ex);
            });
        }

        private void AnalyzeSourceCode_ApiDefinationFileDocumatationComment(FileInfo file)
        {
            void AnalyzeApiRelatedRequest(string expectedApiName, string expectedApiVerb, string expectedApiRoute)
            {
                FileInfo[] modelDefinationFiles = new DirectoryInfo(_dirSourceCode)
                    .GetAllFiles($"{_dirSourceCodeForApiModels}/*{expectedApiName}{MEMBER_NAME_SUFFIX_REQUEST_MODEL}.cs")
                    .Where(f => string.Equals(f.Name, $"{expectedApiName}{MEMBER_NAME_SUFFIX_REQUEST_MODEL}.cs", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (modelDefinationFiles.Length == 0)
                    throw new CodeAnalyzerException($"找不到 API '{expectedApiName}' 对应的请求模型源代码文件。");
                else if (modelDefinationFiles.Length > 1)
                    throw new CodeAnalyzerException($"找到 API '{expectedApiName}' 多个冲突的对应请求模型源代码文件。");

                using Stream fileStream = modelDefinationFiles.Single().OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();

                ClassDeclarationSyntax[] classDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .Where(mc => mc.Identifier.ToFullString().Trim().Equals($"{expectedApiName}{MEMBER_NAME_SUFFIX_REQUEST_MODEL}"))
                    .ToArray();
                if (classDeclarationSyntaxes.Length == 0)
                    throw new CodeAnalyzerException($"找不到 API '{expectedApiName}' 对应的请求模型。");

                foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarationSyntaxes)
                {
                    string comment = classDeclarationSyntax.GetLeadingTrivia().Single(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)).ToFullString();
                    Match commentRegexMatch = new Regex(REGEX_DOCCOMMENT_REQUEST).Match(comment);
                    string actualApiVerb = commentRegexMatch.Groups[1].Value;
                    string actualApiRoute = commentRegexMatch.Groups[2].Value;

                    if (!string.Equals(expectedApiVerb, actualApiVerb))
                        throw new CodeAnalyzerException($"API '{expectedApiName}' 中注释的谓词与请求模型中注释的谓词不匹配。 (Expected: '{expectedApiVerb}', Actual: '{actualApiVerb}')");
                    if (!string.Equals(expectedApiRoute, actualApiRoute))
                        throw new CodeAnalyzerException($"API '{expectedApiName}' 中注释的路由与请求模型中注释的路由不匹配。 (Expected: '{expectedApiRoute}', Actual: '{actualApiRoute}')");
                }
            }

            void AnalyzeApiRelatedResponse(string expectedApiName, string expectedApiVerb, string expectedApiRoute)
            {
                FileInfo[] modelDefinationFiles = new DirectoryInfo(_dirSourceCode)
                    .GetAllFiles($"{_dirSourceCodeForApiModels}/*{expectedApiName}{MEMBER_NAME_SUFFIX_RESPONSE_MODEL}.cs")
                    .Where(f => string.Equals(f.Name, $"{expectedApiName}{MEMBER_NAME_SUFFIX_RESPONSE_MODEL}.cs", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (modelDefinationFiles.Length == 0)
                    throw new CodeAnalyzerException($"找不到 API '{expectedApiName}' 对应的响应模型源代码文件。");
                else if (modelDefinationFiles.Length > 1)
                    throw new CodeAnalyzerException($"找到 API '{expectedApiName}' 多个冲突的对应响应模型源代码文件。");

                using Stream fileStream = modelDefinationFiles.Single().OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();

                ClassDeclarationSyntax[] classDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .Where(mc => mc.Identifier.ToFullString().Trim().Equals($"{expectedApiName}{MEMBER_NAME_SUFFIX_RESPONSE_MODEL}"))
                    .ToArray();
                if (classDeclarationSyntaxes.Length == 0)
                    throw new CodeAnalyzerException($"找不到 API '{expectedApiName}' 对应的响应模型。");

                foreach (ClassDeclarationSyntax classDeclarationSyntax in classDeclarationSyntaxes)
                {
                    string comment = classDeclarationSyntax.GetLeadingTrivia().Single(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)).ToFullString();
                    Match commentRegexMatch = new Regex(REGEX_DOCCOMMENT_RESPONSE).Match(comment);
                    string actualApiVerb = commentRegexMatch.Groups[1].Value;
                    string actualApiRoute = commentRegexMatch.Groups[2].Value;

                    if (!string.Equals(expectedApiVerb, actualApiVerb))
                        throw new CodeAnalyzerException($"API '{expectedApiName}' 中注释的谓词与响应模型中注释的谓词不匹配。 (Expected: '{expectedApiVerb}', Actual: '{actualApiVerb}')");
                    if (!string.Equals(expectedApiRoute, actualApiRoute))
                        throw new CodeAnalyzerException($"API '{expectedApiName}' 中注释的路由与响应模型中注释的路由不匹配。 (Expected: '{expectedApiRoute}', Actual: '{actualApiRoute}')");
                }
            }

            void AnalyzeBlockSyntax(string expectedApiName, string expectedApiVerb, string expectedApiRoute, BlockSyntax blockSyntax)
            {
                bool identified = false;
                void DeepAnalyzeSyntaxNode(SyntaxNode syntaxNode)
                {
                    if (syntaxNode.IsKind(SyntaxKind.LocalDeclarationStatement))
                    {
                        VariableDeclarationSyntax[] variableDeclarationSyntaxes = syntaxNode.ChildNodes()
                            .Where(n => n.IsKind(SyntaxKind.VariableDeclaration))
                            .Select(n => (VariableDeclarationSyntax)n)
                            .ToArray();
                        foreach (VariableDeclarationSyntax variableDeclarationSyntax in variableDeclarationSyntaxes)
                        {
                            VariableDeclaratorSyntax variableDeclaratorSyntax = variableDeclarationSyntax.Variables
                                .Where(v => v.Initializer.ToFullString().Contains($".{_targetSdkApiMethodInnerFlurlRequestInitializerIdentifier}("))
                                .SingleOrDefault();
                            if (variableDeclaratorSyntax == null)
                                continue;

                            string variableBindingExpression = variableDeclaratorSyntax.ToFullString()
                                .Split(new string[] { $".{_targetSdkApiMethodInnerFlurlRequestInitializerIdentifier}(" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                .Split('\n')[0]
                                .Trim()
                                .TrimEnd(')', ';')
                                .Trim();

                            variableBindingExpression = variableBindingExpression.Substring(variableBindingExpression.Split(',')[0].Length + 1).Trim();

                            string actualApiVerb;
                            if (Regex.IsMatch(variableBindingExpression, "HttpMethod.[a-zA-Z]+"))
                                actualApiVerb = Regex.Match(variableBindingExpression, "HttpMethod.([a-zA-Z]+)").Groups[1].Value;
                            else if (Regex.IsMatch(variableBindingExpression, "new\\s*HttpMethod\\(\"[a-zA-Z]+\"\\)"))
                                actualApiVerb = Regex.Match(variableBindingExpression, "new\\s*HttpMethod\\(\"([a-zA-Z]+)\"\\)").Groups[1].Value;
                            else
                                actualApiVerb = "(unknown)";

                            if (!string.Equals(expectedApiVerb, actualApiVerb, StringComparison.OrdinalIgnoreCase))
                                throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码的谓词与注释的谓词不匹配。 (Expected: '{expectedApiVerb}', Actual: '{actualApiVerb}')");

                            variableBindingExpression = variableBindingExpression.Substring(variableBindingExpression.Split(',')[0].Length + 1).Trim();

                            string[] expectedRouteSegments = expectedApiRoute.Split('?')[0].Split('/').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                            string[] actualRouteSegments = variableBindingExpression.Split('?')[0].Replace("/", "\", \"").Split(',').Select(e => e.Trim()).Where(s => !string.IsNullOrEmpty(s.Replace("\"", ""))).ToArray();
                            if (expectedRouteSegments.Length != actualRouteSegments.Length)
                            {
                                throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码的路由段长度与注释的路由段长度不匹配。 (Expected: {expectedRouteSegments.Length} of '{string.Join(", ", expectedRouteSegments)}', Actual: {actualRouteSegments.Length} of '{string.Join(", ", actualRouteSegments)}')");
                            }
                            else
                            {
                                for (int i = 0; i < expectedRouteSegments.Length; i++)
                                {
                                    string s1 = expectedRouteSegments[i];
                                    string s2 = actualRouteSegments[i];
                                    if (s1.StartsWith("{") && s1.EndsWith("}"))
                                    {
                                        if (s2.StartsWith("\"") || s2.EndsWith("\""))
                                            throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码的路由段 {i + 1} 与注释的路由段 {i + 1} 不匹配。 (Expected: '{s1}', Actual: '{s2}')");
                                    }
                                    else
                                    {
                                        s2 = s2.Trim('\"').Trim();
                                        if (!string.Equals(s1, s2))
                                            throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码的路由段 {i + 1} 与注释的路由段 {i + 1} 不匹配。 (Expected: '{s1}', Actual: '{s2}')");
                                    }
                                }
                            }

                            identified = true;
                            return;
                        }
                    }

                    foreach (SyntaxNode childSyntaxNode in syntaxNode.ChildNodes())
                    {
                        DeepAnalyzeSyntaxNode(childSyntaxNode);
                    }
                }

                DeepAnalyzeSyntaxNode(blockSyntax);

                if (!identified)
                    throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码找不到有效的 FlurlRequest 构造结构（形式：“IFlurlRequest flurlRequest = client.CreateRequest(apiRequest);”）。");
            }

            WrapExceptionalAction(() =>
            {
                using Stream fileStream = file.OpenRead();
                SourceText sourceText = SourceText.From(fileStream);
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceText).WithFilePath(file.FullName);
                CompilationUnitSyntax syntaxRoot = syntaxTree.GetCompilationUnitRoot();
                if (!LanguageNames.CSharp.Equals(syntaxRoot.Language))
                    throw new NotSupportedException($"文件 '{file.FullName}' 无法以 C# 源码的形式加载。");

                MethodDeclarationSyntax[] methodDeclarationSyntaxes = syntaxRoot.Members
                    .Where(mn => mn.IsKind(SyntaxKind.NamespaceDeclaration))
                    .Select(mn => (NamespaceDeclarationSyntax)mn)
                    .SelectMany(mn =>
                        mn.Members
                            .Where(mc => mc.IsKind(SyntaxKind.ClassDeclaration))
                            .Select(mc => (ClassDeclarationSyntax)mc)
                            .ToArray()
                    )
                    .SelectMany(mc =>
                        mc.Members
                            .Where(mm => mm.IsKind(SyntaxKind.MethodDeclaration))
                            .Select(mm => (MethodDeclarationSyntax)mm)
                            .ToArray()
                    )
                    .ToArray();
                foreach (MethodDeclarationSyntax methodDeclarationSyntax in methodDeclarationSyntaxes)
                {
                    string methodName = methodDeclarationSyntax.Identifier.ToFullString().Trim();
                    if (!new Regex($"^{MEMBER_NAME_PREFIX_API_METHOD}([a-zA-Z0-9]+){MEMBER_NAME_SUFFIX_API_METHOD}$").IsMatch(methodName))
                        continue;

                    SyntaxTrivia? syntaxTrivia = methodDeclarationSyntax.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                    if (syntaxTrivia == null)
                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{methodName}' 节点找不到文档注释结构。");

                    string comment = syntaxTrivia.Value.ToFullString();
                    if (!new Regex(REGEX_DOCCOMMENT_API).IsMatch(comment))
                        throw new CodeAnalyzerException($"文件 '{file.FullName}' 下 '{methodName}' 节点找不到有效的文档注释结构（形如：“异步调用 [FOO] /bar 接口。”）。");

                    Match commentRegexMatch = new Regex(REGEX_DOCCOMMENT_API).Match(comment);
                    string expectedApiVerb = commentRegexMatch.Groups[1].Value;
                    string expectedApiRoute = commentRegexMatch.Groups[2].Value;
                    string expectedApiName = methodName.Substring(MEMBER_NAME_PREFIX_API_METHOD.Length, methodName.Length - MEMBER_NAME_PREFIX_API_METHOD.Length - MEMBER_NAME_SUFFIX_API_METHOD.Length);

                    WrapExceptionalAction(() =>
                    {
                        AnalyzeApiRelatedRequest(expectedApiName: expectedApiName, expectedApiVerb: expectedApiVerb, expectedApiRoute: expectedApiRoute);
                    });

                    WrapExceptionalAction(() =>
                    {
                        AnalyzeApiRelatedResponse(expectedApiName: expectedApiName, expectedApiVerb: expectedApiVerb, expectedApiRoute: expectedApiRoute);
                    });

                    WrapExceptionalAction(() =>
                    {
                        BlockSyntax[] blockSyntaxes = methodDeclarationSyntax.ChildNodes()
                            .Where(n => n.IsKind(SyntaxKind.Block))
                            .Select(n => (BlockSyntax)n)
                            .ToArray();
                        if (blockSyntaxes.Length > 1)
                            throw new CodeAnalyzerException($"API '{expectedApiName}' 中实现代码包含多个块级结构。");

                        AnalyzeBlockSyntax(expectedApiName: expectedApiName, expectedApiVerb: expectedApiVerb, expectedApiRoute: expectedApiRoute, blockSyntaxes.Single());
                    });
                }
            });
        }

        private void AnalyzeTestSample()
        {
            DirectoryInfo dir = new DirectoryInfo(_dirTestSample);
            Trace.Assert(dir.Exists, $"工作目录 \"{dir.FullName}\" 不存在。");

            FileInfo[] modelSampleFiles = dir.GetAllFiles($"{_dirTestSampleForApiModels}/*");
            FileInfo[] eventSampleFiles = dir.GetAllFiles($"{_dirTestSampleForApiEvents}/*");
            Trace.Assert(modelSampleFiles.Any() || _allowNotFoundModelSamples, $"工作目录下不存在 API 模型示例文件，请检查 \"{_dirTestSampleForApiModels}\" 目录是否为空。");
            Trace.Assert(eventSampleFiles.Any() || _allowNotFoundEventSamples, $"工作目录下不存在 API 事件示例文件，请检查 \"{_dirTestSampleForApiEvents}\" 目录是否为空。");

            /* 校验 API 模型示例文件 */
            Parallel.ForEach(modelSampleFiles, AnalyzeTestSample_ModelSampleFile);

            /* 校验 API 事件示例文件 */
            Parallel.ForEach(eventSampleFiles, AnalyzeTestSample_EventSampleFile);
        }

        private void AnalyzeTestSample_ModelSampleFile(FileInfo file)
        {
            WrapExceptionalAction(() =>
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name).Split('.').First();
                string typeName = $"{TargetAssemblyNamespaceForApiModels}.{fileName}";
                Type? targetType = TargetAssembly.GetType(typeName, throwOnError: false);

                if (targetType == null)
                    throw new CodeAnalyzerException($"目标程序集中找不到类型 '{typeName}'。");

                if (targetType.IsAbstract || targetType.IsInterface)
                    return;

                using Stream fileStream = file.OpenRead();
                using TextReader fileReader = new StreamReader(fileStream);
                string fileFormat = Path.GetExtension(file.Name).ToLower();
                string fileContent = fileReader.ReadToEnd();
                switch (fileFormat)
                {
                    case ".json":
                        {
                            if (!Helpers.JsonHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 JSON 反序列化失败：{ex}。");
                        }
                        break;

                    case ".xml":
                        {
                            if (!Helpers.XmlHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 XML 反序列化失败：{ex}。");
                        }
                        break;
                }
            });
        }

        private void AnalyzeTestSample_EventSampleFile(FileInfo file)
        {
            WrapExceptionalAction(() =>
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name).Split('.').First();
                string typeName = $"{TargetAssemblyNamespaceForApiEvents}.{fileName}";
                Type? targetType = TargetAssembly.GetType(typeName, throwOnError: false);

                if (targetType == null)
                    throw new CodeAnalyzerException($"目标程序集中找不到类型 '{typeName}'。");

                if (targetType.IsAbstract || targetType.IsInterface)
                    return;

                using Stream fileStream = file.OpenRead();
                using TextReader fileReader = new StreamReader(fileStream);
                string fileFormat = Path.GetExtension(file.Name).ToLower();
                string fileContent = fileReader.ReadToEnd();
                switch (fileFormat)
                {
                    case ".json":
                        {
                            if (!Helpers.JsonHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 JSON 反序列化失败：{ex}。");
                        }
                        break;

                    case ".xml":
                        {
                            if (!Helpers.XmlHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                throw new CodeAnalyzerException($"类型 '{targetType}' 尝试 XML 反序列化失败：{ex}。");
                        }
                        break;
                }
            });
        }

        private void WrapExceptionalAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (CodeAnalyzerException error)
            {
                Errors.Add(error.ToFullString());
            }
            catch (Exception error)
            {
                Errors.Add(new CodeAnalyzerException("遇到未处理的异常。", error).ToFullString());
            }
        }
    }
}
