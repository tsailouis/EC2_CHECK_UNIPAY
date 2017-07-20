using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace EC2_CHECK_UNIPAY
{
    class SendMail : UnionPaySetting
    {
       
        private string bathType;
        private Dictionary<string, List<CTCBAPI.Model.UnionMod>> failResult;
        private DataTable dt;
        private List<string> refundSuccessli;

        public void setBathType(string bathType) { this.bathType = bathType; }
        public void setDictionary(Dictionary<string, List<CTCBAPI.Model.UnionMod>> failResult) { this.failResult = failResult; }
        public void setDt(DataTable dt) { this.dt = dt; }
        public void setList(List<string> refundSuccessli) { this.refundSuccessli = refundSuccessli; }

        public void sendRefundMail() 
        {
          try
            {
                  var li = dt.ToList<UnipayList>();
                  var query = from b in li
                              where refundSuccessli.Contains(b.OrderID)
                              select new
                                {
                                    b.OrderID,
                                    b.TotalReceiveAmount,
                                    b.email
                                };
                  String strTemplatePath = System.IO.Directory.GetCurrentDirectory() + "\\Template\\refundMailTemplate.txt";
                  System.IO.StreamReader file = new System.IO.StreamReader(strTemplatePath, System.Text.Encoding.Default);
                  string strMailBody = file.ReadToEnd();
                  file.Close();

                  foreach (var i in query) 
                  {
                      string strTempMailBody = strMailBody;
                      strTempMailBody = strTempMailBody.Replace("@orderID", i.OrderID.ToString()).Replace("@date", DateTime.Now.ToString("yyyy/MM/dd")).Replace("@refundDate", DateTime.Now.AddDays(1).ToString("yyyy/MM/dd")).Replace("@TotalReceiveAmount", i.TotalReceiveAmount.ToString());
                      insertS_MailQ(strRefundTitle, strTempMailBody, i.email);
                  }

            }
            catch (Exception ex) { InsertLog.insertLog(ex.ToString()); }
         
        }

        public void sendFailMail() 
        {
            try
            {
              
                String strTemplatePath = System.IO.Directory.GetCurrentDirectory() + "\\Template\\mailTemplate.txt";
                System.IO.StreamReader file = new System.IO.StreamReader(strTemplatePath,System.Text.Encoding.Default);
                string strMailBody = file.ReadToEnd();
                file.Close();

                string strContent = "";
                foreach (var oneItem in failResult)
                {
                    strContent += "<TR><TD align='center'>" + oneItem.Value[0].lidm + "</TD> <TD align='center'>" + oneItem.Value[0].settleAmount + "</TD> <TD >" + oneItem.Value[0].respMsg + "</TD></TR>";

                }

                string strMoneyDescription = (bathType.Equals("1")) ? "請款金額" : "退款金額";
                string strTitle = (bathType.Equals("1")) ? "請款失敗通知信" : "退貨失敗通知信";

                strMailBody = strMailBody.Replace("@money", strMoneyDescription).Replace("@Content", strContent);
                
                insertS_MailQ(strTitle,strMailBody,strReceiver);
               
            }
            catch (Exception ex) { InsertLog.insertLog(ex.ToString()); }
           
            
        }


        private void insertS_MailQ(string strTitle, string strMailBody, string strReceiver) 
        {
            try
            {
                

                ConnDB();


                string sql = " insert into S_MailQ (title, content, sender, receiver, createDate, status, " +
                                                   " priority, try, contentType, mailType, Project_ID) " +
                                         " values (@title, @content, @sender, @receiver, getdate(), 0, 1, 0, 2, 1, 1) ";
                SqlCommand scmd2 = new SqlCommand(sql, conn);
                scmd2.CommandText = sql;
                scmd2.Parameters.Clear();
                scmd2.Parameters.Add("@title", SqlDbType.NVarChar).Value = strTitle;
                scmd2.Parameters.Add("@content", SqlDbType.NText).Value = strMailBody;
                scmd2.Parameters.Add("@sender", SqlDbType.NVarChar).Value = strSender;
                scmd2.Parameters.Add("@receiver", SqlDbType.NVarChar).Value = strReceiver;
                scmd2.ExecuteNonQuery();

                scmd2.Dispose();
            }
            catch (Exception ex) { InsertLog.insertLog(ex.ToString()); }
            finally { CloseDB(); }
        }
    }
}
