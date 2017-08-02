using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using CTCBAPI.Common;
namespace EC2_CHECK_UNIPAY
{
    class UnionPayQueryByLidm : UnionPaySetting
    {
        private DataTable dt;
        private string bathType;
        private Dictionary<string, List<CTCBAPI.Model.UnionMod>> failResult;
        private List<string> refundSuccessli;
        private List<CTCBAPI.Model.UnionMod> li;
        private string rtnCode ;
        public void setDt(DataTable dt) { this.dt = dt; }
        public void setBathType(string bathType) { this.bathType = bathType; }
        public void setDictionary(Dictionary<string, List<CTCBAPI.Model.UnionMod>> failResult) { this.failResult = failResult; }
        public void setList(List<string> refundSuccessli) { this.refundSuccessli = refundSuccessli; }


        public void unionPayQueryByLidm() 
         {
             try
             {
                 foreach (DataRow dr in dt.Rows)
                 {


                     CTCBAPI.UnionPayQueryByLidm uc = new CTCBAPI.UnionPayQueryByLidm();

                     uc.server = strChinaTrustServer; //設置伺服器網址
                     uc.merId = strMerId; // 設置特店編號
                     uc.macKey = strMacKey; // 設置壓碼鍵值
                     uc.lidm = dr["OrderId"].ToString(); //訂單編號
                     rtnCode = uc.doAction(); // 執行交易 (查詢交易)
                     li = uc.outPara;

                     if (rtnCode.Equals("Success"))
                     {



                         if (bathType.Equals("2") && li[0].orderStatus.Equals("13"))
                         {
                             //13發動取消授權
                             UnionPayCancel unCancel = new UnionPayCancel();
                             unCancel.setList(li);
                             unCancel.setDr(dr);
                             unCancel.setBathType(bathType);
                             unCancel.updateUnionPayCancel();
                         }
                         else if (bathType.Equals("2") && li[0].orderStatus.Equals("23"))
                         {
                             //先判斷是否有請款資料[O_UnionPay_Rec],如果沒有則需先新增
                             if (dr["IsAuth"].ToString().Equals("Y"))
                             {
                                 
                                 UpdateOrderStatus upOrder = new UpdateOrderStatus();
                                 upOrder.setList(li);
                                 upOrder.setDr(dr);
                                 upOrder.setBathType("1");
                                 upOrder.UpdateOrderStatusToDB();
                             }
                             //23發動退貨
                             UnionPayRefund unRefund = new UnionPayRefund();
                             unRefund.setList(li);
                             unRefund.setDr(dr);
                             unRefund.setBathType(bathType);
                             unRefund.updateUnionPayRefund();
                         }
                         else
                         {
                             UpdateOrderStatus upOrder = new UpdateOrderStatus();
                             upOrder.setList(li);
                             upOrder.setDr(dr);
                             upOrder.setBathType(bathType);
                             upOrder.UpdateOrderStatusToDB();

                             if (bathType.Equals("2") && li.Count > 0)
                             {
                                 if (li[0].orderStatus.Equals("43") && dr["RTN_MAIL_SEND"].ToString().Equals("0") && (dr["OrderStatus"].ToString().Equals("91") || dr["OrderStatus"].ToString().Equals("92")))
                                 {

                                     //43:退貨成功

                                     refundSuccessli.Add(li[0].lidm.ToString());
                                 }
                             }

                         }

                         if (li.Count > 0)
                         {
                             if (li[0].orderStatus.Equals("44") || li[0].orderStatus.Equals("24"))
                             {
                                 //44:退貨失敗 24:請款失敗 把失敗訊息存到MAP
                                 failResult.Add(li[0].lidm.ToString(), li);

                             }
                         }


                     }
                     else
                     {
                         //查詢失敗
                         InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + " 回應値:" + rtnCode);
                         if (li != null && li.Count > 0)
                         {


                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "回應碼 = " + li[0].respCode);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "回應訊息 = " + li[0].respMsg);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "壓碼值 = " + li[0].inMac);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "訂單編號 = " + li[0].lidm);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "交易識別碼 = " + li[0].xid);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "交易時間 = " + li[0].respTime);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "交易請求時間 = " + li[0].requestTime);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "訂單狀態代碼 = " + li[0].orderStatus);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "系統追蹤號 = " + li[0].traceNumber);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "系統追蹤時間 = " + li[0].traceTime);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "交易流水號 = " + li[0].qid);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "清算金額 = " + li[0].settleAmount);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "清算幣別 = " + li[0].settleCurrency);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "清算日期 = " + li[0].settleDate);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "清算匯率 = " + li[0].exchangeRate);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "清算匯率換算日期 = " + li[0].exchangeDate);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "回應碼 = " + li[0].respCode);
                             InsertLog.insertLog("查詢失敗訂單編號:" + dr["OrderId"].ToString() + "回應訊息 = " + li[0].respMsg);
                         }
                         else
                         {
                             InsertLog.insertLog("查詢失敗 回傳List沒有値");
                         }
                     }
                 }
             }
             catch (Exception ex)
             {

                 InsertLog.insertLog(ex.ToString());

             }
             finally 
             { 
               InsertLog addlog =new InsertLog();
               addlog.setList(li);
               addlog.O_Orders_InsertUnionCardQryLog(rtnCode);
             }
             
         }


       
    }
}
