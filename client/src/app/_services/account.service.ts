import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);  

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user)); // We can use sessionStorage as well
          this.currentUser.set(user); // emit is a method of signal
        }
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user)); // We can use sessionStorage as well
          this.currentUser.set(user); // emit is a method of signal
        }
        return user;
      })
    )
  }

  logout(){
    localStorage.removeItem('user'); // removes the user from the local storage
    this.currentUser.set(null);
  }
}
