using System.Linq;
using FluentAssertions;
using IDisposableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace IDisposableAnalyzer.Test.DisposeableFixerAnalyzerSpecs.ObjectCreations
{
    [TestFixture]
    internal class If_Analyser_runs_ObjectCreation_that_is_part_of_property : DisposeableFixerAnalyzerSpec
    {
        private const string Code = @"
namespace DisFixerTest.ObjectCreationAssignedToField {
     public class SomeCode {
            public System.IDisposable Property {
                get {
                    return new System.IO.MemoryStream();
                }
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
            diagnostic.Descriptor.Should()
                .Be(SyntaxNodeAnalysisContextExtension.InfoRule);
        }
    }
}