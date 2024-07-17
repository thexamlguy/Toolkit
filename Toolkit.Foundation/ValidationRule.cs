namespace Toolkit.Foundation;

public class ValidationRule
{
    private readonly Func<bool>? syncValidation;
    private readonly Func<Task<bool>>? asyncValidation;

    public ValidationRule(Func<bool> validation,
        string message)
    {
        syncValidation = validation;
        Message = message;
    }

    public ValidationRule(Func<Task<bool>> validation,
        string message)
    {
        asyncValidation = validation;
        Message = message;
    }

    public ValidationRule(Func<bool> validation)
    {
        syncValidation = validation;
        Message = "";
    }

    public ValidationRule(Func<Task<bool>> validation)
    {
        asyncValidation = validation;
        Message = "";
    }

    public async Task<bool> ValidateAsync()
    {
        if (syncValidation is not null)
        {
            return syncValidation();
        }

        if (asyncValidation is not null)
        {
            return await asyncValidation();
        }

        return false;
    }

    public string Message { get; }
}