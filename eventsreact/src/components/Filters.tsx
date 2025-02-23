import { FilterProps, EventCategories } from '../types.ts';
import { ChangeEventHandler } from 'react';
import { getEventCategoryText } from '../helpers/category.ts';

const Filters = (props: FilterProps) => {
  const handleNameChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    props.setFilters({ ...props.filters, name: e.target.value });
  };

  const handleDateFromChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    props.setFilters({
      ...props.filters,
      dateFrom: new Date(e.target.value).toISOString(),
    });
  };

  const handleDateToChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    props.setFilters({
      ...props.filters,
      dateTo: new Date(e.target.value).toISOString(),
    });
  };

  const handleLocationChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    props.setFilters({ ...props.filters, location: e.target.value });
  };

  const handleCategoryChange: ChangeEventHandler<HTMLSelectElement> = (e) => {
    // @ts-expect-error(string to enum from select)
    props.setFilters({ ...props.filters, category: e.target.value });
  };

  const generateCategories = () => {
    const options = [];
    for (let i = EventCategories.Unspecified; i < EventCategories.Gaming; i++) {
      options.push(<option value={i}>{getEventCategoryText(i)}</option>);
    }
    return options;
  };

  return (
    <div className='filters'>
      <h2>Фильтры</h2>
      <div className='filters-wrapper'>
        <label>
          Название события
          <input
            type='text'
            value={props.filters.name}
            onChange={handleNameChange}
          />
        </label>
        <label>
          Начиная с
          <input type='date' onChange={handleDateFromChange} />
        </label>
        <label>
          Заканчивая
          <input type='date' onChange={handleDateToChange} />
        </label>
        <label>
          Место проведения
          <input type='text' onChange={handleLocationChange} />
        </label>
        <label>
          Категория
          <select
            value={props.filters.category}
            onChange={handleCategoryChange}
          >
            {generateCategories()}
          </select>
        </label>
      </div>
    </div>
  );
};

export default Filters;
