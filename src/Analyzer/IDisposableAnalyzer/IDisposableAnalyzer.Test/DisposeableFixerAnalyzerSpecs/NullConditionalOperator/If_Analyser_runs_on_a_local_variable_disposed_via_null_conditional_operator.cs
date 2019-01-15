using FluentAssertions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace IDisposableAnalyzer.Test.DisposeableFixerAnalyzerSpecs.NullConditionalOperator
{
    [TestFixture]
    internal class If_Analyser_runs_on_a_local_variable_disposed_via_null_conditional_operator :
        DisposeableFixerAnalyzerSpec
    {
        private Diagnostic[] _diagnostics;

        protected override void BecauseOf()
        {
            _diagnostics = MyHelper.RunAnalyser(Code, Sut);
        }

        private const string Code = @"
using System.IO;
using System.Threading.Tasks;
namespace DisFixerTest.Async
{
    internal class MyClass : IDisposable
    {
        public MyClass()
        {
            var exampleDisposable = new MemoryStream();
            exampleDisposable?.Dispose();
        }
    }
}
";

        [Test]
        public void Then_there_should_be_no_Diagnostics()
        {
            _diagnostics.Length.Should().Be(0);
        }
    }
}