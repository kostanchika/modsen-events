import { FormEventHandler, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import axios from '../axiosConfig.ts';

const RegisterPage = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');
  const [lastName, setLastName] = useState('');
  const [birthDateTime, setBirthDateTime] = useState('');
  const [email, setEmail] = useState('');

  const navigate = useNavigate();

  const handleRegister: FormEventHandler = async (e) => {
    e.preventDefault();

    const response = await axios.post(`/api/auth/register`, {
      login,
      password,
      name,
      lastName,
      birthDateTime,
      email,
    });

    if (response.status === 200) {
      localStorage.setItem('accessToken', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      toast.success('Успешный вход!');
      navigate('/');
    }
  };

  return (
    <div className='register-form'>
      <h1>Регистрация</h1>
      <form onSubmit={handleRegister}>
        <div className='form-row'>
          <label>
            Логин
            <input
              type='text'
              placeholder='myLogin'
              value={login}
              onChange={(e) => setLogin(e.target.value)}
            />
          </label>
          <label>
            Пароль
            <input
              type='password'
              placeholder='veryStrongPassword123_'
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </label>
        </div>
        <div className='form-row'>
          <label>
            Имя
            <input
              type='text'
              placeholder='Николай'
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </label>
          <label>
            Фамилия
            <input
              type='text'
              placeholder='Науменко'
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
          </label>
        </div>
        <div className='form-row'>
          <label>
            Дата рождения
            <input
              type='date'
              value={birthDateTime}
              onChange={(e) => setBirthDateTime(e.target.value)}
              required={true}
            />
          </label>
          <label>
            Email
            <input
              type='text'
              placeholder='example@gmail.com'
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </label>
        </div>

        <button type='submit'>Зарегистрироваться</button>
        <button
          type='button'
          className='toggle-button'
          onClick={() => navigate('/login')}
        >
          Перейти к авторизации
        </button>
      </form>
    </div>
  );
};

export default RegisterPage;
