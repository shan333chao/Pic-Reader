using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadPic
{
   public class UserModel
    {
       public string nickname { get; set; }
       public string telephone { get; set; }

       public DateTime fileTime { get; set; }
       public string filePath { get; set; }
    }
}
