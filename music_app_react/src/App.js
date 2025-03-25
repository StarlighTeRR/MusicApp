import React, { useState, useEffect } from 'react';
import { getMusicians, getTracks } from './services/api';
import MusicianList from './components/MusicianList';
import AlbumList from './components/AlbumList';
import TrackList from './components/TrackList';

import './App.css';

function App() {
  const [musicians, setMusicians] = useState([]);
  const [selectedMusician, setSelectedMusician] = useState(null);
  const [selectedAlbumId, setSelectedAlbumId] = useState(null);
  const [tracks, setTracks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [tracksLoading, setTracksLoading] = useState(false);
  const [tracksError, setTracksError] = useState(null);

  // Загрузка музыкантов при монтировании компонента
  useEffect(() => {
    const fetchMusicians = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getMusicians();
        setMusicians(data);
      } catch (err) {
        console.error('Ошибка при загрузке музыкантов:', err);
        setError('Не удалось загрузить музыкантов');
      } finally {
        setLoading(false);
      }
    };
    fetchMusicians();
  }, []);

  // Загрузка треков при выборе альбома
  useEffect(() => {
    if (selectedAlbumId) {
      const fetchTracks = async () => {
        setTracksLoading(true);
        setTracksError(null);
        try {
          const data = await getTracks(selectedAlbumId);
          setTracks(data);
        } catch (err) {
          console.error(`Ошибка при загрузке треков для albumId=${selectedAlbumId}:`, err);
          setTracksError('Не удалось загрузить треки');
        } finally {
          setTracksLoading(false);
        }
      };
      fetchTracks();
    } else {
      setTracks([]);
    }
  }, [selectedAlbumId]);

  const handleMusicianSelect = (musician) => {
    setSelectedMusician(musician);
    setSelectedAlbumId(null); // Сбрасываем выбранный альбом
  };

  const handleAlbumSelect = (albumId) => {
    setSelectedAlbumId(albumId);
  };

  return (
    <div className="app">
      <div className="musicians-container">
        <h2>Музыканты</h2>
        {loading && <div className="loading">Загрузка музыкантов...</div>}
        {error && <div className="error">{error}</div>}
        {!loading && !error && (
          <MusicianList
            musicians={musicians}
            selectedMusician={selectedMusician}
            onMusicianSelect={handleMusicianSelect}
          />
        )}
      </div>
      
      <div className="albums-container">
        <h2>Альбомы</h2>
        {selectedMusician ? (
          selectedMusician.albums && selectedMusician.albums.length > 0 ? (
            <AlbumList
              albums={selectedMusician.albums}
              selectedAlbumId={selectedAlbumId}
              onAlbumSelect={handleAlbumSelect}
            />
          ) : (
            <div>У этого музыканта нет альбомов</div>
          )
        ) : (
          <div>Выберите музыканта для просмотра альбомов</div>
        )}
      </div>
      
      <div className="tracks-container">
        <h2>Треки</h2>
        {tracksLoading && <div className="loading">Загрузка треков...</div>}
        {tracksError && <div className="error">{tracksError}</div>}
        {!tracksLoading && !tracksError && (
          selectedAlbumId ? (
            <TrackList tracks={tracks} />
          ) : (
            <div>Выберите альбом для просмотра треков</div>
          )
        )}
      </div>
    </div>
  );
}

export default App;