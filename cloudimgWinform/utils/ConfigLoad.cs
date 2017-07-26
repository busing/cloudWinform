using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace cloudimgWinform.utils
{
    class ConfigLoad
    {
        public ConfigLoad(string conFilePath)
        {
            this.conFilePath = conFilePath;
            ReadConfig();


        }
        /// <summary>  
        /// 配置文件的目录  
        /// </summary>  
        #region ConFilePath  
        string conFilePath;
        public string ConFilePath
        {
            set { conFilePath = value; }
            get { return conFilePath; }
        }
        #endregion


        /// <summary>  
        /// 配置文件属性值  
        /// </summary>  
        private List<string> configName = new List<string>();//名称集合  
        private List<string> configValue = new List<string>(); //数值集合  


        /// <summary>  
        /// 读取配置文件的属性值  
        /// </summary>  
        public bool ReadConfig()
        {
            //检查配置文件是否存在  
            if (!File.Exists(this.conFilePath))
            {
                return false;
            }


            StreamReader sr = new StreamReader(this.conFilePath, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("#"))
                {
                    continue;
                }
                string cName, cValue;
                string[] cLine = line.Split('=');
                if (cLine.Length == 2)
                {
                    cName = cLine[0].ToLower();
                    cValue = cLine[1].ToLower();
                    configName.Add(cName);
                    configValue.Add(cValue);
                }


            }
            sr.Close();
            return true;


        }
        #region GetConfigValue  
        /// <summary>  
        /// 返回变量的字符串值  
        /// </summary>  
        /// <param name="cName">变量名称</param>  
        /// <returns>变量值</returns>  


        public string GetStringValue(string cName)
        {


            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    return configValue[i];
                }
            }


            return null;
        }
        public int GetIntValue(string cName)
        {


            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    int result;
                    if (int.TryParse(configValue[i], out result))
                    {
                        return result;
                    }


                }
            }
            return 0;
        }
        public float GetFloatValue(string cName)
        {


            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    float result;
                    if (float.TryParse(configValue[i], out result))
                    {
                        return result;
                    }


                }
            }
            return 0;
        }
        #endregion
    }
}
