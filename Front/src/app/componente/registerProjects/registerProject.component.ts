import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { RegisterProjectsService } from '../../services/registerProjets.service';
import { GetSkillsService, Skills } from '../../services/get-skills.service';
import {Experiences, GetExperiencesService} from '../../services/get-experiences.service';
import { FooterComponent } from '../footer/footer.component';


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

  constructor(private fb: FormBuilder, private registerService: RegisterProjectsService, private getSkillsService:GetSkillsService, private getExperiencesService:GetExperiencesService) {
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
  onSubmit() {

    if (this.registerForm.valid) {

      // Convertir los valores de string a arrays numéricos
      const formData = {
        id: Math.floor(Math.random() * 1000), // Generar un ID temporal
        ...this.registerForm.value,
        skillIds: this.skillsSelected,
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
          this.skillsSelected = []; // Resetear los seleccionados de habilidades
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
