import { AdminMenuListProps } from '../types.ts';
import axios from '../axiosConfig.ts';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const AdminMenuList = (props: AdminMenuListProps) => {
  const navigate = useNavigate();

  const handleClick = (id: number) => {
    navigate(`/events/${id}`);
  };

  const handleDeleteClick = async (id: number) => {
    if (!confirm('Вы уверены?')) return;
    try {
      await axios.delete(`/api/events/${id}`);
      toast.success('Элемент успешно удалён');
      const newEvents = props.events.filter((event) => event.id !== id);
      props.setEvents(newEvents);
    } catch {
      toast.error('Не удалось удалить событие');
    }
  };

  return (
    <div className='event-list-admin'>
      {props.events.map((event, index) => (
        <div key={index} className='event-list-admin__item'>
          <img src={'http://localhost:8080' + event.imagePath} />
          <p>{event.name}</p>
          <div className='event-list-admin__item-buttons'>
            <button onClick={() => handleClick(event.id)}>Открыть</button>
            <button onClick={() => navigate(`/create/${event.id}`)}>
              Изменить
            </button>
            <button
              style={{ background: 'red' }}
              onClick={() => handleDeleteClick(event.id)}
            >
              Удалить
            </button>
          </div>
        </div>
      ))}
      <h2 className='event-list__empty'>
        {props.events.length == 0 ? 'Событий не найдено' : ''}
      </h2>
    </div>
  );
};

export default AdminMenuList;
