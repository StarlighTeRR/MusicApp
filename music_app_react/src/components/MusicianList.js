import React from 'react';
import './MusicianList.css';

function MusicianList({ musicians, selectedMusician, onMusicianSelect }) {
  return (
    <ul className="musician-list">
      {musicians.map(musician => (
        <li
          key={musician.id}
          className={`musician-item ${selectedMusician && selectedMusician.id === musician.id ? 'selected' : ''}`}
          onClick={() => onMusicianSelect(musician)}
        >
          <div className="musician-name">{musician.name}</div>
          <div className="musician-genre">{musician.genre}</div>
        </li>
      ))}
    </ul>
  );
}

export default MusicianList;