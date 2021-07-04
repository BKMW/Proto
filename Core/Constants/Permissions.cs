using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class Permissions
    {

        #region training
        // no use
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermissions;
        }
        #endregion region

        #region Permissions
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class Auth
        {
            public const string View = "Permissions.Auth.View";
            public const string Create = "Permissions.Auth.Create";
            public const string Edit = "Permissions.Auth.Edit";
            public const string Delete = "Permissions.Auth.Delete";
        }
        #endregion 
    }
}