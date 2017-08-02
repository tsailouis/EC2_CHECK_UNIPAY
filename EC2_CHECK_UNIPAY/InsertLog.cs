using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace EC2_CHECK_UNIPAY
{
    class InsertLog : UnionPaySetting
    {
        private List<CTCBAPI.Model.UnionMod> li;
        public void setList(List<CTCBAPI.Model.UnionMod> li) { this.li = li; }


        static object lockLog = new object();
        private static string strXMLDay = DateTime.Now.ToString("yyyyMMdd");
        public static void createLogFile() 
        {
         string logPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\log";

            //檢查此路徑有無資料夾
            if (!Directory.Exists(logPath))
            {
                //新增資料夾
                Directory.CreateDirectory(logPath);
            }
        }

        //Write Log 
         public static void insertLog(string message)
        {
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\log";
            lock (lockLog)
            {
                //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                File.AppendAllText(logPath + @"\" + strXMLDay + ".txt", "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + message);
            }
        }


         public void O_Orders_InsertUnionCardQryLog(string rtnCode)
         {

             ConnDB();
             try
             {

                 SqlCommand sda;
                 string sql = "exec Web_O_Orders_InsertUnionCardQryLog @OrderId ,@TXType,@ReturnCode ,@ResponseCode ,@ResponseMessage,@Lidm ,@Xid ,@ResponseTime ,@OrderStatus,@TraceNumber ,@TraceTime,@Qid ,@SettleAmount ,@SettleCurrency ,@SettleDate ,@exchangeRate ,@exchangeDate,@VarOutput ";
                 sda = new SqlCommand(sql, conn);
                 sda.Parameters.AddWithValue("@OrderId", String.IsNullOrEmpty(li[0].lidm) ? "" : li[0].lidm);
                 sda.Parameters.AddWithValue("@TXType", "");
                 sda.Parameters.AddWithValue("@ReturnCode", rtnCode);
                 sda.Parameters.AddWithValue("@ResponseCode", String.IsNullOrEmpty(li[0].respCode) ? "" : li[0].respCode);
                 sda.Parameters.AddWithValue("@ResponseMessage", String.IsNullOrEmpty(li[0].respMsg) ? "" : li[0].respMsg);
                 sda.Parameters.AddWithValue("@Lidm", String.IsNullOrEmpty(li[0].lidm) ? "" : li[0].lidm);
                 sda.Parameters.AddWithValue("@Xid", String.IsNullOrEmpty(li[0].xid) ? "" : li[0].xid);
                 sda.Parameters.AddWithValue("@ResponseTime", String.IsNullOrEmpty(li[0].respTime) ? "" : li[0].respTime);
                 sda.Parameters.AddWithValue("@OrderStatus", String.IsNullOrEmpty(li[0].orderStatus) ? "" : li[0].orderStatus);
                 sda.Parameters.AddWithValue("@TraceNumber", String.IsNullOrEmpty(li[0].traceNumber) ? "" : li[0].traceNumber);
                 sda.Parameters.AddWithValue("@TraceTime", String.IsNullOrEmpty(li[0].traceTime) ? "" : li[0].traceTime);
                 sda.Parameters.AddWithValue("@Qid", String.IsNullOrEmpty(li[0].qid) ? "" : li[0].qid);
                 sda.Parameters.AddWithValue("@SettleAmount", String.IsNullOrEmpty(li[0].settleAmount) ? "" : li[0].settleAmount);
                 sda.Parameters.AddWithValue("@SettleCurrency", String.IsNullOrEmpty(li[0].settleCurrency) ? "" : li[0].settleCurrency);
                 sda.Parameters.AddWithValue("@SettleDate", String.IsNullOrEmpty(li[0].settleDate) ? "" : li[0].settleDate);
                 sda.Parameters.AddWithValue("@exchangeRate", String.IsNullOrEmpty(li[0].exchangeRate) ? "" : li[0].exchangeRate);
                 sda.Parameters.AddWithValue("@exchangeDate", String.IsNullOrEmpty(li[0].exchangeDate) ? "" : li[0].exchangeDate);
                 sda.Parameters.AddWithValue("@VarOutput", "" );
                 sda.ExecuteReader();

             }
             catch (Exception ex)
             {
                 
                 InsertLog.insertLog(ex.ToString());

             }
             finally { CloseDB(); }

         }
    }
}
