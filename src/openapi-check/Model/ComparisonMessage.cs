namespace OpenApiCheck.Model
{
    public class ComparisonMessage
    {
        public CompareStatus Status { get; }
        public string Message { get; }

        public ComparisonMessage(CompareStatus status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}