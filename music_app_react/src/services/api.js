
// Базовый URL API сервера
const API_BASE_URL = 'http://localhost:5298/api'; 

/**
 * Получение списка всех музыкантов с их альбомами
 * @returns {Promise<Array>} Массив музыкантов
 */

export const RatingType = {
  // Примечание: эти значения должны точно соответствовать значениям перечисления RatingType на сервере
  NONE: 0,
  LIKE: 1,
  DISLIKE: 2,
  // Добавьте другие значения, если они есть в вашем перечислении RatingType
};

/**
 * Получение названия рейтинга по его значению
 * @param {number} rating - Значение рейтинга
 * @returns {string} Название рейтинга
 */
export function getRatingName(rating) {
  switch (rating) {
    case RatingType.NONE:
      return 'Нет оценки';
    case RatingType.LIKE:
      return 'Нравится';
    case RatingType.DISLIKE:
      return 'Не нравится';
    default:
      return 'Неизвестная оценка';
  }
}

/**
 * Проверка, является ли рейтинг положительным (лайк)
 * @param {number} rating - Значение рейтинга
 * @returns {boolean} true, если рейтинг положительный
 */
export function isPositiveRating(rating) {
  return rating === RatingType.LIKE;
}

/**
 * Проверка, является ли рейтинг отрицательным (дизлайк)
 * @param {number} rating - Значение рейтинга
 * @returns {boolean} true, если рейтинг отрицательный
 */
export function isNegativeRating(rating) {
  return rating === RatingType.DISLIKE;
}

export async function getMusicians() {
  try {
    const response = await fetch(`${API_BASE_URL}/Musician`);
    
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    
    const data = await response.json();
    console.log('Исходные данные музыкантов от сервера:', data);
    
    // Проверяем формат данных и преобразуем их
    let musicians = [];
    if (data && data.$values) {
      musicians = data.$values;
    } else if (data && data.$id && data.$values && Array.isArray(data.$values)) {
      // Если данные имеют формат { $id: '1', $values: [...] }
      musicians = data.$values;
    } else if (Array.isArray(data)) {
      // Если данные уже являются массивом
      musicians = data;
    } else {
      // Если это что-то другое, возвращаем пустой массив
      console.warn('Получены данные неожиданного формата:', data);
      return [];
    }

    // Получаем альбомы для каждого музыканта
    const albums = await getAlbums();
    
    // Связываем музыкантов с их альбомами
    return musicians.map(musician => {
      const musicianAlbums = albums.filter(album => album.artistId === musician.id);
      return {
        ...musician,
        albums: musicianAlbums
      };
    });
  } catch (error) {
    console.error('Ошибка при получении музыкантов:', error);
    // Возвращаем пустой массив в случае ошибки
    return [];
  }
}

/**
 * Получение списка всех альбомов
 * @returns {Promise<Array>} Массив альбомов
 */
export async function getAlbums() {
  try {
    const response = await fetch(`${API_BASE_URL}/Album`);
    
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    
    const data = await response.json();
    console.log('Исходные данные альбомов от сервера:', data);
    
    // Проверяем формат данных и преобразуем их
    if (data && data.$values) {
      return data.$values;
    } else if (data && data.$id && data.$values && Array.isArray(data.$values)) {
      // Если данные имеют формат { $id: '1', $values: [...] }
      return data.$values;
    } else if (Array.isArray(data)) {
      // Если данные уже являются массивом
      return data;
    } else {
      // Если это что-то другое, возвращаем пустой массив
      console.warn('Получены данные альбомов неожиданного формата:', data);
      return [];
    }
  } catch (error) {
    console.error('Ошибка при получении альбомов:', error);
    // Возвращаем пустой массив в случае ошибки
    return [];
  }
}

/**
 * Получение списка треков для конкретного альбома
 * @param {number} albumId - ID альбома
 * @returns {Promise<Array>} Массив треков
 */
export async function getTracks(albumId) {
  try {
    const response = await fetch(`${API_BASE_URL}/Track?albumId=${albumId}`);
    
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    
    const data = await response.json();
    console.log('Полученные треки:', data);
    
    // Обрабатываем данные в том же формате, что и другие функции
    if (data && data.$values) {
      return data.$values;
    } else if (data && data.$id && data.$values && Array.isArray(data.$values)) {
      return data.$values;
    } else if (Array.isArray(data)) {
      return data;
    } else {
      console.warn('Получены данные треков неожиданного формата:', data);
      return [];
    }
  } catch (error) {
    console.error(`Ошибка при получении треков для albumId=${albumId}:`, error);
    // Возвращаем пустой массив в случае ошибки
    return [];
  }
}

/**
 * Добавление/удаление трека из избранного
 * @param {number} trackId - ID трека
 * @param {boolean} isFavorite - true для добавления, false для удаления
 * @returns {Promise<Object>} Результат операции
 */
export async function addToFavorites(trackId, isFavorite) {
  try {
    const method = isFavorite ? 'POST' : 'DELETE';
    const url = `${API_BASE_URL}/Track/${trackId}/favorite`;

    const response = await fetch(url, {
      method: method,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error('Ошибка при изменении статуса избранного:', error);
    throw error;
  }
}

/**
 * Отметка/снятие отметки трека как прослушанного
 * @param {number} trackId - ID трека
 * @param {boolean} isListened - true для отметки, false для снятия отметки
 * @returns {Promise<Object>} Результат операции
 */
export async function markAsListened(trackId, isListened) {
  try {
    const response = await fetch(`${API_BASE_URL}/Track/${trackId}/listened`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ isListened }), // Send boolean value
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error('Ошибка при изменении статуса прослушивания:', error);
    throw error;
  }
}

/**
 * Оценка трека (установка рейтинга)
 * @param {number} trackId - ID трека
 * @param {number} rating - Значение рейтинга 
 * @returns {Promise<Object>} Результат операции
 */
export async function rateTrack(trackId, rating) {
  try {
    // Проверяем, что рейтинг является числом
    if (typeof rating !== 'number' || !Number.isInteger(rating)) {
      throw new Error('Рейтинг должен быть целым числом');
    }
    
    const response = await fetch(`${API_BASE_URL}/Track/${trackId}/rate`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ rating }),
    });
    
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`HTTP error! Status: ${response.status}. ${errorText}`);
    }
    
    return await response.json();
  } catch (error) {
    console.error('Ошибка при оценке трека:', error);
    throw error;
  }
}
