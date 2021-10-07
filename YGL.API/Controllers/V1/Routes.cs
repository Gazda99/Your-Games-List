namespace YGL.API.Controllers.V1 {
public static class Routes {
    private const string Version = "v1";

    public static class Identity {
        private const string Root = "identity";
        private const string Base = Root + "/" + Version;

        public const string Register = Base + "/register";
        public const string Login = Base + "/login";
        public const string Refresh = Base + "/refresh";
        public const string EmailConfirmation = Base + "/emailconfirmation";
        public const string EmailConfirmationResendEmail = Base + "/emailconfirmationresendemail";
        public const string ResetPasswordSendEmail = Base + "/passwordresetsendemail";
        public const string ResetPasswordConfirmation = Base + "/passwordresetconfirmation";
        public const string ResetPassword = Base + "/passwordreset";
    }

    public static class User {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/users";

        public const string GetUsers = Base;
        public const string GetUser = Base + "/{userId:long}";
        public const string UpdateUser = Base + "/{userId:long}";
        public const string DeleteUser = Base + "/{userId:long}";

        public static string GetLocation(long userId) {
            return $"{GetUsers}/{userId}";
        }
    }

    public static class Company {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/companies";

        public const string GetCompanies = Base;
        public const string GetCompany = Base + "/{companyId:int}";

        public static string GetLocation(int companyId) {
            return $"{GetCompanies}/{companyId}";
        }
    }
}
}