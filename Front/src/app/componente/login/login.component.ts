import { Component } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FooterComponent } from '../footer/footer.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, FooterComponent],
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
    private http: HttpClient
  ) {}

  // Método que se llama cuando el usuario hace login
  onLogin() {
    this.loginService.login(this.email, this.password).subscribe({
      next: (response) => {
        const now = new Date().getTime(); // Tiempo actual en milisegundos
        const expirationTime = now + 60 * 60 * 1000; // 1 hora en milisegundos
        localStorage.setItem('expirationTime', expirationTime.toString()); // Almacena la expiración del token
        // Si el login es exitoso, almacenas el token y rediriges
        localStorage.setItem('token', response.token); // Asumiendo que el backend retorna un token
        sessionStorage.setItem('token', response.token);
        console.log('Login exitoso', response);

        // Redirige a la página principal o dashboard
        this.router.navigate(['/']);
      },
      error: (error) => {
        // Aquí puedes manejar el error si la solicitud falla
        console.error('Error en el login', error);
      },
    });
  }
}
