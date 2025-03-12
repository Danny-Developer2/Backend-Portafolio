import { Component, OnInit } from '@angular/core';
import { Project, ProjectsService } from '../../services/projects.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FooterComponent } from '../footer/footer.component';
import { ValidateTokenService } from '../../services/validate-token.service';

@Component({
  selector: 'app-editar-project',
  imports:[ReactiveFormsModule],
  templateUrl: './editar-project.component.html',
  styleUrls: ['./editar-project.component.scss']
})
export class EditarProjectComponent implements OnInit {

  project!: Project;
  id: number = 0;
  errorMessage: string = '';
  projectForm: FormGroup;
  token: string | null=null;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectsService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private vT: ValidateTokenService
  ) {
    this.projectForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      technology: ['', Validators.required],
      url: ['', [Validators.required,Validators.pattern(/^(https?:\/\/)?([\w.-]+)+(:\d+)?(\/[\w.-]*)*\/?$/)]],
      imgUrl: ['', [Validators.required,Validators.pattern(/^(https?:\/\/)?([\w.-]+)+(:\d+)?(\/[\w.-]*)*\/?$/)]]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.id = +idParam;
        this.getProjectDetail(this.id);
      }
    });
  }

  getProjectDetail(id: number) {
    this.projectService.getProjectById(id).subscribe(
      (data: Project) => {
        if (data) {
          this.project = data;

          // **Cargar los datos en el formulario**
          this.projectForm.patchValue({
            name: data.name,
            description: data.description,
            technology: data.technology,
            url: data.url,
            imgUrl: data.imgUrl
          });
        }
      },
      (error) => {
        console.error('Error al obtener los detalles del proyecto:', error);
      }
    );
  }

  updateProject() {
    if (this.projectForm.invalid) {
      this.errorMessage = 'Por favor, corrige los errores de validación.';
      return;
    }
  
    this.token = localStorage.getItem('token');
    if (!this.token) {
      this.toastr.error('No se encontró un token de autenticación');
      return;
    }
  
    // Verificar el token antes de actualizar el proyecto
    this.vT.verifyToken(this.token!).subscribe(
      (response) => {  // ✅ Manejo de la respuesta exitosa
        
  
        // ✅ Si el token es válido, proceder con la actualización
        const formData = {
          id: this.id,
          ...this.projectForm.value
        };
  
        this.projectService.updateProject(this.id, formData).subscribe(
          (isSuccess) => {
            if (isSuccess) {
              this.toastr.success('Proyecto actualizado con éxito', '', {
                timeOut: 5000,
                positionClass: 'toast-top-right',
              });
              this.router.navigate(['/']);
            } else {
              this.toastr.error('Error al actualizar el proyecto', '', {
                timeOut: 5000,
                positionClass: 'toast-top-right',
              });
            }
          },
          (error) => {
            console.error('Error al actualizar el proyecto:', error);
            this.toastr.error('Hubo un error inesperado al actualizar el proyecto.');
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
  
  
}
