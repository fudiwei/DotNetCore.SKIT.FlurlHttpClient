using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    using SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers;

    internal static class StaticFileRules
    {
        public const string RULE_ID_PREFIX = "static-file";

        public static IReadOnlyList<(string Id, StaticFileRule Rule)> Defaults { get; } = new[]
        {
            UseModelNameCaseRule(),
            UseModelDocumentedRule(),
            UseMethodNameCaseRule(),
            UseMethodDocumentedRule(),
            UseWebhookNameCaseRule(),
            UseFixtureJsonDeserializableRule(),
            UseFixtureXmlDeserializableRule(),
        };

        private static (string Id, StaticFileRule Rule) UseModelNameCaseRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-name-case";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModelsCodes;
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(ctx.ReadFile(unit)).WithFilePath(unit.FileInfo.FullName);
                    ClassDeclarationSyntax[] classes = RoslynHelper.FindTopLevelClassDeclarations(tree);
                    if (classes.Length == 0)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口模型源代码中不存在类声明语法结构。", unit);
                        yield break;
                    }
                    else if (classes.Length > 1)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口模型源代码中存在多个类声明语法结构。", unit);
                        yield break;
                    }

                    string className = classes[0].Identifier.ValueText;
                    string fileName = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                    if (!string.Equals(fileName, className, StringComparison.Ordinal))
                        yield return new StaticFileAnalysisIssue(RuleId, $"接口模型源代码类声明的与文件名不匹配。（Expected: \"{fileName}\", Actual: \"{className}\"）", unit);
                }
            }
            );
        }

        private static (string Id, StaticFileRule Rule) UseModelDocumentedRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/model-documented";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModelsCodes;
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    string className = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                    string typeName = $"{ctx.Options.SDKModelsNamespace}.{className}";
                    Type? type = ctx.Options.Assembly.GetType(typeName, throwOnError: false, ignoreCase: true);
                    if (type is null)
                    {
                        yield break;
                    }

                    string? documentation = RoslynHelper.GetTypeDocumentationSummary(ctx.ReadFile(unit), type);
                    if (string.IsNullOrEmpty(documentation))
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口模型源代码中不存在有效的 XML 文档注释语法结构。", unit);
                        yield break;
                    }

                    switch (unit.Kind)
                    {
                        case StaticFileKind.RequestModelCode:
                            {
                                Regex regex = GetCustomConfigForRequestModelDocumentationRegex(ctx);
                                if (!regex.IsMatch(documentation))
                                    yield return new StaticFileAnalysisIssue(RuleId, "接口模型源代码中不存在表示请求模型的 XML 文档注释语法结构。", unit);
                            }
                            break;
                        case StaticFileKind.ResponseModelCode:
                            {
                                Regex regex = GetCustomConfigForResponseModelDocumentationRegex(ctx);
                                if (!regex.IsMatch(documentation))
                                    yield return new StaticFileAnalysisIssue(RuleId, "接口模型源代码中不存在表示响应模型的 XML 文档注释语法结构。", unit);
                            }
                            break;
                    }
                }
            }
            );
        }

        private static (string Id, StaticFileRule Rule) UseMethodNameCaseRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/method-name-case";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethodsCodes;
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(ctx.ReadFile(unit)).WithFilePath(unit.FileInfo.FullName);
                    ClassDeclarationSyntax[] classes = RoslynHelper.FindTopLevelClassDeclarations(tree);
                    if (classes.Length == 0)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口方法源代码中不存在类声明语法结构。", unit);
                        yield break;
                    }
                    else if (classes.Length > 1)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口方法源代码中存在多个类声明语法结构。", unit);
                        yield break;
                    }

                    string className = classes[0].Identifier.ValueText;
                    string fileName = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                    if (!string.Equals(fileName, className, StringComparison.Ordinal))
                    {
                        if (fileName.Contains('_'))
                        {
                            fileName = fileName.Split('_')[0];
                            if (string.Equals(fileName, className, StringComparison.Ordinal))
                                yield break;
                        }

                        yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码类声明的与文件名不匹配。（Expected: \"{fileName}\", Actual: \"{className}\"）", unit);
                    }
                }
            }
            );
        }

        private static (string Id, StaticFileRule Rule) UseMethodDocumentedRule()
        {
            const string PATH_VARIABLE_PLACEHOLDER = "{var}";

            const string RuleId = $"{RULE_ID_PREFIX}/method-documented";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKMethodsCodes;
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(ctx.ReadFile(unit)).WithFilePath(unit.FileInfo.FullName);
                    SyntaxNode root = tree.GetCompilationUnitRoot();
                    MethodDeclarationSyntax[] methods = root.ChildNodes()
                        .Where(node => StaticFileCodeSyntaxes.BaseNamespaceDeclaration.Contains(node.Kind()))
                        .OfType<BaseNamespaceDeclarationSyntax>()
                        .SelectMany(ns => ns.Members.OfType<ClassDeclarationSyntax>())
                        .SelectMany(type => type.Members.OfType<MethodDeclarationSyntax>())
                        .Where(method =>
                        {
                            string name = method.Identifier.ValueText;
                            return name.StartsWith(Consts.METHOD_NAMING_PREFIX, StringComparison.Ordinal) &&
                                   name.EndsWith(Consts.METHOD_NAMING_SUFFIX, StringComparison.Ordinal) &&
                                   method.ParameterList.Parameters.Count >= 3;
                        })
                        .ToArray();
                    if (methods.Length == 0)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "接口方法源代码中不存在函数声明语法结构。", unit);
                        yield break;
                    }

                    foreach (MethodDeclarationSyntax method in methods)
                    {
                        string methodName = method.Identifier.ValueText;
                        string? className = (method.Parent as ClassDeclarationSyntax)?.Identifier.ValueText;
                        Type? type = className is null
                            ? null
                            : ctx.Options.Assembly.GetType($"{ctx.Options.SDKMethodsNamespace}.{className}", throwOnError: false, ignoreCase: false);
                        if (type is null)
                            continue;

                        int parameterCount = method.ParameterList.Parameters.Count;
                        int typeParameterCount = method.TypeParameterList?.Parameters.Count ?? 0;
                        MethodInfo? methodInfo = type
                            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .FirstOrDefault(m =>
                                string.Equals(m.Name, methodName, StringComparison.Ordinal) &&
                                m.GetParameters().Length == parameterCount &&
                                m.GetGenericArguments().Length == typeParameterCount);
                        if (methodInfo is null)
                            continue;

                        // 验证接口方法文档注释
                        string? documentation = RoslynHelper.GetMethodDocumentationSummary(tree, methodInfo);
                        if (string.IsNullOrEmpty(documentation))
                        {
                            yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码中函数 \"{methodName}\" 不存在有效的 XML 文档注释语法结构。", unit);
                            yield break;
                        }
                        Match documentationMatch = GetCustomConfigForExecutingMethodDocumentationRegex(ctx).Match(documentation);
                        if (!documentationMatch.Success)
                            yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码中函数 \"{methodName}\" 不存在表示接口方法的 XML 文档注释语法结构。", unit);

                        // 验证接口方法对应的请求模型文档注释
                        if (documentationMatch.Success)
                        {
                            Type requestType = methodInfo.GetParameters()[1].ParameterType;
                            string requestName = requestType.GetNameWithoutGenerics();
                            StaticFileAnalysisUnit requestUnit = ctx.UnitsOfSDKModelsCodes.Single(u => u.Kind == StaticFileKind.RequestModelCode && requestName.Equals(Path.GetFileNameWithoutExtension(u.FileInfo.Name), StringComparison.Ordinal));
                            string? requestDocumentation = RoslynHelper.GetTypeDocumentationSummary(ctx.ReadFile(requestUnit), requestType);
                            if (!string.IsNullOrEmpty(requestDocumentation))
                            {
                                Match m1 = documentationMatch;
                                Match m2 = GetCustomConfigForRequestModelDocumentationRegex(ctx).Match(requestDocumentation);
                                if (m1.Success && m2.Success)
                                {
                                    string verb1 = m1.Groups[1].Value;
                                    string path1 = m1.Groups[2].Value;
                                    string verb2 = m2.Groups[1].Value;
                                    string path2 = m2.Groups[2].Value;
                                    if (!string.Equals(verb1, verb2, StringComparison.Ordinal) || !string.Equals(path1, path2, StringComparison.Ordinal))
                                        yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码中函数 \"{methodName}\" 的 XML 文档注释与对应请求模型类代码文件不匹配。（Expected: \"[{verb1}] {path1}\", Actual: \"[{verb2}] {path2}\"）", unit);
                                }
                            }
                        }

                        // 验证接口方法对应的响应模型文档注释
                        if (documentationMatch.Success)
                        {
                            Type responseType = methodInfo.ReturnType.GetGenericArguments().Single();
                            string responseName = responseType.GetNameWithoutGenerics();
                            StaticFileAnalysisUnit responseUnit = ctx.UnitsOfSDKModelsCodes.Single(u => u.Kind == StaticFileKind.ResponseModelCode && responseName.Equals(Path.GetFileNameWithoutExtension(u.FileInfo.Name), StringComparison.Ordinal));
                            string? responseDocumentation = RoslynHelper.GetTypeDocumentationSummary(ctx.ReadFile(responseUnit), responseType);
                            if (!string.IsNullOrEmpty(responseDocumentation))
                            {
                                Match m1 = documentationMatch;
                                Match m2 = GetCustomConfigForResponseModelDocumentationRegex(ctx).Match(responseDocumentation);
                                if (m1.Success && m2.Success)
                                {
                                    string verb1 = m1.Groups[1].Value;
                                    string path1 = m1.Groups[2].Value;
                                    string verb2 = m2.Groups[1].Value;
                                    string path2 = m2.Groups[2].Value;
                                    if (!string.Equals(verb1, verb2, StringComparison.Ordinal) || !string.Equals(path1, path2, StringComparison.Ordinal))
                                        yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码中函数 \"{methodName}\" 的 XML 文档注释与对应响应模型类代码文件不匹配。（Expected: \"[{verb1}] {path1}\", Actual: \"[{verb2}] {path2}\"）", unit);
                                }
                            }
                        }

                        // 验证接口方法代码实现
                        if (documentationMatch.Success)
                        {
                            if (!TryExtractMethodImpl(method, out string? implVerb, out string[]? implSegments))
                            {
                                yield return new StaticFileAnalysisIssue(RuleId, $"接口方法源代码中函数 \"{methodName}\" 未识别到 `CreateFlurlRequest` 调用。", unit);
                            }
                            else
                            {
                                string implPath = (implSegments!.Length > 0 ? "/" : "") + string.Join("/", implSegments!);
                                string docsVerb = documentationMatch.Groups[1].Value;
                                string docsPath = new Regex(@"\{[^}]*\}", RegexOptions.Compiled | RegexOptions.IgnoreCase).Replace(documentationMatch.Groups[2].Value, PATH_VARIABLE_PLACEHOLDER).Split('?')[0];

                                string error = $"接口方法源代码中函数 \"{methodName}\" 的 XML 文档注释与代码实现不匹配。（Expected: \"[{docsVerb.ToUpperInvariant()}] {docsPath}\", Actual: \"[{implVerb}] {implPath}\"）";
                                if (!string.Equals(docsVerb, implVerb, StringComparison.OrdinalIgnoreCase))
                                    yield return new StaticFileAnalysisIssue(RuleId, error, unit);
                                else if (!string.Equals(docsPath, implPath, StringComparison.Ordinal) && !(docsPath.Equals(PATH_VARIABLE_PLACEHOLDER) && implPath.Equals(string.Empty)))
                                    yield return new StaticFileAnalysisIssue(RuleId, error, unit);
                            }
                        }
                    }
                }
            }
            );

            static bool TryExtractMethodImpl(MethodDeclarationSyntax method, out string? verb, out string[]? segments)
            {
                verb = null;
                segments = null;

                SyntaxNode? root = (SyntaxNode?)method.Body ?? method.ExpressionBody;
                if (root is null)
                    return false;

                foreach (InvocationExpressionSyntax invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    string? name = invocation.Expression switch
                    {
                        MemberAccessExpressionSyntax ma => ma.Name.Identifier.ValueText,
                        IdentifierNameSyntax id => id.Identifier.ValueText,
                        _ => null
                    };
                    if (name != Consts.METHOD_IMPLEMENT_FUNC)
                        continue;

                    var args = invocation.ArgumentList.Arguments;
                    if (args.Count < 2)
                        continue;

                    string? httpMethod = args[1].Expression switch
                    {
                        MemberAccessExpressionSyntax ma => ma.Name.Identifier.ValueText.ToUpperInvariant(),
                        ObjectCreationExpressionSyntax oc => GetFirstStringLiteral(oc.ArgumentList)?.ToUpperInvariant(),
                        ImplicitObjectCreationExpressionSyntax ioc => GetFirstStringLiteral(ioc.ArgumentList)?.ToUpperInvariant(),
                        _ => null
                    };
                    if (httpMethod is null)
                        continue;

                    // 字符串字面量取其值，其他类型表达式为 "{var}"（表示变量）
                    verb = httpMethod.ToUpper();
                    segments = args.Skip(2)
                        .Select(a => a.Expression is LiteralExpressionSyntax lit && lit.IsKind(SyntaxKind.StringLiteralExpression) ? lit.Token.ValueText : PATH_VARIABLE_PLACEHOLDER)
                        .ToArray();
                    return true;
                }

                return false;
            }

            static string? GetFirstStringLiteral(ArgumentListSyntax? argumentList)
            {
                if (argumentList is not null && argumentList.Arguments.Count > 0 &&
                    argumentList.Arguments[0].Expression is LiteralExpressionSyntax lit && lit.IsKind(SyntaxKind.StringLiteralExpression))
                    return lit.Token.ValueText;
                return null;
            }
        }

        private static (string Id, StaticFileRule Rule) UseWebhookNameCaseRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/webhook-name-case";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKWebhooksCodes;
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(ctx.ReadFile(unit)).WithFilePath(unit.FileInfo.FullName);
                    ClassDeclarationSyntax[] classes = RoslynHelper.FindTopLevelClassDeclarations(tree);
                    if (classes.Length == 0)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "回调通知事件源代码中不存在类声明语法结构。", unit);
                        yield break;
                    }
                    else if (classes.Length > 1)
                    {
                        yield return new StaticFileAnalysisIssue(RuleId, "回调通知事件源代码中存在多个类声明语法结构。", unit);
                        yield break;
                    }

                    string className = classes[0].Identifier.ValueText;
                    string fileName = Path.GetFileNameWithoutExtension(unit.FileInfo.Name);
                    if (!string.Equals(fileName, className, StringComparison.Ordinal))
                        yield return new StaticFileAnalysisIssue(RuleId, $"回调通知事件源代码类声明的与文件名不匹配。（Expected: \"{fileName}\", Actual: \"{className}\"）", unit);
                }
            }
            );
        }

        private static (string Id, StaticFileRule Rule) UseFixtureJsonDeserializableRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/fixture-json-deserializable";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModelsFixtures.Concat(ctx.UnitsOfSDKWebhooksFixtures);
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    if (!unit.FileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                        yield break;

                    string name = Path.GetFileNameWithoutExtension(unit.FileInfo.Name).Split('#').First();
                    string content = ctx.ReadFile(unit);

                    Type? type = null;
                    switch (unit.Kind)
                    {
                        case StaticFileKind.RequestModelFixture:
                        case StaticFileKind.ResponseModelFixture:
                            {
                                type = ctx.Options.Assembly.GetType($"{ctx.Options.SDKModelsNamespace}.{name}", throwOnError: false, ignoreCase: true);
                                if (type is null)
                                {
                                    yield return new StaticFileAnalysisIssue(RuleId, "找不到接口模型样例文件对应的类型声明。", unit);
                                }
                                else if (!JsonHelper.TryDeserialize(content, type, out Exception ex))
                                {
                                    string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                                    yield return new StaticFileAnalysisIssue(RuleId, $"接口模型样例文件 JSON 反序列化时遇到异常：{message}", unit);
                                }
                            }
                            break;

                        case StaticFileKind.WebhookEventFixture:
                            {
                                type = ctx.Options.Assembly.GetType($"{ctx.Options.SDKWebhooksNamespace}.{name}", throwOnError: false, ignoreCase: true);
                                if (type is null)
                                {
                                    yield return new StaticFileAnalysisIssue(RuleId, "找不到回调通知事件样例文件对应的类型声明。", unit);
                                }
                                else if (!JsonHelper.TryDeserialize(content, type, out Exception ex))
                                {
                                    string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                                    yield return new StaticFileAnalysisIssue(RuleId, $"回调通知事件样例文件 JSON 反序列化时遇到异常：{message}", unit);
                                }
                            }
                            break;
                    }
                }
            }
            );
        }

        private static (string Id, StaticFileRule Rule) UseFixtureXmlDeserializableRule()
        {
            const string RuleId = $"{RULE_ID_PREFIX}/fixture-xml-deserializable";
            return (RuleId, (StaticFileAnalyzerContext ctx) =>
            {
                var units = ctx.UnitsOfSDKModelsFixtures.Concat(ctx.UnitsOfSDKWebhooksFixtures);
                return ParallelHelper.ForEachOrdered(units, RuleFn).SelectMany(issues => issues);

                IEnumerable<StaticFileAnalysisIssue> RuleFn(StaticFileAnalysisUnit unit)
                {
                    if (!unit.FileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        yield break;

                    string name = Path.GetFileNameWithoutExtension(unit.FileInfo.Name).Split('#').First();
                    string content = ctx.ReadFile(unit);

                    Type? type = null;
                    switch (unit.Kind)
                    {
                        case StaticFileKind.RequestModelFixture:
                        case StaticFileKind.ResponseModelFixture:
                            {
                                type = ctx.Options.Assembly.GetType($"{ctx.Options.SDKModelsNamespace}.{name}", throwOnError: false, ignoreCase: true);
                                if (type is null)
                                {
                                    yield return new StaticFileAnalysisIssue(RuleId, "找不到接口模型样例文件对应的类型声明。", unit);
                                }
                                else if (!XmlHelper.TryDeserialize(content, type, out Exception ex))
                                {
                                    string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                                    yield return new StaticFileAnalysisIssue(RuleId, $"接口模型样例文件 XML 反序列化时遇到异常：{message}", unit);
                                }
                            }
                            break;

                        case StaticFileKind.WebhookEventFixture:
                            {
                                type = ctx.Options.Assembly.GetType($"{ctx.Options.SDKWebhooksNamespace}.{name}", throwOnError: false, ignoreCase: true);
                                if (type is null)
                                {
                                    yield return new StaticFileAnalysisIssue(RuleId, "找不到回调通知事件样例文件对应的类型声明。", unit);
                                }
                                else if (!XmlHelper.TryDeserialize(content, type, out Exception ex))
                                {
                                    string message = ex.GetBaseException()?.ToString() ?? ex.ToString();
                                    yield return new StaticFileAnalysisIssue(RuleId, $"回调通知事件样例文件 XML 反序列化时遇到异常：{message}", unit);
                                }
                            }
                            break;
                    }
                }
            }
            );
        }

        private static Regex GetCustomConfigForRequestModelDocumentationRegex(StaticFileAnalyzerContext ctx)
        {
            if (ctx.Options.CustomRuleConfigurations is not null && ctx.Options.CustomRuleConfigurations.TryGetValue(string.Empty, out var customConfigs))
            {
                if (customConfigs.TryGetValue("RequestModelDocumentationRegex", out object? regexPattern) && regexPattern is not null)
                {
                    return (Regex)regexPattern;
                }
            }

            return new Regex(@"表示 \[([a-zA-Z]+)\] (\S.*?) 接口的请求。", RegexOptions.Compiled);
        }

        private static Regex GetCustomConfigForResponseModelDocumentationRegex(StaticFileAnalyzerContext ctx)
        {
            if (ctx.Options.CustomRuleConfigurations is not null && ctx.Options.CustomRuleConfigurations.TryGetValue(string.Empty, out var customConfigs))
            {
                if (customConfigs.TryGetValue("ResponseModelDocumentationRegex", out object? regex) && regex is not null)
                {
                    return (Regex)regex;
                }
            }

            return new Regex(@"表示 \[([a-zA-Z]+)\] (\S.*?) 接口的响应。", RegexOptions.Compiled);
        }

        private static Regex GetCustomConfigForExecutingMethodDocumentationRegex(StaticFileAnalyzerContext ctx)
        {
            if (ctx.Options.CustomRuleConfigurations is not null && ctx.Options.CustomRuleConfigurations.TryGetValue(string.Empty, out var customConfigs))
            {
                if (customConfigs.TryGetValue("ExecutingMethodDocumentationRegex", out object? regex) && regex is not null)
                {
                    return (Regex)regex;
                }
            }

            return new Regex(@"异步调用 \[([a-zA-Z]+)\] (\S.*?) 接口。", RegexOptions.Compiled);
        }
    }
}
