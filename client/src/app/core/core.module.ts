import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AppFrameComponent } from './components/app-frame/app-frame.component';
import { ShortNumberPipe } from './pipes/short-number';
import { ReversePipe } from './pipes/reverse';
import { TimeUntilPipe } from './pipes/time-until.pipe';
import { ComponentFrameComponent } from './components/component-frame/component-frame.component';
import { ClipboardModule } from 'ngx-clipboard';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NumericPositionPipe } from './pipes/numeric-position';



@NgModule({
  declarations: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe,
    NumericPositionPipe,
    ComponentFrameComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ClipboardModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule
  ],
  exports: [
    AppFrameComponent,
    ShortNumberPipe,
    ReversePipe,
    TimeUntilPipe,
    ComponentFrameComponent,
    NumericPositionPipe
  ],
  providers: [
    NumericPositionPipe,
    TimeUntilPipe
  ]
})
export class CoreModule { }
