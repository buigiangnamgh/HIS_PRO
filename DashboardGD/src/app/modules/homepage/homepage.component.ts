import { Component, ViewChild, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { HomePageService } from './homepage.service';
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

@Component({
  selector: 'homepagecomponent',
  templateUrl: 'homepage.component.html',
  styleUrls: ['homepage.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class HomePageComponent extends BaseComponent implements OnInit, OnDestroy{
  public navItems : Array<MenuModel>;
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


	//begin chart hoat dong phau thuat thu thuat
	public data0: Object[] = [
        { 'x': 'Phẫu thuật SL: 97', y: 97, text: '97' },
        { 'x': 'Thủ thuật SL: 43', y: 43, text: '43' }        
    ];
    //Initializing Legend
    public legendSettings0: Object = {
        visible: true
    };
    public dataLabe0l: Object = {
        visible: true, position: 'Outside',
        connectorStyle: { length: '10%' }, name: 'text',
    };
    public startAngle0: number = 0;
    public endAngle0: number = 360;
    public enableSmartLabels: boolean = true;
    //Initializing Tooltip
    public tooltip0: Object = {
        enable: true, format: '${point.x} : <b>${point.y}%</b>'
    };
    public title0: string = 'Hoạt động Phẫu thuật thủ thuật';
     // custom code start
    public onLoad0(args: IAccLoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.accumulation.theme = <AccumulationTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    }
	
	//end chart hoat dong phau thuat thu thuat


	//begin chart so luong kham benh
	public dataslkb: Object[] = [
        { x: 'Phòng 1 (BH: 7, VP: 4)', y: 18 }, { x: 'Phòng 2 (BH: 8, VP: 1)', y: 9 }, { x: 'Phòng 3 (BH: 13, VP: 7)', y: 20 }, { x: 'Phòng 4 (BH: 11, VP: 13)', y: 24 },
        { x: 'Phòng 5 (BH: 11, VP: 16)', y: 27 }, { x: 'Phòng 6 (BH: 3, VP: 15)', y: 18 }, { x: 'Phòng 7 (BH: 11, VP: 18)', y: 29 }, { x: 'Phòng 8 (BH: 13, VP: 19)', y: 32 },
		{ x: 'Phòng 9 (BH: 11, VP: 12)', y: 23 }, { x: 'Phòng 10 (BH: 10, VP: 4)', y: 14 }, { x: 'Phòng 11 (BH: 12, VP: 18)', y: 30 }, { x: 'Phòng 12 (BH: 3, VP: 15)', y: 18 }
    ];    
   
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
        enable: true, format: 'SL: <b>${point.y}</b>'
    };
     // custom code start
    public loadslkb(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
     // custom code end
    public titleslkb: string = 'Số lượng khám bệnh';
    public chartAreaslkb: Object = {
        border: {
            width: 0
        }
    };
    public widthslkb: string = Browser.isDevice ? '100%' : '92%';
	//end chart so luong kham benh


	//chart 1
	public data: Object[] = [
        { 'x': 'Chrome', y: 37, text: '37%' }, { 'x': 'UC Browser', y: 17, text: '17%' },
        { 'x': 'iPhone', y: 19, text: '19%' },
        { 'x': 'Others', y: 4, text: '4%' }, { 'x': 'Opera', y: 11, text: '11%' },
        { 'x': 'Android', y: 12, text: '12%' }
    ];

	@ViewChild('pie')
    public pie: AccumulationChartComponent | AccumulationChart;
    public pieangle(e: Event): void {
        let angle: string = (document.getElementById('pieangle') as HTMLInputElement).value;
        this.pie.series[0].startAngle = parseFloat(angle);
        this.pie.series[0].endAngle = parseFloat(angle);
        this.pie.series[0].animation.enable = false;
        document.getElementById('pieangleText').innerHTML = angle;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    };
    public pieradius(e: Event): void {
        let radius: string = (document.getElementById('pieradius') as HTMLInputElement).value;
        this.pie.series[0].radius = radius + '%';
        document.getElementById('pieradiusText').innerHTML = (parseInt(radius, 10) / 100).toFixed(2);
        this.pie.series[0].animation.enable = false;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    };
    public pieexploderadius(e: Event): void {
        let radius: string = (document.getElementById('pieexploderadius') as HTMLInputElement).value;
        this.pie.visibleSeries[0].explodeOffset = radius + '%';
        document.getElementById('pieexploderadiusText').innerHTML = (parseInt(radius, 10) / 100).toFixed(2);
        this.pie.series[0].animation.enable = false;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    };
    public pieexplodeindex(e: Event): void {
        let index: number = +(document.getElementById('pieexplodeindex') as HTMLInputElement).value;
        this.pie.visibleSeries[0].explodeIndex = index;
        document.getElementById('pieexplodeindexText').innerHTML = index.toString();
        this.pie.series[0].animation.enable = false;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    };
    public piecenterx(e: Event): void {
        let x: string = (document.getElementById('x') as HTMLInputElement).value;
        this.pie.center.x = x + '%';
        document.getElementById('xvalue').innerHTML = x + '%';
        this.pie.series[0].animation.enable = false;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    }
    public piecentery(e: Event): void {
        let y: string = (document.getElementById('y') as HTMLInputElement).value;
        this.pie.center.y = y + '%';
        document.getElementById('yvalue').innerHTML = y + '%';
        this.pie.series[0].animation.enable = false;
        this.pie.removeSvg();
        this.pie.refreshSeries();
        this.pie.refreshChart();
    }
    public animation: Object = {
        enable: false
    };
    //Initializing Legend
    public legendSettings: Object = {
        visible: false,
    };
    //Initializing Datalabel
    public dataLabel: Object = {
        visible: true,
        position: 'Inside', name: 'text',
        font: {
            fontWeight: '600'
        }
    };
        
      // custom code start
    public load(args: IAccLoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.accumulation.theme = <AccumulationTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
      // custom code end
    public center: Object = {x: '50%', y: '50%'};
    public startAngle: number = 0;
    public endAngle: number = 360;
    public explode: boolean = true;
    public enableAnimation: boolean = true;
    public tooltip: Object = { enable: true, format: '${point.x} : <b>${point.y}%</b>' };
    public title: string = 'Số lượng khám bệnh';
  //end chart1-------------------------
  
  
  //chart hoat dong can lam san
  public data1: Object[] = [
        { x: 'Xét nghiệm', y: 7.3 },
        { x: 'X quang', y: 7.9 },
        { x: 'Cắt lớp vi tính', y: 6.8 },
        { x: 'Cộng hưởng từ', y: 7.0 },
        { x: 'Siêu âm 2D', y: 12.9 },
		{ x: 'Siêu âm 3D', y: 15.9 },
		{ x: 'Nội soi tai mũi họng', y: 16.9 },
		{ x: 'Nội soi tiêu hóa', y: 22.9 },
		{ x: 'Điện tim đồ', y: 3.9 },
		{ x: 'Điện não đồ', y: 9.9 },
		{ x: 'Đo loãng xương', y: 19.9 }
    ];
    public data2: Object[] = [
        { x: '', y: 17.3 },
        { x: '', y: 8.9 },
        { x: '', y: 46.8 },
        { x: '', y: 12.0 },
        { x: '', y: 16.9 },
		{ x: '', y: 5.9 },
		{ x: '', y: 22.9 },
		{ x: '', y: 33.9 },
		{ x: '', y: 8.9 },
		{ x: '', y: 9.9 },
		{ x: '', y: 11.9 }
    ];
    //Initializing Primary X Axis
    // public primaryXAxis: Object = {
        // valueType: 'Category',
        // interval: 1, crosshairTooltip: { enable: true },
        // isIndexed: true,
		// labelRotation : 45,
		// labelFormat: 'x',
    // };
    // //Initializing Primary Y Axis
    // public primaryYAxis: Object = {
        // title: 'Hoạt động cận lâm sàn',
        // labelFormat: '{value}'
    // };
    public marker: Object = {
        visible: false,
        height: 10,
        width: 10,
    };
	
	//Initializing Primary X Axis
    public primaryXAxis: Object = {
        title: 'Hoạt động cận lâm sàn',
        valueType: 'Category',
        interval: 1,
        //labelIntersectAction: 'Rotate135',
		labelRotation : 45,
    };
    //Initializing Primary Y Axis
    public primaryYAxis: Object = {
        title: 'Số lượng',
        labelFormat: '{value}',
        minimum: 0,
        maximum: 1000000
    };
	
	
	public legend1: Object = {
        visible: true,
        toggleVisibility: false
    };
    public marker1: Object = {
        visible: false,
        height: 10,
        width: 10,
    };
    //Initializing Crosshair
    public crosshairLabel: Object = { enable: true, lineType: 'Vertical' };
    public tooltip1: Object = { enable: true, shared: true };
    // custom code start
    public load1(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
    // custom code end
    public title1: string = 'Hoạt động cận lâm sàn';
    @ViewChild('chart')
    public chart: ChartComponent;
    public isIndexed(e: Event): void {
        let element: HTMLInputElement = <HTMLInputElement>e.target;
        this.chart.primaryXAxis.isIndexed = element.checked;
        if (this.chart.primaryXAxis.isIndexed) {
            this.chart.series[0].type = 'Column';
            this.chart.series[1].type = 'Column';
            this.chart.series[0].marker.visible = false;
            this.chart.series[1].marker.visible = false;
            this.chart.primaryXAxis.labelRotation = 45;
            this.chart.crosshair.line.width = 1;
        } else {
            this.chart.series[0].type = 'Line';
            this.chart.series[1].type = 'Line';
            this.chart.series[0].marker.visible = true;
            this.chart.series[1].marker.visible = true;
            this.chart.primaryXAxis.labelRotation = 90;
            this.chart.crosshair.line.width = 0;
        }
        this.chart.refresh();
    }
  //end chart hoat dong can lam san
  
  
  //start chart 3
  
  public data3: Object[] = [
        { x: 'Germany', y: 72, country: 'GER: 72'},
        { x: 'Russia', y: 103.1, country: 'RUS: 103.1'},
        { x: 'Brazil', y: 139.1, country: 'BRZ: 139.1'},
        { x: 'India', y: 462.1, country: 'IND: 462.1'},
        { x: 'China', y: 721.4, country: 'CHN: 721.4'},
        { x: 'United States<br>Of America', y: 286.9, country: 'USA: 286.9'},
        { x: 'Great Britain', y: 115.1, country: 'GBR: 115.1'},
        { x: 'Nigeria', y: 97.2, country: 'NGR: 97.2'},
    ];
    public tooltipMappingName2: 'country';
    public marker2: Object = {
        dataLabel: {
            visible: true,
            position: 'Top', font: {
                fontWeight: '600',
                color: '#ffffff'
            }
        }
    }
    //Initializing Primary X Axis
    public primaryXAxis2: Object = {
        title: 'Country',
        valueType: 'Category',
        majorGridLines: { width: 0 },
        enableTrim: false,
    };
    public pointRender2(args: IPointRenderEventArgs): void {
        let materialColors: string[] = ['#00bdae', '#404041', '#357cd2', '#e56590', '#f8b883', '#70ad47', '#dd8abd', '#7f84e8', '#7bb4eb',
            '#ea7a57', '#404041', '#00bdae'];
        let fabricColors: string[] = ['#4472c4', '#ed7d31', '#ffc000', '#70ad47', '#5b9bd5',
            '#c1c1c1', '#6f6fe2', '#e269ae', '#9e480e', '#997300', '#4472c4', '#70ad47', '#ffc000', '#ed7d31'];
        let bootstrapColors: string[] = ['#a16ee5', '#f7ce69', '#55a5c2', '#7ddf1e', '#ff6ea6',
            '#7953ac', '#b99b4f', '#407c92', '#5ea716', '#b91c52'];
        let highContrastColors: string[] = ['#79ECE4', '#E98272', '#DFE6B6', '#C6E773', '#BA98FF',
            '#FA83C3', '#00C27A', '#43ACEF', '#D681EF', '#D8BC6E'];
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        if (selectedTheme && selectedTheme.indexOf('fabric') > -1) {
            args.fill = fabricColors[args.point.index % 10];
        } else if (selectedTheme === 'material') {
            args.fill = materialColors[args.point.index % 10];
        } else if (selectedTheme === 'highcontrast') {
            args.fill = highContrastColors[args.point.index % 10];
        } else {
            args.fill = bootstrapColors[args.point.index % 10];
        }
    };
      // custom code start
    public load2(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
      // custom code end
    //Initializing Primary Y Axis
    public primaryYAxis2: Object = {
        minimum: 0,
        maximum: 800,
        labelFormat: Browser.isDevice ? '{value}' : '{value}M',
        edgeLabelPlacement: 'Shift',
        majorGridLines: { width: 0 },
        majorTickLines: { width: 0 },
        lineStyle: { width: 0 },
        labelStyle: {
            color: 'transparent'
        }
    };
    //Initializing Tooltip
    public tooltip2: Object = {
        enable: true, format: '${point.tooltip}'
    };
    public chartArea2: Object = {
        border: {
            width: 0
        }
    };
    public legend2: Object = {
        visible: false
    }
    public width2: string = Browser.isDevice ? '100%' : '60%';

    public title2: string = 'Internet Users – 2016';
   
  //end chart 3
  
  //start chart 4
   public chartArea4: Object = {
        border: {
            width: 0
        }
    };
    //Initializing Chart Width
    public width4: string = Browser.isDevice ? '100%' : '60%';
    public data4: Object[] = [
        { x: 'Egg', y: 2.2 }, { x: 'Fish', y: 2.4 },
        { x: 'Misc', y: 3 }, { x: 'Tea', y: 3.1 }
    ];
    public data41: Object[] = [
        { x: 'Egg', y: 1.2 }, { x: 'Fish', y: 1.3 },
        { x: 'Misc', y: 1.5 }, { x: 'Tea', y: 2.2 }
    ];
    //Initializing Marker
    public marker4: Object = {
        dataLabel: {
            visible: true,
            position: 'Top',
            font: {
                fontWeight: '600', color: '#ffffff'
            }
        }
    }
    //Initializing Primary X Axis
    public primaryXAxis4: Object = {
        valueType: 'Category',
        title: 'Food',
        interval: 1,
        majorGridLines: { width: 0 }
    };
    //Initializing Primary Y Axis
    public primaryYAxis4: Object = {
        labelFormat: '{value}B',
        edgeLabelPlacement: 'Shift',
        majorGridLines: { width: 0 },
        majorTickLines: { width: 0 },
        lineStyle: { width: 0 },
        labelStyle: {
            color: 'transparent'
        }
    };
    public tooltip4: Object = {
        enable: true
    };
      // custom code start
    public load4(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
      // custom code end
    public title4: string = 'UK Trade in Food Groups - 2015';
  
  //end chart 4
  
  
  //start chart 5
  public annotations5: ChartAnnotationSettingsModel[] = [
        {
            x: 'Sun',
            y: 2,
            coordinateUnits: 'Point',
            verticalAlignment: 'Top',
            content: '<div id="chart_cloud"><img src="./assets/chart/images/cloud.png" style="width: 41px; height: 41px"/></div>'
        },{
            x: 'Tue',
            y: 33,
            coordinateUnits: 'Point',
            verticalAlignment: 'Top',
            content: '<div id="chart_cloud"><img src="./assets/chart/images/sunny.png" style="width: 41px; height: 41px"/></div>'
        }
    ];

    public data5: Object[] = [
        { x: 'Sun', y: 15 }, { x: 'Mon', y: 22 },
        { x: 'Tue', y: 32 },
        { x: 'Wed', y: 31 },
        { x: 'Thu', y: 29 }, { x: 'Fri', y: 24 },
        { x: 'Sat', y: 18 },
    ];
    public data51: Object[] = [
        { x: 'Sun', y: 10 }, { x: 'Mon', y: 18 },
        { x: 'Tue', y: 28 },
        { x: 'Wed', y: 28 },
        { x: 'Thu', y: 26 }, { x: 'Fri', y: 20 },
        { x: 'Sat', y: 15 }
    ];
    public data52: Object[] = [
        { x: 'Sun', y: 2 }, { x: 'Mon', y: 12 },
        { x: 'Tue', y: 22 },
        { x: 'Wed', y: 23 },
        { x: 'Thu', y: 19 }, { x: 'Fri', y: 13 },
        { x: 'Sat', y: 8 },
    ];
    public chartArea5: Object = {
        border: {
            width: 0
        }
    };
    public width5: string = Browser.isDevice ? '100%' : '90%';
    //Initializing Primary X Axis
    public primaryXAxis5: Object = {
        valueType: 'Category',
        interval: 1, majorGridLines: { width: 0 },
        labelIntersectAction: 'Rotate90'
    };
    //Initializing Primary Y Axis
    public primaryYAxis5: Object = {
        labelFormat: '{value}°C',
        lineStyle: { width: 0 },
        majorTickLines: { width: 0 },
        minorTickLines: { width: 0 }
    };
    public marker5: Object = {
        visible: true,
        width: 10,
        height: 10
    };
    public tooltip5: Object = {
        enable: true
    };
     // custom code start
    public load5(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
     // custom code end
    public title5: string = 'NC Weather Report - 2016';
  
  //end chart 5
  
  
  //start chart6
   public chartArea6: Object = {
        border: {
            width: 0
        }
    };
    public width6: string = Browser.isDevice ? '100%' : '90%';
    public data6: Object[] = [
        { x: new Date(2002, 0, 1), y: 2.2 }, { x: new Date(2003, 0, 1), y: 3.4 },
        { x: new Date(2004, 0, 1), y: 2.8 }, { x: new Date(2005, 0, 1), y: 1.6 },
        { x: new Date(2006, 0, 1), y: 2.3 }, { x: new Date(2007, 0, 1), y: 2.5 },
        { x: new Date(2008, 0, 1), y: 2.9 }, { x: new Date(2009, 0, 1), y: 3.8 },
        { x: new Date(2010, 0, 1), y: 1.4 }, { x: new Date(2011, 0, 1), y: 3.1 }
    ];
    public data61: Object[] = [
        { x: new Date(2002, 0, 1), y: 2 }, { x: new Date(2003, 0, 1), y: 1.7 },
        { x: new Date(2004, 0, 1), y: 1.8 }, { x: new Date(2005, 0, 1), y: 2.1 },
        { x: new Date(2006, 0, 1), y: 2.3 }, { x: new Date(2007, 0, 1), y: 1.7 },
        { x: new Date(2008, 0, 1), y: 1.5 }, { x: new Date(2009, 0, 1), y: 2.8 },
        { x: new Date(2010, 0, 1), y: 1.5 }, { x: new Date(2011, 0, 1), y: 2.3 }
    ];
    public data62: Object[] = [
        { x: new Date(2002, 0, 1), y: 0.8 }, { x: new Date(2003, 0, 1), y: 1.3 },
        { x: new Date(2004, 0, 1), y: 1.1 }, { x: new Date(2005, 0, 1), y: 1.6 },
        { x: new Date(2006, 0, 1), y: 2 }, { x: new Date(2007, 0, 1), y: 1.7 },
        { x: new Date(2008, 0, 1), y: 2.3 }, { x: new Date(2009, 0, 1), y: 2.7 },
        { x: new Date(2010, 0, 1), y: 1.1 }, { x: new Date(2011, 0, 1), y: 2.3 }
    ];
    //Initializing Primary X Axis
    public primaryXAxis6: Object = {
        valueType: 'DateTime',
        labelFormat: 'y',
        majorGridLines: { width: 0 },
        intervalType: 'Years',
        edgeLabelPlacement: 'Shift'
    };
    //Initializing Primary Y Axis
    public primaryYAxis6: Object = {
        labelFormat: '{value}%',
        lineStyle: { width: 0 },
        maximum: 4,
        interval: 1,
        majorTickLines: { width: 0 },
        minorTickLines: { width: 0 }
    };
    public marker6: Object = {
        visible: false
    };
     // custom code start
    public load6(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
     // custom code end
    //Initializing Chart Title
    public title6: string = 'Inflation Rate in Percentage';
  //end chart 6
  
  
  //start chart 7
   //Initializing Primary X Axis
    public primaryXAxis7: Object = {
        title: 'Years',
        valueType: 'DateTime',
        skeleton: 'yMMM',
        edgeLabelPlacement: 'Shift',
        majorGridLines: { width: 0 }
    };
    //Initializing Primary Y Axis
    public primaryYAxis7: Object = {
        title: 'Profit ($)',
        rangePadding: 'None',
        lineStyle: { width: 0 },
        majorTickLines: { width: 0 }
    };
    public legend7: Object = {
        visible: false
    };
    public animation7: Object = { enable: false };
    public zoomSettings7: Object = {
        mode: 'X',
        enableMouseWheelZooming: true,
        enablePinchZooming: true,
        enableSelectionZooming: true,
        enableScrollbar: true
    };
    public chartArea7: Object = {
        border: {
            width: 0
        }
    };
     // custom code start
    public width7: string = Browser.isDevice ? '100%' : '90%';
    public load7(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
        args.chart.scrollBarModule = new ScrollBar(args.chart);
    };
     // custom code end
    public border7: Object = { width: 0.5, color: '#00bdae' };
    public title7: string = 'Sales History of Product X';
    public series71: Object = this.homeService.GetZoomingData().series1;
  
  //end chart 7
  
  
  
  //start chart 8
  public annotations8: ChartAnnotationSettingsModel[] = [
        {
            x: '20%',
            y: '25%',
            coordinateUnits: 'Pixel', region: 'Series',
            content: '<div id="chart_annotation" style="width: 200px; height: 200px"></div>'
        }
    ];
    public pie8: AccumulationChart;
    public render8: boolean = false;
    @ViewChild('chart8')
    public chart8: ChartComponent;
    public legend8: Object = {
        visible: true,
        toggleVisibility: false
    };
    //Initializing dataSource
    public dataSource8: Object = [
        { x: '2014', y0: 51, y1: 77, y2: 66, y3: 34 }, { x: '2015', y0: 67, y1: 49, y2: 19, y3: 38 },
        { x: '2016', y0: 143, y1: 121, y2: 91, y3: 44 }, { x: '2017', y0: 19, y1: 28, y2: 65, y3: 51 },
        { x: '2018', y0: 30, y1: 66, y2: 32, y3: 61 }, { x: '2019', y0: 189, y1: 128, y2: 122, y3: 76 },
        { x: '2020', y0: 72, y1: 97, y2: 65, y3: 82 }
    ];
    public pieDataSource8: Object[] = [
        { x: 'UK', y: 51, text: '22%' }, { x: 'Germany', y: 77, text: '34%' },
        { x: 'France', y: 66, text: '29%' }, { x: 'Italy', y: 34, text: '15%' }
    ];
    //Initializing Primary X Axis
    public primaryXAxis8: Object = {
        title: 'Years',
        majorGridLines: { width: 0 }, minorGridLines: { width: 1 },
        minorTickLines: { width: 1 }, interval: 1,
        labelIntersectAction: 'Rotate45',
        valueType: 'Category'
    };
    public chartArea8: Object = {
        border: {
            width: 0
        }
    };
    public width8: string = Browser.isDevice ? '100%' : '90%';
    // custom code start
    public load8(args: ILoadedEventArgs): void {
        let selectedTheme: string = location.hash.split('/')[1];
        selectedTheme = selectedTheme ? selectedTheme : 'Material';
        args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    };
    // custom code end
    //Initializing Primary Y Axis
    public primaryYAxis8: Object = {
        title: 'Sales in Billions', lineStyle: { width: 0 },
        minimum: 0, maximum: 700, interval: 100,
        majorGridLines: { width: 1 }, minorGridLines: { width: 1 },
        majorTickLines: { width: 0 }, minorTickLines: { width: 0 }, labelFormat: '{value}B',
    };
    public getValue8(series: Series[], pointIndex: number, y: number): string {
        let totalValue: number = 0;
        for (let ser of series) {
            totalValue += ser.points[pointIndex].y as number;
        }
        return (Math.round((y / totalValue) * 100)) + '%';
    };
    public title8: string = 'Mobile Game Market by Country';
    public selectedDataIndexes8: any[] = [{ series: 0, point: 0 }];
    public selectionMode8: SelectionMode = 'Cluster';

    public chartMouseUp8(args: IMouseEventArgs): void {
        if (args.target.indexOf('Point') > -1) {
            let pointIndex: number = parseInt(args.target[args.target.length - 1], 10);
            this.pieDataSource8 = [];
            for (let series of this.chart8.visibleSeries) {
                let value: number = series.points[pointIndex].y as number;
                this.pieDataSource8.push({
                    'x': series.name,
                    'y': value,
                    'text': this.getValue8(this.chart8.visibleSeries, pointIndex, value)
                });
            }
            this.pie8.series[0].dataSource = this.pieDataSource8;
            this.pie8.series[0].xName = 'x';
            this.pie8.series[0].yName = 'y';
            this.pie8.refresh();
        }
    }

    public loaded8(args: ILoadedEventArgs): void {
        if (this.render8) {
            this.pie8.destroy();
            let selectedTheme: string = location.hash.split('/')[1];
            selectedTheme = selectedTheme ? selectedTheme : 'Material';
            let theme: ChartTheme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
            this.pie8 = new AccumulationChart({
                background: 'transparent',
                series: [{
                    radius: '65%', animation: { enable: false },
                    dataSource: this.pieDataSource8,
                    xName: 'x', yName: 'y', dataLabel: { visible: true, position: 'Inside', font: { color: 'white' }, name: 'text' },
                }],
                theme: theme,
                legendSettings: { visible: false }
            });
            this.pie8.appendTo('#chart_annotation');
        }


    }
    public animationComplete8(args: ILoadedEventArgs): void {
        this.render8 = true;
        this.pie8 = new AccumulationChart({
            background: 'transparent',
            series: [{
                radius: '65%', animation: { enable: false },
                dataSource: this.pieDataSource8,
                xName: 'x', yName: 'y', dataLabel: { visible: true, position: 'Inside', name: 'text' },
            }],
            load: (args: IAccLoadedEventArgs) => {
                let selectedTheme: string = location.hash.split('/')[1];
                selectedTheme = selectedTheme ? selectedTheme : 'Material';
                args.accumulation.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
            },
            legendSettings: { visible: false },
            resized:  (args: IAccResizeEventArgs) => {
                location.reload();
            }
        });
        this.pie8.appendTo('#chart_annotation');
    }
  
  //end chart 8

  constructor(
    private route: ActivatedRoute,
    public homeService: HomePageService,	
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
	// let now = new Date();
	// this.timeto = ConvertdateToString(now, "235959");
    // this.timefrom = ConvertdateToString(now, "000000");      
	// this.setDateNumberToStringShow(now, now); 
	
    this.loadMenuProfile();
	this.IsAllowCustomDateRange = AppConfig.common.IsAllowCustomDateRangeHomePage;
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
		this.setReportDataMenu();
      }
    },1000)
  }  


  setReportDataMenu() {
	
	this.navItems.forEach(md => {		  
			md.loading = 1;		              
		});  
	  
	this.navItems.forEach(md => {
		
		if(md.url=='/khambenh'){	
			console.log('loading khambenh ' + new Date());
			this.showSpinner('spinner/khambenh');
			let srpt1 = new SearchReport();		
			srpt1.TIME_TO = this.timeto;
			srpt1.TIME_FROM = this.timefrom;
			srpt1.ReportTypeCode="MRS00002";
			srpt1.ReportTemplateCode="MRS0000201";
			this.homeService.loadDataReport(srpt1).subscribe(tk => {			
			  let SUM_COUNT_EXAM = 0;
			  if (tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.DATA_DETAIL != undefined && tk.Data.DATA_DETAIL.Department!= undefined && tk.Data.DATA_DETAIL.Department.length >0) {
				 SUM_COUNT_EXAM = tk.Data.DATA_DETAIL.Department.reduce(function (accumulator, pilot) {
				   return accumulator + pilot.COUNT_EXAM;
				 }, 0);
			  }
			  md.reportdata = SUM_COUNT_EXAM;
			  if(tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.SINGER_DATA != undefined && tk.Data.SINGER_DATA.ReportDate!= undefined && tk.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: " + tk.Data.SINGER_DATA.ReportDate;
			  }
			  
			  console.log('loaded khambenh ' + new Date());
			  md.loading = 0;
			  this.hideSpinner('spinner/khambenh');
			});
		}		
		
		if(md.url=='/khamcapcuu'){
			this.homeService.delay(100);
			console.log('loading khamcapcuu ' + new Date());
			let srpt2 = new SearchReport();				
			srpt2.CREATE_TIME_TO = this.timeto;
			srpt2.CREATE_TIME_FROM = this.timefrom;  
			srpt2.ReportTypeCode="TKB00021";
			srpt2.ReportTemplateCode="TKB0002101_1";
			this.homeService.loadDataReport(srpt2).subscribe(tktkb21 => {
				let SUM_TONG = 0;	
				if (tktkb21 != undefined && tktkb21 != null && tktkb21.Success && tktkb21.Data != undefined && tktkb21.Data.DATA_DETAIL != undefined && ((tktkb21.Data.DATA_DETAIL.Report0!= undefined && tktkb21.Data.DATA_DETAIL.Report0.length >0) || (tktkb21.Data.DATA_DETAIL.Report2!= undefined && tktkb21.Data.DATA_DETAIL.Report2.length >0))) { 
				let reportdatas0 = tktkb21.Data.DATA_DETAIL.Report0;
				//let reportdatas2 = tktkb21.Data.DATA_DETAIL.Report2;
				
				if (reportdatas0!= undefined && reportdatas0.length >0) {						
				
					SUM_TONG = reportdatas0.reduce(function (accumulator, pilot) {
					  return accumulator + pilot.TONG;
					}, 0);	
				}
				// else if (reportdatas2!= undefined && reportdatas2.length >0) {	
					// SUM_TONG = reportdatas2.reduce(function (accumulator, pilot) {
					  // return accumulator + pilot.TONG;
					// }, 0);
				// }				
			  }
			  md.reportdata = SUM_TONG;
			  if(tktkb21 != undefined && tktkb21 != null && tktkb21.Success && tktkb21.Data != undefined && tktkb21.Data.SINGER_DATA != undefined && tktkb21.Data.SINGER_DATA.ReportDate!= undefined && tktkb21.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: "+ tktkb21.Data.SINGER_DATA.ReportDate;
			  }
			  md.loading = 0;
			  console.log('loaded khamcapcuu ' + new Date());
			});
		}		
		
		if(md.url=='/vaovien'){
			this.homeService.delay(100);
			console.log('loading vaovien ' + new Date());
			let srpt464 = new SearchReport();				
			srpt464.TIME_TO = this.timeto;
			srpt464.TIME_FROM = this.timefrom;  
			srpt464.ReportTypeCode="MRS00002";
			srpt464.ReportTemplateCode="MRS0000206_1";			
			this.homeService.loadDataReport(srpt464).subscribe(tk => {				  
			  let SUM_COUNT_TREATMENT_IN=0;
			  let SUM_COUNT_TREATMENT_IN_BHYT=0;
			  let SUM_VP=0;
			  let SUM_TUNGUYEN=0;
			  if (tk != undefined && tk != null && tk.Data != undefined && tk.Data.DATA_DETAIL != undefined && tk.Data.DATA_DETAIL.Report0!= undefined && tk.Data.DATA_DETAIL.Report0.length >0) {  
			  let reportdatas0 = tk.Data.DATA_DETAIL.Report0;
				SUM_COUNT_TREATMENT_IN = reportdatas0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_TREATMENT_IN;
						}, 0);
				SUM_COUNT_TREATMENT_IN_BHYT = reportdatas0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_TREATMENT_IN_BHYT;
						}, 0);
				SUM_VP = (SUM_COUNT_TREATMENT_IN - SUM_COUNT_TREATMENT_IN_BHYT);
			  }
			  md.reportdata =  SUM_COUNT_TREATMENT_IN;
			  md.reportdataext3 = "- " + SUM_COUNT_TREATMENT_IN_BHYT + " (BHYT)";
			  md.reportdataext4 = "- " + SUM_VP + " (Viện Phí)";
			  md.reportdataext5 = "- " + SUM_TUNGUYEN + " (Tự nguyện)";	
			  md.reportdataext6 = " ";
			  md.reportdataext7 = " ";
			  	
			  if(tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.SINGER_DATA != undefined && tk.Data.SINGER_DATA.ReportDate!= undefined && tk.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: " + tk.Data.SINGER_DATA.ReportDate;
			  }
			  
			  md.loading = 0;
			  console.log('loaded vaovien ' + new Date());
			});	
		}
		if(md.url=='/dangnamvien'){
			this.homeService.delay(100);
			console.log('loading dangnamvien ' + new Date());
			let srpt3 = new SearchReport();				
			srpt3.TIME_FROM = this.timefrom;
			srpt3.TIME_TO = this.timeto;  
			srpt3.ReportTypeCode="MRS00555";
			srpt3.ReportTemplateCode="MRS0055501";
			this.homeService.loadDataReport(srpt3).subscribe(tk => {				  
			  let SUM_COUNT_END=0;
			  let SUM_BHYT_DT=0;
			  let SUM_BHYT_TT=0;
			  let SUM_VP=0;
			  if (tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.DATA_DETAIL != undefined && tk.Data.DATA_DETAIL.Report!= undefined && tk.Data.DATA_DETAIL.Report.length >0) {  
				let report0= tk.Data.DATA_DETAIL.Report;
				SUM_COUNT_END = report0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_END;
						}, 0);						
				SUM_BHYT_DT = report0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_END_BHDT;
						}, 0);
				SUM_BHYT_TT = report0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_END_BHTT;
						}, 0); 
				SUM_VP = report0.reduce(function (accumulator, pilot) {
						  return accumulator + pilot.COUNT_END_VP;
						}, 0);				
			  }			  	
			 
			  md.reportdata = SUM_COUNT_END;
			  //md.url0="/#/dangnamvien";
			  md.reportdataext4 = "BN bhyt đúng tuyến: "+ (SUM_BHYT_DT);	
			  md.url4="/#/hiendiennt";
			  md.reportdataext5 = "BN bhyt trái tuyến: " + (SUM_BHYT_TT);
			  md.url5="/#/hiendiennt";
			  md.reportdataext6 = "BN viện phí: " + (SUM_VP);
			  md.url6="/#/hiendiennt";	
			  
			  if(tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.SINGER_DATA != undefined && tk.Data.SINGER_DATA.ReportDate!= undefined && tk.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: " + tk.Data.SINGER_DATA.ReportDate;
			  }
			  
			  md.loading = 0;
			  console.log('loaded dangnamvien ' + new Date());
			});	
		}
		if(md.url=='/ravien'){
			this.homeService.delay(100);
			console.log('loading ravien ' + new Date());
			let srpt4 = new SearchReport();				
			srpt4.TIME_TO = this.timeto;
			srpt4.TIME_FROM = this.timefrom;  
			srpt4.ReportTypeCode="MRS00555";
			srpt4.ReportTemplateCode="MRS0055509_1";
			this.homeService.loadDataReport(srpt4).subscribe(tk => {	
			
			  let SUM_COUNT_EXP =0;
			  let SUM_KHAC =0;//<#Evaluate(<#ReportNew1.COUNT_EXP;>-<#ReportNew1.COUNT_DIE;>-<#ReportNew1.COUNT_OUT_CV;>-<#ReportNew1.COUNT_OUT_TRON;>)>
			  let SUM_CVIEN =0;//<#Evaluate( <#ReportNew1.COUNT_OUT_CV;>-<#ReportNew1.COUNT_OUT_CV_LEN;>)>
			  let SUM_CVIEN1 =0;
			  let SUM_RAVIEN =0;
			  let SUM_TUVONG =0;
			  let SUM_HENKHAM =0;
			  let SUM_TRON =0;
			  let SUM_NANGVE =0; 
			  let SUM_COUNT_OUT_CV_LEN =0;
			  if (tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.DATA_DETAIL != undefined && tk.Data.DATA_DETAIL.Report!= undefined && tk.Data.DATA_DETAIL.Report.length >0) {			    
				SUM_COUNT_EXP = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
						  return accumulator + (pilot.COUNT_EXP);
						}, 0);
				SUM_RAVIEN = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
							  return accumulator + (pilot.COUNT_OUT_RAVIEN);
							}, 0);
				SUM_CVIEN = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
				  return accumulator + (pilot.COUNT_OUT_CV -pilot.COUNT_OUT_CV_LEN );
				}, 0);
				SUM_CVIEN1 = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
				  return accumulator + (pilot.COUNT_OUT_CV );
				}, 0);
				SUM_COUNT_OUT_CV_LEN = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
				  return accumulator + (pilot.COUNT_OUT_CV_LEN );
				}, 0);
				SUM_HENKHAM = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
							  return accumulator + (pilot.COUNT_OUT_HEN);
							}, 0);			
				SUM_TUVONG = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
							  return accumulator +  (pilot.COUNT_DEATH_INFO );
							}, 0);
				SUM_TRON = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
				  return accumulator +  (pilot.COUNT_OUT_TRON );
				}, 0);
				SUM_NANGVE = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
						  return accumulator +  (pilot.COUNT_OUT_NANG);
						}, 0);	
				SUM_KHAC = tk.Data.DATA_DETAIL.Report.reduce(function (accumulator, pilot) {
						  return accumulator +  (pilot.COUNT_EXP - pilot.COUNT_DIE - pilot.COUNT_OUT_CV - pilot.COUNT_OUT_TRON - pilot.COUNT_OUT_NANG);
						}, 0);						
			  }
			  				
			//Tổng XXX 
			// - A (RV khác)
			// - B (C.viện)
			// - C (CV TT)
			// - D (Tử vong)
			// - E (Nặng X.về)		
			  md.reportdata =  SUM_COUNT_EXP;
			  md.reportdataext3 = "- " + SUM_KHAC + " (Ra viện)";
			  md.reportdataext4 = "- " + SUM_CVIEN + " (Chuyển viện chuyên khoa)";
			  md.reportdataext5 = "- " + SUM_COUNT_OUT_CV_LEN + " (Chuyển tuyến trên)";
			  md.reportdataext6 = "- " + SUM_TUVONG + " (Tử vong)";
			  md.reportdataext7 = "- " + SUM_NANGVE + " (Nặng X.về)";
			  if(tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.SINGER_DATA != undefined && tk.Data.SINGER_DATA.ReportDate!= undefined && tk.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: " + tk.Data.SINGER_DATA.ReportDate;
			  }
			  md.loading = 0;
			  console.log('loaded ravien ' + new Date());
			});	
		}		
		if(md.url=='/taichinh'){
			this.homeService.delay(100);
			console.log('loading taichinh ' + new Date());
			
			let SUM_TT_RA_VIEN =0;
			let SUM_THU_TRUC_TIEP =0;
			let SUM_TAM_UNG =0;
			this.showSpinner('spinner/taichinh');
			let srpt5 = new SearchReport();				
			srpt5.TIME_TO = this.timeto;
			srpt5.TIME_FROM = this.timefrom;  
			srpt5.ReportTypeCode="MRS00012";
			srpt5.ReportTemplateCode="MRS00012TKB0003601";
			this.homeService.loadDataReport(srpt5).subscribe(tk => {
			    if (tk != undefined && tk != null && tk.Success && tk.Data != undefined && tk.Data.DATA_DETAIL != undefined && ((tk.Data.DATA_DETAIL.Report0!= undefined && tk.Data.DATA_DETAIL.Report0.length >0))) {
					let report1 = tk.Data.DATA_DETAIL.Report0;				
					if(report1!= undefined && report1!=null && report1.length >0)
					{
						SUM_TT_RA_VIEN = report1.reduce(function (accumulator, pilot) {
							  return accumulator + pilot.BHTT+ pilot.AMOUNT_VP+ pilot.AMOUNT_YC+ pilot.AMOUNT_BH;
							}, 0);	
							
						//console.log('loaded report data: MRS00012TKB0003601 SUM_TT_RA_VIEN'+SUM_TT_RA_VIEN);	
					}
			    }
			
				let srpt6 = new SearchReport();				
				srpt6.TIME_TO = this.timeto;
				srpt6.TIME_FROM = this.timefrom;  
				srpt6.ReportTypeCode="TKB00016";
				srpt6.ReportTemplateCode="TKB0001602";
				this.homeService.loadDataReport(srpt6).subscribe(tk1 => {
					if (tk1 != undefined && tk1 != null && tk1.Success && tk1.Data != undefined && tk1.Data.DATA_DETAIL != undefined && ((tk1.Data.DATA_DETAIL.Report1!= undefined && tk1.Data.DATA_DETAIL.Report1.length >0))) {
						let report2 = tk1.Data.DATA_DETAIL.Report1;				
						if(report2!= undefined && report2!=null && report2.length >0)
						{
							SUM_THU_TRUC_TIEP = report2.reduce(function (accumulator, pilot) {
								  return accumulator + pilot.TOTAL_PRICE;
								}, 0);		
						}				
					  }
				  
					let srpt7 = new SearchReport();				
					srpt7.TIME_TO = this.timeto;
					srpt7.TIME_FROM = this.timefrom;  
					srpt7.ReportTypeCode="MRS00482";
					srpt7.ReportTemplateCode="MRS0048204";
					this.homeService.loadDataReport(srpt7).subscribe(tk2 => {
						if (tk2 != undefined && tk2 != null && tk2.Success && tk2.Data != undefined && tk2.Data.DATA_DETAIL != undefined && ((tk2.Data.DATA_DETAIL.Report!= undefined && tk2.Data.DATA_DETAIL.Report.length >0))) {
						let report3 = tk2.Data.DATA_DETAIL.Report;				
						if(report3!= undefined && report3!=null && report3.length >0)
						{
							SUM_TAM_UNG = report3.reduce(function (accumulator, pilot) {
								  return accumulator + pilot.DEPOSIT_CASH;
								}, 0);
							console.log('loaded taichinh ' + new Date());						
						}				
					  }
					  this.hideSpinner('spinner/taichinh');
					});						
					
				  md.reportdata = this.ConvertToStringShow((SUM_TT_RA_VIEN + SUM_THU_TRUC_TIEP + SUM_TAM_UNG));	
				  md.reportdataext = "Doanh thu: " + this.ConvertToStringShow((SUM_TT_RA_VIEN + SUM_THU_TRUC_TIEP));
				  md.url1="/#/chitietdoanhthu";
				  md.reportdataext1 = "Tạm ứng: " + this.ConvertToStringShow(SUM_TAM_UNG);
				  md.url2="/#/chitiettamung";
				  md.reportdataext3="Đơn vị: Đồng";
				  if(tk1 != undefined && tk1 != null && tk1.Success && tk1.Data != undefined && tk1.Data.SINGER_DATA != undefined && tk1.Data.SINGER_DATA.ReportDate!= undefined && tk1.Data.SINGER_DATA.ReportDate!=null){				  
				  md.reportdate = "Cập nhật: " + tk1.Data.SINGER_DATA.ReportDate;
			  }
				  md.loading = 0;
				});				
			});	
			
		}			
	});
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
	this.setReportDataMenu();
  }
  
  loadMenuProfile() {	 
	  const authorizeResultData = localStorage.getItem('AuthorizeResultData');    
      if(authorizeResultData != undefined && authorizeResultData != null && authorizeResultData != '')
      {     
        this.navItems = JSON.parse(authorizeResultData);
		console.log(this.navItems);
		this.setReportDataMenu();
      }
	  else
	  {
	    this.navItems = new Array<MenuModel>();
        this.menuService.loadMenu().subscribe(tk => {
          //console.log(tk);
          if (tk != null && tk.Data != null && tk.Data.ModuleInRoles !=null && tk.Data.ModuleInRoles.length >0) {            
            tk.Data.ModuleInRoles.forEach(md => {
              if(md.IS_VISIBLE === 1){
                let d = new MenuModel();
                d.number_order=md.NUM_ORDER;
                d.name = md.MODULE_NAME;
				//d.IS_VISIBLE = md.IS_VISIBLE;
                d.url = md.MODULE_LINK;
                d.icon = md.ICON_LINK;
				d.cssname = md.MODULE_URL;
				d.reportdata = null;//TODO
				d.loading = 1;//TODO
                let x = this.navItems.filter(o=>o.url== md.MODULE_LINK);
                if(x.length<=0){
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
  }
 
}
