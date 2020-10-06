using System.Collections.Generic;
using System.Linq;

namespace OpenApiCheck.Model
{
    public class OpenApiComparison
    {
        public CompareStatus Status => Operations.Select(x => x.Status).DefaultIfEmpty(CompareStatus.OK).Max();
        public IReadOnlyList<OperationComparison> Operations { get; }

        public OpenApiComparison(IEnumerable<OperationComparison> operations)
        {
            Operations = operations.ToArray();
        }
    }
}