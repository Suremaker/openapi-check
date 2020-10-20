using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using OpenApiCheck.Model;

namespace OpenApiCheck
{
    public class OpenApiComparer
    {
        private readonly OpenApiDocument _nextDoc;
        private readonly OpenApiDocument _currentDoc;
        private readonly List<OperationComparison> _operations = new List<OperationComparison>();

        public static OpenApiComparison Compare(OpenApiDocument current, OpenApiDocument next)
        {
            return new OpenApiComparer(current, next).Compare();
        }

        private OpenApiComparer(OpenApiDocument currentDoc, OpenApiDocument nextDoc)
        {
            _currentDoc = currentDoc;
            _nextDoc = nextDoc;
        }

        OpenApiComparison Compare()
        {
            foreach (var path in _currentDoc.Paths)
            {
                ComparePath(path);
            }
            return new OpenApiComparison(_operations);
        }

        private void ComparePath(in KeyValuePair<string, OpenApiPathItem> currentPath)
        {
            if (!_nextDoc.Paths.TryGetValue(currentPath.Key, out var nextPath))
                nextPath = new OpenApiPathItem();

            foreach (var (currentOpKey, currentOp) in currentPath.Value.Operations)
            {
                var operation = new OperationComparison(currentOpKey, currentPath.Key, currentOp.Deprecated);
                _operations.Add(operation);
                if (!nextPath.Operations.TryGetValue(currentOpKey, out var nextOp))
                    operation.ReportIssue("Operation no longer exists");
                else
                    CompareOp(currentOp, nextOp, operation);
            }
        }

        private void CompareOp(OpenApiOperation currentOp, OpenApiOperation nextOp, OperationComparison operation)
        {
            foreach (var (statusCode, currentResponse) in currentOp.Responses)
            {
                if (!nextOp.Responses.TryGetValue(statusCode, out var nextResponse))
                    operation.ReportIssue($"Operation no longer returns HTTP {statusCode} code");
                else
                    CompareResponse(statusCode, currentResponse, nextResponse, operation);
            }

            CompareRequest(currentOp.RequestBody, nextOp.RequestBody, operation);
        }

        private void CompareRequest(OpenApiRequestBody currentRequest, OpenApiRequestBody nextRequest, OperationComparison operation)
        {
            if (nextRequest == null)
                return;

            if (nextRequest.Required && (currentRequest == null || !currentRequest.Required))
                operation.ReportIssue("Operation request body is now required, while it was not before");

            if (currentRequest == null)
                return;

            foreach (var (contentType, currentModel) in currentRequest.Content)
            {
                if (!nextRequest.Content.TryGetValue(contentType, out var nextModel))
                    operation.ReportIssue($"Operation no longer accepts request for {contentType}");
                else
                    CompareRequestModels(contentType, currentModel, nextModel, operation);
            }
        }

        private void CompareResponse(string statusCode, OpenApiResponse currentResponse, OpenApiResponse nextResponse, OperationComparison operation)
        {
            foreach (var (contentType, currentModel) in currentResponse.Content)
            {
                if (!nextResponse.Content.TryGetValue(contentType, out var nextModel))
                    operation.ReportIssue($"Operation no longer returns {contentType} for {statusCode} code");
                else
                    CompareResponseModels(statusCode, contentType, currentModel, nextModel, operation);
            }
        }

        private void CompareResponseModels(string statusCode, string contentType, OpenApiMediaType currentModel, OpenApiMediaType nextModel, OperationComparison operation)
        {
            CompareResponseSchema($"response(HTTP {statusCode}|{contentType}).body", currentModel.Schema, nextModel.Schema, operation, operation.IsDeprecated);
        }

        private void CompareRequestModels(string contentType, OpenApiMediaType currentModel, OpenApiMediaType nextModel, OperationComparison operation)
        {
            CompareRequestSchema($"request({contentType}).body", nextModel.Schema, currentModel.Schema, operation, operation.IsDeprecated, false, false);
        }

        private void CompareRequestSchema(string path, OpenApiSchema next, OpenApiSchema current,
            OperationComparison operation, bool isDeprecated, bool isNextRequired, bool isCurrentRequired)
        {
            if (current == null)
                return;
            if (current.Deprecated && next == null)
                return;

            isDeprecated |= current.Deprecated;

            if (next == null)
            {
                ReportPathIssue(operation, path, isDeprecated, "no longer exists");
                return;
            }

            if (current.Type != next.Type)
            {
                ReportPathIssue(operation, path, isDeprecated, $"type does not match (before: {current.Type}, after: {next.Type})");
                return;
            }

            if (!isCurrentRequired && isNextRequired)
            {
                ReportPathIssue(operation, path, isDeprecated, "is now required");
                return;
            }

            if (current.Nullable && !next.Nullable)
            {
                ReportPathIssue(operation, path, isDeprecated, "is no longer nullable");
                return;
            }

            if (current.Type == "array")
            {
                CompareRequestSchema($"{path}[]", next.Items, current.Items, operation, isDeprecated, false, false);
                return;
            }

            foreach (var (name, currentProp) in current.Properties)
            {
                var nextProp = next.Properties.TryGetValue(name, out var n) ? n : null;

                CompareRequestSchema($"{path}.{name}", nextProp, currentProp, operation, isDeprecated, next.Required.Contains(name), current.Required.Contains(name));
            }
            foreach (var (name, nextProp) in next.Properties)
            {
                if (!current.Properties.TryGetValue(name, out _))
                    CompareRequestSchema($"{path}.{name}", nextProp, null, operation, isDeprecated, next.Required.Contains(name), current.Required.Contains(name));
            }
        }

        private void CompareResponseSchema(string path, OpenApiSchema current, OpenApiSchema next, OperationComparison operation, bool isDeprecated)
        {
            isDeprecated |= current.Deprecated;
            if (next == null)
            {
                ReportPathIssue(operation, path, isDeprecated, "no longer exists");
                return;
            }

            if (current.Type != next.Type)
            {
                ReportPathIssue(operation, path, isDeprecated, $"type does not match (before: {current.Type}, after: {next.Type})");
                return;
            }

            if (!current.Nullable && next.Nullable)
            {
                ReportPathIssue(operation, path, isDeprecated, "is now nullable");
                return;
            }

            if (current.Type == "array")
            {
                CompareResponseSchema($"{path}[]", current.Items, next.Items, operation, isDeprecated);
                return;
            }
            foreach (var (name, currentProp) in current.Properties)
            {
                var nextProp = next.Properties.TryGetValue(name, out var n) ? n : null;

                CompareResponseSchema($"{path}.{name}", currentProp, nextProp, operation, isDeprecated);
            }
        }

        private static void ReportPathIssue(OperationComparison operation, string path, bool isDeprecated, string message)
        {
            operation.ReportIssue($"{path} {message}", isDeprecated ? CompareStatus.Warning : CompareStatus.Error);
        }
    }
}