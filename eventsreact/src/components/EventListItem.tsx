import { EventType } from '../types.ts';
import { getEventCategoryText } from '../helpers/category.ts';
import { formatDate } from '../helpers/date.ts';
import { useNavigate } from 'react-router';
import { MouseEventHandler } from 'react';

const EventListItem = (props: EventType) => {
  const navigate = useNavigate();

  const handleClick: MouseEventHandler = () => {
    navigate(`/events/${props.id}`);
  };

  const remainTickets = props.maximumParticipants - props.currentParticipants;

  return (
    <div className='event-list__item' onClick={handleClick}>
      <img src={'http://localhost:8080' + props.imagePath} />
      <p>{props.name}</p>
      <p>{getEventCategoryText(props.category)}</p>
      {new Date(props.eventDateTime) < new Date() ? (
        <p style={{ color: 'red' }}>Завершено</p>
      ) : (
        <>
          <p style={{ color: remainTickets == 0 ? 'red' : '' }}>
            {remainTickets == 0
              ? 'Свободных мест нет'
              : `Осталось мест: ${remainTickets}`}
          </p>
          <p>{formatDate(props.eventDateTime)}</p>
        </>
      )}
    </div>
  );
};

export default EventListItem;
