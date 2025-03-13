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


  ngOnInit() {
    if (this.loginService.usuarioLogeado()) {
      this.router.navigate(['/']);
    }
  }

  setToken(data: string | null){

  
    if(data){
      localStorage.setItem('token', data);
      const token = localStorage.getItem('token');
      const timeExptoken =  this.setExpToken.setExpToken(token!)
      // localStorage.setItem('expirationTime', timeExptoken)
      // sessionStorage.setItem('expirationTime', timeExptoken)
    }

  }

  // Método que se llama cuando el usuario hace login
  onLogin() {
    this.loginService.login(this.email, this.password).subscribe({
      next: (resposne) => {
        this.setToken(resposne.token)
        console.log('login con exito',resposne)
        this.toastr.success('Login exitoso', 'Bienvenido!', {
          timeOut: 5000,
          positionClass: 'toast-top-right',
        });
        this.router.navigate(['/']);

      },
      error: (error) => {
        this.toastr.error('Error en el login', error.error.message, {
          timeOut: 5000,
          positionClass: 'toast-top-right',
        });
        console.log(error)
      }
    })
  }
   
}


// this.loginService.login(this.email, this.password).subscribe({
//   next: (response) => {
//     console.log(response);
//     localStorage.setItem('token',response.token);
//     const data = localStorage.getItem('token');
//     const timeExptoken =  this.setExpToken.setExpToken(data!)
//     localStorage.setItem('expirationTime', timeExptoken)
//     sessionStorage.setItem('expirationTime', timeExptoken)
//     this.toastr.success('Login exitoso', 'Bienvenido!', {
//       timeOut: 5000,
//       positionClass: 'toast-top-right',
//     });
//     console.log('Login exitoso', response);

//     // Redirige a la página principal o dashboard
//     this.router.navigate(['/']);
//   },
//   error: (error) => {
//     this.toastr.error('', error.error.message , {
//       timeOut: 5000,
//       positionClass: 'toast-top-right',
//     });

//     // Aquí puedes manejar el error si la solicitud falla
//     console.error('Error en el login', error);
//   },
// });
