import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-logout',
    templateUrl: '../logout.component.html'
})
export class LogoutComponent implements OnInit {
    constructor(public router: Router) { }

    ngOnInit() { }

    logout() {
        console.log('logout');
        localStorage.clear();          
        this.router.navigate(['/login']);
    }
}