import Header from '../components/Header.tsx';
import { FormEventHandler, useEffect, useState } from 'react';
import { EventCategories, EventType } from '../types.ts';
import { getEventCategoryText } from '../helpers/category.ts';
import axios from '../axiosConfig.ts';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router';

const CreateEventPage = () => {
  const [event, setEvent] = useState<EventType>({
    id: 0,
    name: '',
    eventDateTime: '',
    location: '',
    category: 0,
    maximumParticipants: 0,
    currentParticipants: 0,
    description: '',
    imagePath: '',
  });
  const [category, setCategory] = useState<number | undefined>(0);
  const navigate = useNavigate();
  const { id } = useParams();

  const generateCategories = () => {
    const options = [];
    for (let i = EventCategories.Music; i < EventCategories.Gaming; i++) {
      options.push(
        <option key={i} value={i}>
          {getEventCategoryText(i)}
        </option>
      );
    }
    return options;
  };

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement
    >
  ) => {
    const { name, value } = e.target;
    setEvent((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit: FormEventHandler = async (e) => {
    e.preventDefault();

    const form = e.target as HTMLFormElement;
    const formData = new FormData(form);
    const localTime = formData.get('eventDateTime');
    formData.set('eventDateTime', new Date(localTime as string).toISOString());
    if (event) {
      await axios.put(`/api/events/${event.id}`, formData);
      toast.success('Событие успешно изменено');
      navigate(`/events/${event.id}`);
    } else {
      const response = await axios.post('/api/events', formData);
      toast.success('Событие успешно создано');
      const url = response.headers['location'];
      const id = url.match(/\/api\/Events\/(\d+)/)[1];
      navigate(`/events/${id}`);
    }
  };

  useEffect(() => {
    const getEvent = async () => {
      if (id) {
        const response = await axios.get(`/api/events/${id}`);
        setEvent(response.data);
        setCategory(response.data.category);
      }
    };

    getEvent();
  }, [id]);

  useEffect(() => {
    if (event) {
      setCategory(event.category);
    }
  }, [event]);

  return (
    <>
      <Header />
      <div className='page-container'>
        <form onSubmit={handleSubmit} className='create-event-form'>
          <h2>{id ? 'Изменить событие' : 'Создать событие'}</h2>
          <div className='form-row'>
            <label>
              Название
              <input
                type='text'
                name='name'
                value={event.name}
                onChange={handleChange}
                disabled={event.id !== 0}
              />
            </label>
            <label>
              Время проведения
              <input
                type='datetime-local'
                name='eventDateTime'
                value={
                  event.eventDateTime &&
                  new Date(
                    new Date(event.eventDateTime).getTime() -
                      new Date(event.eventDateTime).getTimezoneOffset() * 60000
                  )
                    .toISOString()
                    .slice(0, 16)
                }
                onChange={handleChange}
              />
            </label>
          </div>
          <div className='form-row'>
            <label>
              Место проведения
              <input
                type='text'
                name='location'
                value={event.location}
                onChange={handleChange}
              />
            </label>
            <label>
              Категория
              <select name='category' value={category} onChange={handleChange}>
                {generateCategories()}
              </select>
            </label>
          </div>
          <div className='form-row'>
            <label>
              Максимальное количество участников
              <input
                type='number'
                name='maximumParticipants'
                value={event.maximumParticipants}
                onChange={handleChange}
              />
            </label>
            <label>
              Изображение
              <input type='file' name='image' />
            </label>
          </div>
          <label>
            Описание
            <textarea
              name='description'
              value={event.description}
              onChange={handleChange}
            />
          </label>

          <button type='submit'>{event ? 'Изменить' : 'Создать'}</button>
        </form>
      </div>
    </>
  );
};

export default CreateEventPage;
