namespace TripTracker.Services.AuthApi.Model.Dto
{
    public class ResponseDto
    {
        public Object? Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
    }
}
