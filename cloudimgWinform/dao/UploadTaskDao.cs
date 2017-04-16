using cloudimgWinform.utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudimgWinform.dao
{
    class UploadTaskDao
    {
        static String UserHome = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static String AppHome = "\\terrydr\\cloudimg\\";
        static String SqliteFileName = "cloudimg.sqlite";
        static SQLiteConnection connection;
        public static void initDataBase()
        {
            try
            {
                String dbFilePath = UserHome + AppHome + SqliteFileName;
                bool first;
                if (Directory.Exists(dbFilePath) == false)//如果不存在就创建file文件夹
                {
                    first = true;
                    Directory.CreateDirectory(UserHome + AppHome);
                    SQLiteConnection.CreateFile(UserHome + AppHome + SqliteFileName);


                }
                else
                {
                    first = false;
                }
                //string connStr = @"Data Source=" + @"D:\sqlliteDb\document.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
                connection = new SQLiteConnection("Data Source=" + dbFilePath + ";Version=3;");

//                connection = new SQLiteConnection(dbFilePath);

                connection.Open();
                //初始化数据表和库
                if (first)
                {
                    string sql = "create table t_uploadtask (name varchar(20), path varchar(256),size int,associated_img_path varchar(256),preview_path varchar(256),scan_rate int,width int,height int,resolution float,status int)";
                    SQLiteHelper.CreateCommand(connection, sql, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
