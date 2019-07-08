using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Text.RegularExpressions;

namespace Nwa45_MusicCenter
{
    public partial class checkDevice : Form
    {
        public checkDevice()
        {
            InitializeComponent();
        }
        
        private void checkDevice_Load(object sender, EventArgs e)
        {
            遍历盘符();
        }

        //遍历盘符方法
        public void 遍历盘符()
        {
            this.comboBox1.Items.Clear();
            List<string> deviceIDs = new List<string>();
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT  *  From  Win32_LogicalDisk ");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                this.comboBox1.Items.Add(mo["DeviceID"].ToString());
            }
        }
        //刷新按钮
        private void button1_Click(object sender, EventArgs e)
        {
            遍历盘符();
        }
        //确认按钮
        private void button2_Click(object sender, EventArgs e)
        {
            info.device = this.comboBox1.SelectedItem.ToString();

            try
            {
                List<string> songs = 获取歌单();
                info.song_list.Clear();
                info.song_list_path.Clear();
                foreach (string i in songs)
                {
                    //添加歌单
                    info.song_list.Add(i);
                    //添加歌单路径
                    info.song_list_path.Add(info.device+"\\"+i);
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("找不到歌单目录,请确保选择了正确的个歌单！");
            }
        }

        //获取歌单
        public List<string> 获取歌单()
        {
            List<string> list = new List<string>();

            string music_path = info.device + "\\MUSIC";
            info.device += "\\MUSIC";
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

        private void checkDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if (e.CloseReason==CloseReason.UserClosing)
            {
                Application.Exit();
            }
            */
        }
    }
}
