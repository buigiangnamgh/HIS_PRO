import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from 'ng2-translate';
import { AuthenService } from '../../authen/authen.service';
import { BaseComponent } from '../../shared/base.component';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
//import { CustomValidators } from '../shared/validator/custom.validator';
// import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
// import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

declare var jQuery: any;

@Component({
    selector: 'app-dashboard',
    templateUrl: 'forgotpassword.component.html'
})
export class ForgotPasswordComponent extends BaseComponent  {

    public isDisableButton: boolean;
    public isShowFailDialog: boolean;
    public isShowSuccessDialog: boolean;
    public dialogContent: string;

    public forgotPasswordForm: FormGroup;

    constructor(
        //public snackBar: MdSnackBar,
    //public modalService: NgbModal,
    public fb: FormBuilder,
    public authenService: AuthenService,
    public translate: TranslateService,
    public router: Router) {
    super(router);
  }

    ngOnInit() {
        //jQuery(document.getElementsByTagName('body')).removeClass();
        //jQuery(document.getElementsByTagName('body')).addClass('hold-transition lockscreen');

        this.isDisableButton = false;
        this.isShowFailDialog = false;
        this.isShowSuccessDialog = false;

        this.forgotPasswordForm = this.fb.group({
            'email': ["", [Validators.required]]
        });

        // this.forgotPasswordForm = this.fb.group({
        //     'email': ["", [Validators.required, Validators.pattern('^[A-Za-z0-9+_.-]+@[A-Za-z0-9+_-]+(?:\.[a-zA-Z0-9-]+)+$')]]
        // });
    }

    forgotPassword(forgotPasswordForm: any) {
        this.isDisableButton = true;
        this.authenService.forgotPassword(forgotPasswordForm)
            .subscribe(responseResult => {
                this.isDisableButton = false;
                if (responseResult.Success) {
                    this.dialogContent = "Thành công! Check mail...";
                    this.isShowSuccessDialog = true;
                } else {
                    //this.dialogContent = responseResult.Param.Messages;
                    this.isShowFailDialog = true;
                }
                setTimeout(() => {
                    this.isShowFailDialog = false;
                    this.isShowSuccessDialog = false;
                }, 3000);
            });

    }

    backto(){
        this.router.navigate(['/login']);
    }
}
