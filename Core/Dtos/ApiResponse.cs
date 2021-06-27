namespace Core.Dtos
{
    public class ApiResponse
    {
        public int resultCode { get; set; }
        public string refreshToken { get; set; }
        public string token { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T data { get; set; }
    }
    public class ApiResponse<T,K> : ApiResponse
    {
        public K info { get; set; }

        public T data { get; set; }
    }
}