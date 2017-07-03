using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace EC2_CHECK_UNIPAY
{
    class InsertLog
    {

        static object lockLog = new object();
        private static string strXMLDay = DateTime.Now.ToString("yyyyMMdd");
        public static void createLogFile() 
        {
         string logPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\log";

            //檢查此路徑有無資料夾
            if (!Directory.Exists(logPath))
            {
                //新增資料夾
                Directory.CreateDirectory(logPath);
            }
        }

        //Write Log 
         public static void insertLog(string message)
        {
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\log";
            lock (lockLog)
            {
                //把內容寫到目的檔案，若檔案存在則附加在原本內容之後(換行)
                File.AppendAllText(logPath + @"\" + strXMLDay + ".txt", "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + message);
            }
        }
    }
}
