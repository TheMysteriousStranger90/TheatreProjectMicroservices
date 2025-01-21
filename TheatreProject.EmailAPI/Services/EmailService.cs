using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TheatreProject.EmailAPI.Helpers;
using TheatreProject.EmailAPI.Models.DTOs;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Services;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> options)
    {
        _mailSettings = options.Value;
    }

    public async Task SendOrderConfirmationAsync(OrderConfirmationDto orderDetails)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.Name, _mailSettings.UserName));
            email.To.Add(MailboxAddress.Parse(orderDetails.CustomerEmail));
            email.Subject = $"Order Confirmation - #{orderDetails.OrderId}";

            var builder = new BodyBuilder();
            builder.HtmlBody = GetOrderConfirmationTemplate(orderDetails);
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
            await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error sending confirmation email: {ex.Message}");
        }
    }

    private string GetOrderConfirmationTemplate(OrderConfirmationDto order)
    {
        var items = string.Join("\n", order.OrderDetails.Select(item =>
            $@"<tr>
            <td>{item.PerformanceName}</td>
            <td>{item.SeatNumbers}</td>
            <td>{item.Quantity}</td>
            <td>${item.PricePerTicket:F2}</td>
            <td>${item.SubTotal:F2}</td>
        </tr>"
        ));

        return $@"
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; }}
            .container {{ padding: 20px; }}
            .header {{ background-color: #f8f9fa; padding: 20px; }}
            table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
            th, td {{ padding: 10px; text-align: left; border-bottom: 1px solid #ddd; }}
            .total {{ font-weight: bold; margin-top: 20px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h2>Order Confirmation</h2>
                <p>Dear {order.CustomerName},</p>
                <p>Thank you for your order! Here are your order details:</p>
            </div>

            <table>
                <thead>
                    <tr>
                        <th>Performance</th>
                        <th>Seats</th>
                        <th>Quantity</th>
                        <th>Original Price (Without Discount)</th>
                        <th>Original Subtotal (Without Discount)</th>
                    </tr>
                </thead>
                <tbody>
                    {items}
                </tbody>
            </table>

            <div class='total'>
                <p>Total Amount: ${order.TotalAmount:F2}</p>
            </div>

            <p>Order Date: {order.OrderDate:g}</p>
            <p>Order ID: {order.OrderId}</p>

            <p>If you have any questions, please don't hesitate to contact us.</p>
            <p>Best regards,<br>Theatre Team</p>
        </div>
    </body>
    </html>";
    }
}