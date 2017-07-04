using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTCBAPI.Common;
using System.Configuration;

namespace EC2_CHECK_UNIPAY
{
    class UnionPayRefund : BathSetting
    {

        private List<CTCBAPI.Model.UnionMod> li;
        public void setList(List<CTCBAPI.Model.UnionMod> li) { this.li = li; }



        public Dictionary<string, string> updateUnionPayRefund()
        {
            try
            { 
                Dictionary<string, string> refundResult = new Dictionary<string, string>();
                CTCBAPI.UnionPayRefund uc = new CTCBAPI.UnionPayRefund();
                uc.server = strChinaTrustServer; //設置伺服器網址
                uc.merId = strMerId; // 設置特店編號
                uc.macKey = strMacKey; // 設置壓碼鍵值
                uc.xid = li[0].xid; // 設定交易識別碼
                uc.lidm = li[0].lidm; // 設置訂單編號
                string rtnCode = uc.doAction(); // 執行交易 (取消交易)
                if (rtnCode.Equals("000"))
                {
                   
                    refundResult.Add("rtnCode", rtnCode);
                    refundResult.Add("rtnCode", uc.outPara.respCode);
                    refundResult.Add("rtnCode", uc.outPara.respMsg);

                    
                }
                return refundResult;
            }
            catch (Exception ex)
            {
             
                InsertLog.insertLog(ex.ToString());
                return null;
            }
        }
    }
}
