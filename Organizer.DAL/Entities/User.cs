namespace Organizer.DAL.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public override string IdColumnName => "UserId";

        public override string TableName => "Users";
    }
}