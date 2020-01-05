using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListViewEx
{
    public class ListViewEx : ListView
    {
        #region 虚拟模式相关操作

        ///<summary>
        /// 前台行集合
        ///</summary>
        public List<ListViewItem> CurrentCacheItemsSource;

        public ListViewEx()
        {
            this.CurrentCacheItemsSource = new List<ListViewItem>();
            this.VirtualMode = true;
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView_RetrieveVirtualItem);

            // This is what you need, for drawing unchecked checkboxes
            this.OwnerDraw = true;
            this.DrawItem +=
                new DrawListViewItemEventHandler(listView_DrawItem);

            this.DrawColumnHeader +=new DrawListViewColumnHeaderEventHandler(ListView_DrawColumnHeader);

            // Redraw when checked or doubleclicked
            this.MouseClick += new MouseEventHandler(listView_MouseClick);
            this.MouseDoubleClick += new MouseEventHandler(listView_MouseDoubleClick);
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = this.GetItemAt(e.X, e.Y);
            if (lvi != null)
                this.Invalidate(lvi.Bounds);
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = this.GetItemAt(e.X, e.Y);
            if (lvi != null)
            {
                if (e.X < (lvi.Bounds.Left + 16))
                {
                    lvi.Checked = !lvi.Checked;
                    this.Invalidate(lvi.Bounds);
                }
            }
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawText();
        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if (!e.Item.Checked)
            {
                e.Item.Checked = true;
                e.Item.Checked = false;
            }
        }

        ///<summary>
        /// 重置listview集合
        ///</summary>
        ///<param name="l"></param>
        public void ReSet(IList<ListViewItem> l)
        {
            this.CurrentCacheItemsSource.Clear();
            this.CurrentCacheItemsSource = new List<ListViewItem>();
            foreach (var item in l)
            {
                this.CurrentCacheItemsSource.Add(item);
            }
            this.VirtualListSize = this.CurrentCacheItemsSource.Count;
        }

        ///<summary>
        /// 虚拟模式事件
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this.CurrentCacheItemsSource == null || this.CurrentCacheItemsSource.Count == 0)
            {
                return;
            }

            ListViewItem lv = this.CurrentCacheItemsSource[e.ItemIndex];
            e.Item = lv;
        }

        ///<summary>
        /// 获取选中的第一行的指定tag值
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public string FirstSelectItemValue(string key)
        {
            if (this.SelectedIndices.Count == 0)
                return "";
            int i = GetColumnsIndex(key);

            return this.CurrentCacheItemsSource[this.SelectedIndices[0]].SubItems[i].Text;
        }

        ///<summary>
        /// 获取列名的索引
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public int GetColumnsIndex(string key)
        {
            int i = 0;
            for (; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Name == key)
                {
                    break;
                }
            }

            return i;
        }

        ///<summary>
        /// 获取选中项
        ///</summary>
        ///<returns></returns>
        public List<ListViewItem> GetSelectItem()
        {
            List<ListViewItem> l = new List<ListViewItem>();
            foreach (var item in this.SelectedIndices)
            {
                l.Add(this.CurrentCacheItemsSource[int.Parse(item.ToString())]);
            }
            return l;
        }

        ///<summary>
        /// 获取选中行的某列集合
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public List<string> GetListViewField(string key)
        {
            List<string> ids = new List<string>();

            foreach (var item in this.SelectedIndices)
            {
                string id = this.CurrentCacheItemsSource[int.Parse(item.ToString())].SubItems[GetColumnsIndex(key)].Text;
                ids.Add(id);
            }
            return ids;
        }

        private ListViewItemComparer mySorter;
        ///<summary>
        /// 排序
        ///</summary>
        ///<param name="e"></param>
        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            string dbType = this.Columns[e.Column].Tag.ToString();

            if (this.mySorter == null)
            {
                this.mySorter = new ListViewItemComparer(e.Column, SortOrder.Ascending, dbType);
            }
            else
            {
                if (this.mySorter.SortColumn == e.Column)
                {
                    if (this.mySorter.Order == SortOrder.Ascending)
                    {
                        this.mySorter.Order = SortOrder.Descending;
                    }
                    else
                    {
                        this.mySorter.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    this.mySorter.SortColumn = e.Column;
                    this.mySorter.Order = SortOrder.Ascending;
                }

                this.mySorter.DbType = dbType;

                this.CurrentCacheItemsSource.Sort(this.mySorter);

                this.Invalidate();
            }
        }
        #endregion

        #region 普通模式下排序
        /*普通模式下排序
        public void ReLoadColumn()
        {
            this.ListViewItemSorter = new ListViewItemComparer(0, SortOrder.Ascending, this.Columns[0].Tag.ToString());
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            string dbType = this.Columns[e.Column].Tag.ToString();


            if (this.ListViewItemSorter == null)
            {
                this.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending, dbType);
            }
            else
            {
                ListViewItemComparer comparer = this.ListViewItemSorter as ListViewItemComparer;
                if (comparer.SortColumn == e.Column)
                {
                    if (comparer.Order == SortOrder.Ascending)
                    {
                        comparer.Order = SortOrder.Descending;
                    }
                    else
                    {
                        comparer.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    comparer.SortColumn = e.Column;
                    comparer.Order = SortOrder.Ascending;
                }

                //MessageBox.Show(dbType);

                comparer.DbType = dbType;

                //仅仅改变了ListViewItemSorter属性值，这里不会自动调用Sort()方法，需要显式指定执行Sort()方法实现排序。
                this.Sort();
            }
        }
        */
        #endregion

        #region ListView排序逻辑
        ///<summary>
        /// ListView排序逻辑
        ///</summary>
        private class ListViewItemComparer : System.Collections.Generic.IComparer<ListViewItem>
        {
            public string DbType;
            public ListViewItemComparer()
            {
                this.SortColumn = 0;
                this.Order = SortOrder.None;
            }

            public ListViewItemComparer(int column)
                : this()
            {
                this.SortColumn = column;
            }

            ///<summary>
            /// 
            ///</summary>
            ///<param name="column">哪列</param>
            ///<param name="sortOrder">排序方式</param>
            ///<param name="dbType">类型</param>
            public ListViewItemComparer(int column, SortOrder sortOrder, string dbType)
                : this(column)
            {
                Order = sortOrder;
                DbType = dbType.ToLower();
            }

            #region IComparer 成员
            public int Compare(ListViewItem x, ListViewItem y)
            {
                int result = 0;
                string c1 = "";
                string c2 = "";

                try
                {
                    c1 = x.SubItems[this.SortColumn].Text;
                    c2 = y.SubItems[this.SortColumn].Text;
                }
                catch (Exception )
                {
                    //    MessageBox.Show(ex.Message);
                    return 0;
                }

                switch (DbType)
                {
                    case "int":
                        result = Fn.ToInt(c1, 0) - Fn.ToInt(c2, 0);
                        break;

                    case "datetime":
                        DateTime t1 = Fn.ToDateTime(c1, DateTime.MinValue);
                        DateTime t2 = Fn.ToDateTime(c2, DateTime.MinValue);

                        if (DateTime.TryParse(c1, out t1) && DateTime.TryParse(c2, out t2))
                        {
                            int cha1 = Fn.ToInt((t1 - t2).TotalSeconds.ToString(), 0);
                            result = Fn.ToInt((t1 - t2).TotalSeconds.ToString(), 0);

                            if (cha1 == 0)
                                result = 0;
                            else if (cha1 < 0)
                                result = -1;
                            else
                                result = 1;
                        }
                        else
                        {
                            result = string.Compare(c1, c2);
                        }

                        break;

                    case "double":
                        double d1 = Fn.ToDouble(c1, 0);
                        double d2 = Fn.ToDouble(c2, 0);

                        if (d1 == d2)
                            result = 0;
                        else if (d1 < d2)
                            result = -1;
                        else
                            result = 1;
                        break;

                    default:
                        result = string.Compare(c1, c2);
                        break;
                }

                if (this.Order == SortOrder.Ascending)
                {
                    return result;
                }
                else if (this.Order == SortOrder.Descending)
                {
                    return (-result);
                }
                else
                {
                    return 0;
                }
            }
            #endregion

            ///<summary>
            /// 当前排序列
            ///</summary>
            public int SortColumn
            {
                get;
                set;
            }

            ///<summary>
            /// 当前列排序方式
            ///</summary>
            public SortOrder Order
            {
                get;
                set;
            }
        }
        #endregion
    }
}
