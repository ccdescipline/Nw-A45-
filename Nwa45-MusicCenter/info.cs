using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwa45_MusicCenter
{
    static class info
    {
        public static string device;        //设备盘符
        public static List<string> song_list = new List<string>();       //歌单
        public static List<string> song_list_path = new List<string>();       //歌单路径
        public static List<string> songs_path = new List<string>();          //要导入的歌曲路径
        public static int select_songs;       //选中的歌单索引
    }
}
