using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
namespace EC2_CHECK_UNIPAY
{
    class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG

            GetOrderList gtest = new GetOrderList();
            gtest.setBathType("2");
            gtest.CallGetOrderListSP();
            #endif
                      
            
            //1=授權/付款狀態 2=授權取消/退款狀態
            if (args.Length!=0)
            {
               
            
                GetOrderList gt = new GetOrderList();
                gt.setBathType(args[0].ToString());
                gt.CallGetOrderListSP();
            }
           
        }

    
    }
}
