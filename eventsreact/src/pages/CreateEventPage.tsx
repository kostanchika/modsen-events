import Header from '../components/Header.tsx';
import { FormEventHandler } from 'react';
import { EventCategories } from '../types.ts';
import { getEventCategoryText } from '../helpers/category.ts';
import axios from '../axiosConfig.ts';
import { AxiosError } from 'axios';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const CreateEventPage = () => {
  const navigate = useNavigate();

  const generateCategories = () => {
    const options = [];
    for (let i = EventCategories.Music; i < EventCategories.Gaming; i++) {
      options.push(
        <option key={i} value={i} defaultValue={i}>
          {getEventCategoryText(i)}
        </option>
      );
    }
    return options;
  };

  const handleSubmit: FormEventHandler = async (event) => {
    event.preventDefault();

    const form = event.target as HTMLFormElement;
    const formData = new FormData(form);

    try {
      const response = await axios.post('/api/events', formData);
      toast.success('Событие успешно создано');
      const url = response.headers['location'];
      const id = url.match(/\/api\/Events\/(\d+)/)[1];
      navigate(`/events/${id}`);
    } catch (error) {
      if (!(error instanceof AxiosError)) return;
      if (!error.response) return;
      if (error.response.status === 400) {
        const validationErrors = error.response.data.errors;
        Object.keys(validationErrors).map((key) =>
          validationErrors[key].forEach((error: string) => {
            if (!/^[A-Za-z0-9 .,?!']+$/.test(error)) {
              toast.error(error);
            }
          })
        );
      }
    }
  };

  return (
    <>
      <Header />
      <div className='page-container'>
        <form onSubmit={handleSubmit} className='create-event-form'>
          <h2>Создать событие</h2>
          <div className='form-row'>
            <label>
              Название
              <input type='text' name='name' />
            </label>
            <label>
              Время проведения
              <input type='datetime-local' name='eventDateTime' />
            </label>
          </div>
          <div className='form-row'>
            <label>
              Место проведения <input type='text' name='location' />{' '}
            </label>
            <label>
              Категория
              <select name='category'>{generateCategories()}</select>
            </label>
          </div>
          <div className='form-row'>
            <label>
              Максимальное количество участников
              <input type='number' name='maximumParticipants' />
            </label>
            <label>
              Изображение <input type='file' name='image' />{' '}
            </label>
          </div>
          <label>
            Описание
            <textarea name='description' />
          </label>

          <button type='submit'>Создать</button>
        </form>
      </div>
    </>
  );
};

export default CreateEventPage;
