using System;
namespace DatingAPI.Errors
{
    public class APIException
    {
        public APIException(int statusCode,
            string message= null,
            string detail=null)
        {
            this.StatusCode     = statusCode;
            this.Message        = message;
            this.Detail         = detail;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
