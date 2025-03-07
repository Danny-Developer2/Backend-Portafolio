import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { RegisterProjectsService } from '../../services/registerProjets.service';
import { GetSkillsService, Skills } from '../../services/get-skills.service';


@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterProjectComponent {

  token: string | null=null;
  skills: Skills[] = [];
  experiences: string[] = [];


  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private registerService: RegisterProjectsService, private getSkillsService:GetSkillsService) {
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

  ngOnInit(): void {
    this.getSkills(); // Llamar al método para obtener las habilidades al cargar el componente
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
      this.token = localStorage.getItem('token')
      if (!this.token) {
        alert('No se encontró un token de autenticación');
        return;
      }
      this.registerService.registerProject(formData,this.token!).subscribe(
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

  

}
