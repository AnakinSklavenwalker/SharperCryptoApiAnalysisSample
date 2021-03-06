﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IDisposableAnalyzer.Configuration {
    internal interface IDetector
    {
        bool IsIgnoredInterface(INamedTypeSymbol namedType);
        bool IsIgnoredType(INamedTypeSymbol namedType);

        bool IsTrackedType(INamedTypeSymbol namedType, ObjectCreationExpressionSyntax node,
            SemanticModel semanticModel);

        bool IsTrackingMethodCall(InvocationExpressionSyntax methodInvocation, SemanticModel semanticModel);
        bool IsIgnoredFactoryMethod(InvocationExpressionSyntax methodInvocation, SemanticModel semanticModel);
    }
}