using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;

namespace FirstAPI.DbContext
{
    public class DALSoloQ
    {
        public void SaveMatchInfos(string json,string gameId)
        {
            string firstFolderLocation = AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles";
            string secondFolderLocation = AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles" + Path.DirectorySeparatorChar + "MatchInfos";
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory+ "SoloQFiles"+Path.DirectorySeparatorChar+"MatchInfos" +Path.DirectorySeparatorChar+gameId);

            if(!Directory.Exists(firstFolderLocation)){
                Directory.CreateDirectory(firstFolderLocation);
            }
            if (!Directory.Exists(secondFolderLocation)){
                Directory.CreateDirectory(secondFolderLocation);
            }
            StreamWriter sw = new StreamWriter(location);
            sw.WriteLine(json);
            sw.Close();
        }

        public string GetMatchInfos(string gameId)
        {
            string json = "";
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles" + Path.DirectorySeparatorChar + "MatchInfos" + Path.DirectorySeparatorChar + gameId);
            if (File.Exists(location))
            {
                StreamReader sr = new StreamReader(location);
                json = sr.ReadToEnd();
            }
            
            return json;
        }

        public void SaveTimelinesMatchInfos(string json, string gameId)
        {
            string firstFolderLocation = AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles";
            string secondFolderLocation = AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles" + Path.DirectorySeparatorChar + "Timelines";
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles" + Path.DirectorySeparatorChar + "Timelines" + Path.DirectorySeparatorChar + gameId);
            if (!Directory.Exists(firstFolderLocation))
            {
                Directory.CreateDirectory(firstFolderLocation);
            }
            if (!Directory.Exists(secondFolderLocation))
            {
                Directory.CreateDirectory(secondFolderLocation);
            }
            StreamWriter sw = new StreamWriter(location);
            sw.WriteLine(json);
            sw.Close();
        }

        public string GetTimelinesMatchInfos(string gameId)
        {
            string json = "";
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "SoloQFiles" + Path.DirectorySeparatorChar + "Timelines" + Path.DirectorySeparatorChar + gameId);
            if (File.Exists(location))
            {
                StreamReader sr = new StreamReader(location);
                json = sr.ReadToEnd();
            }
            return json;
        }
    }
}