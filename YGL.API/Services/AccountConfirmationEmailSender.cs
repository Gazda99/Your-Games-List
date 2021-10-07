using System;
using YGL.API.Helpers;
using YGL.API.Settings;

namespace YGL.API.Services {
public class AccountConfirmationEmailSender : EmailSender<AccountConfirmationEmailSender> {
    public AccountConfirmationEmailSender(ConfirmationEmailSettings confirmationEmailSettings) :
        base(confirmationEmailSettings) { }

    protected override string Subject(string toName) {
        string subject = $"Hello {toName}, confirm your email address for newly created account in Your Games List";
        return subject;
    }

    protected override string Body(string toName, string url) {
        string body = $"{url}";
        return body;
    }

    protected override string GenerateUrl(int size) {
        string randomUrl = Convert.ToBase64String(RngHelper.GenerateRandomByteArray(size));
        return randomUrl;
    }
}
}