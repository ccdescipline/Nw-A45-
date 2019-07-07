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

namespace Nwa45_MusicCenter
{
    public partial class checkList : Form
    {
        public checkList()
        {
            InitializeComponent();
        }
        //加载歌单
        private void checkList_Load(object sender, EventArgs e)
        {
            foreach (string i in info.song_list)
            {
                this.comboBox1.Items.Add(i);
            }
        }
        //确认按钮
        private void button1_Click(object sender, EventArgs e)
        {
            info.select_songs = this.comboBox1.SelectedIndex;
            this.Close();
        }

        //关闭设置
        
    }
}
