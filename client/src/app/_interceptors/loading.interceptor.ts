import { HttpInterceptorFn } from '@angular/common/http';
import { BusyService } from '../_services/busy.service';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {

  const busyService = inject(BusyService)
  busyService.busy(); // starts our spinner when the request is sent. 

  return next(req).pipe(
    delay(1000),
    finalize(() => busyService.idle()) // when the request comes back, we set it back to idle.
  )
};
