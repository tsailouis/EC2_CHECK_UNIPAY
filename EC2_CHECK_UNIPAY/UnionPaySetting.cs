using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
namespace EC2_CHECK_UNIPAY
{
    class UnionPaySetting
    {
        internal static readonly string strChinaTrustServer = ConfigurationManager.AppSettings["ChinaTrustServer"].ToString().Trim();

        internal static readonly string strReceiver = ConfigurationManager.AppSettings["receiver"].ToString().Trim();
        internal static readonly string strSender = ConfigurationManager.AppSettings["sender"].ToString().Trim();
        internal static readonly string strRefundTitle = ConfigurationManager.AppSettings["refundTitle"].ToString().Trim();
        internal static readonly string strEn1 = ConfigurationManager.AppSettings["en1"].ToString().Trim();
        internal static readonly string strEn2 = ConfigurationManager.AppSettings["en2"].ToString().Trim();
        internal static readonly string strMacKey = CryptHelper.CryptHelper.DesDecrypt( ConfigurationManager.AppSettings["macKey"].ToString().Trim(), strEn1, strEn2);
        internal static readonly string strMerId = CryptHelper.CryptHelper.DesDecrypt(ConfigurationManager.AppSettings["merId"].ToString().Trim(), strEn1, strEn2);
        internal SqlConnection conn;
        //internal SqlCommand sqlCmd = new SqlCommand();

         
        #region 取得連線
        /// <summary>
        /// 連結DB...路徑由app.config設定
        /// </summary>
        internal void ConnDB()
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

        internal void CloseDB()
        {
            conn.Close();
        }
        #endregion
    }
}
