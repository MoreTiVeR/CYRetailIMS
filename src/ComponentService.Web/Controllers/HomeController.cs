using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ChartAPI;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
//using Point = DotNet.Highcharts.Options.Point;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class HomeController : BaseController
{
    //PieChart : https://www.codeproject.com/Articles/820349/Highcharts-in-asp-net-using-jquery-ajax

    private readonly IChartAPI _chartAPI;
    public HomeController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IChartAPI chartAPI)
        : base(httpClientRequest, mapper, log)
    {
        _chartAPI = chartAPI;
    }

    public IActionResult Index()
    {
        //base.InitialData();
        //BaseResponse<Highcharts> resPieChart = GeneratePieChart();
        //ViewBag.PieChart = resPieChart;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> GetBarChartDataByMonth(int nMonth)
    {
        try
        {
            BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>> resChartData = await _chartAPI.GetMontlySameSummaryAsync(new GetMontlySaleSummaryBarchartQuery { month = nMonth });
            if (!resChartData.result)
            {
                return Json(new { result = false, message = resChartData.error.error.message });
            }

            // Replace this with your data retrieval logic (from database, service, etc.)
            HighchartsModel highchartsModel = new HighchartsModel
            {
                title_text = "ยอดขายประจำเดือน" + new DateTime(DateTime.Now.Year, nMonth, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("th")),
                data = new List<HighchartsDataModel>()
            };
            highchartsModel.data = resChartData.data.Select(s => new HighchartsDataModel
            {
                xvalue = s.branchname,
                yvalue = Convert.ToDouble(s.totalamount)
            }).ToList();
            return Json(new { result = true, data = highchartsModel, message = "Success" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAreaChartDataByYear()
    {
        try
        {
            BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>> resChartData = await _chartAPI.GetMontlySaleSummaryByYearAsync(new GetMontlySaleSummaryByYearQuery { year = DateTime.Now.Year });
            if (!resChartData.result)
            {
                return Json(new { result = false, message = resChartData.error.error.message });
            }

            // Replace this with your data retrieval logic (from database, service, etc.)
            HighchartsModel highchartsModel = new HighchartsModel
            {
                title_text = $"ยอดขายประจำปี {DateTime.Now.Year}",
                data = new List<HighchartsDataModel>()
            };
            highchartsModel.data = resChartData.data.Select(s => new HighchartsDataModel
            {
                xvalue = s.monthname,
                yvalue = Convert.ToDouble(s.totalamount)
            }).ToList();
            return Json(new { result = true, data = highchartsModel, message = "Success" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    //public BaseResponse<Highcharts> GeneratePieChart()
    //{
    //	decimal totalAmountRecive = 0;
    //	decimal totalAmountNotRecive = 0;
    //	try
    //	{
    //		totalAmountNotRecive = 1000;
    //		totalAmountRecive = 2900;

    //		#region Main Chart
    //		this._chart
    //		.InitChart(new Chart { PlotBackgroundColor = null, PlotBorderWidth = null, PlotShadow = false })
    //		.SetTitle(new Title { Text = "สรุปยอดขาย รับสินค้าและรอรับสินค้า" })
    //		.SetTooltip(new Tooltip { PointFormat = "{series.name}: <b>{point.y:,.2f} บาท(฿)</b>" })
    //		.SetPlotOptions(new PlotOptions
    //		{
    //			Pie = new PlotOptionsPie
    //			{
    //				AllowPointSelect = true,
    //				Cursor = Cursors.Pointer,
    //				DataLabels = new PlotOptionsPieDataLabels
    //				{
    //					Enabled = true,
    //					Color = ColorTranslator.FromHtml("#000000"),
    //					ConnectorColor = ColorTranslator.FromHtml("#000000"),
    //					Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+Highcharts.numberFormat(this.y, 2, '.', ',')+' บาท(฿)'; }"
    //				}
    //			}
    //		})
    //		.SetSeries(new Series
    //		{
    //			Type = ChartTypes.Pie,
    //			Name = "จำนวนเงิน",
    //			Data = new Data(new object[]
    //				{
    //						new Point
    //						{
    //							Name = "ยอดขาย รอรับสินค้า",
    //							Y = Convert.ToInt32(totalAmountNotRecive),
    //							Sliced = true,
    //							Selected = true
    //						},
    //                           //new object[] { "กำไรก่อนหักค่าใช้จ่าย", chartData.TotalAmountBeforeNet },
    //                           new object[] { "ยอดขาย รับสินค้า", totalAmountRecive }
    //				})
    //		});

    //		#endregion
    //	}
    //	catch (Exception ex)
    //	{

    //	}
    //	return new BaseResponse<Highcharts>
    //	{
    //		result = true,
    //		data = this._chart
    //	};
    //}
}