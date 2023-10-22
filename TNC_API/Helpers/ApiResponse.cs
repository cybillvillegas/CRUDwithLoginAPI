namespace TNC_API.Helpers
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
            ErrorMessage = null;
            StatusCode = 200; // Default to OK status code
        }

        public ApiResponse(string errorMessage, int statusCode = 500)
        {
            Data = default; // Default value for T
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}
