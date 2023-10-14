import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AppFrameComponent } from './components/app-frame/app-frame.component';
import { ShortNumberPipe } from './pipes/short-number';
import { ReversePipe } from './pipes/reverse';
import { TimeUntilPipe } from './pipes/time-until.pipe';
import { ComponentFrameComponent } from './components/component-frame/component-frame.component';
import { ClipboardModule } from 'ngx-clipboard';



@NgModule({
  declarations: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe,
    ComponentFrameComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ClipboardModule
  ],
  exports: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe,
    ComponentFrameComponent
  ]
})
export class CoreModule { }
