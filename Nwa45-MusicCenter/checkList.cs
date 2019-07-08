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
        //判断是导入歌曲还是删除歌曲界面，作用于chenckList
        public enum checkType
        {
            Insert,
            Delete
        }

        private checkType type;

        //增加判断是添加设备还是删除设备
        public checkList(checkType c)
        {
            InitializeComponent();
            if (c==checkType.Insert)
            {
                this.label1.Text = "请选择添加的歌单";
                this.type = checkType.Insert;
            }
            else
            {
                this.label1.Text = "请选择删除的歌单";
                this.type = checkType.Delete;
            }
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
            if (type==checkType.Insert)
            {
                //将选择的信息放入info里
                info.select_songs = this.comboBox1.SelectedIndex;
                this.Close();
            }
            else
            {
                //将选择的歌单删除
                DialogResult dr = MessageBox.Show("再问一遍，确定删除？");
                if (dr==DialogResult.OK)
                {
                    //删除歌单文件
                    File.Delete(info.song_list_path[this.comboBox1.SelectedIndex]);
                    MessageBox.Show("删除成功！");
                }
            }
            
        }

        //关闭设置
        
    }
}
