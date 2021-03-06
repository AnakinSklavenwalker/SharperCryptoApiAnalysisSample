using FluentAssertions;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace IDisposableAnalyzer.Test.DisposeableFixerAnalyzerSpecs.Async
{
    [TestFixture]
    internal class If_Analyser_runs_on_class_that_uses_await_in_return_statement :
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
    class AsyncFileStream
    {
        public Task<MemoryStream> Data()
        {
            return Task.FromResult(new MemoryStream());
        }
        public async Task<MemoryStream> Test(){
            return await Data();
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