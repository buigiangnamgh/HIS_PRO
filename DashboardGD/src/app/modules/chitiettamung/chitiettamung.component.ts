import { Component, OnInit, Inject, OnDestroy } from "@angular/core";
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
import { Router } from '@angular/router';
import { ChiTietTamUngService } from './chitiettamung.service';
import { BaseComponent } from '../../shared/base.component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AppConfig } from "../../config/app.config";
import { ExcelService } from "../../core/excel.service ";
import { ParamData } from "../../model/apidata.model";
import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import { reportDatas } from "../../model/report";
import 'rxjs/Rx';
import { DOCUMENT } from '@angular/common';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from './getWeek';
import { LoadingBarService } from '@ngx-loading-bar/core';
import { ChonNgayComponent } from '../chonngay/chonngay.dialog.component';
import { BsModalService, ModalDirective  } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';


@Component({
  selector: 'chitiettamungcomponent',
  templateUrl: 'chitiettamung.component.html',
  styleUrls: ['chitiettamung.component.css']
})
export class ChiTietTamUngComponent extends BaseComponent implements OnInit, OnDestroy {
  reportdatas: any;
  reportdatas1: any;
  dynamic: string;
  HaveData0: Number;
  HaveData1: Number;
  HaveData: Number;
  total0: number = 0;
  total1: number = 0;
  sumDEPOSIT_CASH: number = 0;
  searchReport: SearchReport;
  searchRangeDate: SearchRangeDate;
  modalRef: BsModalRef;
  pageSize: number = AppConfig.pageSize;
  currentpageSize: number = AppConfig.pageSize;
  curentSelectToNavigate: number;
  curPage: number = 1;
  start: number = 0;
  limit: number = 0;    
  loading: boolean;  
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
    public chitiettamungService: ChiTietTamUngService,
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
	this.HaveData1 = -10;
    this.searchReport = new SearchReport();
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
		this.loadDataAuto();
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
	this.loadData();
  }

  loadData() { 
	this.reportdatas1=null;
    this.loadDataAuto();
  }

  loadDataAuto() {    
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
	this.HaveData0 = -1;
	this.chitiettamungService.loadData(this.searchReport, 1, paramData)
	  .subscribe(t => {
		if (t != null && t.Success) {
		  this.reportdatas = t.Data.DATA_DETAIL.Report;
		  this.total0 = 0;
		  this.total1 = 0;
		  this.loading = false;
		  if (this.reportdatas != undefined && this.reportdatas != null && this.reportdatas.length > 0) {
			this.HaveData0 = 0;
			this.sumDEPOSIT_CASH = this.reportdatas.reduce(function (accumulator, pilot) {
				        return accumulator + pilot.DEPOSIT_CASH;
				       }, 0);
			this.total0 = this.reportdatas.length;
			//14/04/2020 14:35:00 
			 let dateMap  = this.reportdatas
			 .reduce(
				 (m, v) => {
					 
					 let TRANSACTION_DATE_STR = v.TRANSACTION_TIME_STR.substring(0,10)
					 
					 const entry = m[TRANSACTION_DATE_STR]
					 if (typeof entry === 'undefined') {
						 m[TRANSACTION_DATE_STR] = [v] 
					 }
					 else {
						 entry.push(v)
					 }
					 return m
				 },
				 {}    
			 );

			 this.reportdatas1 = Object.keys(dateMap).map(d => {
				 return {
				 TRANSACTION_DATE_STR: d,
				 datas: dateMap[d],
				 DEPOSIT_CASH: dateMap[d].reduce(function (accumulator, pilot) {
				        return accumulator + pilot.DEPOSIT_CASH;
				       }, 0)			 
				 }
			 })			 
		  }
		  else {
			this.HaveData0 = 1;
		  }
		  
		  console.log(this.reportdatas);
		}else{
			this.HaveData0 = 1;
		}
	  });
	}

  GetPatientTypeData(patienttypeId: number): string {
	  let ValueShow = "";
	  if(patienttypeId==1)
		  ValueShow="BHYT";
	  else
		ValueShow="Thu phí";
	  return ValueShow;
  }

  exportExcel() {
    if (this.reportdatas != undefined && this.reportdatas != null && this.reportdatas.length > 0) {
      //Xuất excel
      this.excelService.exportAsExcelFile(this.reportdatas, "testexportexcel");
    }
  }
  LoadSearch(searchPanel: any) {
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();
    //this.searchReport.Keyword = searchPanel.keyword;
    this.loadData();
  }
}
