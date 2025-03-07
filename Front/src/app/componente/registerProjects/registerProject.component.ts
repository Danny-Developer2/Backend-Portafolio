import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { RegisterProjectsService } from '../../services/registerProjets.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterProjectComponent {


  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private registerService: RegisterProjectsService) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      technology: ['', Validators.required],
      url: ['', [Validators.required, Validators.pattern('https?://.+')]],
      imgUrl: ['', [Validators.required, Validators.pattern('https?://.+')]],
      skillIds: [''],
      experienceIds: [''],
      userIds: ['']
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      // Convertir los valores de string a arrays numéricos
      const formData = {
        id: Math.floor(Math.random() * 1000), // Generar un ID temporal
        ...this.registerForm.value,
        skillIds: this.convertToArray(this.registerForm.value.skillIds),
        experienceIds: this.convertToArray(this.registerForm.value.experienceIds),
        userIds: this.convertToArray(this.registerForm.value.userIds)
      };

      this.registerService.registerProject(formData).subscribe(
        response => {
          console.log('Proyecto registrado:', response);
          alert('Proyecto registrado con éxito!');
          this.registerForm.reset();
        },
        error => {
          console.error('Error al registrar el proyecto:', error);
          alert('Hubo un error al registrar el proyecto');
        }
      );
    }
  }

  private convertToArray(value: string): number[] {
    return value ? value.split(',').map(Number).filter(num => !isNaN(num)) : [];
  }

}
