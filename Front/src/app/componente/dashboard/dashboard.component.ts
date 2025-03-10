import { Component } from '@angular/core';
import { FooterComponent } from '../footer/footer.component';
import { RouterLink, RouterModule } from '@angular/router';
import { GetSkillsService } from '../../services/get-skills.service';

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink,RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  
  skills: any;


  constructor(public getSkills:GetSkillsService){}


  ngOnInit(): void {
    this.getSkills.getSkills().subscribe((skills) => {
      this.skills = skills;
    });
  }

  isSidebarHidden = true;

  toggleSidebar() {
    this.isSidebarHidden = !this.isSidebarHidden;
  }

}
