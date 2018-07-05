namespace Shared.DTO
{
    public class DeleteResponse : BaseResponse
    {
        public DeleteResponse() : base(true, ResponseAction.Deleted)
        {
        }

        public DeleteResponse(bool success, ResponseAction responseAction) : base(success, responseAction)
        {

        }
    }
}