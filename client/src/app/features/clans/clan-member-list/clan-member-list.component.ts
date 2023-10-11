import { Component, Input } from '@angular/core';
import { Member } from '../../../data/models/member';

@Component({
  selector: 'clan-member-list',
  templateUrl: './clan-member-list.component.html',
  styleUrls: ['./clan-member-list.component.scss']
})
export class ClanMemberListComponent {
  @Input() members: Member[] = [];
}
