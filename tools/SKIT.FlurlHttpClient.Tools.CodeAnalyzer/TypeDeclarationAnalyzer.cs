using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers.Internal;

    /// <summary>
    /// 类型声明质量分析器。该分析器用于分析 SDK 中的类型声明问题。
    /// </summary>
    public sealed class TypeDeclarationAnalyzer : IAnalyzer
    {
        private bool _disposed;

        private readonly Assembly _sdkAssembly;
        private readonly string _sdkRequestModelDeclarationNamespace;
        private readonly string _sdkResponseModelDeclarationNamespace;
        private readonly string _sdkExecutingExtensionDeclarationNamespace;
        private readonly string? _sdkExecutingExtensionDeclarationClassNameRegex;
        private readonly string _sdkWebhookEventDeclarationNamespace;
        private readonly Func<Type, bool>? _ignoreRequestModelTypes;
        private readonly Func<Type, bool>? _ignoreResponseModelTypes;
        private readonly Func<Type, bool>? _ignoreExecutingExtensionTypes;
        private readonly Func<MethodInfo, bool>? _ignoreExecutingExtensionMethods;
        private readonly Func<Type, bool>? _ignoreWebhookEventTypes;
        private readonly bool _throwOnNotFoundRequestModelTypes;
        private readonly bool _throwOnNotFoundResponseModelTypes;
        private readonly bool _throwOnNotFoundExecutingExtensionTypes;
        private readonly bool _throwOnNotFoundWebhookEventTypes;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly List<TypeDeclarationAnalyzerRule> _customRules;
        private Func<Assembly, IEnumerable<Type>>? _customRequestModelTypesScanner;
        private Func<Assembly, IEnumerable<Type>>? _customResponseModelTypesScanner;
        private Func<Assembly, IEnumerable<Type>>? _customExecutingExtensionTypesScanner;
        private Func<Assembly, IEnumerable<MethodInfo>>? _customExecutingExtensionMethodsScanner;
        private Func<Assembly, IEnumerable<Type>>? _customWebhookEventTypesScanner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TypeDeclarationAnalyzer(TypeDeclarationAnalyzerOptions options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            _sdkAssembly = options.SdkAssembly;
            _sdkRequestModelDeclarationNamespace = options.SdkRequestModelDeclarationNamespace;
            _sdkResponseModelDeclarationNamespace = options.SdkResponseModelDeclarationNamespace;
            _sdkExecutingExtensionDeclarationNamespace = options.SdkExecutingExtensionDeclarationNamespace;
            _sdkExecutingExtensionDeclarationClassNameRegex = options.SdkExecutingExtensionDeclarationClassNameRegex;
            _sdkWebhookEventDeclarationNamespace = options.SdkWebhookEventDeclarationNamespace;
            _ignoreRequestModelTypes = options.IgnoreRequestModelTypes;
            _ignoreResponseModelTypes = options.IgnoreResponseModelTypes;
            _ignoreExecutingExtensionTypes = options.IgnoreExecutingExtensionTypes;
            _ignoreExecutingExtensionMethods = options.IgnoreExecutingExtensionMethods;
            _ignoreWebhookEventTypes = options.IgnoreWebhookEventTypes;
            _throwOnNotFoundRequestModelTypes = options.ThrowOnNotFoundRequestModelTypes;
            _throwOnNotFoundResponseModelTypes = options.ThrowOnNotFoundResponseModelTypes;
            _throwOnNotFoundExecutingExtensionTypes = options.ThrowOnNotFoundExecutingExtensionTypes;
            _throwOnNotFoundWebhookEventTypes = options.ThrowOnNotFoundWebhookEventTypes;

            _customRules = new List<TypeDeclarationAnalyzerRule>();
            UseRule_ReqAndRespModelClassNameCase();
            UseRule_ReqAndRespModelClassInPairing();
            UseRule_ReqAndRespModelClassHasValidCollectionProperty();
            UseRule_ReqAndRespModelClassNoDuplicateJsonPropertyName();
            UseRule_ReqAndRespModelClassCanSerialized();
            UseRule_ReqAndRespModelClassHasRelatedMethod();
            UseRule_ReqAndRespExtensionMethodReturnATask();
            UseRule_ExecutingExtensionMethodNameCase();
            UseRule_ExecutingExtensionMethodHasRelatedModel();
            UseRule_ExecutingExtensionMethodHasValidParameters();
            UseRule_WebhookEventClassNoDuplicateJsonPropertyName();
            UseRule_WebhookEventClassCanSerialized();
        }

        /// <summary>
        /// 添加自定义规则。
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public TypeDeclarationAnalyzer AddRule(TypeDeclarationAnalyzerRule rule)
        {
            if (rule is null) throw new ArgumentNullException(nameof(rule));
            if (_disposed) throw new ObjectDisposedException(nameof(TypeDeclarationAnalyzer));

            _customRules.Add(rule);

            return this;
        }

        /// <summary>
        /// 设置特定成员类型的扫描方式，在断言时将替换默认的扫描方式。
        /// </summary>
        /// <param name="memberKind"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public TypeDeclarationAnalyzer SetMemberScanner(TypeDeclarationMemberKinds memberKind, Func<Assembly, IEnumerable<MemberInfo>> scanner)
        {
            if (scanner is null) throw new ArgumentNullException(nameof(scanner));
            if (_disposed) throw new ObjectDisposedException(nameof(TypeDeclarationAnalyzer));

            switch (memberKind)
            {
                case TypeDeclarationMemberKinds.RequestModelClass:
                    _customRequestModelTypesScanner = (_) => scanner.Invoke(_sdkAssembly).OfType<Type>();
                    break;

                case TypeDeclarationMemberKinds.ResponseModelClass:
                    _customResponseModelTypesScanner = (_) => scanner.Invoke(_sdkAssembly).OfType<Type>();
                    break;

                case TypeDeclarationMemberKinds.ExecutingExtensionClass:
                    _customExecutingExtensionTypesScanner = (_) => scanner.Invoke(_sdkAssembly).OfType<Type>();
                    break;

                case TypeDeclarationMemberKinds.ExecutingExtensionMethod:
                    _customExecutingExtensionMethodsScanner = (_) => scanner.Invoke(_sdkAssembly).OfType<MethodInfo>();
                    break;

                case TypeDeclarationMemberKinds.WebhookEventClass:
                    _customWebhookEventTypesScanner = (_) => scanner.Invoke(_sdkAssembly).OfType<Type>();
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

        private TypeDeclarationAnalyzerOptions ExtractAnalyzerOptions()
        {
            return new TypeDeclarationAnalyzerOptions()
            {
                SdkAssembly = _sdkAssembly,
                SdkRequestModelDeclarationNamespace = _sdkRequestModelDeclarationNamespace,
                SdkResponseModelDeclarationNamespace = _sdkResponseModelDeclarationNamespace,
                SdkExecutingExtensionDeclarationNamespace = _sdkExecutingExtensionDeclarationNamespace,
                SdkExecutingExtensionDeclarationClassNameRegex = _sdkExecutingExtensionDeclarationClassNameRegex,
                SdkWebhookEventDeclarationNamespace = _sdkWebhookEventDeclarationNamespace,
                IgnoreRequestModelTypes = _ignoreRequestModelTypes,
                IgnoreResponseModelTypes = _ignoreResponseModelTypes,
                IgnoreExecutingExtensionTypes = _ignoreExecutingExtensionTypes,
                IgnoreExecutingExtensionMethods = _ignoreExecutingExtensionMethods,
                IgnoreWebhookEventTypes = _ignoreWebhookEventTypes,
                ThrowOnNotFoundRequestModelTypes = _throwOnNotFoundRequestModelTypes,
                ThrowOnNotFoundResponseModelTypes = _throwOnNotFoundResponseModelTypes,
                ThrowOnNotFoundExecutingExtensionTypes = _throwOnNotFoundExecutingExtensionTypes,
                ThrowOnNotFoundWebhookEventTypes = _throwOnNotFoundWebhookEventTypes
            };
        }

        private IEnumerable<TypeDeclarationAnalyzerRuleUnit> ScanAnalyzerRuleUnits()
        {
            Type[] scannedRequestModelTypes = _customRequestModelTypesScanner is not null
                ? _customRequestModelTypesScanner.Invoke(_sdkAssembly).ToArray()
                : _sdkAssembly.GetExportedTypes()
                    .Where(t =>
                        t.Namespace is not null &&
                        t.Namespace.StartsWith(_sdkRequestModelDeclarationNamespace) &&
                        t.IsNonAbstractClass() &&
                        t.IsNonNestedClass() &&
                        typeof(ICommonRequest).IsAssignableFrom(t)
                    )
                    .Where(t => _ignoreRequestModelTypes is null || !_ignoreRequestModelTypes.Invoke(t))
                    .ToArray();
            if (!scannedRequestModelTypes.Any() && _throwOnNotFoundRequestModelTypes)
                throw new AnalysisException($"未在程序集 {_sdkAssembly} 下扫描到表示 API 请求模型的类型声明，请检查 \"{_sdkResponseModelDeclarationNamespace}\" 命名空间是否为空。");

            Type[] scannedResponseModelTypes = _customResponseModelTypesScanner is not null
                ? _customResponseModelTypesScanner.Invoke(_sdkAssembly).ToArray()
                : _sdkAssembly.GetExportedTypes()
                    .Where(t =>
                        t.Namespace is not null &&
                        t.Namespace.StartsWith(_sdkResponseModelDeclarationNamespace) &&
                        t.IsNonAbstractClass() &&
                        t.IsNonNestedClass() &&
                        typeof(ICommonResponse).IsAssignableFrom(t)
                    )
                    .Where(t => _ignoreResponseModelTypes is null || !_ignoreResponseModelTypes.Invoke(t))
                    .ToArray();
            if (!scannedResponseModelTypes.Any() && _throwOnNotFoundResponseModelTypes)
                throw new AnalysisException($"未在程序集 {_sdkAssembly} 下扫描到表示 API 响应模型的类型声明，请检查 \"{_sdkResponseModelDeclarationNamespace}\" 命名空间是否为空。");

            Type[] scannedExecutingExtensionTypes = _customExecutingExtensionTypesScanner is not null
                ? _customExecutingExtensionTypesScanner.Invoke(_sdkAssembly).ToArray()
                : _sdkAssembly.GetExportedTypes()
                    .Where(t =>
                        t.Namespace is not null &&
                        t.Namespace.StartsWith(_sdkExecutingExtensionDeclarationNamespace) &&
                        t.IsStaticClass() &&
                        new Regex(_sdkExecutingExtensionDeclarationClassNameRegex ?? AnalyzerDefaults.DEFAULT_EXECUTING_EXTENSION_NAME_REGEX).IsMatch(t.Name)
                    )
                    .Where(t => _ignoreExecutingExtensionTypes is null || !_ignoreExecutingExtensionTypes.Invoke(t))
                    .ToArray();
            MethodInfo[] scannedExecutingExtensionMethods = _customExecutingExtensionMethodsScanner is not null
                ? _customExecutingExtensionMethodsScanner.Invoke(_sdkAssembly).ToArray()
                : scannedExecutingExtensionTypes.SelectMany(t => t.GetMethods())
                    .Where(m =>
                        m.IsPublic &&
                        m.IsStatic &&
                        m.Name.StartsWith(AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX) &&
                        m.Name.EndsWith(AnalyzerDefaults.NAMING_EXECUTING_METHOD_SUFFIX) &&
                        m.IsDefined(typeof(ExtensionAttribute), false) &&
                        m.GetParameters().Any() &&
                        typeof(ICommonClient).IsAssignableFrom(m.GetParameters().First().ParameterType)
                    )
                    .Where(m => _ignoreExecutingExtensionMethods is null || !_ignoreExecutingExtensionMethods.Invoke(m))
                    .ToArray();
            if ((!scannedExecutingExtensionTypes.Any() || !scannedExecutingExtensionMethods.Any()) && _throwOnNotFoundExecutingExtensionTypes)
                throw new AnalysisException($"未在程序集 {_sdkAssembly} 下扫描到表示 API 接口方法的类型声明，请检查 \"{_sdkExecutingExtensionDeclarationNamespace}\" 命名空间是否为空。");

            Type[] scannedWebhookEventTypes = _customWebhookEventTypesScanner is not null
                ? _customWebhookEventTypesScanner.Invoke(_sdkAssembly).ToArray()
                : _sdkAssembly.GetExportedTypes()
                    .Where(t =>
                        t.Namespace is not null &&
                        t.Namespace.StartsWith(_sdkWebhookEventDeclarationNamespace) &&
                        t.IsNonAbstractClass() &&
                        t.IsNonNestedClass() &&
                        typeof(ICommonWebhookEvent).IsAssignableFrom(t)
                    )
                    .Where(t => _ignoreWebhookEventTypes is null || !_ignoreWebhookEventTypes.Invoke(t))
                    .ToArray();
            if (!scannedWebhookEventTypes.Any() && _throwOnNotFoundWebhookEventTypes)
                throw new AnalysisException($"未在程序集 {_sdkAssembly} 下扫描到表示 API 回调通知事件模型的类型声明，请检查 \"{_sdkWebhookEventDeclarationNamespace}\" 命名空间是否为空。");

            return Array.Empty<TypeDeclarationAnalyzerRuleUnit>()
                .Concat(scannedRequestModelTypes.Select(t => new TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds.RequestModelClass, t)))
                .Concat(scannedResponseModelTypes.Select(t => new TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds.ResponseModelClass, t)))
                .Concat(scannedExecutingExtensionTypes.Select(t => new TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds.ExecutingExtensionClass, t)))
                .Concat(scannedExecutingExtensionMethods.Select(t => new TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds.ExecutingExtensionMethod, t)))
                .Concat(scannedWebhookEventTypes.Select(t => new TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds.WebhookEventClass, t)))
                .ToImmutableArray();
        }

        private void UseRule_ReqAndRespModelClassNameCase()
        {
            /**
             * 目标：
             *   API 请求或响应模型类。
             * 
             * 规则：
             *   请求模型类名必须以 "Request" 结尾；
             *   响应模型类名必须以 "Response" 结尾。
             */

            AddRule((_, _, cur) =>
            {
                switch (cur.MemberKind)
                {
                    case TypeDeclarationMemberKinds.RequestModelClass:
                        {
                            if (!cur.MemberAsType.GetNameWithoutGenerics().EndsWith(AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX))
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个请求模型，但类名未以 \"{AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX}\" 结尾。");
                        }
                        break;

                    case TypeDeclarationMemberKinds.ResponseModelClass:
                        {
                            if (!cur.MemberAsType.GetNameWithoutGenerics().EndsWith(AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX))
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个响应模型，但类名未以 \"{AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX}\" 结尾。");
                        }
                        break;
                }
            });
        }

        private void UseRule_ReqAndRespModelClassInPairing()
        {
            /**
             * 目标：
             *   API 请求或响应模型类类。
             * 
             * 规则：
             *   请求/响应模型类必须有成对定义的响应/请求模型类。
             */

            AddRule((_, agg, cur) =>
            {
                switch (cur.MemberKind)
                {
                    case TypeDeclarationMemberKinds.RequestModelClass:
                        {
                            string reqTypeName = cur.MemberAsType.GetNameWithoutGenerics();
                            string respTypeName = $"{reqTypeName.Substring(0, reqTypeName.Length - AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX.Length)}{AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX}";
                            if (!agg.Any(e => e.MemberKind == TypeDeclarationMemberKinds.ResponseModelClass && e.MemberAsType.GetNameWithoutGenerics() == respTypeName))
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个请求模型，但在程序集中找不到对应的响应模型类型 \"{respTypeName}\"。");
                        }
                        break;

                    case TypeDeclarationMemberKinds.ResponseModelClass:
                        {
                            string respTypeName = cur.MemberAsType.GetNameWithoutGenerics();
                            string reqTypeName = $"{respTypeName.Substring(0, respTypeName.Length - AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX.Length)}{AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX}";
                            if (!agg.Any(e => e.MemberKind == TypeDeclarationMemberKinds.RequestModelClass && e.MemberAsType.GetNameWithoutGenerics() == reqTypeName))
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个响应模型，但在程序集中找不到对应的请求模型类型 \"{reqTypeName}\"。");
                        }
                        break;
                }
            });
        }

        private void UseRule_ReqAndRespModelClassHasValidCollectionProperty()
        {
            /**
             * 目标：
             *   API 请求或响应模型类。
             * 
             * 规则：
             *   请求模型中的可序列化字段，如果是集合类型，则必须定义为列表（<see cref="byte[]"/> 除外）；
             *   响应模型中的可序列化字段，如果是集合类型，则必须定义为数组（<see cref="byte[]"/> 除外）。
             */

            const string NESTED_CLASS = "Types";

            static void DeepExecuteRule(Type rootType, Type currentType)
            {
                PropertyInfo[] properties = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>() is null &&
                        property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>() is null &&
                        property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true) is null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true) is null)
                        continue;

                    if (rootType.GetNameWithoutGenerics().EndsWith(AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX))
                    {
                        if (property.PropertyType.IsArray && property.PropertyType.GetElementType() != typeof(byte))
                            throw new AnalysisException($"类型 \"{rootType.Name}\" 是一个请求模型，但定义了数组结构的属性 \"{property.Name}\"。");
                    }
                    else if (rootType.GetNameWithoutGenerics().EndsWith(AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX))
                    {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                            throw new AnalysisException($"类型 \"{rootType.Name}\" 是一个响应模型，但定义了列表结构的属性 \"{property.Name}\"。");
                    }
                }

                Type[] nestedTypes = currentType.GetNestedType(NESTED_CLASS)?.GetNestedTypes() ?? Array.Empty<Type>();
                foreach (Type nestedType in nestedTypes)
                {
                    DeepExecuteRule(rootType, nestedType);
                }
            }

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.RequestModelClass && cur.MemberKind != TypeDeclarationMemberKinds.ResponseModelClass)
                    return;

                DeepExecuteRule(cur.MemberAsType, cur.MemberAsType);
            });
        }

        private void UseRule_ReqAndRespModelClassNoDuplicateJsonPropertyName()
        {
            /**
             * 目标：
             *   API 请求或响应模型类。
             * 
             * 规则：
             *   模型中的可序列化字段，声明的 <see cref="Newtonsoft.Json.JsonPropertyAttribute"/> 和 <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/> 名称必须相同。
             */

            const string NESTED_CLASS = "Types";

            static void DeepExecuteRule(Type rootType, Type currentType)
            {
                PropertyInfo[] properties = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>() is null &&
                        property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>() is null &&
                        property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true) is null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true) is null)
                        continue;

                    Newtonsoft.Json.JsonPropertyAttribute attr1 = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true)!;
                    System.Text.Json.Serialization.JsonPropertyNameAttribute attr2 = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true)!;
                    if (attr1.PropertyName != attr2.Name)
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了两个不同的 JSON 别名：\"{attr1.PropertyName}\" 和 \"{attr2.Name}\"。");
                    if (string.IsNullOrEmpty(attr1.PropertyName) || attr1.PropertyName != attr1.PropertyName.Trim())
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了无效的 JSON 别名：\"{attr1.PropertyName}\"。");
                    if (string.IsNullOrEmpty(attr2.Name) || attr2.Name != attr2.Name.Trim())
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了无效的 JSON 别名：\"{attr2.Name}\"。");
                }

                Type[] nestedTypes = currentType.GetNestedType(NESTED_CLASS)?.GetNestedTypes() ?? Array.Empty<Type>();
                foreach (Type nestedType in nestedTypes)
                {
                    DeepExecuteRule(rootType, nestedType);
                }
            }

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.RequestModelClass && cur.MemberKind != TypeDeclarationMemberKinds.ResponseModelClass)
                    return;

                DeepExecuteRule(cur.MemberAsType, cur.MemberAsType);
            });
        }

        private void UseRule_ReqAndRespModelClassCanSerialized()
        {
            /**
             * 目标：
             *   API 请求或响应模型类。
             * 
             * 规则：
             *   模型可被 JSON 序列化。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.RequestModelClass && cur.MemberKind != TypeDeclarationMemberKinds.ResponseModelClass)
                    return;

                if (cur.MemberAsType.IsAbstract || cur.MemberAsType.IsGenericType)
                    return;

                if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(cur.MemberAsType)!, cur.MemberAsType, out Exception ex))
                    throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 尝试 JSON 序列化遇到异常。", ex);
            });
        }

        private void UseRule_ReqAndRespModelClassHasRelatedMethod()
        {
            /**
             * 目标：
             *   API 请求或响应模型类。
             * 
             * 规则：
             *   API 请求/响应模型对应的接口方法所必须已被定义。
             */

            AddRule((_, agg, cur) =>
            {
                switch (cur.MemberKind)
                {
                    case TypeDeclarationMemberKinds.RequestModelClass:
                        {
                            bool hasRelatedMethod = agg.Any(e =>
                            {
                                if (e.MemberKind == TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                                {
                                    ParameterInfo[] parameters = e.MemberAsMethod.GetParameters();
                                    if (parameters.Length <= 2)
                                        return false;

                                    Type reqType = parameters[1].ParameterType;
                                    return reqType.GetNameWithoutGenerics() == cur.MemberAsType.GetNameWithoutGenerics();
                                }

                                return false;
                            });
                            if (!hasRelatedMethod)
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个请求模型，但对应的接口方法函数未定义。");
                        }
                        break;

                    case TypeDeclarationMemberKinds.ResponseModelClass:
                        {
                            bool hasRelatedMethod = agg.Any(e =>
                            {
                                if (e.MemberKind == TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                                {
                                    ParameterInfo[] parameters = e.MemberAsMethod.GetParameters();
                                    if (parameters.Length <= 2)
                                        return false;

                                    Type returnType = e.MemberAsMethod.ReturnType;
                                    if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                                        return false;

                                    Type respType = returnType.GetGenericArguments().Single();
                                    return respType.GetNameWithoutGenerics() == cur.MemberAsType.GetNameWithoutGenerics();
                                }

                                return false;
                            });
                            if (!hasRelatedMethod)
                                throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 是一个响应模型，但对应的接口方法函数未定义。");
                        }
                        break;
                }
            });
        }

        private void UseRule_ReqAndRespExtensionMethodReturnATask()
        {
            /**
             * 目标：
             *   API 接口方法函数。
             * 
             * 规则：
             *   API 接口方法的返回值必须是一个 <see cref="Task&lt;ICommonResponse&gt;>"/> 对象。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                    return;

                Type returnType = cur.MemberAsMethod.ReturnType;
                if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但返回值类型不是 \"Task<>\"。");

                Type respType = returnType.GetGenericArguments().Single();
                if (!typeof(ICommonResponse).IsAssignableFrom(respType))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但返回值类型不是 \"Task<T extends ICommonResponse>\"。");
            });
        }

        private void UseRule_ExecutingExtensionMethodNameCase()
        {
            /**
             * 目标：
             *   API 接口方法函数。
             * 
             * 规则：
             *   API 接口方法的函数名必须与对应的请求/响应模型类名匹配。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                    return;

                ParameterInfo[] parameters = cur.MemberAsMethod.GetParameters();
                if (parameters.Length <= 2)
                    return;

                Type returnType = cur.MemberAsMethod.ReturnType;
                if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                    return;

                Type reqType = parameters[1].ParameterType;
                Type respType = returnType.GetGenericArguments().Single();
                string reqTypeName = reqType.GetNameWithoutGenerics();
                string reqTypeNameWithoutAffix = reqTypeName.Remove(reqType.Name.Length - AnalyzerDefaults.NAMING_REQUEST_MODEL_SUFFIX.Length);
                string respTypeName = respType.GetNameWithoutGenerics();
                string respTypeNameWithoutAffix = respTypeName.Remove(respTypeName.Length - AnalyzerDefaults.NAMING_RESPONSE_MODEL_SUFFIX.Length);
                string methodName = cur.MemberAsMethod.Name;
                string methodNameWithoutAffix = methodName.Substring(AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX.Length, methodName.Length - AnalyzerDefaults.NAMING_EXECUTING_METHOD_PREFIX.Length - AnalyzerDefaults.NAMING_EXECUTING_METHOD_SUFFIX.Length);

                if (methodNameWithoutAffix != reqTypeNameWithoutAffix)
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 应是一个 API 接口方法，但函数名与传入的请求模型类名不匹配。 (Expected: \"{methodNameWithoutAffix}\", Actual: \"{reqTypeNameWithoutAffix}\")");
                if (methodNameWithoutAffix != respTypeNameWithoutAffix)
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 应是一个 API 接口方法，但函数名与返回的响应模型类名不匹配。 (Expected: \"{methodNameWithoutAffix}\", Actual: \"{respTypeNameWithoutAffix}\")");
            });
        }

        private void UseRule_ExecutingExtensionMethodHasRelatedModel()
        {
            /**
             * 目标：
             *   API 接口方法函数。
             * 
             * 规则：
             *   API 接口方法所对应的请求/响应模型必须已被定义。
             */

            AddRule((_, agg, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                    return;

                ParameterInfo[] parameters = cur.MemberAsMethod.GetParameters();
                if (parameters.Length <= 2)
                    return;

                Type returnType = cur.MemberAsMethod.ReturnType;
                if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                    return;

                Type reqType = parameters[1].ParameterType;
                Type respType = returnType.GetGenericArguments().Single();
                if (!agg.Any(e => e.MemberKind == TypeDeclarationMemberKinds.RequestModelClass && e.MemberAsType.GetNameWithoutGenerics() == reqType.GetNameWithoutGenerics()))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但对应的请求模型类型未定义。");
                if (!agg.Any(e => e.MemberKind == TypeDeclarationMemberKinds.ResponseModelClass && e.MemberAsType.GetNameWithoutGenerics() == respType.GetNameWithoutGenerics()))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但对应的响应模型类型未定义。");
            });
        }

        private void UseRule_ExecutingExtensionMethodHasValidParameters()
        {
            /**
             * 目标：
             *   API 接口方法函数。
             * 
             * 规则：
             *   API 接口方法的参数个数必须是 4 个，且顺序依次为 "client"、"request"、"cancellationToken"，且 "cancellationToken" 具有默认值。
             */

            AddRule((_, agg, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.ExecutingExtensionMethod)
                    return;

                ParameterInfo[] parameters = cur.MemberAsMethod.GetParameters();
                if (parameters.Length != 3)
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但参数个数错误。");

                if (!typeof(ICommonClient).IsAssignableFrom(parameters[0].ParameterType))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但第一个参数类型不是 \"{nameof(ICommonClient)}\"。");
                if (!typeof(ICommonRequest).IsAssignableFrom(parameters[1].ParameterType))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但第二个参数类型不是 \"{nameof(ICommonRequest)}\"。");
                if (!typeof(CancellationToken).IsAssignableFrom(parameters[2].ParameterType))
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但第三个参数类型不是 \"{nameof(CancellationToken)}\"。");
                if (!parameters[2].HasDefaultValue)
                    throw new AnalysisException($"函数 \"{cur.MemberAsMethod.Name}\" 是一个 API 接口方法，但第三个参数未设置默认值。");
            });
        }

        private void UseRule_WebhookEventClassNoDuplicateJsonPropertyName()
        {
            /**
             * 目标：
             *   回调通知事件模型类。
             * 
             * 规则：
             *   模型中的可序列化字段，声明的 <see cref="Newtonsoft.Json.JsonPropertyAttribute"/> 和 <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/> 名称必须相同。
             */

            const string NESTED_CLASS = "Types";

            static void DeepExecuteRule(Type rootType, Type currentType)
            {
                PropertyInfo[] properties = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>() is null &&
                        property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>() is null &&
                        property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>(inherit: true) is not null)
                        continue;
                    if (property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true) is null)
                        continue;
                    if (property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true) is null)
                        continue;

                    Newtonsoft.Json.JsonPropertyAttribute attr1 = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true)!;
                    System.Text.Json.Serialization.JsonPropertyNameAttribute attr2 = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true)!;
                    if (attr1.PropertyName != attr2.Name)
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了两个不同的 JSON 别名：\"{attr1.PropertyName}\" 和 \"{attr2.Name}\"。");
                    if (string.IsNullOrEmpty(attr1.PropertyName) || attr1.PropertyName != attr1.PropertyName.Trim())
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了无效的 JSON 别名：\"{attr1.PropertyName}\"。");
                    if (string.IsNullOrEmpty(attr2.Name) || attr2.Name != attr2.Name.Trim())
                        throw new AnalysisException($"类型 \"{rootType.Name}\" 中的同一属性 \"{property.Name}\" 描述了无效的 JSON 别名：\"{attr2.Name}\"。");
                }

                Type[] nestedTypes = currentType.GetNestedType(NESTED_CLASS)?.GetNestedTypes() ?? Array.Empty<Type>();
                foreach (Type nestedType in nestedTypes)
                {
                    DeepExecuteRule(rootType, nestedType);
                }
            }

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.WebhookEventClass)
                    return;

                DeepExecuteRule(cur.MemberAsType, cur.MemberAsType);
            });
        }

        private void UseRule_WebhookEventClassCanSerialized()
        {
            /**
             * 目标：
             *   回调通知事件模型类。
             * 
             * 规则：
             *   模型可被 JSON 序列化。
             */

            AddRule((_, _, cur) =>
            {
                if (cur.MemberKind != TypeDeclarationMemberKinds.WebhookEventClass)
                    return;

                if (cur.MemberAsType.IsAbstract || cur.MemberAsType.IsGenericType)
                    return;

                if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(cur.MemberAsType)!, cur.MemberAsType, out Exception ex))
                    throw new AnalysisException($"类型 \"{cur.MemberAsType.Name}\" 尝试 JSON 序列化遇到异常。", ex);
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
                    _customRequestModelTypesScanner = null;
                    _customResponseModelTypesScanner = null;
                    _customExecutingExtensionTypesScanner = null;
                    _customExecutingExtensionMethodsScanner = null;
                    _customWebhookEventTypesScanner = null;

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
}
