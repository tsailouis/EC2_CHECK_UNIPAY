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
    class UpdateOrderStatus
    {

        private SqlConnection conn;
        private SqlCommand sqlCmd = new SqlCommand();
        internal UpdateOrderStatus() { }
        internal UpdateOrderStatus( List<CTCBAPI.Model.UnionMod> li,string bathType,DataRow dr) {
            orderStatusToDB(li,bathType, dr);
        }
        private void orderStatusToDB(List<CTCBAPI.Model.UnionMod> li, string bathType, DataRow dr) {

            string sql = "";
            if (bathType.Equals("1")) 
            {
                //未授權
                if (dr["isAuth"].ToString().Equals("N"))
                { 
                    
                } 
                else if (dr["isAuth"].ToString().Equals("Y")) 
                { 
                }
            }
            else if (bathType.Equals("2")) 
            { 
            
            }

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
