namespace API.Errors
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        // Uso del modificador 'new' para ocultar la propiedad Message heredada
        public new string Message { get; set; }

        public string Details { get; set; }

        public ApiException(int statusCode, string message, string details = "")
            : base(message)
        {
            StatusCode = statusCode;
            Message = message ?? throw new ArgumentNullException(nameof(message)); 
            Details = details ?? "No details provided";
        }
    }
}
