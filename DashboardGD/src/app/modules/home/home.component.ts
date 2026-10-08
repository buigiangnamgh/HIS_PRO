import { Component, ViewChild, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators,
} from "@angular/forms";
import { HomeService } from "./home.service";
import { MenuModel } from "../../model/menu.model";
import { BaseComponent } from "../../shared/base.component";
import { MenuService } from "../../containers/default-layout/menu.service";
import { reportDatas } from "../../model/report";
import { ReportIntegrate } from "../../model/ReportIntegrate";
import { SearchReport, SearchRangeDate } from "../../model/Search.model";
import {
  getWeek,
  ConvertdateToString,
  addDays,
  MondayofWeek,
} from "../taichinh/getWeek";
import { AppConfig } from "../../config/app.config";
import { ChonNgayComponent } from "../../modules/chonngay/chonngay.dialog.component";
import { BsModalService, ModalDirective } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { BsLocaleService } from "ngx-bootstrap/datepicker";
import { listLocales } from "ngx-bootstrap/chronos";
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";
import { ParamData } from "../../model/apidata.model";
import { ILoadedEventArgs, IMouseEventArgs, IPointRenderEventArgs, ChartComponent, ChartTheme, ChartAnnotationSettingsModel, AccumulationChartComponent, AccumulationChart, IAccLoadedEventArgs, AccumulationTheme, ScrollBar, SelectionMode, Series, IAccResizeEventArgs } from '@syncfusion/ej2-angular-charts';
// import { NgxSpinnerService } from 'ngx-spinner';
import { Browser } from '@syncfusion/ej2-base';

import {
  AccumulationDataLabel
} from '@syncfusion/ej2-charts';
import { Size } from '@syncfusion/ej2-charts/src/sparkline/utils/helper';
AccumulationChart.Inject(AccumulationDataLabel);


@Component({
  selector: "homecomponent",
  templateUrl: "home.component.html",
  styleUrls: ["home.component.css"],
  host: {
    "(document:keydown)": "handleKeyboardEvents($event)",
  },
  encapsulation: ViewEncapsulation.None
})
export class TrangChuComponent
  extends BaseComponent
  implements OnInit, OnDestroy
{
  public navItems: Array<MenuModel>;
  reportdatas0: any;
  mainItems: Array<MenuModel>;
  groupItems: Array<Array<MenuModel>> ;
  public dnum: number;
  today: Date;
  timefrom: string;
  timeto: string;
  dayto: string;
  monthto: string;
  yearto: string;
  dayfrom: string;
  monthfrom: string;
  yearfrom: string;

  DefaultFromDateBedRatioShow: string;
  DefaultToDateBedRatioShow: string;
  timefromBedRatio: any;
  timetoBedRatio: any;
  currentPrecious: any;
  currentYear: any;
  Listyear: Array<number>;

  searchPanel: FormGroup;
  SearchFor: number; // 1:ngày 2:tuần 3:tháng 4:năm
  searchReport: SearchReport;
  searchRangeDate: SearchRangeDate;
  loading1: number;
  loading2: number;
  loading3: number;
  loading4: number;
  loading5: number;
  loading6: number;
  interval;
  timeLeft: number = 10;
  IsAllowCustomDateRange: number = 0;
  modalRef: BsModalRef;
  HaveData: number;
  BedRatioData: any;
  thoigianthongke: string;
  loading0: boolean;
  listAllowReportTemplateCode: string =
    AppConfig.common.ListAllowReportTemplateCode;

  constructor(
    private route: ActivatedRoute,
    public homeService: HomeService,
    private menuService: MenuService,
    private modalService: BsModalService,
    private localeService: BsLocaleService,
    public fb: FormBuilder,
    public router: Router
  ) {
    super(router);
  }

  ngOnInit() {
    this.searchPanel = this.fb.group({
      //'keyword': ['', ""],
    });
    this.dnum = 0;
    this.SearchFor = 1;
    this.HaveData = -1;
    this.loading0 = false;
    this.initDateRange();
    // let now = new Date();
    // this.timeto = ConvertdateToString(now, "235959");
    // this.timefrom = ConvertdateToString(now, "000000");
    // this.setDateNumberToStringShow(now, now);
    this.setDatepickerLanguage();
    this.loadMenuProfile();
    this.IsAllowCustomDateRange =
      AppConfig.common.IsAllowCustomDateRangeHomePage;
    if (AppConfig.common.TimerInterval > 0) {
      this.timeLeft = AppConfig.common.TimerInterval;
      this.startTimer();
    }
  }

  showSpinner(name: string) {
    // this.spinner.show(name);
  }

  hideSpinner(name: string) {
    // this.spinner.hide(name);
  }

  handleKeyboardEvents = function (event) {
    this.key = event.which || event.keyCode;
    console.log("this key is: " + this.key);
    if (event.key == "F9") {
      console.log("this key is: F9");
      this.menuService.RefeshCacheServer().subscribe();
      window.location.reload();
    }
  };

  initDateRange(): void {
    this.searchReport = new SearchReport();
    const objectstore__searchReport = localStorage.getItem(
      "objectstore__searchReport"
    );
    let isInit = false;
    if (
      objectstore__searchReport != undefined &&
      objectstore__searchReport != null &&
      objectstore__searchReport != ""
    ) {
      this.searchReport = JSON.parse(objectstore__searchReport);
      if (this.searchReport != undefined && this.searchReport != null) {
        this.timeto = this.searchReport.TIME_TO;
        this.timefrom = this.searchReport.TIME_FROM;
        isInit = true;
      }
    }
    if (isInit == false) {
      let now = new Date();
      this.timeto = ConvertdateToString(now, "235959");
      this.timefrom = ConvertdateToString(now, "000000");
    }
    this.searchRangeDate = this.convertDateStringToDateRangeNoStorage(
      this.timefrom,
      this.timeto
    );

    let now1 = new Date();
    this.currentPrecious = "1";
    this.currentYear = now1.getFullYear();
    this.Listyear = new Array<number>();
    let i = now1.getFullYear();
    while (i != 1900) {
      this.Listyear.push(i);
      i--;
    }
    this.changePrecious(this.currentPrecious);
  }

  public loadslkb(args: ILoadedEventArgs): void {
    let selectedTheme: string = location.hash.split('/')[1];
    selectedTheme = selectedTheme ? selectedTheme : 'Material';
    args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
};

//begin chart so luong kham benh
public titleslkb: string = "BIỂU ĐỒ THỜI GIAN TRUNG BÌNH KHÁM";
public tooltipslkb: string = "BIỂU ĐỒ THỜI GIAN TRUNG BÌNH KHÁM";
//begin chart so luong kham benh
public titleslIcd: string = "BIỂU ĐỒ TÌNH HÌNH BỆNH TẬT";
public tooltipslIcd: string = "BIỂU ĐỒ TÌNH HÌNH BỆNH TẬT";

public titleslTrongBuong: string = "BUỒNG ĐIỀU TRỊ NGOẠI TRÚ";
public tooltipslTrongBuong: string = "BUỒNG ĐIỀU TRỊ NGOẠI TRÚ";
//begin chart so luong kham benh
public titleslVao: string = "VÀO VIỆN ĐIỀU TRỊ NGOẠI TRÚ";
public tooltipslVao: string = "VÀO VIỆN ĐIỀU TRỊ NGOẠI TRÚ";

public dataslIcd: any;

public dataslTrongBuong: any;

public dataslVao: any;

public primaryXAxisslkb: Object = {
  valueType: 'Category',
  majorGridLines: { width: 0 }
};  
public primaryYAxisslkb: Object = {
  lineStyle: { width: 0},
  majorTickLines: {width: 0},
  labelFormat: '{value}',
  edgeLabelPlacement: 'Shift'
};    
public primaryXAxisslIcd: Object = {
  valueType: 'Category',
  majorGridLines: { width: 0 }
};
//Initializing Primary Y Axis
public primaryYAxisslIcd: Object = {
  lineStyle: { width: 0},
  majorTickLines: {width: 0},
  labelFormat: '{value}',
  edgeLabelPlacement: 'Shift'
};    
   
public primaryXAxisslTrongBuong: Object = {
  valueType: 'Category',
  majorGridLines: { width: 0 }
};  
public primaryYAxisslTrongBuong: Object = {
  lineStyle: { width: 0},
  majorTickLines: {width: 0},
  labelFormat: '{value}',
  edgeLabelPlacement: 'Shift'
};    
public primaryXAxisslVao: Object = {
  valueType: 'Category',
  majorGridLines: { width: 0 }
};
//Initializing Primary Y Axis
public primaryYAxisslVao: Object = {
  lineStyle: { width: 0},
  majorTickLines: {width: 0},
  labelFormat: '{value}',
  edgeLabelPlacement: 'Shift'
};  
//Initializing Legend
public legendSettings0: Object = {
  visible: false,
  shapeHeight: 15, shapeWidth: 15,
  alignment:'Far',position:'Bottom',
  width: '250', height: '85', border: { width: 1, color: 'pink'},

};
public tooltip0: Object = {
  enable: true, format: '${point.x} : <b>${point.y}%</b>'
};
public explode: boolean = true;
public explodeOffset: string = '10%';
public enableSmartLabels: boolean  = true;
public enableAnimation: boolean = true;
public titleStyle: object = {  
  color: "#267326",
  size: '24.5px'
}
public legendSettings: object = {
  position:'bottom' ,alignment:'Near'
}
public startAngle0: number = 0;
public endAngle0: number = 360;
//Initializing Datalabel
public dataLabel: Object = {
  visible: true,  
  //template: "<div style='color: aqua;' id='dataLabelTemplate'> Số tiền: {pSUM_TIEN_MAT} % <br/> 100000 </div>",
  position: 'Inside',maxWidth:230 ,
  textWrap:'Wrap', 
  name: 'text',
  enableRotation:true,
  font: {
    fontWeight: '900',
    color: '#ffffff', 
    size: '15px'
  }
};
public title0: string = 'TỶ LỆ BỆNH NHÂN VÀO/RA';
public title1: string = 'TỶ LỆ HÌNH THỨC THU TIỀN';
	//begin chart hoat dong phau thuat thu thuat
	public data0: Object[] = [];

public data1: Object[];
// custom code start
public onLoad0(args: IAccLoadedEventArgs): void {
  let selectedTheme: string = location.hash.split('/')[1];
  selectedTheme = selectedTheme ? selectedTheme : 'Material';
  args.accumulation.theme = <AccumulationTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
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
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.timeLeft = AppConfig.common.TimerInterval;
        console.log(
          "--------------------------------------------Load du lieu dashboard: ----------------------------------  sau " +
            this.timeLeft +
            " giây sẽ tiếp tục tải lại....."
        );
        this.setReportDataMenu();
      }
    }, 1000);
  }

  public chartAreaslkb: Object = {
    border: {
        width: 0
    }
};

public chartAreaslIcd: Object = {
  border: {
      width: 0
  }
};

// khai bao bien
public khambenh__SUM_COUNT_EXAM : number = 0;
public khambenh__SUM_COUNT_CHUYENVIEN : number = 0;
public khambenhyeucau__SUM_TONG : number = 0;
public khambenhyeucau__TOTAL_PRICE : number = 0;
public khamcapcuu__SUM_TONG = 0;
public dangnamvien__SUM = 0;
public dangnamvien__SUM_VP = 0;
public dangnamvien__SUM_BHYT = 0;
public vaovien__SUM_COUNT_TREATMENT_IN_BHYT : number;
public vaovien__SUM_VP : number;
public ravien__SUM_COUNT_EXP : number  = 0;
public ravien__SUM_CVIEN = 0;
public ravien__SUM_RAVIEN = 0;
public ravien__SUM_TUVONG = 0;
public ravien__SUM_NANGVE = 0;
public ravien__SUM_COUNT_OUT_CV_LEN = 0;
public ravien__SUM_KHAC= 0;
public doanhthu__SUM = 0;
public doanhthu__SUM_TT_RA_VIEN = 0;
public doanhthu__SUM_THU_TRUC_TIEP = 0;
public doanhthu__SUM_TAM_UNG = 0;
public khambenh__SUM_NHA_THUOC = 0;
public cls__SUM_MRI = 0;
public cls__SUM_MRI_DXL = 0;
public cls__SUM_MRI_HT = 0;
public cls__SUM_CT = 0;
public cls__SUM_CT_DXL = 0;
public cls__SUM_CT_HT = 0;
public cls__SUM_XQ = 0;
public cls__SUM_XQ_DXL = 0;
public cls__SUM_XQ_HT = 0;
public cls__SUM_NS = 0;
public cls__SUM_NS_DXL = 0;
public cls__SUM_NS_HT = 0;
public cls__SUM_SA = 0;
public cls__SUM_SA_DXL = 0;
public cls__SUM_SA_HT = 0;
public donmau__SUM_AMOUNT = 0;
public donmau__SUM_AMOUNT_DXL = 0;
public donmau__SUM_AMOUNT_HT = 0;
public xn__SH = 0;
public xn__SH_DXL = 0;
public xn__SH_HT = 0;
public xn__HH = 0;
public xn__HH_DXL = 0;
public xn__HH_HT = 0;
public xn__VS = 0;
public xn__VS_DXL = 0;
public xn__VS_HT = 0;
public xn__SHPT = 0;
public xn__SHPT_DXL = 0;
public xn__SHPT_HT = 0;
public xn__GPB = 0;
public xn__GPB_DXL = 0;
public xn__GPB_HT = 0;
public pt__SUM = 0;
public pt__SUM_CC = 0;
public tt__SUM = 0;
public tt__SUM_DXL = 0;
public tt__SUM_HT = 0;
public npm__SUM = 0;
public npm__SUM_DXL = 0;
public npm__SUM_HT = 0;
public ct__TM = 0;
public ct__TM_DXL = 0;
public ct__TM_HT = 0;
public ct__DQ = 0;
public ct__DQ_DXL = 0;
public ct__DQ_HT = 0;
public ct__XQ = 0;
public ct__XQ_DXL = 0;
public ct__XQ_HT = 0;



public widthslkb: string = Browser.isDevice ? '100%' : '92%';
public widthslIcd: string = Browser.isDevice ? '100%' : '92%';

setReportDataMenu(){
  this.LoadKhamBenh();
  this.LoadChuyenVien();
  this.LoadKhamBenhYeuCau();
  this.LoadKhamCapCuu();
  this.LoadDangNamVien();
  this.LoadVaoVien();
  this.LoadRaVien();
  this.LoadDoanhThu();
  this.LoadNhaThuoc();
  this.LoadHinhThucThu();
  // this.LoadTiLeVaoRaVien();
  this.LoadCls();
  this.LoadDonMau();
  this.LoadXNGPB();
  this.LoadPttt();
  this.LoadCthiep();
  this.loadTbKham();
  this.loadTopIcd();
  this.loadTrongBuong();
  this.loadVao();
}

LoadKhamBenh()  {
  console.log("loading khambenh " + new Date());
  let srpt1 = new SearchReport();
  srpt1.TIME_TO = this.timeto;
  srpt1.TIME_FROM = this.timefrom;
  srpt1.ReportTypeCode = "MRS00002";
  srpt1.ReportTemplateCode = "MRS0000201";
  this.homeService.loadDataReport(srpt1).subscribe((tk) => {
           
    if (
      tk != undefined &&
      tk != null &&
      tk.Success &&
      tk.Data != undefined &&
      tk.Data.DATA_DETAIL != undefined &&
      tk.Data.DATA_DETAIL.Department != undefined &&
      tk.Data.DATA_DETAIL.Department.length > 0
    ) {
      this.khambenh__SUM_COUNT_EXAM = tk.Data.DATA_DETAIL.Department.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.COUNT_EXAM;
      },
      0);
    }
  });
}

LoadChuyenVien()  {
  console.log("loading chuyenvien " + new Date());
  let srpt1 = new SearchReport();
  srpt1.TIME_TO = this.timeto;
  srpt1.TIME_FROM = this.timefrom;
  this.menuService.loadDataChuyenVien(srpt1).subscribe((tk) => {
           
    if (
      tk != undefined &&
      tk != null &&
      tk.Success &&
      tk.Data != undefined &&
      tk.Data.DATA_DETAIL != undefined &&
      tk.Data.DATA_DETAIL.Report0 != undefined &&
      tk.Data.DATA_DETAIL.Report0.length > 0
    ) {
      this.khambenh__SUM_COUNT_CHUYENVIEN = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.COUNT_CHUYENVIEN;
      },
      0);
    }
  });
}

LoadKhamBenhYeuCau()  {
  console.log("loading KhamBenhYeuCau " + new Date());
  // this.showSpinner("spinner/khambenh");
 let srpt2 = new SearchReport();
				srpt2.CREATE_TIME_TO = this.timeto;
				srpt2.CREATE_TIME_FROM = this.timefrom;
				srpt2.ReportTypeCode = "TKB12234";
				srpt2.ReportTemplateCode = "TKB1223401";
				this.homeService.loadDataReport(srpt2).subscribe(TKB12234 => {
					if (TKB12234 != undefined && TKB12234 != null && TKB12234.Success && TKB12234.Data != undefined && TKB12234.Data.DATA_DETAIL != undefined && ((TKB12234.Data.DATA_DETAIL.Report0 != undefined && TKB12234.Data.DATA_DETAIL.Report0.length > 0) || (TKB12234.Data.DATA_DETAIL.Report0 != undefined && TKB12234.Data.DATA_DETAIL.Report0.length > 0))) {
						let reportdatas0 = TKB12234.Data.DATA_DETAIL.Report0;

						if (reportdatas0 != undefined && reportdatas0.length > 0) {

							this.khambenhyeucau__SUM_TONG = reportdatas0.reduce(function (accumulator, pilot) {
								return accumulator + pilot.AMOUNT;
							}, 0);
						}	
					
					}
					if (TKB12234 != undefined && TKB12234 != null && TKB12234.Success && TKB12234.Data != undefined && TKB12234.Data.DATA_DETAIL != undefined && ((TKB12234.Data.DATA_DETAIL.Report4 != undefined && TKB12234.Data.DATA_DETAIL.Report4.length > 0) || (TKB12234.Data.DATA_DETAIL.Report4 != undefined && TKB12234.Data.DATA_DETAIL.Report4.length > 0))) {
						let reportdatas0 = TKB12234.Data.DATA_DETAIL.Report4;

						if (reportdatas0 != undefined && reportdatas0.length > 0) {

							this.khambenhyeucau__TOTAL_PRICE = reportdatas0.reduce(function (accumulator, pilot) {
								return accumulator + pilot.TOTAL_PRICE;
							}, 0);
						}				
					}
				});
}

LoadKhamCapCuu()  {
  console.log("loading khamcapcuu " + new Date());
        let srpt2 = new SearchReport();
        srpt2.CREATE_TIME_TO = this.timeto;
        srpt2.CREATE_TIME_FROM = this.timefrom;
        srpt2.ReportTypeCode = "TKB00021";
        srpt2.ReportTemplateCode = "TKB0002101";
        this.homeService.loadDataReport(srpt2).subscribe((tktkb21) => {
          
          if (
            tktkb21 != undefined &&
            tktkb21 != null &&
            tktkb21.Success &&
            tktkb21.Data != undefined &&
            tktkb21.Data.DATA_DETAIL != undefined &&
            ((tktkb21.Data.DATA_DETAIL.Report0 != undefined &&
              tktkb21.Data.DATA_DETAIL.Report0.length > 0) ||
              (tktkb21.Data.DATA_DETAIL.Report2 != undefined &&
                tktkb21.Data.DATA_DETAIL.Report2.length > 0))
          ) {
            let reportdatas0 = tktkb21.Data.DATA_DETAIL.Report0;
            if (reportdatas0 != undefined && reportdatas0.length > 0) {
              this.khamcapcuu__SUM_TONG = reportdatas0.reduce(function (accumulator, pilot) {
                return accumulator + pilot.TONG;
              }, 0);
            }
          }
        });
}

LoadDangNamVien()  {
  console.log("loading dangnamvien " + new Date());
        let srpt3 = new SearchReport();
        srpt3.TIME_FROM = this.timefrom;
        srpt3.TIME_TO = this.timeto;
        srpt3.ReportTypeCode = "MRS00555";
        srpt3.ReportTemplateCode = "MRS0055501";
        this.homeService.loadDataReport(srpt3).subscribe((tk) => {
          let SUM_COUNT_END = 0;
          let SUM_BHYT_DT = 0;
          let SUM_BHYT_TT = 0;
          
          if (
            tk != undefined &&
            tk != null &&
            tk.Success &&
            tk.Data != undefined &&
            tk.Data.DATA_DETAIL != undefined &&
            tk.Data.DATA_DETAIL.Report != undefined &&
            tk.Data.DATA_DETAIL.Report.length > 0
          ) {
            let report0 = tk.Data.DATA_DETAIL.Report;
            SUM_COUNT_END = report0.reduce(function (accumulator, pilot) {
              return accumulator + pilot.COUNT_END;
            }, 0);
            SUM_BHYT_DT = report0.reduce(function (accumulator, pilot) {
              return accumulator + pilot.COUNT_END_BHDT;
            }, 0);
            SUM_BHYT_TT = report0.reduce(function (accumulator, pilot) {
              return accumulator + pilot.COUNT_END_BHTT;
            }, 0);
            this.dangnamvien__SUM_VP = report0.reduce(function (accumulator, pilot) {
              return accumulator + pilot.COUNT_END_VP;
            }, 0);
          }

          this.dangnamvien__SUM = SUM_COUNT_END;
          this.dangnamvien__SUM_BHYT = SUM_COUNT_END - this.dangnamvien__SUM_VP;
        });
}

LoadVaoVien()  {
  console.log("loading vaovien " + new Date());
        let srpt464 = new SearchReport();
        srpt464.TIME_TO = this.timeto;
        srpt464.TIME_FROM = this.timefrom;
        srpt464.ReportTypeCode = "MRS00002";
        srpt464.ReportTemplateCode = "MRS0000206";
        this.homeService.loadDataReport(srpt464).subscribe((tk) => {
          let SUM_COUNT_TREATMENT_IN = 0;
       
          let SUM_TUNGUYEN = 0;
          if (
            tk != undefined &&
            tk != null &&
            tk.Data != undefined &&
            tk.Data.DATA_DETAIL != undefined &&
            tk.Data.DATA_DETAIL.Report0 != undefined &&
            tk.Data.DATA_DETAIL.Report0.length > 0
          ) {
            let reportdatas0 = tk.Data.DATA_DETAIL.Report0;
            SUM_COUNT_TREATMENT_IN = reportdatas0.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_TREATMENT_IN;
            },
            0);
            this.data0 = [{ 'x': 'Vào viện: '+SUM_COUNT_TREATMENT_IN, y: SUM_COUNT_TREATMENT_IN, text: 'Vào viện: ' + SUM_COUNT_TREATMENT_IN}];
            this.vaovien__SUM_COUNT_TREATMENT_IN_BHYT = reportdatas0.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_TREATMENT_IN_BHYT;
            },
            0);
            this.vaovien__SUM_VP = SUM_COUNT_TREATMENT_IN - this.vaovien__SUM_COUNT_TREATMENT_IN_BHYT;
          }
        });
}

LoadRaVien()  {
  console.log("loading ravien " + new Date());
        let srpt4 = new SearchReport();
        srpt4.TIME_TO = this.timeto;
        srpt4.TIME_FROM = this.timefrom;
        srpt4.ReportTypeCode = "MRS00555";
        srpt4.ReportTemplateCode = "MRS0055509";
        this.homeService.loadDataReport(srpt4).subscribe((tk) => {
          if (
            tk != undefined &&
            tk != null &&
            tk.Success &&
            tk.Data != undefined &&
            tk.Data.DATA_DETAIL != undefined &&
            tk.Data.DATA_DETAIL.Report != undefined &&
            tk.Data.DATA_DETAIL.Report.length > 0
          ) {
            this.ravien__SUM_COUNT_EXP = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_EXP;
            },
            0);
            const objO = this.data0[0];
            const obj  = { 'x': 'Ra viện: '+this.ravien__SUM_COUNT_EXP, y: Math.round(this.ravien__SUM_COUNT_EXP*100/(objO["y"]+this.ravien__SUM_COUNT_EXP)), text: 'Ra viện: '+ this.ravien__SUM_COUNT_EXP};
            
            objO["y"] = Math.round(objO["y"]*100/(objO["y"]+this.ravien__SUM_COUNT_EXP));
            this.data0=[objO,obj];

            this.ravien__SUM_RAVIEN = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_OUT_RAVIEN;
            },
            0);
            this.ravien__SUM_CVIEN = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return (
                accumulator + (pilot.COUNT_OUT_CV - pilot.COUNT_OUT_CV_LEN)
              );
            },
            0);
            this.ravien__SUM_COUNT_OUT_CV_LEN = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_OUT_CV_LEN;
            },
            0);
          
            this.ravien__SUM_TUVONG = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_DIE;
            },
            0);
            this.ravien__SUM_NANGVE = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return accumulator + pilot.COUNT_OUT_NANG;
            },
            0);
            this.ravien__SUM_KHAC = tk.Data.DATA_DETAIL.Report.reduce(function (
              accumulator,
              pilot
            ) {
              return (
                accumulator +
                (pilot.COUNT_EXP -
                  pilot.COUNT_DIE -
                  pilot.COUNT_OUT_CV -
                  pilot.COUNT_OUT_RAVIEN -
                  pilot.COUNT_OUT_NANG)
              );
            },
            0);
          }
        });
}

LoadDoanhThu()  {
  console.log("loading taichinh " + new Date());

          let SUM_TT_RA_VIEN = 0;
          let SUM_THU_TRUC_TIEP = 0;
          let SUM_TAM_UNG = 0;
          this.showSpinner("spinner/taichinh");
          let srpt5 = new SearchReport();
          srpt5.TIME_TO = this.timeto;
          srpt5.TIME_FROM = this.timefrom;
          srpt5.ReportTypeCode = "TKB00036";
          srpt5.ReportTemplateCode = "TKB0003601";
          this.homeService.loadDataReport(srpt5).subscribe((tk) => {
            if (
              tk != undefined &&
              tk != null &&
              tk.Success &&
              tk.Data != undefined &&
              tk.Data.DATA_DETAIL != undefined &&
              tk.Data.DATA_DETAIL.Report0 != undefined &&
              tk.Data.DATA_DETAIL.Report0.length > 0
            ) {
              let report1 = tk.Data.DATA_DETAIL.Report0;
              if (
                report1 != undefined &&
                report1 != null &&
                report1.length > 0
              ) {
                this.doanhthu__SUM_TT_RA_VIEN = report1.reduce(function (accumulator, pilot) {
                  return (
                    accumulator +
                    Number(pilot.TOTAL_HEIN_PRICE) +
                    Number(pilot.TOTAL_PATIENT_PRICE)
                  );
                }, 0);

                //console.log('loaded report data: MRS00012TKB0003601 SUM_TT_RA_VIEN'+SUM_TT_RA_VIEN);
              }
            }

            let srpt6 = new SearchReport();
            srpt6.TIME_TO = this.timeto;
            srpt6.TIME_FROM = this.timefrom;
            srpt6.ReportTypeCode = "TKB00016";
            srpt6.ReportTemplateCode = "TKB0001602";
            this.homeService.loadDataReport(srpt6).subscribe((tk1) => {
              if (
                tk1 != undefined &&
                tk1 != null &&
                tk1.Success &&
                tk1.Data != undefined &&
                tk1.Data.DATA_DETAIL != undefined &&
                tk1.Data.DATA_DETAIL.Report1 != undefined &&
                tk1.Data.DATA_DETAIL.Report1.length > 0
              ) {
                let report2 = tk1.Data.DATA_DETAIL.Report1;
                if (
                  report2 != undefined &&
                  report2 != null &&
                  report2.length > 0
                ) {
                  this.doanhthu__SUM_THU_TRUC_TIEP = report2.reduce(function (
                    accumulator,
                    pilot
                  ) {
                    return accumulator + pilot.TOTAL_PRICE;
                  },
                  0);
                }
              }

              let srpt7 = new SearchReport();
              srpt7.TIME_TO = this.timeto;
              srpt7.TIME_FROM = this.timefrom;
              srpt7.ReportTypeCode = "MRS00482";
              srpt7.ReportTemplateCode = "MRS0048204";
              this.homeService.loadDataReport(srpt7).subscribe((tk2) => {
                if (
                  tk2 != undefined &&
                  tk2 != null &&
                  tk2.Success &&
                  tk2.Data != undefined &&
                  tk2.Data.DATA_DETAIL != undefined &&
                  tk2.Data.DATA_DETAIL.Report != undefined &&
                  tk2.Data.DATA_DETAIL.Report.length > 0
                ) {
                  let report3 = tk2.Data.DATA_DETAIL.Report;
                  if (
                    report3 != undefined &&
                    report3 != null &&
                    report3.length > 0
                  ) {
                    this.doanhthu__SUM_TAM_UNG = report3.reduce(function (accumulator, pilot) {
                      return accumulator + pilot.DEPOSIT_CASH;
                    }, 0);
                    console.log("loaded taichinh " + new Date());
                  }
                }
                this.hideSpinner("spinner/taichinh");
                this.doanhthu__SUM = this.doanhthu__SUM_THU_TRUC_TIEP + this.doanhthu__SUM_TT_RA_VIEN;
               
              });
            });
          });
}

LoadNhaThuoc()  {
  console.log("loading NhaThuoc " + new Date());
  let srpt1 = new SearchReport();
  srpt1.TIME_TO = this.timeto;
  srpt1.TIME_FROM = this.timefrom;
  this.menuService.loadDataNhaThuoc(srpt1).subscribe((tk) => {
           
    if (
      tk != undefined &&
      tk != null &&
      tk.Success &&
      tk.Data != undefined &&
      tk.Data.DATA_DETAIL != undefined &&
      tk.Data.DATA_DETAIL.Report0 != undefined &&
      tk.Data.DATA_DETAIL.Report0.length > 0
    ) {
      this.khambenh__SUM_NHA_THUOC = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.AMOUNT;
      },
      0);
    }
  });
}

LoadHinhThucThu()  {
  console.log("loading HinhThucThu " + new Date());
  let srpt1 = new SearchReport();
  srpt1.TIME_TO = this.timeto;
  srpt1.TIME_FROM = this.timefrom;
  this.menuService.loadDataHinhThucThu(srpt1).subscribe((tk) => {
    let SUM_TIEN_MAT = 0;
    let SUM_QR = 0;
    let SUM_CK = 0;     
    if (
      tk != undefined &&
      tk != null &&
      tk.Success &&
      tk.Data != undefined &&
      tk.Data.DATA_DETAIL != undefined &&
      tk.Data.DATA_DETAIL.Report0 != undefined &&
      tk.Data.DATA_DETAIL.Report0.length > 0
    ) {
      SUM_TIEN_MAT = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.SUM_TIEN_MAT;
      },
      0);
      SUM_CK = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.SUM_CK;
      },
      0);
      SUM_QR = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.SUM_QR;
      },
      0);
      let pSUM_TIEN_MAT = Math.round(SUM_TIEN_MAT*100/(SUM_TIEN_MAT+SUM_CK+SUM_QR));
    let pSUM_QR = Math.round(SUM_QR*100/(SUM_TIEN_MAT+SUM_CK+SUM_QR));
    let pSUM_CK = Math.round(SUM_CK*100/(SUM_TIEN_MAT+SUM_CK+SUM_QR));
  this.data1 =  [
    { 'x': 'Tiền mặt: '+this.ConvertToStringShow(SUM_TIEN_MAT), y: pSUM_TIEN_MAT,  text: 'TM: ' + pSUM_TIEN_MAT + '% ' +this.ConvertToStringShow(SUM_TIEN_MAT)  },
    { 'x': 'Chuyển khoản: '+this.ConvertToStringShow(SUM_CK), y: pSUM_CK, text: 'CK: '+ pSUM_CK+'% ' +this.ConvertToStringShow(SUM_CK)},
    { 'x': 'QR code: '+this.ConvertToStringShow(SUM_QR), y: pSUM_QR, text: 'QR code: ' + pSUM_QR+'% ' +this.ConvertToStringShow(SUM_QR)},  
  ];
    }
  });
}

LoadCls(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataCls(this.searchReport, 1, paramData)
          .subscribe((t) => {
            if (t != null && t.Success) {
              this.cls__SUM_MRI = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_MRI === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_MRI_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_MRI === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_MRI_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_MRI === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_CT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CLVT32 === 1 || o.IS_CLVT64 === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_CT_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => (o.IS_CLVT32 === 1 || o.IS_CLVT64 === 1) && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_CT_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => (o.IS_CLVT32 === 1 || o.IS_CLVT64 === 1) && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_XQ = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_XQ === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_XQ_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_XQ === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_XQ_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_XQ === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_SA = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_SA === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_SA_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_SA === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_SA_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_SA === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_NS = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_NSTH === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_NS_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_NSTH === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.cls__SUM_NS_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_NSTH === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
            }
          });
}

LoadDonMau()  {
  console.log("loading DonMau " + new Date());
  let srpt1 = new SearchReport();
  srpt1.TIME_TO = this.timeto;
  srpt1.TIME_FROM = this.timefrom;
  this.menuService.loadDataDonMau(srpt1).subscribe((tk) => {
           
    if (
      tk != undefined &&
      tk != null &&
      tk.Success &&
      tk.Data != undefined &&
      tk.Data.DATA_DETAIL != undefined &&
      tk.Data.DATA_DETAIL.Report0 != undefined &&
      tk.Data.DATA_DETAIL.Report0.length > 0
    ) {
      this.donmau__SUM_AMOUNT = tk.Data.DATA_DETAIL.Report0.reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + pilot.AMOUNT;
      },
      0);
      this.donmau__SUM_AMOUNT_DXL = tk.Data.DATA_DETAIL.Report0
      .filter((o) => o.SERVICE_REQ_STT_ID === 2).reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + Number(pilot.AMOUNT);
      },
      0);
      this.donmau__SUM_AMOUNT_HT = tk.Data.DATA_DETAIL.Report0
      .filter((o) => o.SERVICE_REQ_STT_ID === 3).reduce(function (
        accumulator,
        pilot
      ) {
        return accumulator + Number(pilot.AMOUNT);
      },
      0);
    }
  });
}

LoadXNGPB(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataXNGPB(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
              this.xn__GPB = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K42").reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__GPB_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K42" && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__GPB_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K42" && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.xn__HH = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K36").reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__HH_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K36" && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__HH_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K36" && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.xn__SH = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K37").reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__SH_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K37" && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__SH_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K37" && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.xn__VS = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K38").reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__VS_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K38" && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__VS_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K38" && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.xn__SHPT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K46").reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__SHPT_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K46" && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.xn__SHPT_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.DEPARTMENT_CODE === "K46" && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
            }
          });
}

LoadPttt(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataPttt(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
              this.pt__SUM = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 11 && o.IS_GMHS === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.pt__SUM_CC = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 11 && o.IS_GMHS === 1 && o.IS_EMERGENCY === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.tt__SUM = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 4).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.tt__SUM_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 4 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.tt__SUM_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 4 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.npm__SUM = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 11 && o.IS_GMHS != 1 ).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.npm__SUM_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.TDL_SERVICE_TYPE_ID === 11 && o.IS_GMHS != 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.npm__SUM_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) =>o.TDL_SERVICE_TYPE_ID === 11 && o.IS_GMHS != 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
            }
          });
}

LoadCthiep(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataCthiep(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
              this.ct__TM = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTTM === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__TM_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTTM === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__TM_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTTM === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.ct__DQ = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTDQ === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__DQ_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) =>  o.IS_CTDQ === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__DQ_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTDQ === 1  && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              
              this.ct__XQ = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTXQ === 1).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__XQ_DXL = t.Data.DATA_DETAIL.Report0
              .filter((o) =>o.IS_CTXQ === 1 && o.SERVICE_REQ_STT_ID === 2).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
              this.ct__XQ_HT = t.Data.DATA_DETAIL.Report0
              .filter((o) => o.IS_CTXQ === 1 && o.SERVICE_REQ_STT_ID === 3).reduce(function (
                accumulator,
                pilot
              ) {
                return accumulator + Number(pilot.AMOUNT);
              },
              0);
            }
          });
}
  
loadTbKham() {    
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
      this.menuService.loadDataTbKham(this.searchReport, 1, paramData)
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

loadTopIcd(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataTopIcd(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
            this.dataslIcd = t.Data.DATA_DETAIL.Report0;
            for (let index = 0; index < this.dataslIcd.length; index++) {
              const element = this.dataslIcd[index];
                element.Y =element.Y*1;
            }
            console.log(this.dataslIcd);
            }
          });
}

loadTrongBuong(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataTrongBuong(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
            this.dataslTrongBuong = t.Data.DATA_DETAIL.Report0;
            for (let index = 0; index < this.dataslTrongBuong.length; index++) {
              const element = this.dataslTrongBuong[index];
                element.Y =element.Y*1;
            }
            console.log(this.dataslTrongBuong);
            }
          });
}

loadVao(){
  if (this.timefrom != undefined && this.timefrom != "")
          this.searchReport.TIME_FROM = this.timefrom;
        else this.searchReport.TIME_FROM = null;
        if (this.timeto != undefined && this.timeto != "")
          this.searchReport.TIME_TO = this.timeto;
        else this.searchReport.TIME_TO = null;
        if (this.searchReport === undefined || this.searchReport === null)
          this.searchReport = new SearchReport();
        console.log(this.searchReport);

        let paramData = new ParamData();
        this.menuService
          .loadDataVao(this.searchReport)
          .subscribe((t) => {
            if (t != null && t.Success) {
            this.dataslVao = t.Data.DATA_DETAIL.Report0;
            for (let index = 0; index < this.dataslVao.length; index++) {
              const element = this.dataslVao[index];
                element.Y =element.Y*1;
            }
            console.log(this.dataslVao);
            }
          });
}
  onHidden(): void {
    console.log("Dropdown is hidden");
  }
  onShown(): void {
    console.log("Dropdown is shown");
  }
  isOpenChange(): void {
    console.log("Dropdown state is changed");
  }

  ClickSearch(value: number) {
    this.SearchFor = value;

    //TODO
    if (this.SearchFor == 0) {
      this.timefrom = "";
      this.timeto = "";
    }
    if (this.SearchFor == 1) {
      //Hôm nay
      let now = new Date();
      this.timefrom = ConvertdateToString(now, "000000");
      this.timeto = ConvertdateToString(now, "235959");
    }
    if (this.SearchFor == 2) {
      //Tuần này
      let now = new Date();
      let now1 = new Date();
      now1.setDate(now1.getDate() - now1.getDay() + 1);
      if (now1.getDate() <= now.getDate()) {
        this.timeto = ConvertdateToString(now, "235959");
        this.timefrom = ConvertdateToString(now1, "000000");
      } else {
        this.timeto = ConvertdateToString(now1, "235959");
        this.timefrom = ConvertdateToString(now, "000000");
      }
    }
    if (this.SearchFor == 3) {
      //Tháng này
      let now = new Date();
      let now1 = new Date();
      now.setFullYear(now.getFullYear(), now.getMonth(), 1);
      this.timefrom = ConvertdateToString(now, "000000");
      now1.setFullYear(now1.getFullYear(), now1.getMonth(), 1);
      now1.setMonth(now1.getMonth() + 1);
      now1.setDate(now1.getDate() - 1);
      this.timeto = ConvertdateToString(now1, "235959");
    }
    if (this.SearchFor == 4) {
      //Năm nay
      let now = new Date();
      let now1 = new Date();
      now.setFullYear(now.getFullYear(), 0, 1);
      this.timefrom = ConvertdateToString(now, "000000");
      now1.setFullYear(now1.getFullYear() + 1, 0, 1);
      now1.setDate(now1.getDate() - 1);
      this.timeto = ConvertdateToString(now1, "235959");
    }
    if (this.SearchFor == 5) {
      //Hôm qua
      let now = new Date();
      now.setDate(now.getDate() - 1);
      this.timefrom = ConvertdateToString(now, "000000");
      this.timeto = ConvertdateToString(now, "235959");
    }
    if (this.SearchFor == 6) {
      //Tháng trước
      let now = new Date();
      let now1 = new Date();

      if (now.getMonth() == 0) {
        now.setFullYear(now.getFullYear() - 1, 11, 1);
        now1.setFullYear(now1.getFullYear() - 1, 11, 1);
      } else {
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
    if (this.SearchFor == 7) {
      //Năm trước
      let now = new Date();
      let now1 = new Date();

      now.setFullYear(now.getFullYear() - 1, 0, 1);
      this.timefrom = ConvertdateToString(now, "000000");

      now1.setFullYear(now1.getFullYear(), 0, 1);
      now1.setDate(now1.getDate() - 1);
      this.timeto = ConvertdateToString(now1, "235959");
    }
    this.searchRangeDate = this.convertDateStringToDateRange(
      this.timefrom,
      this.timeto
    );
    this.setReportDataMenu();
  }

  loadMenuProfile() {
    const authorizeResultData = localStorage.getItem("AuthorizeResultData");
    if (
      authorizeResultData != undefined &&
      authorizeResultData != null &&
      authorizeResultData != ""
    ) {
      this.navItems = JSON.parse(authorizeResultData);
      console.log(this.navItems);
      this.setReportDataMenu();
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

              d.cssname = md.MODULE_URL;
              d.reportdata = null; //TODO
              d.loading = 1; //TODO
              let x = this.navItems.filter((o) => o.url == md.MODULE_LINK);
              if (x.length <= 0) {
                this.navItems.push(d);
              }
              d.MODULE_GROUP_NAME = md.MODULE_GROUP_NAME ?? 'Nhóm';
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

          this.setReportDataMenu();
        }
      });
    }
    var navItemChooses = this.navItems.filter((o) => o.number_order > 0);
    this.navItems = navItemChooses;
  }
  // set language of datepicker
  setDatepickerLanguage() {
    defineLocale("vi", viLocale);
    this.localeService.use("vi");
  }
  dtFromTimeChange(value: string) {
    console.log(value);
    this.timefromBedRatio = this.convertDateStringToDateNumber1(value, true);
    console.log(this.timefromBedRatio);
  }

  dtToTimeChange(value: string) {
    console.log(value);
    this.timetoBedRatio = this.convertDateStringToDateNumber1(value, false);
    console.log(this.timetoBedRatio);
  }

  changePrecious(value) {
    // Quý 1 của năm là từ tháng 1 tới hết tháng 3
    // Quý 2 của năm là từ tháng 4 tới hết tháng 6
    // Quý 3 của năm là từ tháng 7 tới hết tháng 9
    // Quý 4 của năm là từ tháng 10 tới hết tháng 12
    this.currentPrecious = value;
    let now = new Date();
    let now1 = new Date();
    if (
      this.currentYear == undefined ||
      this.currentYear == null ||
      this.currentYear == ""
    ) {
      this.currentYear = now.getFullYear().toString();
    }

    if (this.currentPrecious == "1") {
      now.setFullYear(parseInt(this.currentYear), 0, 1);
      now1.setFullYear(parseInt(this.currentYear), 3, 1);
      now1.setDate(now1.getDate() - 1);
    } else if (this.currentPrecious == "2") {
      now.setFullYear(parseInt(this.currentYear), 3, 1);
      now1.setFullYear(parseInt(this.currentYear), 6, 1);
      now1.setDate(now1.getDate() - 1);
    } else if (this.currentPrecious == "3") {
      now.setFullYear(parseInt(this.currentYear), 6, 1);
      now1.setFullYear(parseInt(this.currentYear), 9, 1);
      now1.setDate(now1.getDate() - 1);
    } else if (this.currentPrecious == "4") {
      now.setFullYear(parseInt(this.currentYear), 9, 1);
      now1.setFullYear(now1.getFullYear() + 1, 0, 1);
      now1.setDate(now1.getDate() - 1);
    }

    this.DefaultFromDateBedRatioShow = this.formatDatetoStringInputDate(now);
    this.DefaultToDateBedRatioShow = this.formatDatetoStringInputDate(now1);
    this.timefromBedRatio = ConvertdateToString(now, "000000");
    this.timetoBedRatio = ConvertdateToString(now1, "235959");

    console.log(
      "value=" +
        value +
        "|currentPrecious=" +
        this.currentPrecious +
        "|DefaultFromDateBedRatioShow=" +
        this.DefaultFromDateBedRatioShow +
        "|DefaultToDateBedRatioShow=" +
        this.DefaultToDateBedRatioShow
    );
  }

  changeYear(value) {
    console.log(value);

    this.currentYear = value;
    if (
      this.currentPrecious != undefined &&
      this.currentPrecious != null &&
      this.currentPrecious != ""
    ) {
      this.changePrecious(this.currentPrecious);
    }
  }

  getBedRatioReality1(BedRatioData): string {
    let result = "";
    if (BedRatioData != undefined && BedRatioData != null) {
      result =
        this.roundNumber(
          (BedRatioData?.BED_NUM_BHYT /
            BedRatioData?.NUM_DAY /
            BedRatioData?.REALITY_PATIENT_COUNT_BH) *
            100,
          2
        ) + "";
    }
    return result + "%";
  }
  getBedRatioReality2(BedRatioData): string {
    let result = "";
    if (BedRatioData != undefined && BedRatioData != null) {
      result =
        this.roundNumber(
          (BedRatioData?.BED_NUM /
            BedRatioData?.NUM_DAY /
            BedRatioData?.REALITY_PATIENT_COUNT) *
            100,
          2
        ) + "";
    }
    return result + "%";
  }

  getBedRatioReality3(BedRatioData): string {
    let result = "";
    if (BedRatioData != undefined && BedRatioData != null) {
      result =
        this.roundNumber(
          (BedRatioData?.BED_NUM_BHYT /
            BedRatioData?.NUM_DAY /
            BedRatioData?.THEORY_PATIENT_BED_BH) *
            100,
          2
        ) + "";
    }
    return result + "%";
  }
  getBedRatioReality4(BedRatioData): string {
    let result = "";
    if (BedRatioData != undefined && BedRatioData != null) {
      result =
        this.roundNumber(
          (BedRatioData?.BED_NUM /
            BedRatioData?.NUM_DAY /
            BedRatioData?.THEORY_PATIENT_BED) *
            100,
          2
        ) + "";
    }
    return result + "%";
  }

  // roundNumber(num, scale) {
  // if(scale!=undefined && scale!=null){
  // //return (Math.round(num + "e+" + scale.toString())  + "e-" + scale.toString());
  // return +(Math.round(num + "e+2")  + "e-2");
  // }
  // else{
  // return (Math.round(num));
  // }
  // }

  /**
   * Converts num to a decimal string (if it isn't one already) and then rounds it
   * to at most dp decimal places.
   *
   * For explanation of why you'd want to perform rounding operations on a String
   * rather than a Number, see http://stackoverflow.com/a/38676273/1709587
   *
   * @param {(number|string)} num
   * @param {number} dp
   * @return {string}
   */
  roundNumber(num, dp) {
    //if (arguments.length != 2) throw new Error("2 arguments required");

    num = String(num);
    if (num.indexOf("e+") != -1) {
      // Can't round numbers this large because their string representation
      // contains an exponent, like 9.99e+37
      throw new Error("num too large");
    }
    if (num.indexOf(".") == -1) {
      // Nothing to do
      return num;
    }

    var parts = num.split("."),
      beforePoint = parts[0],
      afterPoint = parts[1],
      shouldRoundUp = afterPoint[dp] >= 5,
      finalNumber;

    afterPoint = afterPoint.slice(0, dp);
    if (!shouldRoundUp) {
      finalNumber = beforePoint + "." + afterPoint;
    } else if (/^9+$/.test(afterPoint)) {
      // If we need to round up a number like 1.9999, increment the integer
      // before the decimal point and discard the fractional part.
      finalNumber = Number(beforePoint) + 1;
    } else {
      // Starting from the last digit, increment digits until we find one
      // that is not 9, then stop
      var i = dp - 1;
      while (true) {
        if (afterPoint[i] == "9") {
          afterPoint = afterPoint.substr(0, i) + "0" + afterPoint.substr(i + 1);
          i--;
        } else {
          afterPoint =
            afterPoint.substr(0, i) +
            (Number(afterPoint[i]) + 1) +
            afterPoint.substr(i + 1);
          break;
        }
      }

      finalNumber = beforePoint + "." + afterPoint;
    }

    // Remove trailing zeroes from fractional part before returning
    return finalNumber.replace(/0+$/, "");
  }

  // roundNumber1(num, scale) {
  // if(!("" + num).includes("e")) {
  // return +(Math.round(num + "e+" + scale)  + "e-" + scale);
  // } else {
  // var arr = ("" + num).split("e");
  // var sig = ""
  // if(+arr[1] + scale > 0) {
  // sig = "+";
  // }
  // return +(Math.round(+arr[0] + "e" + sig + (+arr[1] + scale)) + "e-" + scale);
  // }
  // }

  loadBedRatio() {
    console.log("loadBedRatio");
    let srpt4 = new SearchReport();
    srpt4.TIME_TO = this.timetoBedRatio;
    srpt4.TIME_FROM = this.timefromBedRatio;
    srpt4.ReportTypeCode = "TKB00061";
    srpt4.ReportTemplateCode = "TKB0006101";
    this.thoigianthongke = "";
    this.loading0 = true;
    console.log(srpt4);
    this.homeService.loadDataReportMrs(srpt4).subscribe((tk) => {
      let BedRatioDatas = null;
      if (
        tk != undefined &&
        tk != null &&
        tk.Success &&
        tk.Data != undefined &&
        tk.Data.DATA_DETAIL != undefined &&
        tk.Data.DATA_DETAIL.Report0 != undefined &&
        tk.Data.DATA_DETAIL.Report0.length > 0
      ) {
        BedRatioDatas = tk.Data.DATA_DETAIL.Report0;
        this.BedRatioData = tk.Data.DATA_DETAIL.Report0[0];
        this.HaveData = 1;
      } else {
        this.HaveData = 0;
        this.BedRatioData = null;
      }
      this.loading0 = false;
      console.log(this.BedRatioData);
      console.log(BedRatioDatas);
    });
  }

  public makerTrongBuong = {
    dataLabel: {
        visible: true,
        position: 'Outer',
        font: {
            fontWeight: '600',
            size: '12px',
            color: '#333333'
        }
    }
};
}
