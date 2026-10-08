import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenService } from '../../authen/authen.service';
import { BaseComponent } from '../../shared/base.component';
import { String } from 'typescript-string-operations';
import {WebApiClientService} from '../../core/webApiClient.service';
import { ProfileDatas } from '../../model/danhsachbaocao.model';
import {AppConfig} from '../../config/app.config';
import { TranslateService } from 'ng2-translate';
import { isNumber } from 'util';
import { ProfileService } from '../login/login.service';
declare var jQuery: any;

@Component({
    selector: 'app-dashboard',
    templateUrl: 'login.component.html'
})
export class LoginComponent extends BaseComponent implements OnInit {

    isDisableLoginButton: boolean;
    isShowDialog: boolean;
    isShowCaptcha: boolean;
    dialogContent: string;
    loginForm: FormGroup;

    constructor(public router: Router,
        public authenService: AuthenService,
        public fb: FormBuilder,
        public webApiClientService: WebApiClientService,
        public translateService: TranslateService,
        public profileService: ProfileService,
    ) {
      super(router);
    }

    ngOnInit() {
        // jQuery(document.getElementsByTagName('body')).removeClass();
        // jQuery(document.getElementsByTagName('body')).addClass('hold-transition login-page');

        this.isDisableLoginButton = false;
        this.isShowDialog = false;
        this.dialogContent = '';

        this.loginForm = this.fb.group({
            'username': ['', Validators.required],
            'password': ['', Validators.compose([Validators.required, Validators.minLength(1)])],            
        });
    }

    forgotpassword(){
        this.router.navigate(['/forgotpassword']);
    }

    // Khi đăng nhập vào trang web, trang web tự động nhận diện người dùng theo tài khoản đăng nhập.
    // 1. Nếu có dạng msyt_xxx thì cắt xxx để truy vấn thông tin như hiện tại
    // 2. Nếu có dạng yyy (16 ký tự số) thì coi đó là số thẻ --> hiển thị module Chọn hồ sơ
    // - Truy vấn HID_PERSON theo trường CARD_CODE (tìm chính xác), sử dụng yyy để tìm kiếm
    // - Nếu không tìm được bản ghi nào thì hiển thị thông báo: "Thẻ của bạn chưa được tích hợp với hệ thống
    //   hồ sơ sức khỏe cá nhân. Vui lòng liên hệ bộ phận chăm sóc khách hàng 19006888 để được hỗ trợ." 
    //   --> nsd click OK thì tự động đăng xuất
    // - Nếu tìm được 1 bản ghi thì lấy person_code và hành xử như 1.
    // - Nếu tìm được 2 bản ghi trở lên thì hiển thị màn hình theo dạng chọn hồ sơ (mỗi hồ sơ là 1 person_code).
    //   Nsd tích chọn bấm OK thì sẽ vào màn hình chính như bình thường, dùng person_code được chọn để truy vấn thông tin.
    // Thông thường 1 thẻ sẽ chỉ gắn với 1 person_code hoặc nếu nhiều lắm chỉ là 2, 3 person_code mà thôi
    // Màn hình chọn có thể hiển thị 1 số thông tin tóm lược nhất của person như: mã y tế, tên, ngày sinh, giới tính, tỉnh khai sinh, địa chỉ, cmnd/cccd
    login(loginForm: any) {
        localStorage.clear();
        let username = '' + loginForm.username;
        const password = loginForm.password;      
        this.isDisableLoginButton = true;
        this.authenService.login(username, password)
            .subscribe(result => {
                if (result.Success) {
                    localStorage.setItem('username', result.Data.User.LoginName);
                    localStorage.setItem('fullname', result.Data.User.UserName);
                    localStorage.setItem('tokencode', result.Data.TokenCode);
                    localStorage.setItem('tokendata', JSON.stringify(result.Data));

					this.router.navigate(['/home']);

                } else {
                    this.isDisableLoginButton = false;
                    this.isShowDialog = true;
                    setTimeout(() => this.isShowDialog = false, 5000);                    
                    this.translateService.get("message.xu ly that bai")
                    .subscribe(t => {
                        this.dialogContent= "";
                        if(result.Param!=null && result.Param.Messages!=null && result.Param.Messages.length >0) 
                        {
                            result.Param.Messages.forEach(element => {
                                this.dialogContent += element + " ";
                            });
                        }

                        if(result.Param!=null && result.Param.BugCodes!=null && result.Param.BugCodes.length >0) 
                        {
                            result.Param.BugCodes.forEach(element => {
                                this.dialogContent += element + " ,";
                            });
                        }
                        
                        this.dialogContent += t;
                    });//'Đăng nhập không thành công. Thông tin đăng nhập không chính xác!';
                    
                    // if (loginResult.loginAttempt >= 5) {
                    //     this.isShowCaptcha = true;
                    //     this.captchaUrl = '/admin.api/jcaptcha?t=' + new Date().getTime();
                    //     this.loginForm = this.fb.group({
                    //         'username': ['', Validators.required],
                    //         'password': ['', Validators.compose([Validators.required, Validators.minLength(6)])],
                    //         'captcha': ['', Validators.required]
                    //     });
                    // }
                }
            });

    }
}
