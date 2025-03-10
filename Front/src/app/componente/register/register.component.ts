import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators,AbstractControl } from '@angular/forms';
import { RegisterService } from '../../services/register.service';
import { Router } from '@angular/router';
import { FooterComponent } from '../footer/footer.component';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  registerUserFrom: FormGroup; 

  constructor(private fb: FormBuilder, private registerUser: RegisterService,private router: Router) {
    this.registerUserFrom = this.fb.group({
      name: ['', [Validators.required]],
      email: ['', Validators.required],
      passwordHash: ['', [Validators.required, Validators.minLength(8)]],
      // role: ['', Validators.required]
    });
  }

 

  onSbmit(){
    if(this.registerUserFrom.valid){
      const formData = {
        id: Math.floor(Math.random() * 1000),
       ...this.registerUserFrom.value,
      //  Esto del rol no se hace asi desde el backend tienes que crear el usuario de forma automarica como user 0 sin necesidad del fornt
       role: 0
      }
      console.log(formData);
      this.registerUser.registerUser(formData).subscribe(
        response => {
          console.log('Usuario registrado con éxito:', response);
          localStorage.setItem('token', response.token);  // Asumiendo que el backend retorna un token
          sessionStorage.setItem('token', response.token);
          this.router.navigate(['/']);  // Redirige al dashboard o home page  // Agregar rutas adecuadas en su proyecto
          alert('Usuario registrado con éxito!');
          this.registerUserFrom.reset();
        },
        error => {
          console.error('Error al registrar el usuario:', error);
          alert('Hubo un error al registrar el usuario');
        }
      );
    }else{
      alert('Por favor, complete todos los campos correctamente.');
    }
  }

  
}
