import { useEffect, useState } from 'react';
import { EventType, GetEventParams } from '../types.ts';
import axios from '../axiosConfig.ts';
import EventsList from '../components/EventsList.tsx';
import PaginationButtons from '../components/PaginationButtons.tsx';
import Header from '../components/Header.tsx';
import Filters from '../components/Filters.tsx';

const CatalogPage = () => {
  const [events, setEvents] = useState<EventType[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [filters, setFilters] = useState<GetEventParams>({
    page: 1,
    pageSize: 12,
  });

  const getEvents = async (params: GetEventParams) => {
    params.page = currentPage;
    params.pageSize = params.pageSize || 12;
    const response = await axios.get('/api/events', {
      params,
    });

    const events = response.data;
    const totalPages = parseInt(response.headers['x-page-count'], 10);
    return { events, totalPages };
  };

  useEffect(() => {
    const fetchEvents = async () => {
      const { events, totalPages } = await getEvents(filters);
      setEvents(events);
      setTotalPages(totalPages);
    };

    fetchEvents();
  }, [filters, currentPage]);

  return (
    <>
      <Header />
      <div className='page-container'>
        <Filters filters={filters} setFilters={setFilters}></Filters>
        <PaginationButtons
          currentPage={currentPage}
          totalPages={totalPages}
          setPage={setCurrentPage}
        />
        <EventsList events={events} />
        <PaginationButtons
          currentPage={currentPage}
          totalPages={totalPages}
          setPage={setCurrentPage}
        />
      </div>
    </>
  );
};

export default CatalogPage;
