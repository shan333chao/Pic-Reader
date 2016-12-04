using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadPic
{
 public static  class ExString
    {


        public static bool IsPhoneNumberMached(this string source) {
            Regex reg = new Regex(@"^1[3,4,5,7,8]\d{9}$");

            var result = reg.IsMatch(source);

            return result;
        
        }

    }
}
