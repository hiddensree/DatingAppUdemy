import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError(error => {
      if (error){
        switch (error.status) {
          case 400:
            if (error.error.errors){
              const modalStateErrors = [];
              for (const key in error.error.errors){
                if (error.error.errors[key]){
                  modalStateErrors.push(error.error.errors[key]);
                }
              }
              throw modalStateErrors.flat(); // flat() is a new array method that flattens the array
            }
            else {
              toastr.error(error.error, error.status);
            }
            break;
          case 401:
            toastr.error('Unauthorized', error.status);
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            const navigationExtras = {state: {error: error.error}}; // pass the error object to the server-error component - error in states
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            toastr.error('Something unexpected went wrong');
            break;
          }
        }
        throw error;
      })
    )
};
