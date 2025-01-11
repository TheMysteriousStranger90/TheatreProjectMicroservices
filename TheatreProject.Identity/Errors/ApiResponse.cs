namespace TheatreProject.Identity.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string message = "")
    {
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? GetDefaultMessageForStatusCode(statusCode) : message;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "A bad request, you have made",
            401 => "Authorized, you are not",
            404 => "Resource found, it was not",
            500 => "An error occurred...",
            _ => "Unknown status code"
        };
    }
}