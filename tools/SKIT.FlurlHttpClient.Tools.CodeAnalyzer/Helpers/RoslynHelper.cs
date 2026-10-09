using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer.Helpers
{
    public static class RoslynHelper
    {
        /// <summary>
        /// 获取指定 C# 源代码中所有顶级类声明（不包含泛型类）。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ClassDeclarationSyntax[] FindTopLevelClassDeclarations(string source)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            return FindTopLevelClassDeclarations(tree);
        }

        /// <summary>
        /// 获取指定 C# 语法树中所有顶级类声明（不包含泛型类）。
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static ClassDeclarationSyntax[] FindTopLevelClassDeclarations(SyntaxTree tree)
        {
            return tree.GetRoot()
                .ChildNodes()
                .Where(node => StaticFileCodeSyntaxes.BaseNamespaceDeclaration.Contains(node.Kind()))
                .OfType<BaseNamespaceDeclarationSyntax>()
                .SelectMany(ns => ns.Members.OfType<ClassDeclarationSyntax>())
                .Where(declaration => declaration.TypeParameterList is null)
                .ToArray();
        }

        /// <summary>
        /// 获取指定 C# 源代码中指定类型的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string? GetTypeDocumentationSummary(string source, Type type)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            return GetTypeDocumentationSummary(tree, type);
        }

        /// <summary>
        /// 获取指定 C# 语法树中指定类型的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string? GetTypeDocumentationSummary(SyntaxTree tree, Type type)
        {
            var declaration = FindTypeDeclaration(tree, type);
            if (declaration is null) return null;

            return GetDocumentationSummary(declaration);
        }

        /// <summary>
        /// 获取指定 C# 源代码中指定方法的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string? GetMethodDocumentationSummary(string source, MethodInfo method)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            return GetMethodDocumentationSummary(tree, method);
        }

        /// <summary>
        /// 获取指定 C# 源代码中指定方法声明的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string? GetMethodDocumentationSummary(string source, MethodDeclarationSyntax method)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            return GetMethodDocumentationSummary(tree, method);
        }

        /// <summary>
        /// 获取指定 C# 语法树中指定方法的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string? GetMethodDocumentationSummary(SyntaxTree tree, MethodInfo method)
        {
            if (method.DeclaringType is null) return null;

            var typeDeclaration = FindTypeDeclaration(tree, method.DeclaringType);
            if (typeDeclaration is null) return null;

            var arity = method.IsGenericMethod ? method.GetGenericArguments().Length : 0;
            var parameters = method.GetParameters();
            var candidates = typeDeclaration.Members
                .OfType<MethodDeclarationSyntax>()
                .Where(m =>
                    m.Identifier.ValueText == method.Name &&
                    (m.TypeParameterList?.Parameters.Count ?? 0) == arity &&
                    m.ParameterList.Parameters.Count == parameters.Length)
                .ToArray();
            if (candidates.Length == 0) return null;

            var declaration = candidates.Length == 1
                ? candidates[0]
                : candidates.FirstOrDefault(m => IsParameterTypesMatched(m, parameters)) ?? candidates[0];
            return GetDocumentationSummary(declaration);
        }

        /// <summary>
        /// 获取指定 C# 语法树中指定方法声明的 XML 文档注释中的 &lt;summary&gt; 内容。
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string? GetMethodDocumentationSummary(SyntaxTree tree, MethodDeclarationSyntax method)
        {
            return GetDocumentationSummary(method);
        }

        private static bool IsParameterTypesMatched(MethodDeclarationSyntax declaration, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var syntaxType = declaration.ParameterList.Parameters[i].Type?.ToString().Replace(" ", string.Empty) ?? string.Empty;
                var paramType = parameters[i].ParameterType;
                if (paramType.IsByRef) paramType = paramType.GetElementType()!;

                var simpleName = paramType.Name;
                int backtick = simpleName.IndexOf('`');
                if (backtick >= 0) simpleName = simpleName.Substring(0, backtick);

                var syntaxName = syntaxType;
                int lt = syntaxName.IndexOf('<');
                if (lt >= 0) syntaxName = syntaxName.Substring(0, lt);
                syntaxName = syntaxName.TrimEnd('?', '[', ']', ',');
                int dot = syntaxName.LastIndexOf('.');
                if (dot >= 0) syntaxName = syntaxName.Substring(dot + 1);

                var elementName = simpleName.TrimEnd('[', ']', '&', '*');
                if (!string.Equals(syntaxName, elementName, StringComparison.Ordinal) &&
                    !string.Equals(syntaxName, GetKeywordAlias(elementName), StringComparison.Ordinal) &&
                    !paramType.IsGenericParameter)
                {
                    return false;
                }
            }
            return true;
        }

        private static TypeDeclarationSyntax? FindTypeDeclaration(SyntaxTree tree, Type type)
        {
            var targetType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            var arity = targetType.IsGenericType ? targetType.GetGenericArguments().Length : 0;
            var name = targetType.Name;
            int backtick = name.IndexOf('`');
            if (backtick >= 0) name = name.Substring(0, backtick);

            return tree.GetRoot()
                .DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .FirstOrDefault(d =>
                    d.Identifier.ValueText == name &&
                    (d.TypeParameterList?.Parameters.Count ?? 0) == arity &&
                    string.Equals(GetNamespace(d), targetType.Namespace ?? string.Empty, StringComparison.Ordinal));
        }

        private static string? GetDocumentationSummary(MemberDeclarationSyntax declaration)
        {
            var trivia = declaration.GetLeadingTrivia()
                .Select(t => t.GetStructure())
                .OfType<DocumentationCommentTriviaSyntax>()
                .LastOrDefault();
            if (trivia is null) return null;

            var summary = trivia.Content
                .OfType<XmlElementSyntax>()
                .FirstOrDefault(e => e.StartTag.Name.ToString() == "summary");
            if (summary is null) return null;

            var text = string.Concat(summary.Content.Select(c => c.ToString()));
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim().TrimStart('/').Trim())
                .Where(line => !string.IsNullOrEmpty(line));
            return string.Join(" ", lines);
        }

        private static string GetNamespace(SyntaxNode node)
        {
            var parts = node.Ancestors()
                .OfType<BaseNamespaceDeclarationSyntax>()
                .Select(ns => ns.Name.ToString())
                .Reverse();
            return string.Join(".", parts);
        }

        private static string? GetKeywordAlias(string name)
        {
            switch (name)
            {
                case "Boolean": return "bool";
                case "Byte": return "byte";
                case "SByte": return "sbyte";
                case "Char": return "char";
                case "Decimal": return "decimal";
                case "Double": return "double";
                case "Single": return "float";
                case "Int32": return "int";
                case "UInt32": return "uint";
                case "Int64": return "long";
                case "UInt64": return "ulong";
                case "Int16": return "short";
                case "UInt16": return "ushort";
                case "Object": return "object";
                case "String": return "string";
                default: return null;
            }
        }
    }
}
