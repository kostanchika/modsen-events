import { EventType } from '../types.ts';
import EventListItem from './EventListItem.tsx';

const EventsList = (props: { events: EventType[] }) => {
  return (
    <div className='event-list'>
      {props.events.map((event, index) => (
        <EventListItem
          key={index}
          name={event.name}
          id={event.id}
          category={event.category}
          eventDateTime={event.eventDateTime}
          description={event.description}
          imagePath={event.imagePath}
          location={event.location}
          maximumParticipants={event.maximumParticipants}
          currentParticipants={event.currentParticipants}
        />
      ))}
      <h2 className='event-list__empty'>
        {props.events.length == 0 ? 'Событий не найдено' : ''}
      </h2>
    </div>
  );
};

export default EventsList;
