import { PaginationButtonsProps } from '../types.ts';
import { MouseEventHandler } from 'react';

const PaginationButtons = (props: PaginationButtonsProps) => {
  const prevPage: MouseEventHandler = () => {
    if (props.currentPage <= 1) return;
    props.setPage(props.currentPage - 1);
  };

  const nextPage: MouseEventHandler = () => {
    if (props.currentPage >= props.totalPages) return;
    props.setPage(props.currentPage + 1);
  };

  return (
    <div className='pagination-buttons'>
      <button className='minus-page' onClick={prevPage}>
        -
      </button>
      <span>
        {props.currentPage}/{props.totalPages > 0 ? props.totalPages : 1}
      </span>
      <button className='plus-page' onClick={nextPage}>
        +
      </button>
    </div>
  );
};

export default PaginationButtons;
