import Header from '../components/Header.tsx';
import { useParams } from 'react-router';
import { useEffect, useState } from 'react';
import axios from './../axiosConfig.ts';
import { EventType } from '../types.ts';
import { formatDate } from '../helpers/date.ts';
import { toast } from 'react-toastify';
import { getEventCategoryText } from '../helpers/category.ts';

const EventPage = () => {
  const { id } = useParams();
  const [event, setEvent] = useState<EventType>();
  const [remainTickets, setRemainTickets] = useState(0);
  const [isParticipating, setIsParticipating] = useState(false);
  const [isOver, setIsOver] = useState(false);

  const handleParticipate = () => {
    const register = async () => {
      if (!isParticipating) {
        await axios.put(`/api/events/${id}/register`);
        toast.success('Вы успешно записались на событие');
        setRemainTickets(remainTickets - 1);
      } else {
        await axios.put(`/api/events/${id}/unregister`);
        toast.success('Вы успешно отписались от события');
        setRemainTickets(remainTickets + 1);
      }
      setIsParticipating(!isParticipating);
    };

    register();
  };

  useEffect(() => {
    const getEvent = async () => {
      const response = await axios.get(`/api/events/${id}`);
      setEvent(response.data);
      setIsOver(new Date(response.data.eventDateTime) < new Date());
      const isParticipatingResponse = await axios.get('/api/events/my');
      for (const responseEvent of isParticipatingResponse.data) {
        if (responseEvent.id == id) {
          setIsParticipating(true);
          return;
        }
      }
    };

    getEvent();
  }, []);

  useEffect(
    () =>
      event &&
      setRemainTickets(event.maximumParticipants - event.currentParticipants),
    [event]
  );

  return (
    <>
      <Header />
      {event && (
        <div className='page-container'>
          <div className='single-event'>
            <h2>
              {event.name}({getEventCategoryText(event.category)})
            </h2>
            <img src={'http://localhost:8080' + event.imagePath} />
            {!isOver && (
              <div className='single-event__participate'>
                <p style={{ color: remainTickets == 0 ? 'red' : '' }}>
                  {remainTickets == 0
                    ? 'Свободные мест нет'
                    : `Осталось мест: ${remainTickets}`}
                </p>
                <button
                  disabled={remainTickets === 0 && !isParticipating}
                  onClick={handleParticipate}
                >
                  {isParticipating ? 'Отписаться' : 'Записаться'}
                </button>
              </div>
            )}
            {isOver && <h2 style={{ color: 'red' }}>Завершено</h2>}
            <p>{formatDate(event.eventDateTime)}</p>
            <p>{event.location}</p>
            <p>{event.description}</p>
          </div>
        </div>
      )}
      {!event && (
        <div className='page-container'>
          <h2 className='event-list__empty'>Событие не найдено</h2>
        </div>
      )}
    </>
  );
};

export default EventPage;
