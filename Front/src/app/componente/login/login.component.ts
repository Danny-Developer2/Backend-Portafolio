import { Component } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone:true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {


  email: string = '';
  password: string = '';

  constructor(private loginService: LoginService, private router: Router,private http: HttpClient ){}

  // Método que se llama cuando el usuario hace login
  onLogin() {
    this.loginService.login(this.email, this.password).subscribe({
      next: (response) => {
        // Si el login es exitoso, almacenas el token y rediriges
        localStorage.setItem('token', response.token);  // Asumiendo que el backend retorna un token
        console.log('Login exitoso', response);

        // Redirige a la página principal o dashboard
        this.router.navigate(['/']);
      },
      error: (error) => {
        // Aquí puedes manejar el error si la solicitud falla
        console.error('Error en el login', error);
      }
    });
  }
  

}
