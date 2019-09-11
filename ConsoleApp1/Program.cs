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
            /*
            FileStream fs = new FileStream("C:\\Users\\caochuang\\Music\\林俊杰 _ 五月天 - 黑暗骑士.flac",FileMode.Open,FileAccess.Read);

            //保存歌曲信息的的byte
            byte[] result = new byte[10];
            fs.Read(result,0,result.Length);

            byte[] result1 = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                result1[i] = result[i];
            }
            */
            string path = "C:\\Users\\caochuang\\Music\\林俊杰 _ 五月天 - 黑暗骑士.flac";
            int mp3TagID = 0;
            string[] tags = new string[6];
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[10];
            // fs.Read(buffer, 0, 128);
            string mp3ID = "";

            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(buffer, 0, 10);
            mp3ID = Encoding.Default.GetString(buffer, 0, 3);
            if (mp3ID.Equals("ID3", StringComparison.OrdinalIgnoreCase))
            {

            }
        }

        public static string byteToString(byte[] b)
        {
            Encoding enc = Encoding.GetEncoding("GB2312");
            string str = enc.GetString(b);
            str = str.Substring(0, str.IndexOf('\0') >= 0 ? str.IndexOf('\0') : str.Length);//去掉无用字符     
            return str;
        }


        /*
        public static string[] ReadMp3(string path)
        {
            int mp3TagID = 0;
            string[] tags = new string[6];
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[10];
            // fs.Read(buffer, 0, 128);
            string mp3ID = "";

            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(buffer, 0, 10);
            int size = (buffer[6] & 0x7F) * 0x200000 + (buffer[7] & 0x7F) * 0x400 + (buffer[8] & 0x7F) * 0x80 + (buffer[9] & 0x7F);
            //int size = (buffer[6] & 0x7F) * 0X200000 * (buffer[7] & 0x7f) * 0x400 + (buffer[8] & 0x7F) * 0x80 + (buffer[9]);
            mp3ID = Encoding.Default.GetString(buffer, 0, 3);
            if (mp3ID.Equals("ID3", StringComparison.OrdinalIgnoreCase))
            {
                mp3TagID = 1;
                //如果有扩展标签头就跨过 10个字节
                if ((buffer[5] & 0x40) == 0x40)
                {
                    fs.Seek(10, SeekOrigin.Current);
                    size -= 10;
                }
                tags = ReadFrame(fs, size);
            }
            return tags;
        }


        public static string[] ReadFrame(FileStream fs, int size)
        {
            string[] ID3V2 = new string[6];
            byte[] buffer = new byte[10];
            while (size > 0)
            {
                //fs.Read(buffer, 0, 1);
                //if (buffer[0] == 0)
                //{
                //    size--;
                //    continue;
                //}
                //fs.Seek(-1, SeekOrigin.Current);
                //size++;
                //读取标签帧头的10个字节
                fs.Read(buffer, 0, 10);
                size -= 10;
                //得到标签帧ID
                string FramID = Encoding.Default.GetString(buffer, 0, 4);
                //计算标签帧大小，第一个字节代表帧的编码方式
                int frmSize = 0;

                frmSize = buffer[4] * 0x1000000 + buffer[5] * 0x10000 + buffer[6] * 0x100 + buffer[7];
                if (frmSize == 0)
                {
                    //就说明真的没有信息了
                    break;
                }
                //bFrame 用来保存帧的信息
                byte[] bFrame = new byte[frmSize];
                fs.Read(bFrame, 0, frmSize);
                size -= frmSize;
                string str = GetFrameInfoByEcoding(bFrame, bFrame[0], frmSize - 1);
                if (FramID.CompareTo("TIT2") == 0)
                {
                    ID3V2[0] = "TIT2" + str;
                }
                else if (FramID.CompareTo("TPE1") == 0)
                {
                    ID3V2[1] = "TPE1" + str;
                }
                else if (FramID.CompareTo("TALB") == 0)
                {
                    ID3V2[2] = "TALB" + str;
                }
                else if (FramID.CompareTo("TIME") == 0)
                {
                    ID3V2[3] = "TYER" + str;
                }
                else if (FramID.CompareTo("COMM") == 0)
                {
                    ID3V2[4] = "COMM" + str;
                }
                else if (FramID.CompareTo("APIC") == 0)
                {
                    MessageBox.Show("有图片信息");

                    int i = 0;
                    while (true)
                    {

                        if (255 == bFrame[i] && 216 == bFrame[i + 1])
                        {
                            //在
                            break;

                        }
                        i++;
                    }

                    byte[] imge = new byte[frmSize - i];
                    fs.Seek(-frmSize + i, SeekOrigin.Current);
                    fs.Read(imge, 0, imge.Length);
                    MemoryStream ms = new MemoryStream(imge);
                    Image img = Image.FromStream(ms);
                    FileStream save = new FileStream(@"C:\Users\PC-DELL\Desktop\22.jpeg", FileMode.Create);
                    img.Save(save, System.Drawing.Imaging.ImageFormat.Jpeg);

                    MessageBox.Show("成功");
                    //}
                }


            }
            return ID3V2;
        }
        public static string GetFrameInfoByEcoding(byte[] b, byte conde, int length)
        {
            string str = "";
            switch (conde)
            {
                case 0:
                    str = Encoding.GetEncoding("ISO-8859-1").GetString(b, 1, length);
                    break;
                case 1:
                    str = Encoding.GetEncoding("UTF-16LE").GetString(b, 1, length);
                    break;
                case 2:
                    str = Encoding.GetEncoding("UTF-16BE").GetString(b, 1, length);
                    break;
                case 3:
                    str = Encoding.UTF8.GetString(b, 1, length);
                    break;
            }
            return str;
        }
        */
    }


}
