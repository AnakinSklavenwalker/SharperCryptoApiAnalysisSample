﻿using System.Collections.Generic;
using System.Linq;
using IDisposableAnalyzer.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IDisposableAnalyzer.Extensions
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        public static FieldDeclarationSyntax FindFieldNamed(this ClassDeclarationSyntax classDeclarationSyntax,
            string name)
        {
            return classDeclarationSyntax
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(fds =>
                {
                    return fds
                               .DescendantNodes()
                               .OfType<VariableDeclarationSyntax>()
                               .Count(id => id.Variables.Any(v => v.Identifier.Text == name)) == 1;
                })
                .FirstOrDefault();
        }

        public static IEnumerable<MethodDeclarationSyntax> GetDisposingMethods(
            this ClassDeclarationSyntax @class, SyntaxNodeAnalysisContext context,
            IConfiguration configuration)
        {
            var disposeMethods = configuration.DisposingMethods;


            return @class
                .DescendantNodes<MethodDeclarationSyntax>()
                .Where(mds => mds.IsDisposeMethod(configuration, context.SemanticModel));
        }

        public static IEnumerable<MethodDeclarationSyntax> GetParameterlessMethodNamed(
            this ClassDeclarationSyntax @class, string name)
        {
            return @class
                .DescendantNodes<MethodDeclarationSyntax>()
                .Where(mds => mds.Parent == @class) //filters inner classes with Dispose-Methode
                .Where(mds => mds.Identifier.Text == name && mds.ParameterList.Parameters.Count == 0);
        }
    }
}