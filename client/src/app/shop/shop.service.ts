import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPaginatioin } from '../shared/Models/Pagination';
import { IBrand } from '../shared/Models/ProductBrand';
import { IType } from '../shared/Models/ProductType';
import { map } from 'rxjs/operators';
import { stringify } from 'querystring';
import { ShopParams } from '../shared/Models/ShopParams';
import { IProduct } from '../shared/Models/Product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  constructor(private http: HttpClient) { }

  getProducts(shopParam: ShopParams){
   let params = new HttpParams();
   if (shopParam.brandId !== 0) {
      params = params.append('brandId', shopParam.brandId.toString());
    }
   if (shopParam.typeId !== 0) {
      params = params.append('typeId', shopParam.typeId.toString());
    }
   params = params.append('sort', shopParam.sort);
   params = params.append('pageIndex', shopParam.pageIndex.toString());
   params = params.append('pageIndex', shopParam.pageSize.toString());
   return this.http.get<IPaginatioin>(this.baseUrl + 'products',
   {observe: 'response', params})
   .pipe(map(response =>
    {
      return response.body;
    }));

  }
  getProductById(id: number){

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
  getBrands(){

    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');

    }

  getTypes(){

      return this.http.get<IType[]>(this.baseUrl + 'products/types');

      }

}
