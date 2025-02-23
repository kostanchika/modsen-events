import { BrowserRouter, Routes, Route } from 'react-router';
import CatalogPage from './pages/CatalogPage.tsx';
import LoginPage from './pages/LoginPage.tsx';
import RegisterPage from './pages/RegisterPage.tsx';
import { ToastContainer } from 'react-toastify';

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<CatalogPage />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/register' element={<RegisterPage />} />
        </Routes>
      </BrowserRouter>
      <ToastContainer />
    </>
  );
}

export default App;
