using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC2_CHECK_UNIPAY
{
    class UnipayList 
    {
       
        public string RTN_SUCCESS { get; set; }
        public string RTN_MAIL_SEND { get; set; }
        public string email { get; set; }
        public string OrderID { get; set; }
        public string TotalReceiveAmount { get; set; }
        public string OrderStatus { get; set; }
   
    }
}
