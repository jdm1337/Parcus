namespace Parcus.Api.Authentication.Permission
{
    public static class Permissions
    {
        public static class Users
        {
            public const string Add = "Permissions.Users.Add";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string AddToRole = "Permissions.Users.AddToRole";
            public const string GetUsers = "Permission.Users.GetUsers";
            public const string GetPermissions = "Permissions.Users.GetPermissions";
        }
        public static class Portfolio
        {
            public const string Add = "Permissions.Portfolio.Add";
            public const string Get = "Permissions.Portfolio.Get";

        }
    }
}
