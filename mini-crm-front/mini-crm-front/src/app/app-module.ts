import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Dashboard } from './dashboard/dashboard';
import { apiErrorInterceptor } from './core/api-error.interceptor';
import { ResourcePage } from './resource-page/resource-page';
import { NotFound } from './not-found/not-found';

@NgModule({
  declarations: [App, Dashboard, ResourcePage, NotFound],
  imports: [BrowserModule, FormsModule, AppRoutingModule],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([apiErrorInterceptor])),
  ],
  bootstrap: [App],
})
export class AppModule {}
