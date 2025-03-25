import React, { useState, useEffect } from 'react';
import TrackItem from './TrackItem';
import './TrackList.css';

function TrackList({ tracks: initialTracks }) {
  const [tracks, setTracks] = useState([]);
  const [sortField, setSortField] = useState('name');
  const [sortDirection, setSortDirection] = useState('asc');
  
  // Пагинация
  const [currentPage, setCurrentPage] = useState(1);
  const [tracksPerPage, setTracksPerPage] = useState(5);
  const [totalPages, setTotalPages] = useState(1);
  const [displayedTracks, setDisplayedTracks] = useState([]);

  useEffect(() => {
    if (initialTracks && initialTracks.length > 0) {
      setTracks([...initialTracks]);
      setTotalPages(Math.ceil(initialTracks.length / tracksPerPage));
    }
  }, [initialTracks, tracksPerPage]);

  // Обновляем отображаемые треки при изменении сортировки или страницы
  useEffect(() => {
    const sortedTracks = sortTracks(tracks, sortField, sortDirection);
    const startIndex = (currentPage - 1) * tracksPerPage;
    const endIndex = startIndex + tracksPerPage;
    setDisplayedTracks(sortedTracks.slice(startIndex, endIndex));
  }, [tracks, sortField, sortDirection, currentPage, tracksPerPage]);

  // Функция для сортировки треков
  const sortTracks = (tracksToSort, field, direction) => {
    return [...tracksToSort].sort((a, b) => {
      let valueA, valueB;

      if (field === 'name') {
        valueA = (a.name || a.Name || '').toLowerCase();
        valueB = (b.name || b.Name || '').toLowerCase();
        return direction === 'asc' ? valueA.localeCompare(valueB) : valueB.localeCompare(valueA);
      } else if (field === 'duration') {
        valueA = getDurationInSeconds(a.duration || a.Duration);
        valueB = getDurationInSeconds(b.duration || b.Duration);
        return direction === 'asc' ? valueA - valueB : valueB - valueA;
      }
      return 0;
    });
  };

  // Функция для преобразования длительности в секунды
  const getDurationInSeconds = (duration) => {
    if (!duration) return 0;
    
    if (typeof duration === 'number') {
      return duration;
    }
    
    if (typeof duration === 'string') {
      // Если в формате MM:SS
      const timeRegex = /^([0-9]{1,2}):([0-5][0-9])$/;
      const match = duration.match(timeRegex);
      
      if (match) {
        const minutes = parseInt(match[1], 10);
        const seconds = parseInt(match[2], 10);
        return minutes * 60 + seconds;
      }
      
      // Если просто число в строке
      const seconds = parseInt(duration, 10);
      if (!isNaN(seconds)) {
        return seconds;
      }
    }
    
    return 0;
  };

  // Обработчик изменения поля сортировки
  const handleSortFieldChange = (field) => {
    if (sortField === field) {
      // Если поле то же самое, меняем направление сортировки
      const newDirection = sortDirection === 'asc' ? 'desc' : 'asc';
      setSortDirection(newDirection);
    } else {
      // Если поле новое, устанавливаем его и сортируем по возрастанию
      setSortField(field);
      setSortDirection('asc');
    }
    // Возвращаемся на первую страницу при изменении сортировки
    setCurrentPage(1);
  };

  // Получаем иконку для отображения направления сортировки
  const getSortIcon = (field) => {
    if (sortField !== field) return null;
    return sortDirection === 'asc' ? '↑' : '↓';
  };

  // Обработчики пагинации
  const goToPage = (page) => {
    if (page >= 1 && page <= totalPages) {
      setCurrentPage(page);
    }
  };

  const goToPreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  const goToNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  // Обработчик изменения количества треков на странице
  const handleTracksPerPageChange = (event) => {
    const newTracksPerPage = parseInt(event.target.value, 10);
    setTracksPerPage(newTracksPerPage);
    setTotalPages(Math.ceil(tracks.length / newTracksPerPage));
    setCurrentPage(1); // Сбрасываем на первую страницу
  };

  return (
    <div className="track-list-container">
      <div className="sort-controls">
        <button 
          className={`sort-button ${sortField === 'name' ? 'active' : ''}`}
          onClick={() => handleSortFieldChange('name')}
        >
          Название {getSortIcon('name')}
        </button>
        <button 
          className={`sort-button ${sortField === 'duration' ? 'active' : ''}`}
          onClick={() => handleSortFieldChange('duration')}
        >
          Длительность {getSortIcon('duration')}
        </button>
      </div>
      
      <ul className="track-list">
        {displayedTracks.length > 0 ? (
          displayedTracks.map(track => (
            <TrackItem key={track.id || track.Id} track={track} />
          ))
        ) : (
          <li className="no-tracks">Нет доступных треков</li>
        )}
      </ul>

      {/* Пагинация */}
      {tracks.length > 0 && (
        <div className="pagination-controls">
          <div className="pagination-info">
            <span>Показано {displayedTracks.length} из {tracks.length} треков</span>
            <select 
              value={tracksPerPage} 
              onChange={handleTracksPerPageChange}
              className="tracks-per-page"
            >
              <option value="5">5 на странице</option>
              <option value="10">10 на странице</option>
              <option value="20">20 на странице</option>
              <option value="50">50 на странице</option>
            </select>
          </div>
          
          <div className="pagination-buttons">
            <button 
              onClick={() => goToPage(1)} 
              disabled={currentPage === 1}
              className="pagination-button"
            >
              &laquo;
            </button>
            <button 
              onClick={goToPreviousPage} 
              disabled={currentPage === 1}
              className="pagination-button"
            >
              &lsaquo;
            </button>
            
            <span className="page-indicator">
              Страница {currentPage} из {totalPages}
            </span>
            
            <button 
              onClick={goToNextPage} 
              disabled={currentPage === totalPages}
              className="pagination-button"
            >
              &rsaquo;
            </button>
            <button 
              onClick={() => goToPage(totalPages)} 
              disabled={currentPage === totalPages}
              className="pagination-button"
            >
              &raquo;
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

export default TrackList;