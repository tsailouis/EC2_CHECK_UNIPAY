using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using CTCBAPI.Common;
using System.Data.SqlClient;
namespace EC2_CHECK_UNIPAY
{
    class UpdateOrderStatus : BathSetting
    {

       
        private List<CTCBAPI.Model.UnionMod> li;
        private string bathType;
        private DataRow dr;

        public void setList(List<CTCBAPI.Model.UnionMod> li) { this.li = li; }
        public void setBathType(string bathType) { this.bathType = bathType; }
        public void setDr(DataRow dr) { this.dr = dr; }

        
        public void orderStatusToDB() {
            ConnDB();
            DataTable dt = new DataTable();

            try
            {
                string sql = "";
                SqlCommand sda;
                if (bathType.Equals("1"))
                {
                 

                    sql = "EXEC EC2_UPDATE_UNIPAY_AUTH_AND_UNAUTH @IsAuth,@OrderID,@Xid,@RequestTime ,@RespTime,@OrderStatus,@TraceNumber,@TraceTime,@Qid,@AuthAmount";
                   
                }
                else if (bathType.Equals("2"))
                {
                    sql = "EXEC EC2_CHECK_UNIPAY_REFUND_AND_CANCLE @IsAuth,@OrderID,@Xid,@RequestTime ,@RespTime,@OrderStatus,@TraceNumber,@TraceTime,@Qid,@AuthAmount";
                }

                sda = new SqlCommand(sql, conn);
                sda.Parameters.AddWithValue("@IsAuth", dr["IsAuth"].ToString());
                sda.Parameters.AddWithValue("@OrderID", li[0].lidm);
                sda.Parameters.AddWithValue("@Xid", li[0].xid);
                sda.Parameters.AddWithValue("@RequestTime", li[0].requestTime);
                sda.Parameters.AddWithValue("@RespTime", li[0].respTime);
                sda.Parameters.AddWithValue("@OrderStatus", li[0].orderStatus);
                sda.Parameters.AddWithValue("@TraceNumber", li[0].traceNumber);
                sda.Parameters.AddWithValue("@TraceTime", li[0].traceTime);
                sda.Parameters.AddWithValue("@Qid", li[0].qid);
                sda.Parameters.AddWithValue("@AuthAmount", li[0].settleAmount);

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
