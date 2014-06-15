/// <reference path="../Content/kendoUI/js/kendo.all.min.js" />
$(document).ready(function () {
    var crudServiceBaseUrl = "/SellMore/Customer",
        dataSource = new kendo.data.DataSource({
            transport: {
                read:  {
                    url: crudServiceBaseUrl + "Read",
                    dataType: "json"
                },
                update: {
                    url: crudServiceBaseUrl + "Update",
                    dataType: "json",
                },
                destroy: {
                    url: crudServiceBaseUrl + "Destroy",
                    dataType: "json"
                },
                create: {
                    url: crudServiceBaseUrl + "Create",
                    dataType: "json"
                },
                parameterMap: function(options, operation) {
                    if (operation !== "read" && options.models) {
                        return {models: kendo.stringify(options.models)};
                    }
                }
            },
            batch: true,
            pageSize: 20,
            schema: {
                model: {
                    id: "cid",
                    fields: {
                        cid: { type:"number", editable: false, nullable: false },
                        name: { validation: { required: true } },
                        company: { validation: { required: true } },
                        phone: { validation: { required: true } },
                        email: { validation: { required: true } },
                        channel: { defaultValue: { ChannelId: 1, ChannelName: "中介/业务伙伴" } },
                        product: { defaultValue: { ProductId: 1, ProductName: "按揭贷款" } },
                        money: { type: "number" },
                        status: { defaultValue: { StatusId: 1, StatusName: "评估客户" } }
                    }
                },
                errors:"error"
            },
            error: function (e) {
                dataSource.cancelChanges();
                alert(e.errors); // displays "Invalid query"
            }
        });

    var grid=$("#grid").kendoGrid({
        dataSource: dataSource,
        pageable: true,
        height: 550,
        toolbar: [{ name: "create" ,text:"增加新客户"}],
        columns: [
            {
                field: "name", title: "姓名",
                filterable: {
                    messages:{info:"符合条件的姓名"},
                    operators: {
                        string: {
                            contains: "包含"
                        }
                    }},
                template: "<a href=\"/SellMore/CustomerNote/#=cid#\" target=\"_blank\">#=name#</a>"
            },
            { field: "company", title: "公司", filterable: false },
            { field: "phone", title: "电话", width: "120px", filterable: false },
            { field: "email", title: "电子邮件", filterable: false },
            {
                field: "money", title: "金额",
                filterable: {
                    messages: { info: "符合条件的金额" }
                }
            },
            {
                field: "product", title: "产品", width: "120px", editor: productDropDownEditor, template: "#=product.ProductName#",
                sortable: {
                    compare: function (a, b) {
                        return a.product.ProductId - b.product.ProductId;
                    }
                },
                filterable: false
            },
            {
                field: "status", title: "进展", template: "#=status.StatusName#", width: "120px", editor: statusDropDownEditor,
                sortable: {
                    compare: function (a, b) {
                        return a.status.StatusId - b.status.StatusId;
                    }
                }, filterable: false
            },
            {
                field: "channel", title: "渠道", template: "#=channel.ChannelName#", width: "120px", editor: channelDropDownEditor,
                sortable: {
                    compare: function (a, b) {
                        return a.channel.ChannelId - b.channel.ChannelId;
                    }
                }, filterable: false
            },
            { command: [{ name: "edit", text:{ edit:"编辑",cancel:"取消",update:"更新"} }, { name: "destroy", text: "删除" }], title: "&nbsp;", width: "200px" }],
        resizable: true,
        sortable: {
            mode: "multiple",
            allowUnsort: true
        },
        editable: {
            mode: "popup",
            confirmation:"你确定要删除这个客户的资料吗？",
            window: {
                title: ""
            }
        },
        filterable: {
            extra: false,
            operators: {
                number: {
                    eq: "等于",
                    gte: "大于等于",
                    lte: "小于等于",
                    gt: "大于",
                    lt: "小于"
                }
            }
        }
    });
});

function productDropDownEditor(container, options) {
    $('<input required data-text-field="ProductName" data-value-field="ProductId" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataSource: [
                { ProductId: 1, ProductName: "按揭贷款" },
			    { ProductId: 2, ProductName: "个人贷款" },
                { ProductId: 3, ProductName: "信用卡" },
                { ProductId: 4, ProductName: "理财产品" },
                { ProductId: 5, ProductName: "保险" },
                { ProductId: 6, ProductName: "其他" }
            ]
        });
}

function statusDropDownEditor(container, options) {
    $('<input required data-text-field="StatusName" data-value-field="StatusId" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataSource: [
                { StatusId: 1, StatusName: "评估客户" },
                { StatusId: 2, StatusName: "客户有效" },
                { StatusId: 3, StatusName: "跟进中" },
                { StatusId: 4, StatusName: "开户" },
                { StatusId: 5, StatusName: "进账" },
                { StatusId: 6, StatusName: "账户合格" },
                { StatusId: 7, StatusName: "客户无效" },
                { StatusId: 8, StatusName: "客户流失" }
            ]
        });
}

function channelDropDownEditor(container, options) {
    $('<input required data-text-field="ChannelName" data-value-field="ChannelId" data-bind="value:' + options.field + '"/>')
        .appendTo(container)
        .kendoDropDownList({
            autoBind: false,
            dataSource: [
                { ChannelId: 1, ChannelName: "中介/业务伙伴" },
			    { ChannelId: 2, ChannelName: "公司资源" },
                { ChannelId: 3, ChannelName: "客户介绍" },
                { ChannelId: 4, ChannelName: "展会/路演" },
                { ChannelId: 5, ChannelName: "客户上门" },
                { ChannelId: 6, ChannelName: "其他" }
            ]
        });
}
