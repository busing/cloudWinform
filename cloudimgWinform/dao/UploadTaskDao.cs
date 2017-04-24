﻿using cloudimgWinform.bean;
using cloudimgWinform.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cloudimgWinform.dao
{
    class UploadTaskDao
    {
        static String SqliteFileName = "cloudimg.lite";
        public static SQLiteConnection connection;
        public static void initDataBase()
        {
            try
            {
                String dbFilePath = Dictionary.UserHome + Dictionary.AppHome + SqliteFileName;
                bool first;
                if (File.Exists(dbFilePath) == false)//如果不存在就创建file文件夹
                {
                    first = true;
                    Directory.CreateDirectory(Dictionary.UserHome + Dictionary.AppHome);
                    SQLiteConnection.CreateFile(Dictionary.UserHome + Dictionary.AppHome + SqliteFileName);
                }
                else
                {
                    first = false;
                }
                connection = new SQLiteConnection("Data Source=" + dbFilePath + ";Version=3;");
                connection.Open();
                Console.WriteLine("connect success");
                //初始化数据表和库
                if (first)
                {
                    string sql = "create table t_uploadtask (id integer primary key autoincrement,name varchar(100), path varchar(256),upload_path varchar(256),convert_path varchar(256), size int,scan_rate int,width int,height int,resolution float,status int)";
                    SQLiteCommand command=SQLiteHelper.CreateCommand(connection, sql, null);
                    command.ExecuteNonQuery();
                }
                returnStatus();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
                Console.WriteLine(e.Message);
            }
        }

        //初始化状态 转化中--》待处理，上传中--》待上传
        public static void returnStatus()
        {
            String sql = "update t_uploadtask set status=0 where status=1";
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, null);
            command.ExecuteNonQuery();

            sql = "update t_uploadtask set status=2 where status=4";
            command = SQLiteHelper.CreateCommand(connection, sql, null);
            command.ExecuteNonQuery();
        }

        public static bool addTask(UploadTask ut)
        {
            String sql = "insert into t_uploadtask(name,path,size,status) values(@name,@path,@size,@status)";
            SQLiteParameter p1= SQLiteHelper.CreateParameter("name", DbType.String, ut.name);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("path", DbType.String, ut.path);
            SQLiteParameter p3 = SQLiteHelper.CreateParameter("size", DbType.Int64, ut.size);
            SQLiteParameter p4= SQLiteHelper.CreateParameter("status", DbType.Int16, ut.status);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1, p2, p3, p4 });
            int result=command.ExecuteNonQuery();
            Console.WriteLine("保存数据" + result + "条");
            return result>0?true:false;
        }

        public static bool delTask(int id)
        {
            String sql = "delete from t_uploadtask where id=@id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("id", DbType.Int32, id);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1});
            int result = command.ExecuteNonQuery();
            Console.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool delTaskByStatus(int status)
        {
            String sql = "delete from t_uploadtask where status=@status";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("status", DbType.Int32, status);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1 });
            int result = command.ExecuteNonQuery();
            Console.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool delAllTask()
        {
            String sql = "delete from t_uploadtask";
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql,null);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static IList<UploadTask> query()
        {
            String sql = "select * from t_uploadtask";
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql,null);
            SQLiteDataReader reader = command.ExecuteReader();
            UploadTask task=null;
            IList<UploadTask> tasks = new BindingList<UploadTask>();
            UploadTask.tasks.Clear();
            while (reader.Read())
            {
                task = readUploadTask(reader);
                tasks.Add(task);
            }
            Console.WriteLine("查询数据" + tasks.Count + "条");
            return tasks;
        }

        public static UploadTask getByStatus(int status)
        {
            String sql = "select * from t_uploadtask where status=@status order by id limit 0,1";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("status", DbType.Int32, status);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1 });
            SQLiteDataReader reader = command.ExecuteReader();
            UploadTask task = null;
            while (reader.Read())
            {
                task = readUploadTask(reader);
            }
            return task;
        }

        private static UploadTask readUploadTask(SQLiteDataReader reader)
        {
            UploadTask task = new UploadTask();
            task.id = int.Parse(reader["id"].ToString());
            task.name = reader["name"].ToString();
            task.path = reader["path"].ToString();
            task.uploadPath = reader["upload_path"].ToString();
            task.convertPath = reader["convert_path"].ToString();
            task.size = int.Parse(reader["size"].ToString());
            task.resolution = Utils.isNotEmpty(reader["resolution"].ToString()) ? float.Parse(reader["resolution"].ToString()) : 0;
            task.scanRate = Utils.isNotEmpty(reader["scan_rate"].ToString()) ? int.Parse(reader["scan_rate"].ToString()) : 0;
            task.width = Utils.isNotEmpty(reader["width"].ToString()) ? int.Parse(reader["width"].ToString()) : 0;
            task.height = Utils.isNotEmpty(reader["height"].ToString()) ? int.Parse(reader["height"].ToString()) : 0;
            task.status = int.Parse(reader["status"].ToString());
            return task;
        }

        public static bool updateStatus(int status,int id)
        {
            String sql = "update t_uploadtask set status=@status where id=@id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("status", DbType.Int32, status);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("id", DbType.Int32, id);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2 });
            int result = command.ExecuteNonQuery();
            Console.WriteLine("修改数据状态" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool updateInfo(UploadTask ut)
        {
            String sql = "update t_uploadtask set upload_path=@upload_path,convert_path=@convert_path,scan_rate=@scan_rate,width=@width,height=@height,resolution=@resolution,status=@status where id=@id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("upload_path", DbType.String, ut.uploadPath);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("convert_path", DbType.String, ut.convertPath);
            SQLiteParameter p3 = SQLiteHelper.CreateParameter("scan_rate", DbType.Int16, ut.scanRate);
            SQLiteParameter p4 = SQLiteHelper.CreateParameter("width", DbType.Int16, ut.width);
            SQLiteParameter p5 = SQLiteHelper.CreateParameter("height", DbType.Int16, ut.height);
            SQLiteParameter p6 = SQLiteHelper.CreateParameter("resolution", DbType.Double, ut.resolution);
            SQLiteParameter p7 = SQLiteHelper.CreateParameter("status", DbType.Int16, ut.status);
            SQLiteParameter p8 = SQLiteHelper.CreateParameter("id", DbType.Int32, ut.id);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2, p3, p4, p5, p6, p7,p8 });
            int result = command.ExecuteNonQuery();
            Console.WriteLine("修改数据状态" + result + "条");
            return result > 0 ? true : false;
        }
    }
}
