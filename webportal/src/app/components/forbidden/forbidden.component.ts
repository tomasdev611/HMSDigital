import {Location} from '@angular/common';
import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {OAuthService} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-forbidden',
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.scss'],
})
export class ForbiddenComponent implements OnInit {
  user: any = this.oAuthService.getIdentityClaims();
  showBack = true;

  constructor(
    private oAuthService: OAuthService,
    private location: Location,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.showBack = params?.allowed !== 'none';
    });
  }

  public logout() {
    localStorage.clear();
    this.oAuthService.logOut();
  }
  public goBack() {
    this.location.back();
  }
}
