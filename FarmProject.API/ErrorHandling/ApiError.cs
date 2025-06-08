namespace FarmProject.API.ErrorHandling;

public class ApiError(string code, string message, object? details = null)
{
    public string Code => code;
    public string Message => message;
    public object? Details => details;
}
