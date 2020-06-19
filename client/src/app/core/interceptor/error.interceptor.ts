import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
   constructor(private toast: ToastrService){}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
       return next.handle(req).pipe(
            catchError(error => {

                if (error.statusText === 'Unknown Error') {
                    this.toast.error('Unknown Error', 'title');
                }
                if (error.status === 500) {
                    alert('500 error');
                }
                return throwError(error);
            })
       );
    }

}
