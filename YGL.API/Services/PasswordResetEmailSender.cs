﻿using System;
using YGL.API.Helpers;
using YGL.API.Settings;

namespace YGL.API.Services {
public class PasswordResetEmailSender : EmailSender<PasswordResetEmailSender> {
    public PasswordResetEmailSender(PasswordResetEmailSettings confirmationEmailSettings) : base(
        confirmationEmailSettings) { }

    protected override string Subject(string toName) {
        string subject =
            $"Hello {toName}, you recently requested to reset your password for your 'Your Games List account'. Use the button below to change it.";
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