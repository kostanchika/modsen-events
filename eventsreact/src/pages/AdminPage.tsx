import Header from '../components/Header.tsx';
import AdminMenuList from '../components/AdminMenuList.tsx';
import { useEffect, useState } from 'react';
import { EventType } from '../types.ts';
import axios from '../axiosConfig.ts';
import PaginationButtons from '../components/PaginationButtons.tsx';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const AdminPage = () => {
  const [events, setEvents] = useState<EventType[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const navigate = useNavigate();

  const getEvents = async () => {
    const response = await axios.get('/api/events', {
      params: {
        page: currentPage,
        pageSize: 100,
      },
    });

    const events = response.data;
    const totalPagesResponse = parseInt(response.headers['x-page-count'], 10);
    return { events, totalPagesResponse };
  };

  useEffect(() => {
    const fetchEvents = async () => {
      const { events, totalPagesResponse: totalPages } = await getEvents();
      setEvents(events);
      setTotalPages(totalPages);
    };

    fetchEvents();
  }, [currentPage]);

  useEffect(() => {
    const jwtToken = localStorage.getItem('accessToken');
    if (jwtToken) {
      const decodedToken: { role: string } = jwtDecode(jwtToken);
      const role = decodedToken.role;
      if (role !== 'Admin') navigate('/');
    } else {
      navigate('/');
    }
  }, []);

  return (
    <>
      <Header />
      <div className='page-container'>
        <h2>Админ-меню</h2>
        <button onClick={() => navigate('/create')}>Создать</button>
        <PaginationButtons
          currentPage={currentPage}
          totalPages={totalPages}
          setPage={setCurrentPage}
        />
        <AdminMenuList events={events} setEvents={setEvents} />
        <PaginationButtons
          currentPage={currentPage}
          totalPages={totalPages}
          setPage={setCurrentPage}
        />
      </div>
    </>
  );
};

export default AdminPage;
