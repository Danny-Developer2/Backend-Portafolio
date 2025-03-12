import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Project, ProjectsService } from '../../services/projects.service';
import { LoginService } from '../../services/login.service';
import { RoleUser } from '../../services/role-user.service';

@Component({
  selector: 'app-projects',
  imports: [RouterLink],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent implements OnInit {



     constructor(private projectsService: ProjectsService,private loginService:LoginService,private router: Router, private roleUser:RoleUser,) { }
      projects: Project[] = [];
      displayedProjects: Project[] = []; // Proyectos visibles en la página actual
      currentPage: number = 1; 
      projectsPerPage: number = 3; // Número de proyectos por página
      paginasTotal: number = 1; //
      token: string | null = null;
      role: string = '';

   ngOnInit()  {
      // Llamar al servicio para obtener los proyectos
      this.projectsService.getProjects().subscribe((data: Project[]) => {
        const token = localStorage.getItem('token')
        this.projects = data;
        this.role = this.roleUser.roleUser(token!)
        this.updateDisplayedProjects();
      });
      //TODO Se encarga de validar que el token sea valido si no lo es cierra la session
      this.loginService.checkSession() && this.loginService.logout();
      // console.log(this.loginService.checkSession());
  
        
      
    }
  
    // Función para actualizar la lista de proyectos según la página actual
    updateDisplayedProjects() {
      const startIndex = (this.currentPage - 1) * this.projectsPerPage;
      const endIndex = startIndex + this.projectsPerPage;
      this.displayedProjects = this.projects.slice(startIndex, endIndex);
    }
  
    // Función para cambiar de página
    changePage(newPage: number) {
      this.currentPage = newPage;
      this.updateDisplayedProjects();
    }
  
    // Obtener el total de páginas
    // Obtener el total de páginas asegurando que siempre haya al menos 1 página
  get totalPages(): number {
    return this.paginasTotal= Math.max(1, Math.ceil(this.projects.length / this.projectsPerPage));
  }
  

}
