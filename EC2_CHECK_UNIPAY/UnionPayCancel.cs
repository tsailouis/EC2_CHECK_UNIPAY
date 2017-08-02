using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTCBAPI.Common;
using System.Configuration;
using System.Data;

namespace EC2_CHECK_UNIPAY
{
    class UnionPayCancel : UnionPaySetting
    {
        private List<CTCBAPI.Model.UnionMod> li;
        private string bathType;
        private DataRow dr;

        public void setList(List<CTCBAPI.Model.UnionMod> li) { this.li = li; }
        public void setBathType(string bathType) { this.bathType = bathType; }
        public void setDr(DataRow dr) { this.dr = dr; }




        public void updateUnionPayCancel()
        {
            try{
                CTCBAPI.UnionPayCancel uc = new CTCBAPI.UnionPayCancel();
              
                uc.server = strChinaTrustServer; //設置伺服器網址
                uc.merId = strMerId; // 設置特店編號
                uc.macKey = strMacKey; // 設置壓碼鍵值
                uc.xid = li[0].xid; // 設定交易識別碼
                uc.lidm = li[0].lidm; // 設置訂單編號
                string rtnCode = uc.doAction(); // 執行交易 (取消交易)
                if (rtnCode.Equals("Success") && uc.outPara.respCode.Equals("00"))
                {
                    UpdateOrderStatus upOrder = new UpdateOrderStatus();
                    upOrder.setList(li);
                    upOrder.setDr(dr);
                    upOrder.setBathType(bathType);
                    upOrder.UpdateOrderStatusToDB();
                }
                else
                {   
                    InsertLog.insertLog("發動取消交易失敗 訂單編號:" + dr["OrderId"].ToString() + " 回應値:" + rtnCode);
                    InsertLog.insertLog("發動取消交易失敗 訂單編號:" + dr["OrderId"].ToString() + " 回應訊息 = " + uc.outPara.respCode);
                    InsertLog.insertLog("發動取消交易失敗 訂單編號:" + dr["OrderId"].ToString() + " 回應訊息 = " + uc.outPara.respMsg);
                }

                  
            } catch (Exception ex){

                InsertLog.insertLog(ex.ToString());
                
            }
        }
       
            
    }
}
