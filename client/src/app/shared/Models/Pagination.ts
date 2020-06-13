import { IProduct } from './Product';

export interface IPaginatioin {
    pageSize: number;
    pageIndex: number;
    count: number;
    data: IProduct[];
  }

