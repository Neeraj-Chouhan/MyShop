import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/Models/Product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
product: IProduct;
  constructor(private shopservice: ShopService, private acivatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.loadProduct();
  }

  loadProduct(){

    this.shopservice.getProductById(+this.acivatedRoute.snapshot.paramMap.get('id')).subscribe(res => {this.product = res; }
      , error => {console.log(error); });
  }

}
