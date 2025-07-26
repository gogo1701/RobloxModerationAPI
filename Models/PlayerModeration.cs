using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RobloxModerationAPI.Models
{
    public class PlayerModeration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; } // Unique identifier for the moderation record
        public string UserId { get; set; } // Roblox UserID who has been moderated
        public string Action { get; set; } // Action taken (e.g., "Ban", "Warning")
        public string Reason { get; set; } // Reason for the moderation action
        public DateTime Timestamp { get; set; } // When the moderation action was taken

    }
}
