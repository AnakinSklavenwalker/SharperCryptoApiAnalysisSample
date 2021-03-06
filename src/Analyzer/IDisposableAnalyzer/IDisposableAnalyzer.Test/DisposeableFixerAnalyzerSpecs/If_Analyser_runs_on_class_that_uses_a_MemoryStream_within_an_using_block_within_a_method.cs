using FluentAssertions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace IDisposableAnalyzer.Test.DisposeableFixerAnalyzerSpecs
{
    [TestFixture]
    internal class If_Analyser_runs_on_class_that_uses_a_MemoryStream_within_an_using_block_within_a_method :
        DisposeableFixerAnalyzerSpec
    {
        private Diagnostic[] _diagnostics;

        protected override void BecauseOf()
        {
            _diagnostics = MyHelper.RunAnalyser(Code, Sut);
        }

        private const string Code = @"
using System.IO;
namespace DisFixerTest.UsingBlock {
    public class ClassThatUsedMemoryStreamWithinUsingBlock {
        public void SomeMethod() {
            var memstream = new MemoryStream();
            using (memstream) { }
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