using MongoDB.Driver;
using RobloxModerationAPI.Models;

namespace RobloxModerationAPI.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<PlayerModeration> _collection;

        public MongoService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _collection = database.GetCollection<PlayerModeration>(config["MongoDB:CollectionName"]);
        }

        public async Task InsertModerationAsync(PlayerModeration moderation)
        {
            await _collection.InsertOneAsync(moderation);
        }

        public async Task<List<PlayerModeration>> GetLogsByUserIdAsync(string userId)
        {
            return await _collection.Find(l => l.UserId == userId).ToListAsync();
        }

        public async Task DeleteLogsByUserAsync(string userId)
        {
            await _collection.DeleteManyAsync(l => l.UserId == userId);
        }

        public async Task<bool> UserHasBanAsync(string userId)
        {
            List<PlayerModeration> logs = new List<PlayerModeration>();
            logs = await GetLogsByUserIdAsync(userId);

            if (logs.Where(l => l.Action == "ban").ToList().Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
