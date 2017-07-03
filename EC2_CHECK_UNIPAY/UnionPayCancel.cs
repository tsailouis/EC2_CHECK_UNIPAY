using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTCBAPI.Common;
using System.Configuration;
namespace EC2_CHECK_UNIPAY
{
    class UnionPayCancel : BathSetting
    {

        public UnionPayCancel(){}
        public UnionPayCancel(List<CTCBAPI.Model.UnionMod>  li)
        {
            updateUnionPayCancel( li);
        }
        
   

        private void updateUnionPayCancel(List<CTCBAPI.Model.UnionMod> li)
        {
            try{
                CTCBAPI.UnionPayCancel uc = new CTCBAPI.UnionPayCancel();
                uc.server = BathSetting.strChinaTrustServer; //設置伺服器網址
                uc.merId = BathSetting.strMerId; // 設置特店編號
                uc.macKey = BathSetting.strMacKey; // 設置壓碼鍵值
                uc.xid = li[0].xid; // 設定交易識別碼
                uc.lidm = li[0].lidm; // 設置訂單編號
                string rtnCode = uc.doAction(); // 執行交易 (取消交易)
                
            } catch (Exception ex){
                InsertLog.insertLog(ex.ToString());
            }
        }
       
            
    }
}
