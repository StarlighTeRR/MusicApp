
import React, { useState, useEffect } from 'react';
import { addToFavorites, markAsListened, rateTrack } from '../services/api';
import './TrackItem.css'; 


const RatingType = {
  NONE: 0,
  LIKE: 1,
  DISLIKE: 2
};

function TrackItem({ track }) {

  useEffect(() => {
  }, [track]);


  const [isFavorite, setIsFavorite] = useState(track.isFavorite || track.IsFavorite || false);
  const [isListened, setIsListened] = useState(track.isListened || track.IsListened || false);
  const [rating, setRating] = useState(track.rating || track.Rating || RatingType.NONE);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    console.log('isFavorite:', isFavorite, 'isListened:', isListened, 'rating:', rating, 'isLoading:', isLoading);
  }, [isFavorite, isListened, rating, isLoading]);

  const handleFavoriteClick = async () => {
    console.log('Favorite button clicked');
    try {
      setIsLoading(true);
      setError(null);
      

      const newIsFavorite = !isFavorite;
      

      await addToFavorites(track.id, newIsFavorite);
      
      setIsFavorite(newIsFavorite); 
      
    } catch (err) {
      setError('Не удалось изменить статус избранного');
      console.error(err);
      setIsFavorite(isFavorite);
    } finally {
      setIsLoading(false);
    }
  };

  const handleListenedClick = async () => {
    console.log('Listened button clicked');
    try {
      setIsLoading(true);
      setError(null);
      

      const newIsListened = !isListened;
      

      await markAsListened(track.id, newIsListened);
      
      setIsListened(newIsListened); 
      
    } catch (err) {
      setError('Не удалось изменить статус прослушивания');
      console.error(err);
      setIsListened(isListened); 
    } finally {
      setIsLoading(false);
    }
  };

  const handleRateClick = async (ratingValue) => {
    console.log('Rate button clicked with value:', ratingValue);
    try {
      setIsLoading(true);
      setError(null);
  
      // Toggle rating status
      let newRating = RatingType.NONE;
      if (rating !== ratingValue) {
        newRating = ratingValue;
      }

      await rateTrack(track.id, newRating);
      
      setRating(newRating); 
      
    } catch (err) {
      setError('Не удалось оценить трек');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <li className="track-item">
      <div className="track-info">
        <span className="track-name">{track.name || track.Name}</span>
        <span className="track-duration"> - {formatDuration(track.duration || track.Duration)}</span>
        {track.album && <span className="track-album"> ({track.album.title || track.Album?.Title})</span>}
      </div>
      
      <div className="track-actions">
        {/* Кнопки лайк/дизлайк */}
        <button 
          onClick={() => handleRateClick(RatingType.LIKE)}
          className={`like-button ${rating === RatingType.LIKE ? 'active' : ''}`}
          disabled={isLoading}
        >
          👍
        </button>
        <button 
          onClick={() => handleRateClick(RatingType.DISLIKE)}
          className={`dislike-button ${rating === RatingType.DISLIKE ? 'active' : ''}`}
          disabled={isLoading}
        >
          👎
        </button>
        
        {/* Кнопка "прослушано" */}
        <button 
          onClick={handleListenedClick}
          className={`listened-button ${isListened ? 'active' : ''}`}
          disabled={isLoading}
        >
          ✓
        </button>
        
        {/* Кнопка "избранное" */}
        <button 
          onClick={handleFavoriteClick}
          className={`favorite-button ${isFavorite ? 'active' : ''}`}
          disabled={isLoading}
        >
          ★
        </button>
      </div>
      
      {error && <div className="error-message">{error}</div>}
    </li>
  );
}

function formatDuration(duration) {
  // Если duration отсутствует, возвращаем '0:00'
  if (!duration) return '0:00';
  
  // Если duration уже в формате "MM:SS" (строка)
  if (typeof duration === 'string') {
    // Проверяем, соответствует ли строка формату MM:SS
    const timeRegex = /^([0-9]{1,2}):([0-5][0-9])$/;
    if (timeRegex.test(duration)) {
      return duration; // Возвращаем как есть, если формат верный
    }
    
    // Пробуем преобразовать строку в число (секунды)
    const seconds = parseInt(duration, 10);
    if (!isNaN(seconds)) {
      const minutes = Math.floor(seconds / 60);
      const remainingSeconds = seconds % 60;
      return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
    }
    
    return '0:00'; 
  }
  

  if (typeof duration === 'number') {
    const minutes = Math.floor(duration / 60);
    const remainingSeconds = Math.floor(duration % 60);
    return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
  }
  
  return '0:00'; 
}

export default TrackItem;
