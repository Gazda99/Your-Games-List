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
        public const string GetUser = Base + "/{userIds}";
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
        public const string GetCompany = Base + "/{companyIds}";

        public static string GetLocation(int companyId) {
            return $"{GetCompanies}/{companyId}";
        }
    }

    public static class Genre {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/genres";

        public const string GetGenres = Base;
        public const string GetGenre = Base + "/{genreIds}";

        public static string GetLocation(int genreId) {
            return $"{GetGenres}/{genreId}";
        }
    }

    public static class GameMode {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/gameModes";

        public const string GetGameModes = Base;
        public const string GetGameMode = Base + "/{gameModeIds}";

        public static string GetLocation(int gameModeId) {
            return $"{GetGameModes}/{gameModeId}";
        }
    }

    public static class Platform {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/platforms";

        public const string GetPlatforms = Base;
        public const string GetPlatform = Base + "/{platformIds}";

        public static string GetLocation(int platformId) {
            return $"{GetPlatforms}/{platformId}";
        }
    }

    public static class PlayerPerspective {
        private const string Root = "api";
        private const string Base = Root + "/" + Version + "/playerPerspectives";

        public const string GetPlayerPerspectives = Base;
        public const string GetPlayerPerspective = Base + "/{playerPerspectiveIds}";

        public static string GetLocation(int playerPerspectiveId) {
            return $"{GetPlayerPerspectives}/{playerPerspectiveId}";
        }
    }
}
}