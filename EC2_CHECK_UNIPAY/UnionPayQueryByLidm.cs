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
        private DataTable dt;
        private string bathType;

        public void setDt(DataTable dt) { this.dt = dt; }
        public void setBathType(string bathType) { this.bathType = bathType; }
    
       

         public void getOrderStatus() 
         {
          
             foreach (DataRow dr in dt.Rows) 
             { 
                 CTCBAPI.UnionPayQueryByLidm uc = new CTCBAPI.UnionPayQueryByLidm();
                 uc.server = strChinaTrustServer; //設置伺服器網址
                 uc.merId = strMerId; // 設置特店編號
                 uc.macKey = strMacKey; // 設置壓碼鍵值
                 uc.lidm = dr["OrderId"].ToString(); //訂單編號
                 string rtnCode = uc.doAction(); // 執行交易 (查詢交易)
                 if (rtnCode.Equals("000"))
                 {
                     List<CTCBAPI.Model.UnionMod> li = uc.outPara;
                        
                     //查詢成功
                     if (bathType.Equals("1"))
                     {
                         //未授權
                         UpdateOrderStatus upOrder = new UpdateOrderStatus();
                         upOrder.setList(uc.outPara);
                         upOrder.setDr(dr);
                         upOrder.setBathType(bathType);
                         upOrder.orderStatusToDB();
                     
                     }
                     else if (bathType.Equals("2"))
                     {
                         //11,12,22,41,42,33,43,44
                         if (li[0].orderStatus.Equals("13")) 
                         { //發動取消授權
                             UnionPayCancel unCancel=new UnionPayCancel();
                             unCancel.setList(li);
                             unCancel.updateUnionPayCancel(); 
                         } else if (li[0].orderStatus.Equals("23")) 
                         {//23發動退貨
                             UnionPayRefund unRefund = new UnionPayRefund();
                             unRefund.setList(li);
                             unRefund.updateUnionPayRefund(); 
                         }
                         
                       
                        
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
