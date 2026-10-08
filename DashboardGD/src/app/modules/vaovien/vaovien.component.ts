import { Component, OnInit, Inject, OnDestroy } from "@angular/core";
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
import { Router } from '@angular/router';
import { VaoVienService } from './vaovien.service';
import { BaseComponent } from '../../shared/base.component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AppConfig } from "../../config/app.config";
import { ExcelService } from "../../core/excel.service ";
import { ParamData } from "../../model/apidata.model";
import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import { ChonNgayComponent } from '../../modules/chonngay/chonngay.dialog.component';
import { BsModalService, ModalDirective  } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { reportDatas } from "../../model/report";
import 'rxjs/Rx';
import { DOCUMENT } from '@angular/common';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from '../taichinh/getWeek';
import { LoadingBarService } from '@ngx-loading-bar/core';
import { MenuModel } from "../../model/menu.model";
import { MenuService } from "../../containers/default-layout/menu.service";


@Component({
  selector: 'vaoviencomponent',
  templateUrl: 'vaovien.component.html',
  styleUrls: ['vaovien.component.css']
})
export class VaoVienComponent extends BaseComponent implements OnInit, OnDestroy {
  reportdatas0: any;
  dynamic: string;
  HaveData0: Number;
ReportOptionName: string;
public navItems: Array<MenuModel>;
  HaveData: Number;
  total0: number = 0;
  sumCOUNT_EXAM: number = 0;
  sumCOUNT_EXAM_BHYT: number = 0;
  sumCOUNT_TREATMENT: number = 0;
  sumCOUNT_TREATMENT_BHYT: number = 0;
  sumCOUNT_TREATMENT_IN: number = 0;
  sumCOUNT_TREATMENT_IN_BHYT: number = 0;
  searchReport: SearchReport;
  pageSize: number = AppConfig.pageSize;
  currentpageSize: number = AppConfig.pageSize;
  curentSelectToNavigate: number;
  curPage: number = 1;
  start: number = 0;
  limit: number = 0;  
  loading: boolean;
  searchRangeDate: SearchRangeDate;
  modalRef: BsModalRef;
  
  dateweek: Date;
  TF: boolean;
  today: Date;
  Listyear: Array<number>;
  timefrom: string;
  timeto: string;
  SearchFor: number;// 1:ngày 2:tuần 3:tháng 4:năm  
  showDateFrom: string;
  showDateTo: string; 
  interval;
  timeLeft: number = 10;
  IsAllowCustomDateRange: number = 0;

  constructor(
    @Inject(DOCUMENT) private document: any,
    public vaovienService: VaoVienService,
    private menuService: MenuService,
    public router: Router,
    private loadingBar: LoadingBarService,
    private modalService: BsModalService,
    public excelService: ExcelService) {
    super(router);
  }

  ngOnInit() {
    this.startLoading();
    this.SearchFor =1;
	this.initDateRange();	
    this.HaveData = -10;
	this.HaveData0 = -10;
    this.searchReport = new SearchReport();
    this.loadMenuProfile();
    this.loadData();  
    this.stopLoading();
	this.IsAllowCustomDateRange = AppConfig.common.IsAllowCustomDateRange;
	if(AppConfig.common.TimerInterval > 0)
	{
		this.timeLeft = AppConfig.common.TimerInterval;
		this.startTimer();
	}
  }
  startLoading() {
    this.loadingBar.start();
  }

  stopLoading() {
    this.loadingBar.complete();
  }

  ngOnDestroy(): void {
	clearInterval(this.interval);
  }

  startTimer() {
    this.interval = setInterval(() => {
      if(this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.timeLeft = AppConfig.common.TimerInterval;			 
		console.log('--------------------------------------------Load du lieu loadDataAuto: ----------------------------------  sau '+ this.timeLeft +" giây sẽ tiếp tục tải lại.....");
		this.loadData();
      }
    },1000)
  }  
  
  
  onHidden(): void {
    console.log('Dropdown is hidden');
  }
  onShown(): void {
    console.log('Dropdown is shown');
  }
  isOpenChange(): void {
    console.log('Dropdown state is changed');
  }
  
  initDateRange(): void {	  
	let searchReport0 = new SearchReport();
	const objectstore__searchReport = localStorage.getItem('objectstore__searchReport');    
	let isInit = false;
	if(objectstore__searchReport != undefined && objectstore__searchReport != null && objectstore__searchReport != '')
	{     
	   searchReport0 = JSON.parse(objectstore__searchReport);	  
	   if(searchReport0 != undefined && searchReport0 != null){
		    this.timeto = searchReport0.TIME_TO;
			this.timefrom = searchReport0.TIME_FROM;	
			isInit = true;
	   }	   	   
	}
	if(isInit == false)
	{
		let now = new Date();
		this.timeto = ConvertdateToString(now, "235959");
		this.timefrom = ConvertdateToString(now, "000000");	
	}
	this.searchRangeDate =	this.convertDateStringToDateRangeNoStorage(this.timefrom, this.timeto);
  }
  
  loadMenuProfile() {
    const authorizeResultData = localStorage.getItem("AuthorizeResultData");
    if (
      authorizeResultData != undefined &&
      authorizeResultData != null &&
      authorizeResultData != ""
    ) {
      this.navItems = JSON.parse(authorizeResultData);
      var href = window.location.href;
      var currentNavItem = this.navItems.find((x) => href.endsWith("#")|| href.endsWith(x.url));
      if(currentNavItem !==undefined && currentNavItem != null){
        this.ReportOptionName = currentNavItem.name;
      }
      console.log(this.navItems);
    } else {
      this.navItems = new Array<MenuModel>();
      this.menuService.loadMenu().subscribe((tk) => {
        //console.log(tk);
        if (
          tk != null &&
          tk.Data != null &&
          tk.Data.ModuleInRoles != null &&
          tk.Data.ModuleInRoles.length > 0
        ) {
          tk.Data.ModuleInRoles.forEach((md) => {
            if (md.IS_VISIBLE === 1 && md.NUM_ORDER > 0) {
              let d = new MenuModel();
              d.number_order = md.NUM_ORDER;
              d.name = md.MODULE_NAME;
              //d.IS_VISIBLE = md.IS_VISIBLE;
              d.url = md.MODULE_LINK;
              d.icon = md.ICON_LINK;
              
              if(md.PARENT_ID ==undefined || md.PARENT_ID ==null || md.PARENT_ID <=0){
                d.isparenturl = 1;
              }
              d.cssname = md.MODULE_URL;
              d.reportdata = null; //TODO
              d.loading = 1; //TODO
              let x = this.navItems.filter((o) => o.url == md.MODULE_LINK);
              if (x.length <= 0) {
                this.navItems.push(d);
              }
            }
          });

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

        }
      });
    }
    var navItemChooses = this.navItems.filter((o) => o.number_order > 0);
    this.navItems = navItemChooses;
  }

  ClickSegment(value: number){
    var currentNavItemIndex = this.navItems.findIndex((x) =>x.name === this.ReportOptionName)-value;
    if(currentNavItemIndex !==undefined && currentNavItemIndex != null && currentNavItemIndex>=0 &&currentNavItemIndex<this.navItems.length){
      window.location.assign('#'+this.navItems[currentNavItemIndex].url)
    }
  }
  ChonNgayTuDen() {
    this.modalRef = this.modalService.show(ChonNgayComponent);
    //this.router.navigate(['../common/chonhoso'], { relativeTo: this.route });
    //this.router.navigateByUrl('../chonhoso');
    //this.router.navigateByUrl('chonhoso');
    //this.router.navigate(['/chonhoso']);
  }
  
  ClickSearch(value: number) {
    this.SearchFor = value;
    
	//TODO
	if (this.SearchFor == 0) {
      this.timefrom = '';
      this.timeto = '';
    }
    if (this.SearchFor == 1) {//Hôm nay
      let now = new Date();
      this.timefrom = ConvertdateToString(now, "000000");
      this.timeto = ConvertdateToString(now, "235959");
    }
	if (this.SearchFor == 2) {//Tuần này
      let now = new Date();
	  let now1 = new Date();
      now1.setDate(now1.getDate() - now1.getDay() + 1);
	  if(now1.getDate()<=now.getDate())
	  {
		  this.timeto = ConvertdateToString(now, "235959");	  
		  this.timefrom = ConvertdateToString(now1, "000000");
	  }
	  else
	  {
		  this.timeto = ConvertdateToString(now1, "235959");	  
		  this.timefrom = ConvertdateToString(now, "000000");
	  }
    }    
    if (this.SearchFor == 3) {//Tháng này
      let now = new Date();
	  let now1 = new Date();
      now.setFullYear(now.getFullYear(), now.getMonth(), 1);
	  this.timefrom = ConvertdateToString(now, "000000"); 
	  now1.setFullYear(now1.getFullYear(), now1.getMonth(), 1);
	  now1.setMonth(now1.getMonth() + 1);
	  now1.setDate(now1.getDate() - 1);
	  this.timeto = ConvertdateToString(now1, "235959");
	}
	if (this.SearchFor == 4) {//Năm nay
	  let now = new Date();
	  let now1 = new Date();
	  now.setFullYear(now.getFullYear(), 0, 1);
	  this.timefrom = ConvertdateToString(now, "000000");
	  now1.setFullYear(now1.getFullYear() + 1, 0, 1);
	  now1.setDate(now1.getDate() - 1);
	  this.timeto = ConvertdateToString(now1, "235959");
	}
	if (this.SearchFor == 5) {//Hôm qua
	  let now = new Date();
	  now.setDate(now.getDate() - 1);
	  this.timefrom = ConvertdateToString(now, "000000");
	  this.timeto = ConvertdateToString(now, "235959");
	}
	if (this.SearchFor == 6) {//Tháng trước
	  let now = new Date();
	  let now1 = new Date();
	  
	  if(now.getMonth() == 0){		  
		now.setFullYear(now.getFullYear() - 1, 11, 1);
		now1.setFullYear(now1.getFullYear() - 1, 11, 1);
	  }
	  else{
		now.setFullYear(now.getFullYear(), now.getMonth() - 1, 1);
		now1.setFullYear(now1.getFullYear(), now1.getMonth() - 1, 1);
	  }	  
      //now.setFullYear(now.getFullYear(), now.getMonth(), 1);
	  this.timefrom = ConvertdateToString(now, "000000"); 
	  //now1.setFullYear(now1.getFullYear(), now1.getMonth(), 1);
	  now1.setMonth(now1.getMonth() + 1);
	  now1.setDate(now1.getDate() - 1);
	  this.timeto = ConvertdateToString(now1, "235959");
	}
	if (this.SearchFor == 7) {//Năm trước
	  let now = new Date();
	  let now1 = new Date();
	  
	  now.setFullYear(now.getFullYear() - 1, 0, 1);
	  this.timefrom = ConvertdateToString(now, "000000");
	  
	  now1.setFullYear(now1.getFullYear(), 0, 1);
	  now1.setDate(now1.getDate() - 1);
	  this.timeto = ConvertdateToString(now1, "235959");
	}
	this.searchRangeDate =	this.convertDateStringToDateRange(this.timefrom, this.timeto);
	this.loadData();
  }
  
  loadData() {    
	if(this.timefrom != undefined && this.timefrom != '')
		this.searchReport.TIME_FROM = this.timefrom;
	else this.searchReport.TIME_FROM = null;
	if(this.timeto != undefined && this.timeto != '')
		this.searchReport.TIME_TO = this.timeto;
	else this.searchReport.TIME_TO = null;
    this.loading = true;
    let paramData = new ParamData();  
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();    
	console.log(this.searchReport);
	this.HaveData0 = -0;
    this.vaovienService.loadData(this.searchReport, 1, paramData)
      .subscribe(t => {
        if (t != null && t.Success) {
          this.reportdatas0 = t.Data.DATA_DETAIL.Report0;
          this.total0 = 0;     	
          this.loading = false;
          if (this.reportdatas0 != undefined && this.reportdatas0 != null && this.reportdatas0.length > 0) {
            this.HaveData0 = 0;
			this.total0 = this.reportdatas0.length;			 
			this.sumCOUNT_EXAM = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_EXAM??0);
				   }, 0);
			this.sumCOUNT_EXAM_BHYT = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_EXAM_BHYT??0);
				   }, 0);
			this.sumCOUNT_TREATMENT = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_TREATMENT??0);
				   }, 0);
			this.sumCOUNT_TREATMENT_BHYT = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_TREATMENT_BHYT??0);
				   }, 0);
			this.sumCOUNT_TREATMENT_IN = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_TREATMENT_IN??0);
				   }, 0);
			this.sumCOUNT_TREATMENT_IN_BHYT = this.reportdatas0.reduce(function (accumulator, pilot) {
					return accumulator + (pilot.COUNT_TREATMENT_IN_BHYT??0);
				   }, 0);	   
			// for (let index1 = 0; index1 < this.reportdatas0.length; index1++) {
				// this.totalExam += this.reportdatas0[index1].TONG;
				// this.totalBhyt += this.reportdatas0[index1].BHYT;
				// this.totalFee += this.reportdatas0[index1].VP;
			// }
          }
          else {
            this.HaveData0 = 1;
          }		  		  
		 
          console.log(this.reportdatas0);
        }else{
			this.HaveData0 = 1;
		}
      });
  }

  exportExcel() {
    if (this.reportdatas0 != undefined && this.reportdatas0 != null && this.reportdatas0.length > 0) {
      //Xuất excel
      this.excelService.exportAsExcelFile(this.reportdatas0, "testexportexcel");
    }
  }
  
  LoadSearch(searchPanel: any) {
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();
    //this.searchReport.Keyword = searchPanel.keyword;
    this.loadData();
  }
}
