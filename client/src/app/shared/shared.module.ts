import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { OrderTotalComponent } from './components/order-total/order-total.component';



@NgModule({
  declarations: [OrderTotalComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot()
  ],
  exports: [PaginationModule, OrderTotalComponent]
})
export class SharedModule { }
