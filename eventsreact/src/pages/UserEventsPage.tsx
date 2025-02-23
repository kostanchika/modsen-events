import { useEffect, useState } from 'react';
import { EventType } from '../types.ts';
import axios from '../axiosConfig.ts';
import EventsList from '../components/EventsList.tsx';
import Header from '../components/Header.tsx';

const UserEventsPage = () => {
  const [events, setEvents] = useState<EventType[]>([]);

  const getEvents = async () => {
    const response = await axios.get('/api/events/my');

    const events = response.data;
    return { events };
  };

  useEffect(() => {
    const fetchEvents = async () => {
      const { events } = await getEvents();
      setEvents(events);
    };

    fetchEvents();
  }, []);

  return (
    <>
      <Header />
      <div className='page-container'>
        <EventsList events={events} />
      </div>
    </>
  );
};

export default UserEventsPage;
