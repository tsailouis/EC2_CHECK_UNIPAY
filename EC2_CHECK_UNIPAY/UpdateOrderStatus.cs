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
    class UpdateOrderStatus : UnionPaySetting
    {

       
        private List<CTCBAPI.Model.UnionMod> li;
        private string bathType;
        private DataRow dr;

        public void setList(List<CTCBAPI.Model.UnionMod> li) { this.li = li; }
        public void setBathType(string bathType) { this.bathType = bathType; }
        public void setDr(DataRow dr) { this.dr = dr; }

        
        public void UpdateOrderStatusToDB() {
             ConnDB();
            SqlTransaction tran= conn.BeginTransaction();
            try
            {
               
               
                DataTable dt = new DataTable();
                string sql = "";
                SqlCommand sda;
                if (bathType.Equals("1"))
                {
                 

                    sql = "EXEC EC2_UPDATE_UNIPAY_AUTH_AND_UNAUTH @IsAuth,@OrderID,@Xid,@RequestTime ,@RespTime,@OrderStatus,@TraceNumber,@TraceTime,@Qid,@AuthAmount";
                   
                }
                else if (bathType.Equals("2"))
                {
                    sql = "EXEC EC2_UPDATE_UNIPAY_REFUND_AND_CANCEL @OrderID,@Xid,@RequestTime ,@RespTime,@OrderStatus,@TraceNumber,@TraceTime,@Qid,@AuthAmount";
                }
               
               
                sda = new SqlCommand(sql, conn);
                sda.Transaction = tran;
                if (bathType.Equals("1")) { sda.Parameters.AddWithValue("@IsAuth", dr["IsAuth"].ToString()); }
                sda.Parameters.AddWithValue("@OrderID", String.IsNullOrEmpty(li[0].lidm) ? "" : li[0].lidm);
                sda.Parameters.AddWithValue("@Xid", String.IsNullOrEmpty(li[0].xid) ? "" :li[0].xid);
                sda.Parameters.AddWithValue("@RequestTime", String.IsNullOrEmpty(li[0].requestTime) ? "" :li[0].requestTime);
                sda.Parameters.AddWithValue("@RespTime", String.IsNullOrEmpty(li[0].respTime) ? "" :li[0].respTime);
                sda.Parameters.AddWithValue("@OrderStatus", String.IsNullOrEmpty(li[0].orderStatus) ? "" :li[0].orderStatus);
                sda.Parameters.AddWithValue("@TraceNumber",String.IsNullOrEmpty( li[0].traceNumber) ? "" :li[0].traceNumber);
                sda.Parameters.AddWithValue("@TraceTime", String.IsNullOrEmpty(li[0].traceTime) ? "" :li[0].traceTime);
                sda.Parameters.AddWithValue("@Qid", String.IsNullOrEmpty(li[0].qid) ? "" :li[0].qid);
                sda.Parameters.AddWithValue("@AuthAmount", String.IsNullOrEmpty(li[0].settleAmount) ? 0 : Int32.Parse(li[0].settleAmount));

                sda.ExecuteReader();

                tran.Commit();    

               
            }
            catch (Exception ex)
            {
                tran.Rollback(); 
                InsertLog.insertLog(ex.ToString());

            }
            finally { CloseDB(); }
            

        }

      
    }
}
