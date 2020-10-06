using System.Linq;
using OpenApiCheck;
using OpenApiCheck.Model;
using Shouldly;

namespace OpenApi.Check.Tests
{
    public static class ResultExtensions
    {
        public static OperationComparison[] FindOperations(this OpenApiComparison results, string path)
        {
            var operations = results.Operations.Where(op => op.Path.StartsWith(path)).ToArray();
            operations.ShouldNotBeEmpty(path);
            return operations;
        }

        public static OperationComparison[] ShouldAllBeOk(this OperationComparison[] results)
        {
            results.ShouldAllBe(x => x.Status == CompareStatus.OK);
            return results;
        }

        public static OperationComparison[] ShouldHaveErrors(this OperationComparison[] results, params string[] errors)
        {
            var actual = results.SelectMany(r => r.Messages.Select(m => $"{r.FullName}: [{m.Status}] {m.Message}")).ToArray();
            actual.ShouldBe(errors);
            return results;
        }
    }
}