namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId {  get; set; }

        //Navigation properties
        // These models basically tell the entity framework that
        // these Ids above are foreign keys for walk table
        // in order to create the neccesary relationships 
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
