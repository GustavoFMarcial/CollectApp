using CollectApp.Services;

namespace CollectAppTests.Builders;

public class OperationResultBuilder
{
    private bool _success;
    private string? _message;

    public OperationResultBuilder WithSuccess(bool success)
    {
        _success = success;
        return this;
    }

    public OperationResultBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public OperationResult Build()
    {
        return new OperationResult
        {
            Success = _success,
            Message = _message,
        };
    }
}