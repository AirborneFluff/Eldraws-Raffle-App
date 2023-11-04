import { Component } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

const DISCORD_ICON = "https://cdn.prod.website-files.com/6257adef93867e50d84d30e2/653714c1f22aef3b6921d63d_636e0a6ca814282eca7172c6_icon_clyde_white_RGB.svg";

@Component({
  selector: 'discord-button',
  templateUrl: './discord-button.component.html',
  styleUrls: ['./discord-button.component.scss']
})
export class DiscordButtonComponent {

  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
    iconRegistry.addSvgIconLiteral('discord', sanitizer.bypassSecurityTrustHtml(DISCORD_ICON));
  }
  openDiscord() {

  }
}
