using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using YGL.API.Exceptions;
using YGL.API.Settings;

namespace YGL.API.Services; 

public abstract class EmailSender<T> {
    public string BodySubType { get; set; } = "plain";
    public bool UseSsl { get; set; } = true;
    public int RandomUrlSize { get; set; } = 64;

    private readonly string _fromAddress;
    private readonly string _fromName;
    private readonly string _username;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;

    protected EmailSender(IEmailSettings confirmationEmailSettings) {
        _fromName = confirmationEmailSettings.Name;
        _fromAddress = confirmationEmailSettings.Address;
        _username = confirmationEmailSettings.Username;
        _password = confirmationEmailSettings.Password;
        _host = confirmationEmailSettings.Host;
        _port = confirmationEmailSettings.Port;
    }

    public async Task<string> SendEmailAndGetUrlAsync(string toName, string toAddress, long userId) {
        try {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));
            message.Subject = Subject(toName);

            var pureUrl = GenerateUrl(RandomUrlSize);
            var urlWithUserId = $"{userId.ToString()}_{pureUrl}";

            message.Body = new TextPart(BodySubType) { Text = Body(toName, urlWithUserId) };

            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, UseSsl);

            // Note: only needed if the SMTP server requires authentication
            await client.AuthenticateAsync(_username, _password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return pureUrl;
        }
        catch (MailKit.Security.AuthenticationException) {
            throw new CannotSendEmailException();
        }
    }

    protected abstract string Subject(string toName);

    protected abstract string Body(string toName, string url);

    protected abstract string GenerateUrl(int size);
}