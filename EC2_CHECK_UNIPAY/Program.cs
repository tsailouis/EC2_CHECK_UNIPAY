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
                      gtest.setBathType("1");
                      gtest.CallGetOrderListSP();
            #endif
                      
            
            //1=授權/付款狀態 2=授權取消/退款狀態
            if (args.Length!=0)
            {
               
                #region LOG檔記錄開始
                #endregion
                GetOrderList gt = new GetOrderList();
                gt.setBathType(args[0].ToString());
                gt.CallGetOrderListSP();
            }
           
        }

        #region 新增資料夾

        /// <summary>
        /// 檢查資料夾是否存在，不存在則新增資料夾
        /// </summary>
        /// <param name="StringFilePath">欲新增資料夾路徑</param>
        private static void fnCreateDirectoryInfo(string StringFilePath)
        {
            //檢查資料夾是否存在，不存在則新增資料夾
            DirectoryInfo diDir = new DirectoryInfo(StringFilePath);
            if (!diDir.Exists)
            {
                diDir.Create();
            }
        }

        #endregion 
    }
}
