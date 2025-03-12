import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { RegisterProjectsService } from '../../services/registerProjets.service';
import { GetSkillsService, Skills } from '../../services/get-skills.service';
import {Experiences, GetExperiencesService} from '../../services/get-experiences.service';
import { FooterComponent } from '../footer/footer.component';
import { ValidateTokenService } from '../../services/validate-token.service';
import { ToastrService } from 'ngx-toastr';
import { timeout } from 'rxjs';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterProjectComponent {

  token: string | null=null;
  skills: Skills[] = [];
  experiences: Experiences[] = [];
  skillsSelected: number[] = [];

  

  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private registerService: RegisterProjectsService, private getSkillsService:GetSkillsService, private getExperiencesService:GetExperiencesService, private vT:ValidateTokenService,private toastr: ToastrService,private router:Router) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      technology: ['', Validators.required],
      url: ['', [Validators.required, Validators.pattern('https?://.+')]],
      imgUrl: ['', [Validators.required, Validators.pattern('https?://.+')]],
      skillIds:  [[],[Validators.required, Validators.minLength(5)]],
      experienceIds: ['', Validators.required],
      userIds: ['']
    });
  }

  ngOnInit(): void {
    this.getSkills(); // Llamar al método para obtener las habilidades al cargar el componente
    this.getExperiences(); // Llamar al método para obtener las experiencias al cargar el componente
  }
 
 

// Método onSubmit
onSubmit() {
  // Primero verificar si el formulario es válido
  if (this.registerForm.invalid) {
    this.toastr.error('Por favor, complete todos los campos correctamente.');
    return;  // Detener la ejecución si el formulario es inválido
  }

  // Obtener el token desde el localStorage
  this.token = localStorage.getItem('data');
  if (!this.token) {
    this.toastr.error('No se encontró un token de autenticación');
    return;  // Detener la ejecución si no se encontró el token
  }

  // Verificar la validez del token antes de enviar la solicitud
  this.vT.verifyToken(this.token!).subscribe(
    (response) => {  // ✅ Manejo de la respuesta exitosa
    
  
      // ✅ Si el token es válido, proceder con la creación del proyecto
      const formData = {
        id: Math.floor(Math.random() * 1000), // Generar un ID temporal
        ...this.registerForm.value,
        skillIds: this.skillsSelected,
        experienceIds: this.convertToArray(this.registerForm.value.experienceIds),
        userIds: this.convertToArray(this.registerForm.value.userIds)
      };
  
      this.registerService.registerProject(formData, this.token!).subscribe(
        (response) => {  // ✅ Manejo de éxito en la creación del proyecto
          console.log('Proyecto registrado:', response);
          this.toastr.success('Proyecto registrado con éxito!');
          this.registerForm.reset();
          this.skillsSelected = [];  // Resetear los seleccionados de habilidades
        },
        (error) => {  // ✅ Manejo de error en la creación del proyecto
          console.error('Error al registrar el proyecto:', error);
          this.toastr.error('Hubo un error al registrar el proyecto.');
        }
      );
    },
    (error) => {  // ✅ Manejo de errores en la verificación del token
      console.error('Error verificando el token:', error);
      if (error.status === 401) {
        localStorage.removeItem('token');
        this.toastr.error('Token expirado. Vuelve a iniciar sesión.', '', {
          timeOut: 4000,
          positionClass: 'toast-top-right',
        });
        this.router.navigate(['/auth/login']);
      } else {
        this.toastr.error('Error al verificar el token.');
      }
    }
  );
  
}


  private convertToArray(value: string): number[] {
    return value ? value.split(',').map(Number).filter(num => !isNaN(num)) : [];
  }

  getSkills(): any{
    this.getSkillsService.getSkills().subscribe(
      (data) => {
        this.skills = data;
        console.log('Habilidades obtenidas:', this.skills);
      },
      (error) => {
        console.error('Error al obtener las habilidades:', error);
      }

  
    );
  }

  getExperiences(): any{
    this.getExperiencesService.getExperiences().subscribe(
      (data) => {
        this.experiences = data;
        console.log('Experiencias obtenidas:', this.experiences);
      },
      (error) => {
        console.error('Error al obtener las habilidades:', error);
      }

  
    );
  }



   // Método para manejar el cambio en los checkboxes
   onSkillChange(event: any) {
    const skillId = Number(event.target.value);
    if (event.target.checked) {
      this.skillsSelected.push(skillId);
    } else {
      this.skillsSelected = this.skillsSelected.filter(id => id !== skillId);
    }
    this.registerForm.controls['skillIds'].setValue(this.skillsSelected);
    this.registerForm.controls['skillIds'].updateValueAndValidity();
  }
   // Método para verificar si una habilidad está seleccionada
   isSkillSelected(skillId: number): boolean {
    return this.skillsSelected.includes(skillId);
  }

  

}
