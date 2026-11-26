import { Component, inject } from '@angular/core';
import { RoleService } from '../shared/role.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})
export class Dashboard {
  protected readonly roles = inject(RoleService);
}
