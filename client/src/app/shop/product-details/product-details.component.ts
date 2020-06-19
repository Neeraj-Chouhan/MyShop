import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/Models/Product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem } from 'src/app/shared/Models/Basket';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
product: IProduct;
quantity = 1;
  constructor(private shopservice: ShopService,
              private acivatedRoute: ActivatedRoute,
              private basketService: BasketService) { }

  ngOnInit(): void {

    this.loadProduct();
  }

  loadProduct(){

    this.shopservice.getProductById(+this.acivatedRoute.snapshot.paramMap.get('id'))
     .subscribe(res => {this.product = res; }
      , error => {console.log(error); });
  }

  addItemToCart(){
        this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
    if ( this.quantity > 1){
      this.quantity--;
    }
 }
}
