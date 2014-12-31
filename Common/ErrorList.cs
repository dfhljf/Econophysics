using System;
using System.Collections.Generic;
using System.Text;

namespace CommonType
{
    public class ErrorList
    {
        public static Exception CashOut = new Exception(ErrorMessage.CashOut);
        public static Exception InsufficientStocks = new Exception(ErrorMessage.InsufficientStocks);
        public static Exception UserRuin = new Exception(ErrorMessage.UserRuin);
        public static Exception TradeTwice = new Exception(ErrorMessage.TradeTwice);
        public static Exception UserNotExist = new Exception(ErrorMessage.UserNotExist);
    }
}
