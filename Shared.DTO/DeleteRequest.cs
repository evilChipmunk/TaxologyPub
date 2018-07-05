using System;

namespace Shared.DTO
{
    public class DeleteRequest
    {
        public DeleteRequest(Guid id)
        {
            this.Id = id;
        }
        public Guid Id { get; set; }
    }
}