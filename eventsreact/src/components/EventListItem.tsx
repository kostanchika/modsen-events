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
      <p style={{ color: remainTickets == 0 ? 'red' : '' }}>
        Осталось мест: {remainTickets}
      </p>
      <p>{formatDate(props.eventDateTime)}</p>
      <p>{props.location}</p>
    </div>
  );
};

export default EventListItem;
