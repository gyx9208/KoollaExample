/// <reference path="../Content/highCharts/highcharts.js" />
$(document).ready(function () {
    $("#weeksell").click(function () {
        $("#dashboard").children().remove();
        $.getJSON("/SellMore/weeksell", function (data) {
            $("#dashboard").highcharts({
                chart: {
                    type: 'pyramid',
                    marginRight: 120
                },
                title: {
                    text: '本周业绩',
                    x: -50
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b> ({point.y:,.0f}人次)',
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
                            softConnector: true
                        }
                    }
                },
                legend: {
                    enabled: false
                },
                series: [{
                    name: '客户人次',
                    data: data
                }]
            });
        });
        
    });
    $("#monthmoney").click(function () {
        $("#dashboard").children().remove();

        $.getJSON("/SellMore/monthmoney", function (data) {
            $('#dashboard').highcharts({
                chart: {
                    type: 'area'
                },
                title: {
                    text: '本月销售金额'
                },
                subtitle: {
                    text: '每天目标20000元'
                },
                xAxis: {
                    title:{text:'日期'},
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return this.value; // clean, unformatted number for year
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: '销售金额'
                    },
                    labels: {
                        formatter: function () {
                            return this.value / 1000 + 'k';
                        }
                    }
                },
                tooltip: {
                    shared: true,
                    headerFormat: ' <span style="font-size: 10px">{point.key}日</span><br/>',
                    pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: <b>{point.y}</b>元<br/>'

                },
                plotOptions: {
                    area: {
                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                },
                series: [{
                    name: '目标',
                    data: data.target
                }, {
                    name: '实际',
                    data: data.actual
                }]
            });
        });
    });
    $("#monthcount").click(function () {
        $("#dashboard").children().remove();

        $.getJSON("/SellMore/monthcount", function (data) {
            $('#dashboard').highcharts({
                chart: {
                    type: 'area'
                },
                title: {
                    text: '本月销售人次'
                },
                subtitle: {
                    text: '每天目标1人次'
                },
                xAxis: {
                    title: { text: '日期' },
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: '销售人次'
                    }
                },
                tooltip: {
                    shared: true,
                    headerFormat: ' <span style="font-size: 10px">{point.key}日</span><br/>',
                    pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: <b>{point.y}</b>人次<br/>'
                },
                plotOptions: {
                    area: {
                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                },
                series: [{
                    name: '目标',
                    data: data.target
                }, {
                    name: '实际',
                    data: data.actual
                }]
            });
        });
    });
    $("#sellfilter").click(function () {
        $("#dashboard").children().remove();

        $.getJSON("/SellMore/sellfilter", function (data) {
            $('#dashboard').highcharts({
                chart: {
                    type: 'funnel',
                    marginRight: 100
                },
                title: {
                    text: '所有客户销售漏斗',
                    x: -50
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b> ({point.y:,.0f})',
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
                            softConnector: true
                        },
                        neckWidth: '30%',
                        neckHeight: '25%'

                        //-- Other available options
                        // height: pixels or percent
                        // width: pixels or percent
                    }
                },
                legend: {
                    enabled: false
                },
                series: [{
                    name: '客户人次',
                    data: data
                }]
            });
        });
    });
    $("#routeanalyse").click(function () {
        $("#dashboard").children().remove();

        $.getJSON("/SellMore/routeanalyse", function (data) {
            $('#dashboard').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: '所有客户渠道分析'
                },
                tooltip: {
                    pointFormat: '比例: <b>{point.percentage:.1f}%</b><br>人数：<b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '比例',
                    data: data
                }]
            });
        });

    });
    $("#lossreason").click(function () {
        $("#dashboard").children().remove();
        $("#dashboard").html("<h1>Coming Soon...</h1>");
    });
});