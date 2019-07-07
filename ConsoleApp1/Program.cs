using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shell32;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //写入歌单
            StreamWriter sw = new StreamWriter("C:\\Users\\caochuang\\Desktop\\抒情.m3u8", true);
            sw.WriteLine(String.Format("#EXTINF:{0},{1}", 128, "哈哈哈"));
            sw.WriteLine("F:\\MUSIC\\林俊杰\\曹操\\林俊杰 - 曹操.flac".Substring(9));
            sw.Close();
        }

        class time
        {
            
        }
    }


}
