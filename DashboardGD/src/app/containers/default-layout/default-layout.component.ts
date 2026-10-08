import {
  Component,
  Input,
  ViewChild,
  ElementRef,
  AfterViewInit,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { TokenData } from "../../model/authData.model";
import { AuthenService } from "../../authen/authen.service";
import { ApiData } from "../../model/apidata.model";
import { MatDialog } from "@angular/material/dialog";
import { DoiMatKhauComponent } from "../../modules/doimatkhau/doimatkhau.dialog.component";
import { MenuService } from "./menu.service";
import { AuthorizeResultModel } from "../../model/authorize.result.model";
import { MenuModel } from "../../model/menu.model";
import { AppConfig } from "../../config/app.config";
import { Profile } from "../../model/danhsachbaocao.model";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { AlertService } from "../../shared/alert/_services";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
  styleUrls: ["./default-layout.component.css"],
})
export class DefaultLayoutComponent {
  public navItems: Array<MenuModel>;
  public authorizeResultModel: AuthorizeResultModel;
  public sidebarMinimized = true;
  public sidebarShow = false;
  private changes: MutationObserver;
  public element: HTMLElement = document.body;
  public tokenData: TokenData;
  public userName: string;
  public loginName: string;
  public apiData: ApiData;
  public cardcode: string;
  public personcode: string;
  public nhieuhoso: string;
  public profiledata: Profile;
  modalRef: BsModalRef;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authenService: AuthenService,
    private dialog: MatDialog,
    private menuService: MenuService,
    private modalService: BsModalService,
    private alertService: AlertService
  ) {
    const strTokenData = localStorage.getItem("tokendata");
    if (strTokenData != null) this.tokenData = JSON.parse(strTokenData);
    if (this.tokenData != null) {
      if (this.userName == null || this.userName == "")
        this.userName = this.tokenData.User.UserName;
      this.loginName = this.tokenData.User.LoginName;
    }
    this.changes = new MutationObserver((mutations) => {
      //console.log(mutations[0]);
      console.log("document.body.classList:" + document.body.classList);
      this.sidebarMinimized =
        document.body.classList.contains("sidebar-minimized");
      console.log("changes.sidebarMinimized:" + this.sidebarMinimized);

      if (
        document.body.classList.contains("sidebar-show") &&
        document.body.classList.contains("sidebar-lg-show") &&
        document.body.classList.contains("sidebar-minimized") &&
        document.body.classList.contains("brand-minimized")
      ) {
        this.sidebarShow = true;
      } else if (
        (document.body.classList.contains("sidebar-lg-show") ||
          document.body.classList.contains("sidebar-show")) &&
        !document.body.classList.contains("sidebar-minimized") &&
        !document.body.classList.contains("brand-minimized")
      ) {
        this.sidebarShow = true;
      } else {
        this.sidebarShow = false;
      }

      console.log("changes.sidebarShow:" + this.sidebarShow);
    });

    this.changes.observe(<Element>this.element, {
      attributes: true,
    });

    //this.sidebarMinimized = document.body.classList.contains('sidebar-minimized');
    console.log("constructor.sidebarMinimized:" + this.sidebarMinimized);
    const authorizeResultData = localStorage.getItem("AuthorizeResultData");
    if (
      authorizeResultData != undefined &&
      authorizeResultData != null &&
      authorizeResultData != ""
    ) {
      this.navItems = JSON.parse(authorizeResultData);
      console.log("DefaultLayoutComponent. 1");
    }

    if (
      this.navItems === undefined ||
      this.navItems == null ||
      this.navItems.length == 0
    ) {
      console.log("DefaultLayoutComponent. 2");
      this.navItems = new Array<MenuModel>();
      this.menuService.loadMenu().subscribe((tk) => {
        console.log(tk);
        if (
          tk != undefined &&
          tk.Data != undefined &&
          tk.Data != null &&
          tk.Data.ModuleInRoles != null &&
          tk.Data.ModuleInRoles.length > 0
        ) {
          this.authorizeResultModel = tk.Data;
          tk.Data.ModuleInRoles.forEach((md) => {
            if (md.IS_VISIBLE === 1) {
              let d = new MenuModel();
              d.number_order = md.NUM_ORDER;
              d.name = md.MODULE_NAME;
              d.url = md.MODULE_LINK;
              d.icon = md.ICON_LINK;
              d.cssname = md.MODULE_URL;
              if(md.PARENT_ID ==undefined || md.PARENT_ID ==null || md.PARENT_ID <=0){
                d.isparenturl = 1;
              }

              let x = this.navItems.filter((o) => o.url == md.MODULE_LINK);
              if (x.length <= 0) {
                this.navItems.push(d);
              }
              
              d.MODULE_GROUP_NAME = md.MODULE_GROUP_NAME ?? 'Nhóm';
            }
          });

          console.log(this.navItems);
          this.navItems = this.navItems.sort((obj1, obj2) => {
            if (obj1.name > obj2.name) {
              return 1;
            }

            if (obj1.name < obj2.name) {
              return -1;
            }

            return 0;
          });
          this.navItems = this.navItems.sort((obj1, obj2) => {
            if (obj1.number_order < obj2.number_order) {
              return 1;
            }

            if (obj1.number_order > obj2.number_order) {
              return -1;
            }

            return 0;
          });
          console.log(this.navItems);
          localStorage.setItem(
            "AuthorizeResultData",
            JSON.stringify(this.navItems)
          );
          location.reload();
        }
      });
    }
  }

  toggleMinimize(e) {
    this.sidebarMinimized = e;
    console.log("toggleMinimize.sidebarMinimized:" + this.sidebarMinimized);
  }

  sidebarClick() {
    let sidebarShowByClass =
      document.body.classList.contains("sidebar-show") &&
      document.body.classList.contains("sidebar-lg-show");
    console.log("sidebarClick.sidebarShowByClass:" + sidebarShowByClass);
    if (sidebarShowByClass) {
      let btn = document.getElementById("btnappMobileSidebarToggler");
      btn.click();
    }
  }

  changepassword() {
    //let message:string = "Xử lý thành công!";
    //this.alertService.success(message);

    this.modalRef = this.modalService.show(DoiMatKhauComponent);
    // this.modalRef.content.onClose.subscribe(result => {
    //   console.log('results', result);
    // })
  }

  logout() {
    this.authenService
      .logout(localStorage.getItem("tokencode"))
      .subscribe((t) => {
        if (t != null && t.Success) {
          this.apiData = t;
          console.log("logout success, return to login form");
          localStorage.clear();
          this.router.navigate(["/login"]);
          location.reload();
        }
      });
  }
}
