import { useNavigate } from 'react-router-dom';

const Header = () => {
  const navigate = useNavigate();

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
            <button onClick={() => navigate('/')}>Главная</button>
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
