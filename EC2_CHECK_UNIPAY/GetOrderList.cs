using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EC2_CHECK_UNIPAY
{
    class GetOrderList : BathSetting
    {
     
        private string bathType;
        public void setBathType(string bathType) { this.bathType = bathType; }

        public void CallGetOrderListSP() 
        {
            ConnDB();
            DataTable dt = new DataTable();

            try
            {

                string sql = "";
                if (bathType.Equals("1"))
                {
                    sql = "EXEC EC2_CHECK_UNIPAY_AUTH_AND_UNAUTH";
                }
                else if (bathType.Equals("2"))
                {
                    sql = "EXEC EC2_CHECK_UNIPAY_REFUND_AND_CANCEL";
                }
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                sda.Fill(dt);
               
                
                if(dt.Rows.Count>0)
                {
                    UnionPayQueryByLidm un=new UnionPayQueryByLidm();
                    un.setBathType(bathType);
                    un.setDt(dt);
                    un.getOrderStatus();
                    //跑完迴圈 請款失敗要發信
                }

            }
            catch (Exception ex)
            {
                InsertLog.insertLog(ex.ToString());
               
            }
            finally { CloseDB(); }

           
        }

    
    }
}
