import { Component, OnInit, Inject, OnDestroy } from "@angular/core";
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
// import { department } from './model';
import { Router } from '@angular/router';
import { bacsikhamService } from './bacsikham.service';
import { BaseComponent } from '../../shared/base.component';
import { ParamData } from "../../model/apidata.model";
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AppConfig } from "../../config/app.config";
import { ExcelService } from "../../core/excel.service ";
import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import { ChonNgayComponent } from '../../modules/chonngay/chonngay.dialog.component';
import { BsModalService, ModalDirective  } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import 'rxjs/Rx';
import { DOCUMENT } from '@angular/common';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from '../taichinh/getWeek';
import { LoadingBarService } from '@ngx-loading-bar/core';


@Component({
  selector: 'bacsikhamcomponent',
  templateUrl: 'bacsikham.component.html',
  styleUrls: ['bacsikham.component.css']
})
export class bacsikhamComponent extends BaseComponent implements OnInit, OnDestroy {
	bacsikhams: any;
	dataExt: any[] = [];
  dynamic: string;
  HaveData: Number;
  searchReport: SearchReport;
  pageSize: number = AppConfig.pageSize;
  currentpageSize: number = AppConfig.pageSize;
  curentSelectToNavigate: number;
  curPage: number = 1;
  start: number = 0;
  limit: number = 0;
//   total: number = 0;
//   tongsoTD: number = 0;
//   tongsoDK: number = 0;
//   tongsoCL: number = 0;
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
  //bacsikhamNow: bacsikham;
  interval;
  timeLeft: number = 10;
  IsAllowCustomDateRange: number = 0;

  constructor(
    @Inject(DOCUMENT) private document: any,
    public bacsikhamService: bacsikhamService,
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
    this.searchReport = new SearchReport();
    this.loadDatabacsikham();  
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
		this.loadDatabacsikham();
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
	this.loadDatabacsikham();
  }
  
  loadDatabacsikham() {    
	if(this.timefrom != undefined && this.timefrom != '')
		this.searchReport.CREATE_TIME_FROM = this.timefrom;
	else this.searchReport.CREATE_TIME_FROM = null;
	if(this.timeto != undefined && this.timeto != '')
		this.searchReport.CREATE_TIME_TO = this.timeto;
	else this.searchReport.CREATE_TIME_TO = null;
    this.loading = true;
    let paramData = new ParamData();  
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();    
	console.log(this.searchReport);
	this.HaveData = -1;
    this.bacsikhamService.loadbacsikham(this.searchReport, 1, paramData)
      .subscribe(t => {
        if (t != null && t.Success) {
          this.bacsikhams = t.Data.DATA_DETAIL.Report0;
    const departmentSeen = {};
	let indexDepartment = 0;

    this.dataExt = this.bacsikhams.map(x => {
		const departmentSpan = departmentSeen[x.DEPARTMENT_CODE] ? 0 :
        this.bacsikhams.filter(y => y.DEPARTMENT_CODE === x.DEPARTMENT_CODE).length;
		if(!departmentSeen[x.DEPARTMENT_CODE] )
		indexDepartment ++;

		departmentSeen[x.DEPARTMENT_CODE] = true;

      return { ...x, departmentSpan,indexDepartment };
    });
        //   this.total = 0;
          this.loading = false;
          if (this.bacsikhams != undefined && this.bacsikhams != null && this.bacsikhams.length > 0) {
            this.HaveData = 0;
			// this.total = this.bacsikhams.length;
			
			// this.tongsoTD = this.bacsikhams.reduce(function (accumulator, pilot) {
			// 	        return accumulator + pilot.COUNT_EXAM;
			// 	       }, 0);
			// this.tongsoDK = this.bacsikhams.reduce(function (accumulator, pilot) {
			// 	        return accumulator + pilot.COUNT_DONE_EXAM;
			// 	       }, 0);
			// this.tongsoCL = this.bacsikhams.reduce(function (accumulator, pilot) {
			// 	        return accumulator + (pilot.COUNT_EXAM - pilot.COUNT_DONE_EXAM);
			// 	       }, 0);
			
			// const dateMap  = this.bacsikhams
			// .reduce(
				// (m, v) => {
					// const department = v.EXECUTE_DEPARTMENT_NAME
					// const entry = m[department]
					// if (typeof entry === 'undefined') {
						// m[department] = [v] 
					// }
					// else {
						// entry.push(v)
					// }
					// return m
				// },
				// {}    
			// )

			// this.bacsikhamgroups = Object.keys(dateMap).map(d => {
				// return {
					// EXECUTE_DEPARTMENT_NAME: d,
					// bacsikhams: dateMap[d],
					// COUNT_EXAM: 0,
					// COUNT_DONE_EXAM:0,
					// COUNT_EXAM_BHYT:0,
					// COUNT_TREATMENT:0,
					// COUNT_TREATMENT_BHYT:0,
					// COUNT_TREATMENT_IN:0,
					// COUNT_TREATMENT_IN_BHYT:0,
					// COUNT_DONE_DIV:0,
				// }
			// })
			
			
			
			// this.bacsikhamgroups.forEach(md => { 		
				// md.COUNT_EXAM = md.bacsikhams.reduce(function (accumulator, pilot) {
				  // return accumulator + pilot.COUNT_EXAM;
				// }, 0);	
				// md.COUNT_DONE_EXAM = md.bacsikhams.reduce(function (accumulator, pilot) {
				  // return accumulator + pilot.COUNT_DONE_EXAM;
				// }, 0);
			// });			
			// this.total = this.bacsikhamgroups.length;
			// console.log(this.bacsikhamgroups);	
          }
          else {
            this.HaveData = 1;
          }
          console.log(this.bacsikhams);
        }else{
			this.HaveData = 1;
		}
      });
  }

  exportExcel() {
    if (this.bacsikhams != undefined && this.bacsikhams != null && this.bacsikhams.length > 0) {
      //Xuất excel
      this.excelService.exportAsExcelFile(this.bacsikhams, "testexportexcel");
    }
  }
  LoadSearch(searchPanel: any) {
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();
    //this.searchReport.Keyword = searchPanel.keyword;
    this.loadDatabacsikham();
  }
}
