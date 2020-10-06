using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace OpenApiCheck.Model
{
    public class OperationComparison
    {
        private readonly List<ComparisonMessage> _messages = new List<ComparisonMessage>();

        public OperationType Method { get; }
        public string Path { get; }
        public bool IsDeprecated { get; }
        public IReadOnlyList<ComparisonMessage> Messages => _messages;
        public CompareStatus Status => Messages.Select(x => x.Status).DefaultIfEmpty(CompareStatus.OK).Max();
        public string FullName => $"{(IsDeprecated ? "[deprecated] " : "")}{Method} {Path}";

        public OperationComparison(OperationType method, string path, bool isDeprecated)
        {
            IsDeprecated = isDeprecated;
            Method = method;
            Path = path;
        }

        public void ReportIssue(string message, CompareStatus severity = CompareStatus.Error)
        {
            _messages.Add(new ComparisonMessage(IsDeprecated ? CompareStatus.Warning : severity, message));
        }

        public override string ToString() => $"{FullName}: {Status}";
    }
}