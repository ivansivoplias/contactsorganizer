namespace Organizer.Common.Entities
{
    public class SocialInfo : BaseEntity
    {
        public int ContactId { get; set; }

        public string AppName { get; set; }

        public string AppId { get; set; }

        public override string IdColumnName => "SocialInfoId";

        public override string TableName => "dbo.SocialInfo";
    }
}