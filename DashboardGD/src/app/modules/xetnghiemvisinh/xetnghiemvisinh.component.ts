import { Component, OnInit, OnDestroy } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators,
} from "@angular/forms";
import { xetnghiemvisinhService } from "./xetnghiemvisinh.service";
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
import { ChonNgayComponent } from "../chonngay/chonngay.dialog.component";
import { BsModalService, ModalDirective } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { BsLocaleService } from "ngx-bootstrap/datepicker";
import { listLocales } from "ngx-bootstrap/chronos";
import { defineLocale } from "ngx-bootstrap/chronos";
import { viLocale } from "ngx-bootstrap/locale";
import { ParamData } from "../../model/apidata.model";
import { ExcelService } from "../../core/excel.service ";
// import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: "xetnghiemvisinhcomponent",
  templateUrl: "xetnghiemvisinh.component.html",
  styleUrls: ["xetnghiemvisinh.component.css"],
})
export class xetnghiemvisinhComponent
  extends BaseComponent
  implements OnInit, OnDestroy
{
  total0: number = 0;
  total1: number = 0;
  reportdatas: any;
  reportdatas1: any;
  public navItems: Array<MenuModel>;
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
  loading: boolean;
  interval;
  timeLeft: number = 10;
  IsAllowCustomDateRange: number = 0;
  modalRef: BsModalRef;
  HaveData0: Number;
ReportOptionName: string;
  
  
  BedRatioData: any;
  thoigianthongke: string;
  loading0: boolean;
  listAllowReportTemplateCode: string =
    AppConfig.common.ListAllowReportTemplateCode;
  

  constructor(
    private route: ActivatedRoute,
    public xetnghiemvisinhService: xetnghiemvisinhService,
    private menuService: MenuService,
    private modalService: BsModalService,
    private localeService: BsLocaleService,
    public fb: FormBuilder,
    public router: Router,
    public excelService: ExcelService
  ) {
    super(router);
  }

  ngOnInit() {
    this.searchPanel = this.fb.group({});
    this.dnum = 0;
    this.SearchFor = 1;
    
    
    this.loading0 = false;
    this.initDateRange();
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
    //this.spinner.show(name);
  }

  hideSpinner(name: string) {
    //this.spinner.hide(name);
  }

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

  setReportDataMenu() {
    if (this.timefrom != undefined && this.timefrom != "")
      this.searchReport.TIME_FROM = this.timefrom;
    else this.searchReport.TIME_FROM = null;
    if (this.timeto != undefined && this.timeto != "")
      this.searchReport.TIME_TO = this.timeto;
    else this.searchReport.TIME_TO = null;
    this.loading = true;
    if (this.searchReport === undefined || this.searchReport === null)
      this.searchReport = new SearchReport();
    console.log(this.searchReport);
    this.HaveData0 = -1;

    let paramData = new ParamData();

    this.menuService
    .loadDataXNVS(this.searchReport, 1, paramData)
    .subscribe((t) => {
      if (t != null && t.Success) {
        this.reportdatas = t.Data.DATA_DETAIL.Report0;
        this.total0 = 0;
        this.loading = false;
        if (
          this.reportdatas != undefined &&
          this.reportdatas != null &&
          this.reportdatas.length > 0
        ) {
          this.HaveData0 = 0;
          this.total0 = this.reportdatas.length;
          let parentService: {
            ID: number;
            PARENT_SERVICE_NAME: string;
            AMOUNT: string;
          }[] = [
            {
              ID: 1,
              PARENT_SERVICE_NAME:
                "Test nhanh (HIV, HBsAg, Anti-HCV, TPHA, RPR, HAV, HEV, Rotavirus, H.pylori, HBsAb, HBeAg, ASLO)",
              AMOUNT: this.reportdatas
                .filter((o) => o.TESTNHANH === 1)
                .reduce(function (accumulator, pilot) {
                  return accumulator + Number(pilot.AMOUNT);
                }, 0),
            },
            {
              ID: 2,
              PARENT_SERVICE_NAME: "Nhuộm soi (Vi khuẩn, Vi nấm, KST, Lao)",
              AMOUNT: this.reportdatas
                .filter((o) => o.IS_NHUOMSOI === 1)
                .reduce(function (accumulator, pilot) {
                  return accumulator + Number(pilot.AMOUNT);
                }, 0),
            },
            {
              ID: 3,
              PARENT_SERVICE_NAME:
                "Miễn dịch (Sán lá gan lớn, Giun đũa chó mèo, giun lươn, ANA, dsDNA)",
              AMOUNT: this.reportdatas
                .filter((o) => o.IS_MIENDICH === 1)
                .reduce(function (accumulator, pilot) {
                  return accumulator + Number(pilot.AMOUNT);
                }, 0),
            },
            {
              ID: 4,
              PARENT_SERVICE_NAME: "Nuôi cấy trả kết quả trong ngày",
              AMOUNT: this.reportdatas
                .filter((o) => o.IS_NUOICAY === 1)
                .reduce(function (accumulator, pilot) {
                  return accumulator + Number(pilot.AMOUNT);
                }, 0),
            },
            {
              ID: 5,
              PARENT_SERVICE_NAME: "Số lượng dương tính/Kháng sinh đồ",
              AMOUNT:
                this.reportdatas
                  .filter(
                    (o) =>
                      o.IS_TESTNHANH !== 1 &&
                      o.IS_NHUOMSOI !== 1 &&
                      o.IS_MIENDICH !== 1 &&
                      o.IS_NUOICAY !== 1
                  )
                  .reduce(function (accumulator, pilot) {
                    return accumulator + Number(pilot.AMOUNT_POSITIVE);
                  }, 0) +
                "/" +
                this.reportdatas
                  .filter(
                    (o) =>
                      o.IS_TESTNHANH !== 1 &&
                      o.IS_NHUOMSOI !== 1 &&
                      o.IS_MIENDICH !== 1 &&
                      o.IS_NUOICAY !== 1
                  )
                  .reduce(function (accumulator, pilot) {
                    return accumulator + Number(pilot.AMOUNT);
                  }, 0),
            },
          ];
          this.reportdatas1 = parentService;
        } else {
          this.HaveData0 = 1;
        }

        console.log(this.reportdatas);
      } else {
        this.HaveData0 = 1;
      }
    });
  }

  exportExcel() {
    if (this.reportdatas1 != undefined && this.reportdatas1 != null && this.reportdatas1.length > 0) {
      //Xuất excel
      this.excelService.exportAsExcelFile(this.reportdatas1, "testexportexcel");
    }
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
      var href = window.location.href;
      var currentNavItem = this.navItems.find((x) => href.endsWith("#")|| href.endsWith(x.url));
      if(currentNavItem !==undefined && currentNavItem != null){
        this.ReportOptionName = currentNavItem.name;
      }
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
  ClickSegment(value: number){
    var currentNavItemIndex = this.navItems.findIndex((x) =>x.name === this.ReportOptionName)-value;
    if(currentNavItemIndex !==undefined && currentNavItemIndex != null && currentNavItemIndex>=0 &&currentNavItemIndex<this.navItems.length){
      window.location.assign('#'+this.navItems[currentNavItemIndex].url)
    }
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
  ageOfDob(value: String): string {
    var result = "";
    if (value != undefined && value != null) {
      var dob = new Date(
        Number(value.substring(0, 4)),
        Number(value.substring(4, 2)),
        Number(value.substring(6, 2)),
        0,
        0,
        0
      );
      let timeDiff = Math.abs(Date.now() - dob.getTime());
      result = Math.floor(timeDiff / (1000 * 3600 * 24) / 365.25) + "";
    }
    return result;
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

 
}
