namespace Parcus.Domain.Permission
{
    public static class Permissions
    {
        public static class Account
        {
            public const string Base = "Permissions.Account.Base";
        }
        public static class Users
        {
            public const string Add = "Permissions.Users.Add";
            public const string Update = "Permissions.Users.Update";
            public const string Delete = "Permissions.Users.Delete";
            public const string AddToRole = "Permissions.Users.AddToRole";
            public const string RemoveFromRole = "Permissions.Users.RemoveFromRole";
            public const string GetUsers = "Permissions.Users.GetUsers";
            public const string GetUser = "Permissions.Users.GetUser";
            public const string GetPermissions = "Permissions.Users.GetPermissions";
        }
        public static class Portfolios
        {
            public const string Add = "Permissions.Portfolios.Add";
            public const string Get = "Permissions.Portfolios.Get";
            public const string GetInstruments = "Permissions.Portfolios.GetInstruments";
        }
        public static class Roles
        {
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            
            public const string GetRole = "Permissions.Roles.GetRole";
            public const string GetRoles = "Permissions.Roles.GetRoles";
            public const string AddPermission = "Permissions.Roles.AddPermission";
            public const string GetPermissions = "Permissions.Roles.GetPermissions";
            public const string DeletePermission = "Permissions.Roles.DeletePermission";
        }
        public static class Jwt
        {
            public const string RevokeAccessToken = "Permissions.Users.RevokeAccessToken";
        }
    }
}
