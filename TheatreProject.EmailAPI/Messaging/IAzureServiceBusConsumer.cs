﻿namespace TheatreProject.EmailAPI.Messaging;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}