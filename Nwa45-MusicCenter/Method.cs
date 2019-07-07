using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;
using System.IO;

namespace Nwa45_MusicCenter
{
    static class Method
    {
        //最爱的range
        public static int[] range(int a)
        {
            int[] number = new int[a];
            for (int i = 0; i < number.Length; i++)
            {
                number[i] = i;
            }
            return number;
        }

        //获取歌曲信息，返回值依次为：歌名，歌手，专辑
        public static string[] 获取歌曲信息(string path)
        {
            string[] Info = new string[7];
            Shell32.Shell sh = new Shell();
            Folder dir = sh.NameSpace(System.IO.Path.GetDirectoryName(path));
            FolderItem item = dir.ParseName(System.IO.Path.GetFileName(path));

            Info[0] = dir.GetDetailsOf(item, 21);   // MP3 歌曲名
            if (Info[0]=="")
            {
                Info[0] = Path.GetFileName(path);
            }
            
            Info[1] = dir.GetDetailsOf(item, 20);  //歌手
            if (Info[1] == "")
            {
                Info[1] = "未知";
            }

            Info[2] = dir.GetDetailsOf(item, 14);  // MP3 专辑
            if (Info[2] == "")
            {
                Info[2] = "未知";
            }

            Info[3] = dir.GetDetailsOf(item, 27);  // 获取歌曲时长

            return Info;
        }

        //判断一个目录里面有没有一个子目录
        public static bool 是否存在目录(string path,string name)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            foreach (DirectoryInfo i in TheFolder.GetDirectories())
            {
                if (i.Name==name)
                {
                    return true;
                }
            }

            return false;

        }

        //复制文件方法
        public static void 复制(string oldPath,string newPath)
        {
            if (File.Exists(oldPath))//必须判断要复制的文件是否存在
            {
                File.Copy(oldPath,newPath,true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
            }
        }
    }
}
