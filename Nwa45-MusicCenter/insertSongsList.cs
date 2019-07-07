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
    public partial class insertSongsList : Form
    {
        public insertSongsList()
        {
            InitializeComponent();
        }

        //添加歌单
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text=="")
            {
                MessageBox.Show("请输入歌单名");
            }
            else
            {
                string path = info.device + "\\" + this.textBox1.Text + ".m3u";
                FileStream fs = new FileStream(path,FileMode.Create);
                fs.Close();
                StreamWriter sw = new StreamWriter(path, true);
                sw.WriteLine("#EXTM3U");
                sw.Close();
                this.Close();
            }
        }
    }
}
