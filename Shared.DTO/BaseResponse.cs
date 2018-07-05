namespace Shared.DTO
{
    public class BaseResponse
    {
        public BaseResponse()
        {

        }
        public BaseResponse(bool success, ResponseAction responseAction)
        {
            IsSuccess = success;
            this.ResponseAction = responseAction;
        }

        public ResponseAction ResponseAction { get; set; }
        public string RequestMethod { get; set; }
        public object RequestParams { get; set; }
        public string RequestUri { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class CreatedResponse : BaseResponse
    {
        public CreatedResponse()
        {

        }

        public CreatedResponse(string link) : base(true, ResponseAction.Created)
        {
            RequestUri = link;
        }

        public CreatedResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        { 
        }
    }
}