using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KoollaExample.Models
{
    [DataContractAttribute]
    public class MyCustomer
    {
        [DataMemberAttribute]
        public int cid { get; set; }
        [DataMemberAttribute]
        public string name { get; set; }
        [DataMemberAttribute]
        public string company { get; set; }
        [DataMemberAttribute]
        public string phone { get; set; }
        [DataMemberAttribute]
        public string email { get; set; }
        [DataMemberAttribute]
        public MyChannel channel { get; set; }
        [DataMemberAttribute]
        public MyProduct product { get; set; }
        [DataMemberAttribute]
        public int money { get; set; }
        [DataMemberAttribute]
        public MyStatus status { get; set; }

        public MyCustomer(customer c, deal d)
        {
            cid = c.cid;
            name = c.name;
            company = c.company;
            phone = c.phone;
            email = c.email;
            channel = new MyChannel(c.channel);
            product = new MyProduct(c.product);
            money = c.money;
            status = new MyStatus(d.status);
        }

        public customer BuildDBCustomer(int Iuid)
        {
            customer c = new customer()
            {
                uid=Iuid,
                name=this.name,
                company=this.company,
                phone=this.phone,
                email=this.email,
                channel=this.channel.ChannelId,
                product=this.product.ProductId,
                money=this.money
            };
            return c;
        }

        public deal BuildDBDeal()
        {
            deal d = new deal()
            {
                date = DateTime.Now,
                status = this.status.StatusId,
            };
            return d;
        }
    }

    [DataContractAttribute]
    public class MyProduct
    {
        [DataMemberAttribute]
        public int ProductId { get; set; }
        [DataMemberAttribute]
        public string ProductName { get; set; }

        public MyProduct(int id)
        {
            ProductId = id;
            switch (id)
            {
                case 1:
                    ProductName = "按揭贷款";
                    break;
                case 2:
                    ProductName = "个人贷款";
                    break;
                case 3:
                    ProductName = "信用卡";
                    break;
                case 4:
                    ProductName = "理财产品";
                    break;
                case 5:
                    ProductName = "保险";
                    break;
                case 6:
                    ProductName = "其他";
                    break;
            } 
        }
    }

    [DataContractAttribute]
    public class MyStatus
    {
        [DataMemberAttribute]
        public int StatusId { get; set; }
        [DataMemberAttribute]
        public string StatusName { get; set; }

        public MyStatus(int id)
        {
            StatusId = id;
            switch (id)
            {
                case 1:
                    StatusName = "评估客户";
                    break;
                case 2:
                    StatusName = "客户有效";
                    break;
                case 3:
                    StatusName = "跟进中";
                    break;
                case 4:
                    StatusName = "开户";
                    break;
                case 5:
                    StatusName = "进账";
                    break;
                case 6:
                    StatusName = "账户合格";
                    break;
                case 7:
                    StatusName = "客户无效";
                    break;
                case 8:
                    StatusName = "客户流失";
                    break;
            }
        }
    }

    [DataContractAttribute]
    public class MyChannel
    {
        [DataMemberAttribute]
        public int ChannelId { get; set; }
        [DataMemberAttribute]
        public string ChannelName { get; set; }

        public MyChannel(int id)
        {
            ChannelId = id;
            switch (id)
            {
                case 1:
                    ChannelName = "中介/业务伙伴";
                    break;
                case 2:
                    ChannelName = "公司资源";
                    break;
                case 3:
                    ChannelName = "客户介绍";
                    break;
                case 4:
                    ChannelName = "展会/路演";
                    break;
                case 5:
                    ChannelName = "客户上门";
                    break;
                case 6:
                    ChannelName = "其他";
                    break;
            }
        }
    }
}