import {AbstractControl} from '@angular/forms';
export class PasswordValidation {

    static MatchPassword(AC: AbstractControl) {
       let password = AC.get('newpassword').value; // to get value in input tag
       let confirmPassword = AC.get('newpasswordConfirm').value; // to get value in input tag
        if(password != confirmPassword) {
            //console.log('false');
            AC.get('newpasswordConfirm').setErrors( {MatchPassword: true} )
        } else {
            //console.log('true');
            AC.get('newpasswordConfirm').setErrors(null);
            //return null
        }
    }
}