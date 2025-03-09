import { Component } from '@angular/core';
import { Project, ProjectsService } from '../../services/projects.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FooterComponent } from "../footer/footer.component";
@Component({
  selector: 'app-editar-project',
  imports: [CommonModule, FormsModule, FooterComponent],
  templateUrl: './editar-project.component.html',
  styleUrl: './editar-project.component.scss'
})
export class EditarProjectComponent {

  project!: Project;
  id: number = 0;
  errorMessage: string = '';

  constructor(private getProjectById: ProjectsService,private route:ActivatedRoute,private router: Router,private toastr: ToastrService) { }




  ngOnInit(id: number) {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id'); // Obtener el id de la URL
      if (idParam) {
        this.id = +idParam; // Convertir a número
        this.getProjectDetail(this.id);
      }
    });
  }
  getProjectDetail(id: number) {
    this.getProjectById.getProjectById(id).subscribe(
      (data: Project) => {
        // Aquí asumimos que la API retorna un solo proyecto, no un arreglo
        this.project = data || null; // Si no existe el proyecto, se asigna null
      },
      (error) => {
        console.error('Error al obtener los detalles del proyecto:', error);
      }
    );
  }

  updateProject() {
    if (!this.project.name || !this.project.description) {
      this.errorMessage = 'Por favor, completa todos los campos obligatorios.';
      return;
    }
  
    // Crear un objeto con solo los campos necesarios
    const projectToUpdate = {
      id: this.id, // El id del proyecto se debe pasar como un parámetro para la función de actualización
      name: this.project.name,
      description: this.project.description,
      technology: this.project.technology,
      url: this.project.url,
      imgUrl: this.project.imgUrl
    };
  
    console.log(projectToUpdate); // Verifica los datos que estás enviando
  
    // Llamar a la función de actualización pasando solo los campos necesarios
    this.getProjectById.updateProject(this.id, projectToUpdate).subscribe(
      (isSuccess) => {
        if (isSuccess) {
          this.toastr.success('Proyecto actualizado con éxito', '', {
            timeOut: 5000,
            positionClass: 'toast-top-right',
          });
          this.router.navigate(['/']); // Redirigir al listado de proyectos
        } else {
          this.toastr.error('Error al actualizar el proyecto', '', {
            timeOut: 5000,
            positionClass: 'toast-top-right',
          });
        }
      },
      (error) => {
        console.error('Error al actualizar el proyecto:', error);
      }
    );
  }
  

}
