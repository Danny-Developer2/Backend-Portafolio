import { Component, OnInit } from '@angular/core';
import { FooterComponent } from '../footer/footer.component';
import { Project, ProjectsService } from '../../services/projects.service';
import {jwtDecode} from 'jwt-decode'; 
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-landing-page',
  imports: [
    FooterComponent
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent implements OnInit  {

  constructor(private projectsService: ProjectsService,private loginService:LoginService,private router: Router) { }
  projects: Project[] = [];
  displayedProjects: Project[] = []; // Proyectos visibles en la página actual
  currentPage: number = 1; 
  projectsPerPage: number = 4; // Número de proyectos por página
  paginasTotal: number = 1; //
  token: string | null = null;
  role: string = '';


  // ngOnInit(): void {
  //   this.projectsService.getProjects().subscribe({
  //     next: (projects: Project[]) => {
  //       this.projects = projects;  // Almacena los proyectos en el componente para mostrarlos en la vista
  //       console.log('Proyectos obtenidos:', projects);
  //     },
  //     error: (error) => {
  //       console.error('Error al obtener los proyectos:', error);
  //     }
  //   });
  // }

  ngOnInit() {
    // Llamar al servicio para obtener los proyectos
    this.projectsService.getProjects().subscribe((data: Project[]) => {
      this.projects = data;
      console.log('Proyectos obtenidos:', data);
      this.token = localStorage.getItem('token')
      if (this.token) {
        const decodedToken: any = jwtDecode(this.token);
        this.role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        console.log('Rol del usuarios:', this.role);
      }
      
      console.log('Token:', this.token);
      this.updateDisplayedProjects();
    });
    //TODO Se encarga de validar que el token sea valido si no lo es cierra la session
    this.checkSession() && this.logout();

      
    
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
checkSession() {
  return this.loginService.isTokenExpired();
  
}
// TODO Ver la forma de implementar este logout desde un servicio 
logout(){
  sessionStorage.removeItem('token'); // Elimina el token de sesión
  localStorage.removeItem('token'); // Elimina el token de localStorage
  localStorage.removeItem('expirationTime');
  this.router.navigate(['/']); // Redirige al login
}



}


