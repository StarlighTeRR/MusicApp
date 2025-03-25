import React from 'react';
import './AlbumList.css';

function AlbumList({ albums, selectedAlbumId, onAlbumSelect }) {
  return (
    <ul className="album-list">
      {albums.map(album => (
        <li
          key={album.id}
          className={`album-item ${selectedAlbumId === album.id ? 'selected' : ''}`}
          onClick={() => onAlbumSelect(album.id)}
        >
          <div className="album-title">{album.title}</div>
        </li>
      ))}
    </ul>
  );
}

export default AlbumList;