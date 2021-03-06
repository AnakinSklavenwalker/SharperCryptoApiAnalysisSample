using System.Linq;
using FluentAssertions;
using IDisposableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace IDisposableAnalyzer.Test.DisposeableFixerAnalyzerSpecs.InvokationExpression
{
    [TestFixture]
    internal class If_Analyser_runs_MethodInvocation_that_is_part_of_expression_body_of_a_property :
        DisposeableFixerAnalyzerSpec
    {
        private const string Code = @"
namespace SomeNamespace
     public class SomeCode {
        public System.IDisposable Property => Create();
        private static System.IDisposable Create() {
            return new System.IO.MemoryStream();
        }
    }
}";

        private Diagnostic[] _diagnostics;

        protected override void BecauseOf()
        {
            _diagnostics = MyHelper.RunAnalyser(Code, Sut);
        }

        [Test]
        public void Then_there_should_be_no_Diagnostics()
        {
            var diagnostic = _diagnostics.First();
            diagnostic.Id.Should()
                .Be(SyntaxNodeAnalysisContextExtension.DiagnosticId);
        }
    }
}