import { Component, inject} from "@angular/core";
import { AccountService } from "../_services/account.service";
import { FormsModule } from "@angular/forms";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { TitleCasePipe } from "@angular/common";


@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService); // child component
  private router = inject(Router); // child component 
  private toastr = inject(ToastrService); // child component
  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next:  _ => {
       this.router.navigateByUrl('/members'); // redirect to members page
      },
      error: (error: any) => {
        console.log(error);
        this.toastr.error(error.error); // display error message
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/'); // redirect to home page
  }
}
