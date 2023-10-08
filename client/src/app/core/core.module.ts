import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AppFrameComponent } from './components/app-frame/app-frame.component';
import { ShortNumberPipe } from './pipes/short-number';
import { ReversePipe } from './pipes/reverse';
import { TimeUntilPipe } from './pipes/time-until.pipe';



@NgModule({
  declarations: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe
  ]
})
export class CoreModule { }
