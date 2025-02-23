import { useNavigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';

const Header = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const signOutHandler = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');

    navigate('/login');
  };

  return (
    <div className='header'>
      <nav>
        <ul>
          <li>
            <button
              className={location.pathname === '/' ? 'toggle-button' : ''}
              onClick={() => navigate('/')}
            >
              Главная
            </button>
          </li>
          <li>
            <button
              className={
                location.pathname === '/myEvents' ? 'toggle-button' : ''
              }
              onClick={() => navigate('/myEvents')}
            >
              Мои события
            </button>
          </li>
          <li>
            <button onClick={signOutHandler}>Выход</button>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Header;
