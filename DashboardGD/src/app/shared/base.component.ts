import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { Router } from '@angular/router';
declare var jQuery: any;
import { SearchReport, SearchRangeDate } from '../model/Search.model';

export abstract class BaseComponent implements OnInit {

  constructor(//public snackBar: MdSnackBar,
    public router: Router) {
  }

  abstract ngOnInit();  

  convertSize(bytessize) {
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    let convertedsize = '';
    if (bytessize === 0) {
      convertedsize = '0 Byte';
    }
    const i = Math.floor(Math.log(bytessize) / Math.log(1024));
    const pow = Math.pow(1024, i);
    convertedsize = (Math.round(bytessize / pow)).toString() + ' ' + sizes[i];
    return convertedsize;
  }
  
  convertDateStringToDateRange(sFromDate: string, sToDate: string): SearchRangeDate {
	let searchRangeDate = new SearchRangeDate();
	if ((sFromDate == null || sFromDate == '') && (sToDate == null || sToDate == '')){
      return searchRangeDate;
    }
	
    searchRangeDate.yearfrom = sFromDate.substr(0, 4);    
    if (sFromDate.length >= 6) {
      searchRangeDate.monthfrom = this.getStrMonthFromNumber(parseInt(sFromDate.substr(4, 2)));      
    }
    if (sFromDate.length >= 8) {
      searchRangeDate.dayfrom = sFromDate.substr(6, 2);
    }
	
	searchRangeDate.yearto = sToDate.substr(0, 4);    
    if (sToDate.length >= 6) {
      searchRangeDate.monthto = this.getStrMonthFromNumber(parseInt(sToDate.substr(4, 2)));      
    }
    if (sToDate.length >= 8) {
      searchRangeDate.dayto = sToDate.substr(6, 2);
    }	
	
	let searchReport = new SearchReport();		
	searchReport.TIME_FROM = sFromDate;
	searchReport.TIME_TO = sToDate;
	localStorage.setItem('objectstore__searchReport', JSON.stringify(searchReport));

	return searchRangeDate;
  }   

  convertDateStringToDateRangeNoStorage(sFromDate: string, sToDate: string): SearchRangeDate {
	let searchRangeDate = new SearchRangeDate();
	if ((sFromDate == null || sFromDate == '') && (sToDate == null || sToDate == '')){
      return searchRangeDate;
    }
	
    searchRangeDate.yearfrom = sFromDate.substr(0, 4);    
    if (sFromDate.length >= 6) {
      searchRangeDate.monthfrom = this.getStrMonthFromNumber(parseInt(sFromDate.substr(4, 2)));      
    }
    if (sFromDate.length >= 8) {
      searchRangeDate.dayfrom = sFromDate.substr(6, 2);
    }
	
	searchRangeDate.yearto = sToDate.substr(0, 4);    
    if (sToDate.length >= 6) {
      searchRangeDate.monthto = this.getStrMonthFromNumber(parseInt(sToDate.substr(4, 2)));      
    }
    if (sToDate.length >= 8) {
      searchRangeDate.dayto = sToDate.substr(6, 2);
    }	
	
	return searchRangeDate;
  }
  
  getStrMonthFromNumber(value: number): string {   
    let ValueShow = "";
    switch (value) {
      case 1: 
		ValueShow="TH01";
        break;
      case 2: 
		ValueShow="TH02";
        break;
	  case 3: 
		ValueShow="TH03";
        break;
	  case 4: 
		ValueShow="TH04";
        break;
	  case 5: 
		ValueShow="TH05";
        break;
	  case 6: 
		ValueShow="TH06";
        break;
	  case 7: 
		ValueShow="TH07";
        break;
	  case 8: 
		ValueShow="TH08";
        break;
	  case 9: 
		ValueShow="TH09";
        break;
	  case 10: 
		ValueShow="TH10";
        break;
	  case 11: 
		ValueShow="TH11";
        break;
	  case 12: 
		ValueShow="TH12";
        break;
	}
	return ValueShow;
  }  
  
  /**
  * @author xxxx
  */
  convertDateStringToDateNumber(sDate: string, isFromTime: boolean): number {
    let result = null;
    if (sDate != null && sDate != "") {
      let strs: Array<string> = sDate.split('-');
      let year: string = strs[0];
      let month: string = strs[1];
      let day: string = strs[2];
      if (isFromTime) {
        result = parseInt(year + month + day + '000000');
      }
      else {
        result = parseInt(year + month + day + '235959');
      }
    }
    return result;
  } 
  
  convertDateStringToDateNumber1(sDate: string, isFromTime: boolean): number {
    let result = null;
    if (sDate != null && sDate != "") {
      let strs: Array<string> = sDate.split('-');
      let year: string = strs[2];
      let month: string = strs[1];
      let day: string = strs[0];
      if (isFromTime) {
        result = parseInt(year + month + day + '000000');
      }
      else {
        result = parseInt(year + month + day + '235959');
      }
    }
    return result;
  }
    
  ConvertToStringShow(value: number): string {
		let ValueStr = Math.floor(value).toString();
		let ValueShow = "";
		switch (ValueStr.length % 3) {
		  case 0: {
			for (let i = 0; i < ValueStr.length; i++) {
			  if (i % 3 == 0) {
				if (i == 0) {
				  ValueShow += ValueStr.substr(i, 3);
				}
				else {
				  ValueShow += "." + ValueStr.substr(i, 3);
				}
			  }
			}
			break;
		  }
		  case 1: {
			for (let i = 0; i < ValueStr.length; i++) {
			  if (i == 0) {
				ValueShow += ValueStr.substr(i, 1);
			  } else {
				if (i % 3 == 1) {
				  ValueShow += "." + ValueStr.substr(i, 3);
				}
			  }
			}
			break;
		  }
		  case 2: {
			for (let i = 0; i < ValueStr.length; i++) {
			  if (i == 1) {
				ValueShow += ValueStr.substr(0, 2);
			  }
			  if (i != 1 && i != 0) {
				if (i % 3 == 2) {
				  ValueShow += "." + ValueStr.substr(i, 3);
				}
			  }
			}
		  }
			break;
		}
		return ValueShow;
  }
  
  formatDatetoStringInputDate(n) {
    let datereturn = "";
    if (n == null) {
      return '';
    }
    if (n.getMonth() + 1 >= 10) {
      if (n.getDate() >= 10) {
        datereturn = n.getDate() + "-" + (n.getMonth() + 1).toString() + "-" + n.getFullYear().toString();
      } else {
        datereturn = "0" + n.getDate() + "-" + (n.getMonth() + 1).toString() + "-" + n.getFullYear().toString();
      }
    } else {
      if (n.getDate() >= 10) {
        datereturn = n.getDate() + "-0" + (n.getMonth() + 1).toString() + "-" + n.getFullYear().toString();
      } else {
        datereturn = "0" + n.getDate() + "-0" + (n.getMonth() + 1).toString() + "-" + n.getFullYear().toString();
      }
    }
    return datereturn;
  }  
  
  formatDatetoStringSEARCH(n) {
    let datereturn = "";
    if (n == null) {
      return '';
    }
    if (n.getMonth() + 1 >= 10) {
      if (n.getDate() >= 10) {
        datereturn = n.getFullYear().toString() + "" + (n.getMonth() + 1).toString() + "" + n.getDate();
      } else {
        datereturn = n.getFullYear().toString() + "" + (n.getMonth() + 1).toString() + "0" + n.getDate();
      }
    } else {
      if (n.getDate() >= 10) {
        datereturn = n.getFullYear().toString() + "0" + (n.getMonth() + 1).toString() + "" + n.getDate();
      } else {
        datereturn = n.getFullYear().toString() + "0" + (n.getMonth() + 1).toString() + "0" + n.getDate();
      }
    }
    return datereturn;
  }
    
  formatNumberDate(n) {
    if (n == null){
      return '';
    }
    let fm = '';
    const s = '' + n;
    const y = s.substr(0, 4);
    fm += y;
    if (s.length >= 6) {
      const m = s.substr(4, 2);
      fm = m + '/' + fm;
    }
    if (s.length >= 8) {
      const d = s.substr(6, 2);
      fm = d + '/' + fm;
    }
    if (s.length > 8) {
      const time = s.substr(8, s.length - 8);
      if (time != '000000') {
        fm += ' ' + time.substr(0, 2) + ':' + time.substr(2, 2);
      }
    }
    return fm;
  }

  formatNumberYear(n) {
    if (n == null){
      return '';
    }
    let fm = '';
    const s = '' + n;
    const y = s.substr(0, 4);
    fm += y;
    return fm;
  }
}
