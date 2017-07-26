using cloudimgWinform.bean;
using cloudimgWinform.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
                String dbFilePath = Dictionary.USER_HOME + Dictionary.APP_HOME + SqliteFileName;
                bool first;
                if (File.Exists(dbFilePath) == false)//如果不存在就创建file文件夹
                {
                    first = true;
                    Directory.CreateDirectory(Dictionary.USER_HOME + Dictionary.APP_HOME);
                    SQLiteConnection.CreateFile(Dictionary.USER_HOME + Dictionary.APP_HOME + SqliteFileName);
                }
                else
                {
                    first = false;
                }
                connection = new SQLiteConnection("Data Source=" + dbFilePath + ";Version=3;");
                connection.Open();
                Debug.WriteLine("connect success");
                //初始化数据表和库
                if (first)
                {
                    string sql = "create table t_uploadtask (id integer primary key autoincrement,name varchar(100), path varchar(256),upload_path varchar(256),convert_path varchar(256),associated_img_path varhar(256), size long,md5 varchar(32),scan_rate int,width long,height long,resolution float,status int,user_id integer)";
                    SQLiteCommand command=SQLiteHelper.CreateCommand(connection, sql, null);
                    command.ExecuteNonQuery();
                }
                returnStatus();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
                Debug.WriteLine(e.Message);
            }
        }

        //初始化状态 转化中--》待处理，上传中--》待上传
        public static void returnStatus()
        {
            String sql = "update t_uploadtask set status="+Dictionary.STATUS_WAIT + " where status="+Dictionary.STATUS_UPLOAD;
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, null);
            command.ExecuteNonQuery();
        }

        public static bool addTask(UploadTask ut)
        {
            String sql = "insert into t_uploadtask(name,path,size,md5,status,user_id) values(@name,@path,@size,@md5,@status,@user_id)";
            SQLiteParameter p1= SQLiteHelper.CreateParameter("name", DbType.String, ut.name);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("path", DbType.String, ut.path);
            SQLiteParameter p3 = SQLiteHelper.CreateParameter("size", DbType.Int64, ut.size);
            SQLiteParameter p4 = SQLiteHelper.CreateParameter("md5", DbType.String, ut.md5);
            SQLiteParameter p5= SQLiteHelper.CreateParameter("status", DbType.Int16, ut.status);
            SQLiteParameter p6 = SQLiteHelper.CreateParameter("user_id", DbType.Int16,User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1, p2, p3, p4,p5, p6 });
            int result=command.ExecuteNonQuery();
            Debug.WriteLine("保存数据" + result + "条");
            return result>0?true:false;
        }

        public static bool delTask(int id)
        {
            String sql = "delete from t_uploadtask where id=@id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("id", DbType.Int32, id);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1});
            int result = command.ExecuteNonQuery();
            Debug.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool delTaskByStatus(int status)
        {
            String sql = "delete from t_uploadtask where status=@status and user_id=@user_id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("status", DbType.Int32, status);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("user_id", DbType.Int16, User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2 });
            int result = command.ExecuteNonQuery();
            Debug.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool delAllTask()
        {
            String sql = "delete from t_uploadtask where user_id=@user_id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("user_id", DbType.Int16, User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql,new SQLiteParameter[] { p1});
            int result = command.ExecuteNonQuery();
            Debug.WriteLine("删除数据" + result + "条");
            return result > 0 ? true : false;
        }

        public static IList<UploadTask> query()
        {
            String sql = "select * from t_uploadtask where user_id=@user_id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("user_id", DbType.Int16, User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1 });
            SQLiteDataReader reader = command.ExecuteReader();
            UploadTask task=null;
            IList<UploadTask> tasks = new BindingList<UploadTask>();
            UploadTask.tasks.Clear();
            while (reader.Read())
            {
                task = readUploadTask(reader);
                tasks.Add(task);
            }
            Debug.WriteLine("查询数据" + tasks.Count + "条");
            return tasks;
        }


        public static List<UploadTask> getByStatus(int status)
        {
            List<UploadTask> tasks = new List<UploadTask>();
            String sql = "select * from t_uploadtask where status=@status and user_id=@user_id order by id limit 10";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("status", DbType.Int32, status);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("user_id", DbType.Int16, User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2 });
            SQLiteDataReader reader = command.ExecuteReader();
            UploadTask task = null;
            while (reader.Read())
            {
                task = readUploadTask(reader);
                tasks.Add(task);
            }
            return tasks;
        }


        public static UploadTask getByMd5(String md5)
        {
            String sql = "select * from t_uploadtask where md5=@md5 and user_id=user_id limit 1";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("md5", DbType.String, md5);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("user_id", DbType.Int16, User.loginUser.userId);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2 });
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
            task.md5 = reader["md5"].ToString();
            task.uploadPath = reader["upload_path"].ToString();
            task.size = int.Parse(reader["size"].ToString());
            task.width = Utils.isNotEmpty(reader["width"].ToString()) ? int.Parse(reader["width"].ToString()) : 0;
            task.height = Utils.isNotEmpty(reader["height"].ToString()) ? int.Parse(reader["height"].ToString()) : 0;
            task.md5 = reader["md5"].ToString();
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
            Debug.WriteLine("修改数据状态" + result + "条");
            return result > 0 ? true : false;
        }

        public static bool updateInfo(UploadTask ut)
        {
            String sql = "update t_uploadtask set upload_path=@upload_path,width=@width,height=@height,md5=@md5,status=@status where id=@id";
            SQLiteParameter p1 = SQLiteHelper.CreateParameter("upload_path", DbType.String, ut.uploadPath);
            SQLiteParameter p2 = SQLiteHelper.CreateParameter("width", DbType.Int64, ut.width);
            SQLiteParameter p3 = SQLiteHelper.CreateParameter("height", DbType.Int64, ut.height);
            SQLiteParameter p4 = SQLiteHelper.CreateParameter("md5", DbType.String, ut.md5);
            SQLiteParameter p5 = SQLiteHelper.CreateParameter("status", DbType.Int64, ut.status);
            SQLiteParameter p6 = SQLiteHelper.CreateParameter("id", DbType.Int64, ut.id);
            SQLiteCommand command = SQLiteHelper.CreateCommand(connection, sql, new SQLiteParameter[] { p1,p2, p3, p4, p5, p6 });
            int result = command.ExecuteNonQuery();
            Debug.WriteLine("修改数据状态" + result + "条");
            return result > 0 ? true : false;
        }

    }
}
