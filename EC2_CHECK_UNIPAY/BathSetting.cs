using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace EC2_CHECK_UNIPAY
{
    class BathSetting
    {
        internal static readonly string strChinaTrustServer = ConfigurationManager.AppSettings["ChinaTrustServer"].ToString().Trim();
        internal static readonly string strMerId = ConfigurationManager.AppSettings["merId"].ToString().Trim();
        internal static readonly string strMacKey = ConfigurationManager.AppSettings["macKey"].ToString().Trim();

    }
}
