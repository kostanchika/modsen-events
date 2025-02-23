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
      {props.events.length == 0 ? 'Событий не найдено' : ''}
    </div>
  );
};

export default EventsList;
