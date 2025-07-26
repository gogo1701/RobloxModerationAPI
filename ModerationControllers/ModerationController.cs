using Microsoft.AspNetCore.Mvc;
using RobloxModerationAPI.Models;
using RobloxModerationAPI.Services;

namespace RobloxModerationAPI.ModerationControllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModerationController : ControllerBase
    {
        private readonly MongoService _mongo;
        public ModerationController(MongoService mongo)
        {
            _mongo = mongo;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong!");
        }

        [HttpPost]
        public async Task<IActionResult> ModeratePlayer([FromBody] PlayerModeration data)
        {
            if (string.IsNullOrEmpty(data.UserId) || string.IsNullOrEmpty(data.Action))
            {
                return BadRequest("UserId and Action are required.");
            }

            Console.WriteLine($"Moderation Recieved: {data.Action} | User: {data.UserId} | Reason: {data.Reason}");
            await _mongo.InsertModerationAsync(data);

            return Ok(new
            {
                message = "Moderation action received successfully.",
                recieved = data
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLogs(string userId)
        {
            var logs = await _mongo.GetLogsByUserIdAsync(userId);
            if (logs.Count == 0)
            {
                return NotFound(new
                {
                    message = $"No logs found for UserId: {userId}"
                });
            }

            return Ok(logs);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUserLogs(string userId)
        {
            await _mongo.DeleteLogsByUserAsync(userId);
            return Ok(new
            {
                message = $"All logs for UserId: {userId} have been deleted."
            });
        }

        [HttpGet("user/{userId}/ban")]
        public async Task<IActionResult> CheckUserBan(string userId)
        {
            bool isBanned = await _mongo.UserHasBanAsync(userId);
            if (isBanned)
            {
                return Ok(new
                {
                    message = "true"
                });
            }
            else
            {
                return Ok(new
                {
                    message = "false"
                });
            }
        }
    }
}
