export enum EventCategories {
  Unspecified,
  Music,
  Sports,
  Education,
  Health,
  Art,
  Food,
  Business,
  Literature,
  Film,
  Theatre,
  Fashion,
  Science,
  Gaming,
}

export type GetEventParams = {
  name?: string;
  dateFrom?: string;
  dateTo?: string;
  location?: string;
  category?: EventCategories;
  page: number;
  pageSize: number;
};

export type EventType = {
  id: number;
  name: string;
  description: string;
  eventDateTime: string;
  location: string;
  category: EventCategories;
  maximumParticipants: number;
  currentParticipants: number;
  imagePath: string;
};

export type PaginationButtonsProps = {
  currentPage: number;
  totalPages: number;
  setPage: (page: number) => void;
};

export type FilterProps = {
  filters: GetEventParams;
  setFilters: (filters: GetEventParams) => void;
};

export type EventPageProps = {
  event: EventType;
};
