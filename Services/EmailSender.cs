using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace COMP2139_ICE.Services;

public class EmailSender : IEmailSender
{
    private readonly string _sendGridApiKey;

    public EmailSender(IConfiguration configuration)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"]
                          ?? throw new ArgumentNullException(paramName: "SendGrid key is missing");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("joel.chandrapaul@georgebrown.ca", "PM Tool Default Sender");
            var to = new EmailAddress(email);
            var msg = MailHelper
                .CreateSingleEmail(from, to, subject, 
                    plainTextContent: "Welcome to PM Tool Inc", htmlContent: message);
                
            var response = await client.SendEmailAsync(msg);
                
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"An error occurred while sending email: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            throw;
        }
    }
}