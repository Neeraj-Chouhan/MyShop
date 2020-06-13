import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/Models/Product';
import { ShopService } from './shop.service';
import { IBrand } from '../shared/Models/ProductBrand';
import { IType } from '../shared/Models/ProductType';
import { ShopParams } from '../shared/Models/ShopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
 products: IProduct[];
 brands: IBrand[];
 types: IType[];
 shopParam = new ShopParams();
 totalCount: number;
 sortOptions = [{name: 'A-Z', value: 'name'},
  {name : 'Price:Low to High', value: 'priceAsc'},
  {name : 'Price:High to Low' , value: 'priceDesc'}];
  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProductsList();
    this.getBrandsList();
    this.getTypesList();
  }

  getProductsList(){
      this.shopService.getProducts(this.shopParam)
      .subscribe(response => {
        this.products = response.data;
        this.shopParam.pageIndex = response.pageIndex;
        this.shopParam.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error => {console.log(error); });
  }
  getBrandsList(){
    this.shopService.getBrands().subscribe(response =>
      {this.brands = [{ id: 0, name : 'All'}, ...response]; },
      error => {console.log(error); });
  }
  getTypesList(){
    this.shopService.getTypes().subscribe(response =>
      {this.types = [{ id: 0, name : 'All'}, ...response]; },
      error => {console.log(error); });
  }
  onBrandSelected(brandId: number){
    this.shopParam.brandId = brandId;
    this.getProductsList();
  }
  onTypeSelected(typeId: number){
    this.shopParam.typeId = typeId;
    this.getProductsList();
   }
   onSortSelected(sortType: string){
    this.shopParam.sort = sortType;
    this.getProductsList();
   }
   onPageChanged(event: any){
     this.shopParam.pageIndex = event.page;
     this.getProductsList();
   }

}
