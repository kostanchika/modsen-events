import { BrowserRouter, Routes, Route } from 'react-router';
import CatalogPage from './pages/CatalogPage.tsx';
import LoginPage from './pages/LoginPage.tsx';
import RegisterPage from './pages/RegisterPage.tsx';
import { ToastContainer } from 'react-toastify';
import EventPage from './pages/EventPage.tsx';
import UserEventsPage from './pages/UserEventsPage.tsx';
import AdminPage from './pages/AdminPage.tsx';
import CreateEventPage from './pages/CreateEventPage.tsx';

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<CatalogPage />} />
          <Route path='/myEvents' element={<UserEventsPage />} />
          <Route path='/events/:id' element={<EventPage />} />
          <Route path='/admin' element={<AdminPage />} />
          <Route path='/create' element={<CreateEventPage />} />
          <Route path='/create/:id' element={<CreateEventPage />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/register' element={<RegisterPage />} />
        </Routes>
      </BrowserRouter>
      <ToastContainer />
    </>
  );
}

export default App;
