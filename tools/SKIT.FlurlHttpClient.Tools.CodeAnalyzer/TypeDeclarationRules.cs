using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    internal static class TypeDeclarationRules
    {
        public const string RULE_ID_PREFIX = "type-declaration";

        public static IReadOnlyList<(string Id, TypeDeclarationRule Rule)> Defaults { get; } = new (string, TypeDeclarationRule)[]
        {
            UseModelNameCaseRule(),
            UseModelPairMatchRule(),
            UseModelJsonPropertyNameMatchRule(),
            UseModelJsonSerializableRule(),
            UseMethodReturnTypeRule(),
            UseMethodNameCaseRule(),
            UseMethodModelsMatchRule(),
            UseMethodParametersMatchRule(),
            UseWebhookJsonPropertyNameMatchRule(),
            UseWebhookJsonSerializableRule(),
            UseNoUnusedModelsRule(),
        };

        private static (string, TypeDeclarationRule) UseModelNameCaseRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-name-case";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModels;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    switch (unit.Kind)
                    {
                        case TypeDeclarationKind.RequestModel:
                            if (!unit.GetMemberAsType().GetNameWithoutGenerics().EndsWith(Consts.REQUEST_NAMING_SUFFIX))
                                yield return new TypeDeclarationAnalysisIssue(RuleId, $"请求模型类型名必须以 \"{Consts.REQUEST_NAMING_SUFFIX}\" 结尾。", unit);
                            break;

                        case TypeDeclarationKind.ResponseModel:
                            if (!unit.GetMemberAsType().GetNameWithoutGenerics().EndsWith(Consts.RESPONSE_NAMING_SUFFIX))
                                yield return new TypeDeclarationAnalysisIssue(RuleId, $"响应模型类型名必须以 \"{Consts.RESPONSE_NAMING_SUFFIX}\" 结尾。", unit);
                            break;
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseModelPairMatchRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-pair-match";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModels;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    switch (unit.Kind)
                    {
                        case TypeDeclarationKind.RequestModel:
                            {
                                string selfSuffix = Consts.REQUEST_NAMING_SUFFIX;
                                string selfName = unit.GetMemberAsType().GetNameWithoutGenerics();
                                if (!selfName.EndsWith(selfSuffix))
                                    yield break;

                                string counterpartSuffix = Consts.RESPONSE_NAMING_SUFFIX;
                                string counterpartName = selfName.Substring(0, selfName.Length - selfSuffix.Length) + counterpartSuffix;

                                if (!units.Any(c =>
                                    c.Kind == TypeDeclarationKind.ResponseModel &&
                                    c.GetMemberAsType().Namespace == unit.GetMemberAsType().Namespace &&
                                    c.GetMemberAsType().GetNameWithoutGenerics() == counterpartName))
                                {
                                    yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到请求模型类型对应的响应模型类型。", unit);
                                }
                            }
                            break;

                        case TypeDeclarationKind.ResponseModel:
                            {
                                string selfSuffix = Consts.RESPONSE_NAMING_SUFFIX;
                                string selfName = unit.GetMemberAsType().GetNameWithoutGenerics();
                                if (!selfName.EndsWith(selfSuffix))
                                    yield break;

                                string counterpartSuffix = Consts.REQUEST_NAMING_SUFFIX;
                                string counterpartName = selfName.Substring(0, selfName.Length - selfSuffix.Length) + counterpartSuffix;

                                if (!units.Any(c =>
                                    c.Kind == TypeDeclarationKind.RequestModel &&
                                    c.GetMemberAsType().Namespace == unit.GetMemberAsType().Namespace &&
                                    c.GetMemberAsType().GetNameWithoutGenerics() == counterpartName))
                                {
                                    yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到响应模型类型对应的请求模型类型。", unit);
                                }
                            }
                            break;
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseModelJsonPropertyNameMatchRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-json-property-name-match";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModels;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    foreach (PropertyInfo property in EnumerateProperties(unit.GetMemberAsType()))
                    {
                        var newtonsoftJsonIgnoreAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>();
                        var systemTextJsonIgnoreAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>();
                        if (newtonsoftJsonIgnoreAttr is not null && systemTextJsonIgnoreAttr is not null)
                            continue;

                        var newtonsoftJsonNameAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true);
                        var systemTextJsonNameAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true);
                        if (newtonsoftJsonNameAttr is null && systemTextJsonNameAttr is null)
                            continue;

                        string? newtonsoftJsonName = newtonsoftJsonNameAttr?.PropertyName;
                        string? systemTextJsonName = systemTextJsonNameAttr?.Name;
                        if (string.IsNullOrWhiteSpace(newtonsoftJsonName) || newtonsoftJsonName != newtonsoftJsonName?.Trim())
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"接口模型属性 \"{property.Name}\" 在 Newtonsoft.Json 中的名称为空或包含首尾空白字符。", unit);
                        if (string.IsNullOrWhiteSpace(systemTextJsonName) || systemTextJsonName != systemTextJsonName?.Trim())
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"接口模型属性 \"{property.Name}\" 在 System.Text.Json 中的名称为空或包含首尾空白字符。", unit);
                        if (newtonsoftJsonName != systemTextJsonName)
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"接口模型属性 \"{property.Name}\" 在 Newtonsoft.Json 和 System.Text.Json 中的名称不一致。", unit);
                    }
                }

                IEnumerable<PropertyInfo> EnumerateProperties(Type rootType)
                {
                    var visited = new HashSet<Type>();
                    var pending = new Stack<Type>();
                    pending.Push(rootType);

                    while (pending.Count > 0)
                    {
                        Type current = pending.Pop();
                        if (!visited.Add(current))
                            continue;

                        foreach (PropertyInfo property in current.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            yield return property;

                        Type? nested = current.GetNestedType(Consts.CONTRACT_NESTED_TYPES, BindingFlags.Public);
                        if (nested is null)
                            continue;

                        foreach (Type nestedType in nested.GetNestedTypes(BindingFlags.Public))
                            pending.Push(nestedType);
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseModelJsonSerializableRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-json-serializable";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModels;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    if (unit.GetMemberAsType().IsAbstract)
                        yield break;
                    if (unit.GetMemberAsType().IsGenericType)
                        yield break;
                    if (unit.GetMemberAsType().ContainsGenericParameters)
                        yield break;

                    object instance = Activator.CreateInstance(unit.GetMemberAsType())!;

                    if (!JsonHelper.TrySerialize(instance, unit.GetMemberAsType(), out Exception ex))
                    {
                        string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                        yield return new TypeDeclarationAnalysisIssue(RuleId, $"接口模型 JSON 序列化时遇到异常：{message}", unit);
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseMethodReturnTypeRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/method-return-type";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethods;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    Type returnType = unit.GetMemberAsMethodInfo().ReturnType;
                    if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的返回值类型必须是 \"Task<T extends ICommonResponse>\"。", unit);

                    Type respType = returnType.GetGenericArguments().Single();
                    if (!typeof(ICommonResponse).IsAssignableFrom(respType))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的返回值类型必须是 \"Task<T extends ICommonResponse>\"。", unit);
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseMethodNameCaseRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/method-name-case";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethods;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    ParameterInfo[] parameters = unit.GetMemberAsMethodInfo().GetParameters();
                    if (parameters.Length < 2)
                        yield break;

                    Type returnType = unit.GetMemberAsMethodInfo().ReturnType;
                    if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                        yield break;

                    string methodName = unit.GetMemberAsMethodInfo().Name.Substring(Consts.METHOD_NAMING_PREFIX.Length, unit.GetMemberAsMethodInfo().Name.Length - Consts.METHOD_NAMING_PREFIX.Length - Consts.METHOD_NAMING_SUFFIX.Length);
                    string requestName = parameters[1].ParameterType.GetNameWithoutGenerics();
                    string responseName = returnType.GetGenericArguments().Single().GetNameWithoutGenerics();
                    if (requestName.EndsWith(Consts.REQUEST_NAMING_SUFFIX) && requestName.Substring(0, requestName.Length - Consts.REQUEST_NAMING_SUFFIX.Length) != methodName)
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法名与请求模型名不匹配。", unit);
                    if (responseName.EndsWith(Consts.RESPONSE_NAMING_SUFFIX) && responseName.Substring(0, responseName.Length - Consts.RESPONSE_NAMING_SUFFIX.Length) != methodName)
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法名与响应模型名不匹配。", unit);
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseMethodModelsMatchRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/method-models-match";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethods;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    ParameterInfo[] parameters = unit.GetMemberAsMethodInfo().GetParameters();
                    if (parameters.Length < 2)
                        yield break;

                    Type returnType = unit.GetMemberAsMethodInfo().ReturnType;
                    if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                        yield break;

                    Type requestType = parameters[1].ParameterType;
                    Type responseType = returnType.GetGenericArguments().Single();
                    if (!ctx.UnitsOfSDKModels.Any(m => m.Kind == TypeDeclarationKind.RequestModel && m.GetMemberAsType().GetFullNameWithoutGenerics() == requestType.GetFullNameWithoutGenerics()))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到接口方法对应的请求模型类型。", unit);
                    if (!ctx.UnitsOfSDKModels.Any(m => m.Kind == TypeDeclarationKind.ResponseModel && m.GetMemberAsType().GetFullNameWithoutGenerics() == responseType.GetFullNameWithoutGenerics()))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到接口方法对应的响应模型类型。", unit);
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseMethodParametersMatchRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/method-parameters-match";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethods;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    ParameterInfo[] parameters = unit.GetMemberAsMethodInfo().GetParameters();
                    if (parameters.Length != 3)
                    {
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法必须且只能有 client、request、cancellationToken 三个参数。", unit);
                        yield break;
                    }

                    if (!typeof(ICommonClient).IsAssignableFrom(parameters[0].ParameterType))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的第一个参数必须实现 `ICommonClient`。", unit);
                    if (!typeof(ICommonRequest).IsAssignableFrom(parameters[1].ParameterType))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的第二个参数必须实现 `ICommonRequest`。", unit);
                    if (parameters[2].ParameterType != typeof(CancellationToken))
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的第三个参数必须为 `CancellationToken`。", unit);
                    if (!parameters[2].HasDefaultValue)
                        yield return new TypeDeclarationAnalysisIssue(RuleId, "接口方法的第三个参数必须具有默认值。", unit);
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseWebhookJsonPropertyNameMatchRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/webhook-json-property-name-match";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKWebhooks;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    foreach (PropertyInfo property in EnumerateProperties(unit.GetMemberAsType()))
                    {
                        var newtonsoftJsonIgnoreAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>();
                        var systemTextJsonIgnoreAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>();
                        if (newtonsoftJsonIgnoreAttr is not null && systemTextJsonIgnoreAttr is not null)
                            continue;

                        var newtonsoftJsonNameAttr = property.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>(inherit: true);
                        var systemTextJsonNameAttr = property.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>(inherit: true);
                        if (newtonsoftJsonNameAttr is null && systemTextJsonNameAttr is null)
                            continue;

                        string? newtonsoftJsonName = newtonsoftJsonNameAttr?.PropertyName;
                        string? systemTextJsonName = systemTextJsonNameAttr?.Name;
                        if (string.IsNullOrWhiteSpace(newtonsoftJsonName) || newtonsoftJsonName != newtonsoftJsonName?.Trim())
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"回调通知事件属性 \"{property.Name}\" 在 Newtonsoft.Json 中的名称为空或包含首尾空白字符。", unit);
                        if (string.IsNullOrWhiteSpace(systemTextJsonName) || systemTextJsonName != systemTextJsonName?.Trim())
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"回调通知事件属性 \"{property.Name}\" 在 System.Text.Json 中的名称为空或包含首尾空白字符。", unit);
                        if (newtonsoftJsonName != systemTextJsonName)
                            yield return new TypeDeclarationAnalysisIssue(RuleId, $"回调通知事件属性 \"{property.Name}\" 在 Newtonsoft.Json 和 System.Text.Json 中的名称不一致。", unit);
                    }
                }

                IEnumerable<PropertyInfo> EnumerateProperties(Type rootType)
                {
                    var visited = new HashSet<Type>();
                    var pending = new Stack<Type>();
                    pending.Push(rootType);

                    while (pending.Count > 0)
                    {
                        Type current = pending.Pop();
                        if (!visited.Add(current))
                            continue;

                        foreach (PropertyInfo property in current.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            yield return property;

                        Type? nested = current.GetNestedType(Consts.CONTRACT_NESTED_TYPES, BindingFlags.Public);
                        if (nested is null)
                            continue;

                        foreach (Type nestedType in nested.GetNestedTypes(BindingFlags.Public))
                            pending.Push(nestedType);
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseWebhookJsonSerializableRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/webhook-json-serializable";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKWebhooks;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    if (unit.GetMemberAsType().IsAbstract)
                        yield break;
                    if (unit.GetMemberAsType().IsGenericType)
                        yield break;
                    if (unit.GetMemberAsType().ContainsGenericParameters)
                        yield break;

                    object instance = Activator.CreateInstance(unit.GetMemberAsType())!;

                    if (!JsonHelper.TrySerialize(instance, unit.GetMemberAsType(), out Exception ex))
                    {
                        string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                        yield return new TypeDeclarationAnalysisIssue(RuleId, $"回调通知事件 JSON 序列化时遇到异常：{message}", unit);
                    }
                }
            }
            );
        }

        private static (string, TypeDeclarationRule) UseNoUnusedModelsRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/no-unused-models";
            return (RuleId, (TypeDeclarationAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModels;
                return ParallelHelper.ForEachOrdered(units, unit => RuleFn(unit).ToArray()).SelectMany(issues => issues);

                IEnumerable<TypeDeclarationAnalysisIssue> RuleFn(TypeDeclarationAnalysisUnit unit)
                {
                    switch (unit.Kind)
                    {
                        case TypeDeclarationKind.RequestModel:
                            if (!ctx.UnitsOfSDKMethods.Any(m =>
                            {
                                ParameterInfo[] parameters = m.GetMemberAsMethodInfo().GetParameters();
                                if (parameters.Length < 2)
                                    return false;
                                return parameters[1].ParameterType.GetFullNameWithoutGenerics() == unit.GetMemberAsType().GetFullNameWithoutGenerics();
                            }))
                                yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到请求模型类型对应的接口方法。", unit);
                            break;

                        case TypeDeclarationKind.ResponseModel:
                            if (!ctx.UnitsOfSDKMethods.Any(m =>
                            {
                                Type returnType = m.GetMemberAsMethodInfo().ReturnType;
                                if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
                                    return false;
                                return returnType.GetGenericArguments().Single().GetFullNameWithoutGenerics() == unit.GetMemberAsType().GetFullNameWithoutGenerics();
                            }))
                                yield return new TypeDeclarationAnalysisIssue(RuleId, "找不到响应模型类型对应的接口方法。", unit);
                            break;
                    }
                }
            }
            );
        }
    }
}
