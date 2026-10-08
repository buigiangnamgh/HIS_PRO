import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenService } from './authen.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authenService: AuthenService) { }

    canActivate() : boolean{
        const token = localStorage.getItem('tokencode');
        if (token == null || token == '') {
             this.router.navigate(['/login']);
            return false;
        } else {    
            let d =  this.authenService.requestAuthInfo();    
            console.log(d);
            if (d != null) {
                return true;
            } else {
                this.router.navigate(['/login']);
                return false;
            }
        }
    }
}
