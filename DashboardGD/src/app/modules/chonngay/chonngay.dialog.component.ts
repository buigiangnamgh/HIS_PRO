import { Component, OnInit, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from 'ng2-translate';
import { AuthenService } from '../../authen/authen.service';
import { BaseComponent } from '../../shared/base.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { AlertService } from '../../shared/alert/_services';
import { Subject } from 'rxjs/Subject';
import { PasswordValidation } from '../../shared/passwordValidation ';
import { ToastrService } from 'ngx-toastr';
import { DaterangepickerModule } from 'angular-2-daterangepicker';
import {IAngularMyDpOptions, IMyDateModel, IMyRangeDateSelection} from 'angular-mydatepicker';
import { getWeek, ConvertdateToString, addDays, MondayofWeek } from '../taichinh/getWeek';
import { SearchReport } from '../../model/Search.model';

declare var jQuery: any;

@Component({
    selector: 'chonngay.dialog',
    templateUrl: 'chonngay.dialog.html',
	styleUrls: ['chonngay.dialog.css']
})
export class ChonNgayComponent extends BaseComponent  {

    public isDisableButton: boolean;
    //public isShowFailDialog: boolean;
    //public isShowSuccessDialog: boolean;
    public dialogContent: string;
    //public form: FormGroup;
    public onClose: Subject<boolean>;
	searchReport: SearchReport;
	myDateRangePickerOptions: IAngularMyDpOptions  = {
        // other options...       
		dateFormat: 'dd-mm-yyyy',
        firstDayOfWeek: 'mo',
        sunHighlight: true,
		dateRange: true,
        //height: '34px',
        //width: '260px',
        //inline: false,
        alignSelectorRight: false,
        showSelectorArrow: true,
		closeSelectorOnDateSelect: true,
		closeSelectorOnDocumentClick: true,
		//showClearBtn: false,
		
    };
	//date = new Date();
	modelSearch: IMyDateModel = null;
	
	// modelSearch: any = {beginDate: {
							// year: this.date.getFullYear(),
							// month: this.date.getMonth() + 1,
							// day: this.date.getDate()
						// },
						// endDate: {
							// year: this.date.getFullYear(),
							// month: this.date.getMonth() + 1,
							// day: this.date.getDate()
						// }};

    constructor(
        public fb: FormBuilder,
        public authenService: AuthenService,
        public translate: TranslateService,
        public router: Router,
        private _bsModalRef: BsModalRef,
        private alertService: AlertService,
		private location: Location,
        private toastr: ToastrService
     ) {    
        super(router);
        this.onClose = new Subject();
        //this.dialogContent = data.dialogContent;
    }

    ngOnInit() {        
        this.isDisableButton = false;				
		this.setDateRange();
    }

    save() {
        this.onClose.next(false);
        this._bsModalRef.hide();
    }

    hide() {
        this.onClose.next(false);
        this._bsModalRef.hide();
    }
    
	setDateRange(): void {
		
		
		
        const objectstore__searchReport = localStorage.getItem('objectstore__searchReport');    
		if(objectstore__searchReport != undefined && objectstore__searchReport != null && objectstore__searchReport != '')
		{     
		    this.searchReport = JSON.parse(objectstore__searchReport);
		    let beginDateData;
		    let endDateData;
		    if(this.searchReport!=undefined && this.searchReport!=null && this.searchReport.TIME_FROM!= undefined && this.searchReport.TIME_FROM!=null){			    
			   beginDateData = this.formatNumberDateRange(this.searchReport.TIME_FROM);
		    }
		    if(this.searchReport!=undefined && this.searchReport!=null && this.searchReport.TIME_TO!= undefined && this.searchReport.TIME_TO!=null){			   
			   endDateData = this.formatNumberDateRange(this.searchReport.TIME_TO);
		    }
		  								 
			this.modelSearch = {
				isRange: true, 
				singleDate: null, 
				dateRange: {
				  beginDate: beginDateData,
				  endDate: endDateData
				}
			};					 
								 
		   console.log(this.modelSearch); 
		}
		else{
			let begin: Date = new Date();
			let end: Date = new Date();		

			this.modelSearch = {
				isRange: true, 
				singleDate: null, 
				dateRange: {
				  beginDate: {
					year: begin.getFullYear(), month: begin.getMonth() + 1, day: begin.getDate()
				  },
				  endDate: {
					year: end.getFullYear(), month: end.getMonth() + 1, day: end.getDate()
				  }
				}
			};		
			
		}
			
		
		// this.form = this.fb.group({
            // // Empty string means no initial value. Can be also specific date range for example:
            // // {beginDate: {year: 2018, month: 10, day: 9}, endDate: {year: 2018, month: 10, day: 19}}
            // // which sets this date range to initial value. It is also possible to set initial
            // // value using the selDateRange attribute.

            // myDateRange: ['', Validators.required]
            // // other controls are here...
        // });
    }

    clearDateRange(): void {
        // Clear the date range using the patchValue function
        //this.form.patchValue({myDateRange: ''});
    }

	onRangeDateSelection(event: IMyRangeDateSelection): void {
        console.log('onRangeDateSelection(): event: ', event);
    }

	onDateRangeChanged(event: IMyDateModel): void {
		console.log('onDateChanged(): ', event);
		  
		this.searchReport = new SearchReport();		
		if(event.dateRange!= undefined && event.dateRange.beginDate!=undefined && event.dateRange.beginDate!=null && event.dateRange.beginDate.year!=undefined && event.dateRange.beginDate.year>0 && event.dateRange.endDate.year!=undefined && event.dateRange.endDate.year>0){
			 this.searchReport.TIME_FROM = this.ConvertDaterangeToString(event.dateRange.beginDate, 1);
			 this.searchReport.TIME_TO = this.ConvertDaterangeToString(event.dateRange.endDate, 0);
		}
		else{
			 let now = new Date();
			 this.searchReport.TIME_FROM = ConvertdateToString(now, "000000");
			 this.searchReport.TIME_TO = ConvertdateToString(now, "235959");
		}
		localStorage.setItem('objectstore__searchReport', JSON.stringify(this.searchReport));
		localStorage.setItem('objectstore__IsSearchReport', '1');
		console.log(this.searchReport);
		this.onClose.next(false);
		this._bsModalRef.hide();
		location.reload();		  
    }

	//onDateRangeChanged(event: IMyDateModel) {
	//	console.log('onDateChanged(): ', event);
        // console.log('onDateRangeChanged(): Begin date: ', event.beginDate, ' End date: ', event.endDate);        
		// this.searchReport = new SearchReport();		
		// if(event.beginDate!=undefined && event.beginDate!=null && event.beginDate.year!=undefined && event.beginDate.year>0 && event.endDate.year!=undefined && event.endDate.year>0){
			// this.searchReport.TIME_FROM = this.ConvertDaterangeToString(event.beginDate, 1);
			// this.searchReport.TIME_TO = this.ConvertDaterangeToString(event.endDate, 0);
		// }
		// else{
			// let now = new Date();
			// this.searchReport.TIME_FROM = ConvertdateToString(now, "000000");
			// this.searchReport.TIME_TO = ConvertdateToString(now, "235959");
		// }
		// localStorage.setItem('objectstore__searchReport', JSON.stringify(this.searchReport));
		// console.log(this.searchReport);
		// this.onClose.next(false);
        // this._bsModalRef.hide();
		// location.reload();
    //}
	
	formatNumberDateRange(n) {
		if (n == undefined || n == null){
		  return null;
		}
		let fm;		
		const y = n.substr(0, 4);
		let m = "";
		let d = "";
		
		if (n.length >= 6) {
		  m = n.substr(4, 2);
		}
		if (n.length >= 8) {
		  d = n.substr(6, 2);
		}
		if (y.length > 0 && m.length > 0 && d.length > 0) {
			fm = {year: Number(y), month: Number(m), day: Number(d)}
		}
		return fm;
	}
	
	ConvertDaterangeToString(dt: any, isbegin: number): string
	{
		console.log(dt);
		let valueShow = "";
		let strmonth ="";
		let strday ="";
		if(dt != undefined && dt != null && dt.year >0 && dt.month >0 && dt.day >0){
			if(dt.month<10)
				strmonth = "0"+dt.month;
			else 
				strmonth = ""+dt.month;
			if(dt.day<10)
				strday = "0"+dt.day;
			else
				strday = ""+dt.day;
			valueShow = dt.year + strmonth + strday;	
			if(isbegin==1)
				valueShow +="000000";
			else 
				valueShow +="235959";
		}
		return valueShow;
	}

    backto(){
        this.router.navigate(['/login']);
    }
}
