using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public static class SourceFileCodeSyntaxKinds
    {
        public static ImmutableArray<SyntaxKind> BaseNamespaceDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.NamespaceDeclaration,
            SyntaxKind.FileScopedNamespaceDeclaration
        );

        public static ImmutableArray<SyntaxKind> BaseTypeDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.ClassDeclaration,
            SyntaxKind.StructDeclaration,
            SyntaxKind.InterfaceDeclaration,
            SyntaxKind.EnumDeclaration,
            SyntaxKind.RecordDeclaration,
            SyntaxKind.RecordStructDeclaration
        );

        public static ImmutableArray<SyntaxKind> TypeDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.ClassDeclaration,
            SyntaxKind.StructDeclaration,
            SyntaxKind.InterfaceDeclaration,
            SyntaxKind.RecordDeclaration,
            SyntaxKind.RecordStructDeclaration
        );

        public static ImmutableArray<SyntaxKind> BaseFieldDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.FieldDeclaration,
            SyntaxKind.EventFieldDeclaration
        );

        public static ImmutableArray<SyntaxKind> BaseMethodDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.MethodDeclaration,
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.DestructorDeclaration,
            SyntaxKind.OperatorDeclaration,
            SyntaxKind.ConversionOperatorDeclaration
        );

        public static ImmutableArray<SyntaxKind> BasePropertyDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.PropertyDeclaration,
            SyntaxKind.EventDeclaration,
            SyntaxKind.IndexerDeclaration
        );

        public static ImmutableArray<SyntaxKind> AccessorDeclaration { get; } = ImmutableArray.Create(
            SyntaxKind.GetAccessorDeclaration,
            SyntaxKind.SetAccessorDeclaration,
            SyntaxKind.AddAccessorDeclaration,
            SyntaxKind.RemoveAccessorDeclaration,
            SyntaxKind.UnknownAccessorDeclaration
        );

        public static ImmutableArray<SyntaxKind> InitializerExpression { get; } = ImmutableArray.Create(
            SyntaxKind.ArrayInitializerExpression,
            SyntaxKind.CollectionInitializerExpression,
            SyntaxKind.ComplexElementInitializerExpression,
            SyntaxKind.ObjectInitializerExpression
        );

        public static ImmutableArray<SyntaxKind> DocumentationComment { get; } = ImmutableArray.Create(
            SyntaxKind.SingleLineDocumentationCommentTrivia,
            SyntaxKind.MultiLineDocumentationCommentTrivia
        );

        public static ImmutableArray<SyntaxKind> ConstructorInitializer { get; } = ImmutableArray.Create(
            SyntaxKind.BaseConstructorInitializer,
            SyntaxKind.ThisConstructorInitializer
        );

        public static ImmutableArray<SyntaxKind> LambdaExpression { get; } = ImmutableArray.Create(
            SyntaxKind.ParenthesizedLambdaExpression,
            SyntaxKind.SimpleLambdaExpression
        );

        public static ImmutableArray<SyntaxKind> AnonymousFunctionExpression { get; } = ImmutableArray.Create(
            SyntaxKind.ParenthesizedLambdaExpression,
            SyntaxKind.SimpleLambdaExpression,
            SyntaxKind.AnonymousMethodExpression
        );

        public static ImmutableArray<SyntaxKind> SimpleName { get; } = ImmutableArray.Create(
            SyntaxKind.GenericName,
            SyntaxKind.IdentifierName
        );

        public static ImmutableArray<SyntaxKind> BaseParameterList { get; } = ImmutableArray.Create(
            SyntaxKind.ParameterList,
            SyntaxKind.BracketedParameterList
        );

        public static ImmutableArray<SyntaxKind> BaseArgumentList { get; } = ImmutableArray.Create(
            SyntaxKind.ArgumentList,
            SyntaxKind.BracketedArgumentList
        );

        public static ImmutableArray<SyntaxKind> IntegerLiteralKeyword { get; } = ImmutableArray.Create(
            SyntaxKind.IntKeyword,
            SyntaxKind.LongKeyword,
            SyntaxKind.ULongKeyword,
            SyntaxKind.UIntKeyword
        );

        public static ImmutableArray<SyntaxKind> RealLiteralKeyword { get; } = ImmutableArray.Create(
            SyntaxKind.FloatKeyword,
            SyntaxKind.DoubleKeyword,
            SyntaxKind.DecimalKeyword
        );
    }
}
