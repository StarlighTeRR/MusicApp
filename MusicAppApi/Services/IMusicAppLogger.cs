// Services/IMusicAppLogger.cs
using MusicAppApi.Models;
using System.Threading.Tasks;

namespace MusicAppApi.Services
{
    public interface IMusicAppLogger
    {
        Task LogActionAsync(string actionType, string entityType, int entityId, string details = "");
        Task LogTrackRatedAsync(int trackId, RatingType rating);
        Task LogTrackAddedToFavoritesAsync(int trackId);
        Task LogTrackRemovedFromFavoritesAsync(int trackId);
        Task LogTrackMarkedAsListenedAsync(int trackId);
    }
}