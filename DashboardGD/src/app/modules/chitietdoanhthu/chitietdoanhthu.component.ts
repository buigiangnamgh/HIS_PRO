import { Component, OnInit, Inject, OnDestroy } from "@angular/core";
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
import { Router } from '@angular/router';
import { ChiTietDoanhThuService } from './chitietdoanhthu.service';
import { BaseComponent } from '../../shared/base.component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AppConfig } from "../../config/app.config";
import { ExcelService } from "../../core/excel.service ";
import { ParamData } from "../../model/apidata.model";
import { reportDatas } from "../../model/report";
import 'rxjs/Rx';
import { DOCUMENT } from '@angular/common';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from './getWeek';
import { LoadingBarService } from '@ngx-loading-bar/core';
import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import { ChonNgayComponent } from '../../modules/chonngay/chonngay.dialog.component';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { MenuModel } from "../../model/menu.model";
import { MenuService } from "../../containers/default-layout/menu.service";


@Component({
	selector: 'chitietdoanhthucomponent',
	templateUrl: 'chitietdoanhthu.component.html',
	styleUrls: ['chitietdoanhthu.component.css']
})
export class ChiTietDoanhThuComponent extends BaseComponent implements OnInit, OnDestroy {
	reportdatas: any;
	ReportOptionName: string;
	reportdatasRavien: any;
	reportdatasOther: any;
	reportdatasgroupbydepartment: any;
	dynamic: string;
	HaveData0: Number;
	HaveData1: Number;
	HaveData: Number;
	total0: number = 0;
	total1: number = 0;
	tongthutructiep: number = 0;
	tongthanhtoanravien: number = 0;
	tongthanhtoanravien_bh: number = 0;
	tongthanhtoanravien_vp: number = 0;
	tongthanhtoanravien_yc: number = 0;
	tongthanhtoanravien_dvyc: number = 0;
	tongthanhtoanravien_bntra: number = 0;
	tongtheokhoa: number = 0;
	searchReport: SearchReport;
	searchReport1: SearchReport;
	searchRangeDate: SearchRangeDate;
	modalRef: BsModalRef;
	pageSize: number = AppConfig.pageSize;
	currentpageSize: number = AppConfig.pageSize;
	curentSelectToNavigate: number;
	curPage: number = 1;
	start: number = 0;
	limit: number = 0;
	totalExam: number = 0;
	totalBhyt: number = 0;
	totalFee: number = 0;
	loading: boolean;
	dateweek: Date;
	TF: boolean;
	today: Date;
	Listyear: Array<number>;
	timefrom: string;
	timeto: string;
	SearchFor: number;// 1:ngày 2:tuần 3:tháng 4:năm 
	interval;
	timeLeft: number = 10;
	IsAllowCustomDateRange: number = 0;
	public navItems: Array<MenuModel>;

	constructor(
		@Inject(DOCUMENT) private document: any,
		public chitietdoanhthuService: ChiTietDoanhThuService,
		public router: Router,
		private loadingBar: LoadingBarService,
		private modalService: BsModalService,
		private menuService: MenuService,
		public excelService: ExcelService) {
		super(router);
	}

	ngOnInit() {
		this.startLoading();
		this.SearchFor = 1;
		this.initDateRange();
		this.loadMenuProfile();
		this.HaveData = 1;
		this.HaveData0 = 1;
		this.HaveData1 = 1;
		this.searchReport = new SearchReport();
		this.searchReport1 = new SearchReport();
		this.loadData();
		this.stopLoading();
		this.IsAllowCustomDateRange = AppConfig.common.IsAllowCustomDateRange;
		if (AppConfig.common.TimerInterval > 0) {
			this.timeLeft = AppConfig.common.TimerInterval;
			this.startTimer();
		}
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
				console.log('--------------------------------------------Load du lieu loadDataAuto: ----------------------------------  sau ' + this.timeLeft + " giây sẽ tiếp tục tải lại.....");
				this.loadDataAuto();
			}
		}, 1000)
	}

	startLoading() {
		this.loadingBar.start();
	}

	stopLoading() {
		this.loadingBar.complete();
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
		if (objectstore__searchReport != undefined && objectstore__searchReport != null && objectstore__searchReport != '') {
			searchReport0 = JSON.parse(objectstore__searchReport);
			if (searchReport0 != undefined && searchReport0 != null) {
				this.timeto = searchReport0.TIME_TO;
				this.timefrom = searchReport0.TIME_FROM;
				isInit = true;
			}
		}
		if (isInit == false) {
			let now = new Date();
			this.timeto = ConvertdateToString(now, "235959");
			this.timefrom = ConvertdateToString(now, "000000");
		}
		this.searchRangeDate = this.convertDateStringToDateRangeNoStorage(this.timefrom, this.timeto);
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
			if (now1.getDate() <= now.getDate()) {
				this.timeto = ConvertdateToString(now, "235959");
				this.timefrom = ConvertdateToString(now1, "000000");
			}
			else {
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

			if (now.getMonth() == 0) {
				now.setFullYear(now.getFullYear() - 1, 11, 1);
				now1.setFullYear(now1.getFullYear() - 1, 11, 1);
			}
			else {
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
		this.searchRangeDate = this.convertDateStringToDateRange(this.timefrom, this.timeto);
		this.loadData();
	}

	loadDataAuto() {
		if (this.timefrom != undefined && this.timefrom != '')
			this.searchReport.TIME_FROM = this.timefrom;
		else this.searchReport.TIME_FROM = null;
		if (this.timeto != undefined && this.timeto != '')
			this.searchReport.TIME_TO = this.timeto;
		else this.searchReport.TIME_TO = null;
		this.searchReport.ReportTypeCode = "TKB00016";
		this.searchReport.ReportTemplateCode = "TKB0001602";//Thu trực tiếp 
		this.loading = true;
		let paramData = new ParamData();
		if (this.searchReport === undefined || this.searchReport === null)
			this.searchReport = new SearchReport();
		console.log(this.searchReport);
		this.HaveData1 = -1;
		this.HaveData0 = -1;

		this.chitietdoanhthuService.loadData(this.searchReport, 1, paramData).subscribe(t => {
			if (t != null && t.Success) {
				let report1 = t.Data.DATA_DETAIL.Report1;
				this.total0 = 0;
				this.loading = false;
				this.tongthutructiep = 0;
				if (report1 != undefined && report1 != null && report1.length > 0) {
					this.HaveData0 = 0;
					this.total0 = report1.length;
					this.tongthutructiep = report1.reduce(function (accumulator, pilot) {
						return accumulator + pilot.TOTAL_PRICE;
					}, 0);

					let dateMap = report1
						.reduce(
							(m, v) => {

								let PATIENT_TYPE_NAME = v.PATIENT_TYPE_NAME

								const entry = m[PATIENT_TYPE_NAME]
								if (typeof entry === 'undefined') {
									m[PATIENT_TYPE_NAME] = [v]
								}
								else {
									entry.push(v)
								}
								return m
							},
							{}
						);

					this.reportdatas = Object.keys(dateMap).map(d => {
						return {
							PATIENT_TYPE_NAME: d,
							TOTAL_PRICE: dateMap[d].reduce(function (accumulator, pilot) {
								return accumulator + pilot.TOTAL_PRICE;
							}, 0)
						}
					});


					let dateMapkhoa1 = report1
						.reduce(
							(m, v) => {

								let DEPARTMENT_NAME = v.DEPARTMENT_NAME

								const entry = m[DEPARTMENT_NAME]
								if (typeof entry === 'undefined') {
									m[DEPARTMENT_NAME] = [v]
								}
								else {
									entry.push(v)
								}
								return m
							},
							{}
						);

					this.reportdatasOther = Object.keys(dateMapkhoa1).map(d => {
						return {
							DEPARTMENT_NAME: d,
							TOTAL_PRICE: dateMapkhoa1[d].reduce(function (accumulator, pilot) {
								return accumulator + (pilot.TOTAL_PRICE);
							}, 0)
						}
					});
				}
				else {
					this.HaveData0 = 1;
				}
				console.log(this.reportdatas);
				console.log(this.reportdatasOther);

				if (this.timefrom != undefined && this.timefrom != '')
					this.searchReport1.TIME_FROM = this.timefrom;
				else this.searchReport1.TIME_FROM = null;
				if (this.timeto != undefined && this.timeto != '')
					this.searchReport1.TIME_TO = this.timeto;
				else this.searchReport1.TIME_TO = null;
				this.searchReport1.ReportTypeCode = "MRS00012";
				this.searchReport1.ReportTemplateCode = "MRS00012TKB0003601";//Thanh toán ra viện
				this.chitietdoanhthuService.loadData(this.searchReport1, 1, paramData)
					.subscribe(tk => {
						if (tk != null && tk.Success) {
							let reporttk1 = tk.Data.DATA_DETAIL.Report1;
							let reporttk0 = tk.Data.DATA_DETAIL.Report0;
							this.total1 = 0;
							this.loading = false;
							if (reporttk0 != undefined && reporttk0 != null && reporttk0.length > 0) {
								this.HaveData0 = 0;

								this.tongthanhtoanravien = reporttk0.reduce(function (accumulator, pilot) {
									return accumulator + Number(pilot.TOTAL_HEIN_PRICE) + Number(pilot.TOTAL_PATIENT_PRICE);
								}, 0);
								let dateMapBenhNhan = reporttk0.reduce(
									(m, v) => {

										let PATIENT_TYPE_NAME = v.PATIENT_TYPE_NAME

										const entry = m[PATIENT_TYPE_NAME]
										if (typeof entry === 'undefined') {
											m[PATIENT_TYPE_NAME] = [v]
										}
										else {
											entry.push(v)
										}
										return m
									},
									{}
								);

								let reportdatasBenhnhan = Object.keys(dateMapBenhNhan).map(d => {
									return {
										PATIENT_TYPE_NAME: d,
										TOTAL_PRICE: dateMapBenhNhan[d].reduce(function (accumulator, pilot) {
											return accumulator + Number(pilot.TOTAL_PATIENT_PRICE);
										}, 0)
									}
								});

								let dateMapRavien = reporttk0.reduce(
									(m, v) => {

										let PATIENT_TYPE_NAME = 'BHYT trả'

										const entry = m[PATIENT_TYPE_NAME]
										if (typeof entry === 'undefined') {
											m[PATIENT_TYPE_NAME] = [v]
										}
										else {
											entry.push(v)
										}
										return m
									},
									{}
								);
								this.reportdatasRavien = Object.keys(dateMapRavien).map(d => {
									return {
										PATIENT_TYPE_NAME: d,
										TOTAL_PRICE: dateMapRavien[d].reduce(function (accumulator, pilot) {
											return accumulator + Number(pilot.TOTAL_HEIN_PRICE);
										}, 0)
									}
								});

								this.reportdatasRavien = this.reportdatasRavien.concat(reportdatasBenhnhan);
							}

							if (reporttk1 != undefined && reporttk1 != null && reporttk1.length > 0) {
								this.HaveData1 = 0;
								this.total1 = reporttk1.length;


								let dateMapkhoa = reporttk1
									.reduce(
										(m, v) => {

											let DEPARTMENT_NAME = v.DEPARTMENT_NAME

											const entry = m[DEPARTMENT_NAME]
											if (typeof entry === 'undefined') {
												m[DEPARTMENT_NAME] = [v]
											}
											else {
												entry.push(v)
											}
											return m
										},
										{}
									);

								let reportdatasgroupbydepartmentt1 = Object.keys(dateMapkhoa).map(d => {
									return {
										DEPARTMENT_NAME: d,
										TOTAL_PRICE: dateMapkhoa[d].reduce(function (accumulator, pilot) {
											return accumulator + (pilot.AMOUNT);
										}, 0)
									}
								});

								console.log(reportdatasgroupbydepartmentt1);

								if (this.reportdatasOther != undefined && this.reportdatasOther != null && this.reportdatasOther.length > 0)
									reportdatasgroupbydepartmentt1 = reportdatasgroupbydepartmentt1.concat(this.reportdatasOther);

								if (reportdatasgroupbydepartmentt1 != undefined && reportdatasgroupbydepartmentt1 != null && reportdatasgroupbydepartmentt1.length > 0) {
									let dateMapkhoaconcat = reportdatasgroupbydepartmentt1
										.reduce(
											(m, v) => {

												let DEPARTMENT_NAME = v.DEPARTMENT_NAME

												const entry = m[DEPARTMENT_NAME]
												if (typeof entry === 'undefined') {
													m[DEPARTMENT_NAME] = [v]
												}
												else {
													entry.push(v)
												}
												return m
											},
											{}
										);

									this.reportdatasgroupbydepartment = Object.keys(dateMapkhoaconcat).map(d => {
										return {
											DEPARTMENT_NAME: d,
											TOTAL_PRICE: dateMapkhoaconcat[d].reduce(function (accumulator, pilot) {
												return accumulator + (pilot.TOTAL_PRICE);
											}, 0)
										}
									});

									if (this.reportdatasgroupbydepartment != undefined && this.reportdatasgroupbydepartment != null)
										this.tongtheokhoa = this.reportdatasgroupbydepartment.reduce(function (accumulator, pilot) {
											return accumulator + (pilot.TOTAL_PRICE);
										}, 0);
								}
							}
							else {
								this.HaveData1 = 1;
							}
							console.log(this.reportdatasgroupbydepartment);
						}
					});
			}
			else {
				this.HaveData0 = 1;
				this.HaveData1 = 1;
			}
		});
	}

	loadData() {

		this.reportdatas = null;
		this.reportdatasOther = null;
		this.reportdatasgroupbydepartment = null;
		this.tongthutructiep = 0;
		this.tongthanhtoanravien = 0;
		this.tongthanhtoanravien_bh = 0;
		this.tongthanhtoanravien_vp = 0;
		this.tongthanhtoanravien_yc = 0;
		this.tongthanhtoanravien_bntra = 0;
		this.tongtheokhoa = 0;
		this.loadDataAuto();
	}


	exportExcel() {
		if (this.reportdatas != undefined && this.reportdatas != null && this.reportdatas.length > 0) {
			//Xuất excel
			this.excelService.exportAsExcelFile(this.reportdatas, "testexportexcel");
		}
	}
	exportExcel2() {
		if (this.reportdatasOther != undefined && this.reportdatasOther != null && this.reportdatasOther.length > 0) {
			//Xuất excel
			this.excelService.exportAsExcelFile(this.reportdatasOther, "testexportexcel");
		}
	}
	LoadSearch(searchPanel: any) {
		if (this.searchReport === undefined || this.searchReport === null)
			this.searchReport = new SearchReport();
		//this.searchReport.Keyword = searchPanel.keyword;
		this.loadData();
	}
}
