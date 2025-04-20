import { Routes } from '@angular/router';
import { SubmitComponent } from './pages/submit/submit.component';
import { DisplayComponent } from './pages/display/display.component';

export const routes: Routes = [
  { path: '', redirectTo: 'submit', pathMatch: 'full' },
  { path: 'submit', component: SubmitComponent },
  { path: 'display', component: DisplayComponent },
  { path: '**', redirectTo: 'submit' }
];
