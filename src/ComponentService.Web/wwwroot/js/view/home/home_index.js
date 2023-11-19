

$('.select2').select2();
InitialMontlySaleSummaryBarchart();
InitialYearlySaleSummaryAreaChart();
//$(document).on('change', '.select2', function (e) {
//    // Get the selected value
//    var selectedValue = $(this).val();
//    // Get the data-row attribute to identify the row
//    var row = $(this).data('row');
//    console.log($(this).data('name'));
//    // Log the selected value for the current row (you can replace this with your desired logic)
//    console.log("Row " + row + ": " + selectedValue);
//    //ShowMessageInfo('Selected value :' + selectedValue);
//});

$('.ddl-source-month').on("change", function () {
    var text = $('option:selected', $(this)).text();
    var selectedMonth = parseInt($('option:selected', $(this)).val());
    $("#global-loader").css('display', '');
    var request = $.ajax({
        url: '/Home/GetBarChartDataByMonth',
        async: true,
        type: 'GET',
        dataType: 'JSON',
        data: { "nMonth": selectedMonth },
        success: function (response) {

            if (response.result) {
                //สาขาต้นทาง
                /*ShowMessageSuccess('กำลังสร้างกราฟ...');*/
                renderChart(response.data);
                HideNoChartData();
            }
            else {
                ShowMessageError('ไม่พบข้อมูล');
                /*$("#container").highcharts().destroy();*/
                if ($('#container').highcharts()) $('#container').highcharts().destroy();
                console.log('chart disposed')
                ShowNoChartData();
            }

            $("#global-loader").css('display', 'none');
        },
        failure: function (response) {
            ShowMessageError(response);
            $("#global-loader").css('display', 'none');
        },
        error: function (response) {
            ShowMessageWarning(response);
            $("#global-loader").css('display', 'none');
        }
    });

});

function InitialMontlySaleSummaryBarchart() {
    const dateToday = new Date();
    console.log(dateToday);
    const currentMonth = dateToday.getMonth() + 1;
    console.log(currentMonth);
    var request = $.ajax({
        url: '/Home/GetBarChartDataByMonth',
        async: true,
        type: 'GET',
        dataType: 'JSON',
        data: { "nMonth": currentMonth },
        success: function (response) {

            if (response.result) {
                //สาขาต้นทาง
                /*ShowMessageSuccess('กำลังสร้างกราฟ...');*/
                renderChart(response.data);
                //$("#divEmptyData").hide();
                //$("#divEmptyData").attr("hidden", true);
                HideNoChartData2();
            }
            else {
                ShowNoChartData2();
                ShowMessageError('ไม่พบข้อมูล');
            }
        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });
}
function InitialYearlySaleSummaryAreaChart() {

    var request = $.ajax({
        url: '/Home/GetAreaChartDataByYear',
        async: true,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.result) {
                //สาขาต้นทาง
                /*ShowMessageSuccess('กำลังสร้างกราฟ...');*/
                renderAreaChart(response.data);
                //$("#divEmptyData").hide();
                //$("#divEmptyData").attr("hidden", true);
                HideNoChartData2();
            }
            else {
                ShowNoChartData2();
                ShowMessageError('ไม่พบข้อมูล');
            }
        },
        failure: function (response) {
            ShowMessageError(response);
        },
        error: function (response) {
            ShowMessageWarning(response);
        }
    });
}
function InitialTop10SellingItemPieChart() {
    $('#container3').highcharts({
        chart: {
            type: 'pie'
        },
        title: {
            text: 'สัดส่วนการสินค้า'
        },
        tooltip: {
            valueSuffix: '%'
        },
        subtitle: {
            text:
                ''
        },
        plotOptions: {
            series: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: [{
                    enabled: true,
                    distance: 20
                }, {
                    enabled: true,
                    distance: -40,
                    format: '{point.percentage:.1f}%',
                    style: {
                        fontSize: '1.2em',
                        textOutline: 'none',
                        opacity: 0.7
                    },
                    filter: {
                        operator: '>',
                        property: 'percentage',
                        value: 10
                    }
                }]
            }
        },
        series: [
            {
                name: 'Percentage',
                colorByPoint: true,
                data: [
                    {
                        name: 'Water',
                        y: 55.02
                    },
                    {
                        name: 'Fat',
                        sliced: true,
                        selected: true,
                        y: 26.71
                    },
                    {
                        name: 'Carbohydrates',
                        y: 1.09
                    },
                    {
                        name: 'Protein',
                        y: 15.5
                    },
                    {
                        name: 'Ash',
                        y: 1.68
                    }
                ]
            }
        ]
    });
}

// Function to render Highcharts chart
function renderChart(chartobj) {

    console.log(chartobj);
    /*console.log(chartobj.data.map(item => item.yvalue));*/
    $('#container').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: chartobj.title_text
        },
        xAxis: {
            categories: chartobj.data.map(item => item.xvalue)
        },
        yAxis: {
            title: {
                text: 'ยอดขาย(บาท)'
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' บาท'
        },
        series: [{
            name: 'สาขา',
            data: chartobj.data.map(item => item.yvalue)
        }]
    });
}

// Function to render Highcharts chart
function renderAreaChart(chartobj) {
    const dateToday = new Date();
    console.log(dateToday);
    const currentYear = dateToday.getFullYear();
    console.log(currentYear);

    console.log(chartobj);
    $('#container2').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'ยอดขายประจำปี ' + currentYear
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: chartobj.data.map(item => item.xvalue),
            tickmarkPlacement: 'on',
            title: {
                enabled: false
            }
        },
        yAxis: {
            title: {
                text: 'จำนวนเงิน(บาท)'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' บาท'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    lineWidth: 1,
                    lineColor: '#666666'
                }
            }
        },
        series: [{
            name: 'ปี ' + currentYear,
            data: chartobj.data.map(item => item.yvalue)
        }]
    });
}

function HideNoChartData() {
    $("#divEmptyData").attr("hidden", true);
}

function ShowNoChartData() {
    $("#divEmptyData").attr("hidden", false);
}

function HideNoChartData2() {
    $("#divEmptyData2").attr("hidden", true);
}

function ShowNoChartData2() {
    $("#divEmptyData2").attr("hidden", false);
}