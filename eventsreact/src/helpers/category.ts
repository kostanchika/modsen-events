import { EventCategories } from '../types.ts';

export function getEventCategoryText(category: EventCategories): string {
  switch (category) {
    case EventCategories.Unspecified:
      return 'Все';
    case EventCategories.Music:
      return 'Музыка';
    case EventCategories.Sports:
      return 'Спорт';
    case EventCategories.Education:
      return 'Образование';
    case EventCategories.Health:
      return 'Здоровье';
    case EventCategories.Art:
      return 'Искусство';
    case EventCategories.Food:
      return 'Еда';
    case EventCategories.Business:
      return 'Бизнес';
    case EventCategories.Literature:
      return 'Литература';
    case EventCategories.Film:
      return 'Кино';
    case EventCategories.Theatre:
      return 'Театр';
    case EventCategories.Fashion:
      return 'Мода';
    case EventCategories.Science:
      return 'Наука';
    case EventCategories.Gaming:
      return 'Игры';
    default:
      return 'Неизвестная категория';
  }
}
