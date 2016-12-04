using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using Dapper;
using System.IO;
namespace ReadPic
{
    public class GotoSqlite
    {

        public static void insertToDb(List<UserModel> users)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = Path.Combine(System.Windows.Forms.Application.StartupPath, "MyDB.db");


            using (SQLiteConnection con = new SQLiteConnection(sb.ToString()))
            {
                con.Open();
                con.ExecuteAsync(@"insert into user_list(nickname,telephone,fileTime,filePath )VALUES(@nickname,@telephone,@fileTime,@filePath )", users);
            };
        }


        public static List<UserModel> GetAllUsers()
        {

            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = Path.Combine(System.Windows.Forms.Application.StartupPath, "MyDB.db");

            List<UserModel> list = new List<UserModel>();
            using (SQLiteConnection con = new SQLiteConnection(sb.ToString()))
            {
                con.Open();
                string sql = "select  nickname,telephone from user_list";
                list = con.Query<UserModel>(sql).ToList();
            };
            return list;
        }
    }
}
