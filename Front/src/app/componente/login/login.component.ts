import { Component, inject } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FooterComponent } from '../footer/footer.component';
import { SetExpToken } from '../../services/set-token.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {

  usuarioLogeado: boolean = false;
  

  email: string = '';
  password: string = '';

  constructor(
    private loginService: LoginService,
    private router: Router,
    private http: HttpClient,
    private setExpToken: SetExpToken,
    private toastr: ToastrService,
  ) {}

  // Método que se llama cuando el usuario hace login
  onLogin() {
    this.loginService.login(this.email, this.password).subscribe({
      next: (response) => {
        console.log(response);
        localStorage.setItem('data',response.data);
        const data = localStorage.getItem('data');
        const timeExptoken =  this.setExpToken.setExpToken(data!)
        localStorage.setItem('expirationTime', timeExptoken)
        sessionStorage.setItem('expirationTime', timeExptoken)
        this.toastr.success('Login exitoso', 'Bienvenido!', {
          timeOut: 5000,
          positionClass: 'toast-top-right',
        });
        console.log('Login exitoso', response);

        // Redirige a la página principal o dashboard
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.toastr.error('', error.error.message , {
          timeOut: 5000,
          positionClass: 'toast-top-right',
        });

        // Aquí puedes manejar el error si la solicitud falla
        console.error('Error en el login', error);
      },
    });
  }
}
