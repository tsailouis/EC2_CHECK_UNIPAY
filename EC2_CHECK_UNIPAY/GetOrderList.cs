using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace EC2_CHECK_UNIPAY
{
    class GetOrderList
    {
        #region 建構子
        public GetOrderList() { }
        public GetOrderList(string bathType) 
        {
            CallGetOrderListSP(bathType);
        }
        #endregion
        private SqlConnection conn;
        private SqlCommand sqlCmd = new SqlCommand();

        private void CallGetOrderListSP(string bathType) 
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
                    UnionPayQueryByLidm un=new UnionPayQueryByLidm(dt, bathType);
                    //跑完迴圈 請款失敗要發信
                }

            }
            catch (Exception ex)
            {
                InsertLog.insertLog(ex.ToString());
               
            }
            finally { CloseDB(); }

           
        }

        #region 取得連線
        /// <summary>
        /// 連結DB...路徑由app.config設定
        /// </summary>
        private void ConnDB()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["commerce"].ConnectionString;
                conn = new SqlConnection(connStr);
                conn.Open();
                Console.WriteLine("DB conn OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB conn failed: {0}", ex.Message);
                //errMsg += ex.ToString() + htmlP;
                throw ex; //必須中斷
            }
        }

        private void CloseDB()
        {
            conn.Close();
        }
        #endregion
    }
}
