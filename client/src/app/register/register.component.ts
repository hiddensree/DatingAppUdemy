import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  //usersFromHomeComponent = input.required<any>(); // input signal - compiler support for input
  cancelRegister = output<boolean>(); // output signal - compiler support for output
  model: any = {};

  register(){
    this.accountService.register(this.model).subscribe({
      next: (response: any) => {
        console.log(response);
        this.cancel();
      },
      error: (error: any) => console.log(error)
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}