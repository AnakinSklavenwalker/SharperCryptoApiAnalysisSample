using System;
using System.Collections.Generic;
using System.Linq;
using IDisposableAnalyzer.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IDisposableAnalyzer.Extensions
{
    internal static class MethodDeclarationSyntaxExtension
    {
        public static bool IsDisposeMethod(this MethodDeclarationSyntax method,
            IConfiguration configuration, SemanticModel semanticModel)
        {
            var parameterTypes = new Lazy<string[]>(() =>
            {
                return method.ParameterList.Parameters
                    .Select(p => (semanticModel.GetSymbolInfo(p.Type).Symbol as INamedTypeSymbol).GetFullNamespace())
                    .ToArray();
            });

            IReadOnlyCollection<MethodCall> methods;
            if (configuration.DisposingMethods.TryGetValue(method.Identifier.Text, out methods))
            {
                return methods.Any(mc => mc.IsStatic == method.IsStatic()
                                         && mc.Parameter.Length == method.ParameterList.Parameters.Count
                                         && parameterTypes.Value.SequenceEqual(mc.Parameter));
            }

            return method.AttributeLists
                .SelectMany(als => als.Attributes)
                .Select(a => semanticModel.GetTypeInfo(a).Type)
                .Any(attribute => configuration.DisposingAttributes.Contains(attribute.GetFullNamespace()));
        }

        public static bool IsStatic(this MethodDeclarationSyntax method)
        {
            return method.Modifiers.Any(SyntaxKind.StaticKeyword);
        }

        public static bool Returns(this MethodDeclarationSyntax method, string name)
        {
            if (name == null) return false;
            return method.DescendantNodes<ReturnStatementSyntax>()
                .Select(rss => rss.DescendantNodes<IdentifierNameSyntax>())
                .Any(inss => inss.Any(ins => ins.Identifier.Text == name));
        }
    }
}