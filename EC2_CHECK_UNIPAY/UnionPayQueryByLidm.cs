using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using CTCBAPI.Common;
namespace EC2_CHECK_UNIPAY
{
    class UnionPayQueryByLidm : BathSetting
    {
         public UnionPayQueryByLidm() { }
         public UnionPayQueryByLidm(DataTable dt,string bathType) 
        {
            getOrderStatus(dt, bathType);
        }

       

         private void getOrderStatus(DataTable dt,string bathType) 
         {
          
             foreach (DataRow dr in dt.Rows) 
             { 
                 CTCBAPI.UnionPayQueryByLidm uc = new CTCBAPI.UnionPayQueryByLidm();
                 uc.server = BathSetting.strChinaTrustServer; //設置伺服器網址
                 uc.merId = BathSetting.strMerId; // 設置特店編號
                 uc.macKey = BathSetting.strMacKey; // 設置壓碼鍵值
                 uc.lidm = dr["OrderId"].ToString(); //訂單編號
                 string rtnCode = uc.doAction(); // 執行交易 (查詢交易)
                 if (rtnCode.Equals("000"))
                 {
                     List<CTCBAPI.Model.UnionMod> li = uc.outPara;
                        
                     //查詢成功
                     if (bathType.Equals("1"))
                     {
                         //已授權=>SP
                         //未授權=>SP
                     
                     }
                     else if (bathType.Equals("2"))
                     {
                         //11,12,22,41,42,33,43,44
                         //23發動退貨
                         UnionPayCancel upc=new UnionPayCancel(li);
                         //13授權成功
                     }

                 }
                 else 
                 { 
                     //失敗記LOG
                 }
             }
             
             
          
            
         }
    }
}
