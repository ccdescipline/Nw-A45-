using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Nwa45_MusicCenter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        //加载选择设备窗口
        private void MainForm_Load(object sender, EventArgs e)
        {
            checkDevice cd = new checkDevice();
            cd.ShowDialog();
        }

        //导入本地歌曲
        private void button1_Click(object sender, EventArgs e)
        {
                    //用来保存选区的文件路径
            //用户获取的文件们
           DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr==DialogResult.OK)
            {
                string[] songs = this.openFileDialog1.FileNames;
                //显示到文件列表
                foreach (string i in songs)
                {
                    //保存到变量
                    info.songs_path.Add(i);
                    //读取歌曲相关信息加载在列表上
                    string[] songs_info = Method.获取歌曲信息(i);
                    ListViewItem lvi = new ListViewItem(songs_info[0]);
                    lvi.SubItems.Add(songs_info[1]);
                    lvi.SubItems.Add(songs_info[2]);
                    this.listView1.Items.Add(lvi);
                }
            }
        }

        //删除导入的歌曲
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices != null && this.listView1.SelectedIndices.Count > 0)
            {
                int[] select_result = new int[this.listView1.SelectedIndices.Count];
                foreach (int i in Method.range(this.listView1.SelectedIndices.Count))
                {
                    select_result[i] = this.listView1.SelectedIndices[i];
                }
                IEnumerable<int> reverse = select_result.Reverse();

                foreach (int i in reverse)
                {
                    //删除列表的项
                    this.listView1.Items.RemoveAt(i);
                    //删除info里的歌曲路径
                    info.songs_path.RemoveAt(i);
                }
            }
        }

        //写入歌曲并添加到歌单
        private void button3_Click(object sender, EventArgs e)
        {
            //清除状态栏
            this.toolStripStatusLabel1.Text = "";

            checkList cl = new checkList();
            cl.ShowDialog();

            //保存选中歌曲的索引
            int[] select_result = new int[this.listView1.SelectedIndices.Count];

            foreach (int i in Method.range(this.listView1.SelectedIndices.Count))
            {
                select_result[i] = this.listView1.SelectedIndices[i];
            }

            int step = 1;
            //一首歌曲一首歌曲的操作
            foreach (int i in select_result)
            {
                this.toolStripStatusLabel1.Text = "正在写入第"+step+"首歌";
                step++;

                //复制过去的歌曲路径
                string new_song_path = info.device;


                //判断设备有没有这个歌手名字文件夹
                if (Method.是否存在目录(info.device, this.listView1.Items[i].SubItems[1].Text) == true)
                {
                    new_song_path += "\\" + this.listView1.Items[i].SubItems[1].Text;
                }
                else
                {
                    new_song_path += "\\" + this.listView1.Items[i].SubItems[1].Text;
                    Directory.CreateDirectory(new_song_path);
                }
                //判断设备有没有这个歌手的专辑文件夹
                if ((Method.是否存在目录(new_song_path, this.listView1.Items[i].SubItems[2].Text) == true))
                {
                    new_song_path += "\\" + this.listView1.Items[i].SubItems[2].Text;
                }
                else
                {
                    new_song_path += "\\" + this.listView1.Items[i].SubItems[2].Text;
                    Directory.CreateDirectory(new_song_path);
                }

                //保存移动后文件路径
                string move_song_path = new_song_path + "\\" + Path.GetFileName(info.songs_path[i]);

                //把歌曲复制到相应的位置
                try
                {
                    Method.复制(info.songs_path[i], move_song_path);
                }
                catch (Exception)
                {
                    MessageBox.Show("复制失败");
                    return;
                }

                //MessageBox.Show(Method.获取歌曲信息(info.songs_path[i])[3]);
                //获取歌曲时间
                string [] song_time = Method.获取歌曲信息(info.songs_path[i])[3].Split(':');
                time t = new time(0,int.Parse(song_time[0]), int.Parse(song_time[1]), int.Parse(song_time[2]));
                double song_second = t.getsecond();

                //写入歌单
                StreamWriter sw = new StreamWriter(info.song_list_path[info.select_songs], true);
                sw.WriteLine(String.Format("#EXTINF:{0},{1}",song_second, Path.GetFileNameWithoutExtension(info.songs_path[i])));
                sw.WriteLine(move_song_path.Substring(9));
                sw.Close();
                
                
            }

            this.toolStripStatusLabel1.Text = "写入成功！";
        }

        //菜单栏添加设备窗口
        private void 添加设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkDevice cd = new checkDevice();
            cd.ShowDialog();
        }

        //检测设备是否正确导入
        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (info.device==""||info.device==null)
            {
                this.toolStripStatusLabel1.Text = "你还没添加设备，请在设备菜单栏里面添加！";
            }
            else
            {
                this.toolStripStatusLabel1.Text = "设备添加成功！";
            }

            List<string> songs = 获取歌单();
            info.song_list.Clear();
            info.song_list_path.Clear();
            foreach (string i in songs)
            {
                //添加歌单
                info.song_list.Add(i);
                //添加歌单路径
                info.song_list_path.Add(info.device + "\\" + i);
            }
        }

        //添加歌单
        private void button4_Click(object sender, EventArgs e)
        {
            insertSongsList f = new insertSongsList();
            f.ShowDialog();
        }

        //获取歌单
        public List<string> 获取歌单()
        {
            List<string> list = new List<string>();

            string music_path = info.device;
            DirectoryInfo TheFolder = new DirectoryInfo(music_path);
            //遍历文件
            foreach (FileInfo i in TheFolder.GetFiles())
            {
                list.Add(i.Name);
            }

            //匹配出歌单文件
            IEnumerable<string> res = list.Where(x => Regex.Matches(x, @"\w.m3u").Count != 0);
            List<string> result = new List<string>();
            foreach (string i in res)
            {
                result.Add(i);
            }

            return result;
        }
    }
}
