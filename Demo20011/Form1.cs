using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo20011
{
    public partial class Form1 : Form
    {
        List<ListViewItem> listItem = new List<ListViewItem>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listViewEx1.View = View.Details;
            this.listViewEx1.GridLines = true;
            this.listViewEx1.CheckBoxes = true;
            this.listViewEx1.FullRowSelect = true;

            ImageList il = new ImageList();
            //设置高度
            il.ImageSize = new Size(1, 25);
            //绑定listView控件
            this.listViewEx1.SmallImageList = il;

            AddLVCol("index", "int", "序号", 60);
            AddLVCol("name", "text", "产品名称", 200);
            AddLVCol("nums", "int", "数量", 80);
            AddLVCol("date", "datetime", "生产日期", 200);


        }

        protected void AddLVCol(string pName, string pTag, string pText, int pWidth)
        {
            ColumnHeader ch = new ColumnHeader();
            ch.Name = pName;
            ch.Tag = pTag;
            ch.Text = pText;
            ch.Width = pWidth;
            this.listViewEx1.Columns.Add(ch);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem();
            item.Text = (listItem.Count + 1).ToString();
            item.SubItems.Add("皮鞋");
            item.SubItems.Add("50");
            item.SubItems.Add("2016-02-10 10:06:23");
            item.Checked = false;
            listItem.Add(item);

            item = new ListViewItem();
            item.Text = (listItem.Count + 1).ToString();
            item.SubItems.Add("皮衣");
            item.SubItems.Add("30");
            item.SubItems.Add("2016-02-15 08:06:23");
            listItem.Add(item);

            item = new ListViewItem();
            item.Text = (listItem.Count + 1).ToString();
            item.SubItems.Add("皮裤");
            item.SubItems.Add("80");
            item.SubItems.Add("2016-02-18 16:16:53");
            item.Checked = true;
            listItem.Add(item);

            this.listViewEx1.ReSet(listItem);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(this.listViewEx1.FirstSelectItemValue("name"));
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            List<ListViewItem> items = this.listViewEx1.GetSelectItem();
            foreach(var item in items)
            {
                Console.WriteLine(item.SubItems[1].Text);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            List<ListViewItem> items = this.listViewEx1.GetSelectItem();
            int i = 0;
            foreach (var item in items)
            {
                item.SubItems[1].Text = item.SubItems[1].Text + i.ToString();
                item.SubItems[2].Text = (int.Parse(item.SubItems[2].Text) + i+1).ToString();
                item.SubItems[3].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                i++;
            }

            this.listViewEx1.Invalidate();
        }

        private void ListViewEx1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > (listItem[0].Bounds.Left + 16))
            {
                Console.WriteLine(this.listViewEx1.FirstSelectItemValue("name"));
            }
        }
    }
}
