import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from 'ng2-translate';
import { AuthenService } from '../../authen/authen.service';
import { BaseComponent } from '../../shared/base.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { AlertService } from '../../shared/alert/_services';
import { Subject } from 'rxjs/Subject';
import { PasswordValidation } from '../../shared/passwordValidation ';
import { ToastrService } from 'ngx-toastr';

declare var jQuery: any;

@Component({
    selector: 'doimatkhau.dialog',
    templateUrl: 'doimatkhau.dialog.html'
})
export class DoiMatKhauComponent extends BaseComponent  {

    public isDisableButton: boolean;
    //public isShowFailDialog: boolean;
    //public isShowSuccessDialog: boolean;
    public dialogContent: string;
    public form: FormGroup;
    public onClose: Subject<boolean>;

    constructor(
        public fb: FormBuilder,
        public authenService: AuthenService,
        public translate: TranslateService,
        public router: Router,
        private _bsModalRef: BsModalRef,
        private alertService: AlertService,
        private toastr: ToastrService
     ) {    
        super(router);
        this.onClose = new Subject();
        //this.dialogContent = data.dialogContent;
    }

    ngOnInit() {
        //jQuery(document.getElementsByTagName('body')).removeClass();
        //jQuery(document.getElementsByTagName('body')).addClass('hold-transition lockscreen');

        this.isDisableButton = false;
        //this.isShowFailDialog = false;
        //this.isShowSuccessDialog = false;

        this.form = this.fb.group({
            'oldpassword': ["", [Validators.required]],
            'newpassword': ["", [Validators.required]],
            'newpasswordConfirm': ["", [Validators.required]]
        },
        {
            validator: PasswordValidation.MatchPassword // your validation method
        });
    }

    save() {
        this.onClose.next(false);
        this._bsModalRef.hide();
    }

    hide() {
        this.onClose.next(false);
        this._bsModalRef.hide();
    }

    changePassword(changePasswordForm: any) {
        this.isDisableButton = true;
        this.authenService.changePassword(changePasswordForm)
            .subscribe(responseResult => {
                this.isDisableButton = false;
                if (responseResult.Success) {
                    this.onClose.next(false);
                    this._bsModalRef.hide();
                    //this.dialogContent = "Thành công! Check mail...";
                    //this.isShowSuccessDialog = true;
                } 
                this.alertService.showMessageToas(responseResult.Param,responseResult.Success);

                //else {
                    //this.dialogContent = responseResult.Param.Messages;
                    //this.isShowFailDialog = true;
                //}
                // setTimeout(() => {
                //     this.isShowFailDialog = false;
                //     this.isShowSuccessDialog = false;
                // }, 3000);
            });

    }

    backto(){
        this.router.navigate(['/login']);
    }
}
