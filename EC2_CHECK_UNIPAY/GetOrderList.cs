using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EC2_CHECK_UNIPAY
{
    class GetOrderList : UnionPaySetting
    {
     
        private string bathType;
        public void setBathType(string bathType) { this.bathType = bathType; }
        private List<string> refundSuccessli = new List<string>();
        public void CallGetOrderListSP() 
        {
            ConnDB();
            DataTable dt = new DataTable();
            Dictionary<string, List<CTCBAPI.Model.UnionMod>> failResult = new Dictionary<string, List<CTCBAPI.Model.UnionMod>>();
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
                    un.setDictionary(failResult);
                    un.setList(refundSuccessli);
                    un.unionPayQueryByLidm();
                    
                
                    SendMail se = new SendMail();
                    se.setBathType(bathType);
                    se.setDictionary(failResult);
                    se.setDt(dt);
                    se.setList(refundSuccessli);
                    if (failResult.Count>0)
                        se.sendFailMail();
                    if (refundSuccessli.Count > 0)
                        se.sendRefundMail();
                   
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
