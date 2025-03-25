// Services/MusicAppLogger.cs
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicAppApi.Models;
using System;
using System.Threading.Tasks;

namespace MusicAppApi.Services
{
    public class MusicAppLogger : IMusicAppLogger
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MusicAppLogger> _logger;

        public MusicAppLogger(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<MusicAppLogger> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task LogActionAsync(string actionType, string entityType, int entityId, string details = "")
        {
            var context = _httpContextAccessor.HttpContext;
            var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = context?.Request.Headers.UserAgent.ToString() ?? "Unknown";
            
           
            _logger.LogInformation(
                "Пользователь {IpAddress} выполнил действие {ActionType} для {EntityType} с ID {EntityId}. Детали: {Details}",
                ipAddress, actionType, entityType, entityId, details);
            

            var log = new UserActionLog
            {
                UserId = ipAddress, 
                ActionType = actionType,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Timestamp = DateTime.UtcNow
            };
            
            _context.UserActionLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogTrackRatedAsync(int trackId, RatingType rating)
        {
            await LogActionAsync("Rate", "Track", trackId, $"Rating: {rating}");
        }

        public async Task LogTrackAddedToFavoritesAsync(int trackId)
        {
            await LogActionAsync("AddToFavorites", "Track", trackId);
        }

        public async Task LogTrackRemovedFromFavoritesAsync(int trackId)
        {
            await LogActionAsync("RemoveFromFavorites", "Track", trackId);
        }

        public async Task LogTrackMarkedAsListenedAsync(int trackId)
        {
            await LogActionAsync("MarkAsListened", "Track", trackId);
        }
    }
}