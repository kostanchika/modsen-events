import { useNavigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';

const Header = () => {
  const [isAdmin, setIsAdmin] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const signOutHandler = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');

    navigate('/login');
  };

  useEffect(() => {
    const jwtToken = localStorage.getItem('accessToken');
    if (jwtToken) {
      const decodedToken: { role: string } = jwtDecode(jwtToken);
      const role = decodedToken.role;
      if (role === 'Admin') setIsAdmin(true);
    }
  }, []);

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
          {isAdmin && (
            <li>
              <button
                className={
                  location.pathname === '/admin' ? 'toggle-button' : ''
                }
                onClick={() => navigate('/admin')}
              >
                Админ-меню
              </button>
            </li>
          )}
          <li>
            <button onClick={signOutHandler}>Выход</button>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Header;
