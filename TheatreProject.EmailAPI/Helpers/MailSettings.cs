﻿namespace TheatreProject.EmailAPI.Helpers;

public class MailSettings
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
}