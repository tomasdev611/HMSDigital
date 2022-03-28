import {Routes} from '@angular/router';
import {LoginComponent, ForbiddenComponent} from '../components';

export const RoutePublic: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'forbidden', component: ForbiddenComponent},
];
