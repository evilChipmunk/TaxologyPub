﻿namespace Shared.Service
{
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}