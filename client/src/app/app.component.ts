import { Component, inject, OnInit } from '@angular/core';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { RouterOutlet } from '@angular/router';
import { NgxSpinnerComponent} from 'ngx-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NavComponent, RouterOutlet, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);

  ngOnInit(): void {
    this.setCurrentUser();
 }

 setCurrentUser(){ // This method is used to set the current user
  const userString = localStorage.getItem('user'); // get the user from the local storage
  if (!userString) return;
  const user = JSON.parse(userString); // parse the user from the string to JSON
  this.accountService.currentUser.set(user);
 }

}