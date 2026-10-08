import { Component, ViewChild, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { trungbinhkhamService } from './trungbinhkham.service';
import { MenuModel } from '../../model/menu.model';
import { BaseComponent } from '../../shared/base.component';
import { MenuService } from '../../containers/default-layout/menu.service';
import { reportDatas } from "../../model/report";
import { ReportIntegrate } from "../../model/ReportIntegrate";
import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from '../taichinh/getWeek';
import { AppConfig} from '../../config/app.config';
import { ChonNgayComponent } from '../../modules/chonngay/chonngay.dialog.component';
import { BsModalService, ModalDirective  } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ILoadedEventArgs, IMouseEventArgs, IPointRenderEventArgs, ChartComponent, ChartTheme, ChartAnnotationSettingsModel, AccumulationChartComponent, AccumulationChart, IAccLoadedEventArgs, AccumulationTheme, ScrollBar, SelectionMode, Series, IAccResizeEventArgs } from '@syncfusion/ej2-angular-charts';

import {
     AccumulationDataLabel
} from '@syncfusion/ej2-charts';
AccumulationChart.Inject(AccumulationDataLabel);

import { Browser } from '@syncfusion/ej2-base';
import { ParamData } from '../../model/apidata.model';

@Component({
  selector: 'trungbinhkhamcomponent',
  templateUrl: 'trungbinhkham.component.html',
  styleUrls: ['trungbinhkham.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class trungbinhkhamComponent extends BaseComponent implements OnInit, OnDestroy{
  public navItems : Array<MenuModel>;
  reportdatas0: any;
  public dnum: number;
  today: Date;
  Listyear: Array<number>;
  timefrom: string;
  timeto: string;
  dayto: string;
  monthto: string;
  yearto: string;
  dayfrom: string;
  monthfrom: string;
  yearfrom: string;
  
  SearchFor: number;// 1:ngày 2:tuần 3:tháng 4:năm   
  searchReport: SearchReport;
  searchRangeDate: SearchRangeDate;
  loading1:number;
  loading2:number;
  loading3:number;
  loading4:number;
  loading5:number;
  loading6:number;
  interval;
  timeLeft: number = 10;
  IsAllowCustomDateRange: number = 0;
  modalRef: BsModalRef;
  ReportOptionName: string;

	//begin chart so luong kham benh
	public dataslkb: Object[] = [
        { x: 'Phòng 1 (BH: 7, VP: 4)', y: 11 }, { x: 'Phòng 2 (BH: 8, VP: 1)', y: 9 }, { x: 'Phòng 3 (BH: 13, VP: 7)', y: 20 }, { x: 'Phòng 4 (BH: 11, VP: 13)', y: 24 },
        { x: 'Phòng 5 (BH: 11, VP: 16)', y: 27 }, { x: 'Phòng 6 (BH: 3, VP: 15)', y: 18 }, { x: 'Phòng 7 (BH: 11, VP: 18)', y: 29 }, { x: 'Phòng 8 (BH: 13, VP: 19)', y: 32 },
		{ x: 'Phòng 9 (BH: 11, VP: 12)', y: 23 }, { x: 'Phòng 10 (BH: 10, VP: 4)', y: 14 }, { x: 'Phòng 11 (BH: 12, VP: 18)', y: 30 }, { x: 'Phòng 12 (BH: 3, VP: 15)', y: 18 }
    ];    

   //lấy dữ liệu trung bình khám
   loadData() {    
    if(this.timefrom != undefined && this.timefrom != '')
      this.searchReport.TIME_FROM = this.timefrom;
    else this.searchReport.TIME_FROM = null;
    if(this.timeto != undefined && this.timeto != '')
      this.searchReport.TIME_TO = this.timeto;
    else this.searchReport.TIME_TO = null;
      let paramData = new ParamData();  
      if (this.searchReport === undefined || this.searchReport === null)
        this.searchReport = new SearchReport();    
    console.log(this.searchReport);
      this.trungbinhkhamService.loadData(this.searchReport, 1, paramData)
        .subscribe(t => {
          if (t != null && t.Success) {
            this.reportdatas0 = t.Data.DATA_DETAIL.Report;
            for (let index = 0; index < this.reportdatas0.length; index++) {
              const element = this.reportdatas0[index];
              if(element.COUNT>0)
              {
                element.SUM_TIME =element.SUM_TIME/element.COUNT/60;
              }
              
            }
            console.log(this.reportdatas0);
          }
          this.reportdatas0 = this.reportdatas0.sort((obj1, obj2) => {

            if (obj1.SUM_TIME > obj2.SUM_TIME) {
               return 1;
            }
         
            if (obj1.SUM_TIME < obj2.SUM_TIME) {
               return -1;
            }
         
            return 0;
         });
        });
    }
    
    //Initializing Primary X Axis
    public primaryXAxisslkb: Object = {
        valueType: 'Category',
        majorGridLines: { width: 0 }
    };
    //Initializing Primary Y Axis
    public primaryYAxisslkb: Object = {
        lineStyle: { width: 0},
        majorTickLines: {width: 0},
        labelFormat: '{value}',
        edgeLabelPlacement: 'Shift'
    };    
	public tooltipslkb: Object = {
        enable: true, format: 'SL: <b>${point.y} phút</b>'
    };
     // custom code start
    public loadslkb(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
     // custom code end
    public titleslkb: string = 'Trung bình khám';
    public chartAreaslkb: Object = {
        border: {
            width: 0
        }
    };
    public widthslkb: string = Browser.isDevice ? '100%' : '92%';
	//end chart so luong kham benh

  constructor(
    private route: ActivatedRoute,
    public trungbinhkhamService: trungbinhkhamService,	
	private menuService: MenuService,
	private modalService: BsModalService,
	public fb: FormBuilder,
	// private spinner: NgxSpinnerService,
    public router: Router) {
    super(router);
  }

  ngOnInit() {    
	// this.searchPanel = this.fb.group({
      // 'fromdate': [new Date(), ""],
	  // 'todate': [new Date(), ""]
    // });
	this.dnum=0;
	this.SearchFor =1;
	this.initDateRange();
  this.loadMenuProfile();
  this.loadData();
	this.IsAllowCustomDateRange = AppConfig.common.IsAllowCustomDateRangetrungbinhkham;
	if(AppConfig.common.TimerInterval > 0)
	{
		this.timeLeft = AppConfig.common.TimerInterval;
		this.startTimer();
	}
  }

  showSpinner(name: string) {
    //this.spinner.show(name);
  }

  hideSpinner(name: string) {
    //this.spinner.hide(name);
  }

  initDateRange(): void {	  
	this.searchReport = new SearchReport();
	const objectstore__searchReport = localStorage.getItem('objectstore__searchReport');    
	let isInit = false;
	if(objectstore__searchReport != undefined && objectstore__searchReport != null && objectstore__searchReport != '')
	{     
	   this.searchReport = JSON.parse(objectstore__searchReport);	  
	   if(this.searchReport != undefined && this.searchReport != null){
		    this.timeto = this.searchReport.TIME_TO;
			this.timefrom = this.searchReport.TIME_FROM;	
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


  ngOnDestroy(): void {
	clearInterval(this.interval);
  }

  startTimer() {
    this.interval = setInterval(() => {
      if(this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.timeLeft = AppConfig.common.TimerInterval;			 
		console.log('--------------------------------------------Load du lieu dashboard: ----------------------------------  sau '+ this.timeLeft +" giây sẽ tiếp tục tải lại.....");
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
 
}
