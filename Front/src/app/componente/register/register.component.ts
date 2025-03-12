import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { RegisterService } from '../../services/register.service';
import { Router } from '@angular/router';
import { FooterComponent } from '../footer/footer.component';
import { ValidateTokenService } from '../../services/validate-token.service';
import { ToastrService } from 'ngx-toastr';
import { SetExpToken } from '../../services/set-token.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  registerUserFrom: FormGroup;
  formData = new FormData();

  constructor(
    private fb: FormBuilder,
    private registerUser: RegisterService,
    private router: Router,
    private vT: ValidateTokenService,
    private toastr: ToastrService,
     private setExpToken: SetExpToken,
  ) {
    this.registerUserFrom = this.fb.group({
      name: ['', [Validators.required]],
      email: ['', Validators.required],
      passwordHash: ['', [Validators.required, Validators.minLength(8)]],
      // role: 0
    });
  }

  onSbmit() {
    if (this.registerUserFrom.valid) {
      this.formData = {
        id: Math.floor(Math.random() * 1000),
        ...this.registerUserFrom.value,
        //  Esto del rol no se hace asi desde el backend tienes que crear el usuario de forma automarica como user 0 sin necesidad del fornt
        //  role: 0
      };
      console.log(this.formData);

      this.registerUser.registerUser(this.formData).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          sessionStorage.setItem('token', response.token);
          const timeExptoken = this.setExpToken.setExpToken(response.token!);
          localStorage.setItem('expirationTime', timeExptoken);
          this.toastr.success('Usuario Registrado con Exito','',{
            timeOut: 5000,
            positionClass: 'toast-top-right',
            closeButton: true,
            progressBar: true,
            tapToDismiss: true,
          });
          // this.router.navigate(['/'])
          setTimeout(() => {
            this.router.navigate(['/']);
          }, 3000);
        },
      
      });
    }else{
      this.toastr.error('Todos los campos son obligatorios y la contraseña debe tener al menos 8 caracteres');
      return;
    }

    
  }
}
