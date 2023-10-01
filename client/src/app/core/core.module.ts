import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AppFrameComponent } from './components/app-frame/app-frame.component';



@NgModule({
  declarations: [
    AppFrameComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports: [
    AppFrameComponent
  ]
})
export class CoreModule { }
