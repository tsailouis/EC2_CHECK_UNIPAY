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
                     string rtnCode = uc.doAction(); // 執行交易 (查詢交易)
                     List<CTCBAPI.Model.UnionMod> li = uc.outPara;
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
                                 if (li[0].orderStatus.Equals("43") && dr["RTN_SUCCESS"].ToString().Equals("1") && dr["RTN_MAIL_SEND"].ToString().Equals("0") && (dr["OrderId"].ToString().Equals("91") || dr["OrderId"].ToString().Equals("92")))
                                 {
                                 
                                     //43:退貨成功
                                 
                                    refundSuccessli.Add(li[0].lidm.ToString());
                                 }
                                 
                         }                           
                          
                         if (li[0].orderStatus.Equals("44") || li[0].orderStatus.Equals("24"))
                         {       
                                //44:退貨失敗 24:請款失敗 把失敗訊息存到MAP
                                 failResult.Add(li[0].lidm.ToString(), li);

                         }
                      
                     }
                     else
                     {
                         //查詢失敗
                         InsertLog.insertLog("訂單編號:" + dr["OrderId"].ToString() + " 回應値:" + rtnCode);
                     }
                 }
             }
             catch (Exception ex)
             {

                 InsertLog.insertLog(ex.ToString());
                
             }
             
             
          
            
         }
    }
}
