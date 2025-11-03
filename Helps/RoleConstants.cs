namespace Helps
{
    public static class RoleConstants
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Editor = "Editor";
        public const string Author = "Author";
        public const string Reporter = "Reporter";
        public const string Moderator = "Moderator";
        public const string User = "User";

        public static readonly string[] AllRoles =
        {
            SuperAdmin, Admin, Editor, Author, Reporter, Moderator, User
        };
    }
}