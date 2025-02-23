import { FormEventHandler, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from '../axiosConfig.ts';
import { toast } from 'react-toastify';
import { AxiosError } from 'axios';

const LoginPage = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const navigate = useNavigate();

  const handleAuth: FormEventHandler = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post(`/api/auth/login`, { login, password });

      if (response.status === 200) {
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        toast.success('Успешный вход!');
        navigate('/');
      }
    } catch (error) {
      if (!(error instanceof AxiosError)) return;
      if (!error.response) return;
      if (error.response.status === 400) {
        const validationErrors = error.response.data.errors;
        Object.keys(validationErrors).map((key) =>
          validationErrors[key].forEach((error: string) => {
            toast.error(error);
          })
        );
      } else if (error.response.status === 404) {
        toast.error(error.response.data);
      } else {
        toast.error(`Ошибка: ${error.response.status}`);
      }
    }
  };

  return (
    <div className='login-form'>
      <h1>Авторизация</h1>
      <form onSubmit={handleAuth}>
        <label>
          Логин
          <input
            type='text'
            value={login}
            onChange={(e) => setLogin(e.target.value)}
          />
        </label>
        <label>
          Пароль
          <input
            type='password'
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </label>
        <button type='submit'>Авторизоваться</button>
        <button
          type='button'
          className='toggle-button'
          onClick={() => navigate('/register')}
        >
          Перейти к регистрации
        </button>
      </form>
    </div>
  );
};

export default LoginPage;
