namespace Shared.Service
{
    public interface IRabbitMQConfiguration
    {
        string HostName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}