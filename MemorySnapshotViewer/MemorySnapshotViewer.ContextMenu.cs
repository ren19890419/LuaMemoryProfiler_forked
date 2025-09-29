using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace MemorySnapshotViewer
{
    partial class MemorySnapshotViewer
    {
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem miCopyCell;
        private ToolStripMenuItem miCopyRow;
        private ToolStripMenuItem miExportCsv;
        private ToolStripMenuItem miToggleSubtree;
        private ToolStripMenuItem miSelectAllChildren;

        // 在构造函数 InitializeComponent() 之后调用： InitializeContextMenu();
        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            miCopyCell = new ToolStripMenuItem("复制单元格", null, miCopyCell_Click);
            miCopyRow = new ToolStripMenuItem("复制整行", null, miCopyRow_Click);
            miExportCsv = new ToolStripMenuItem("导出为 CSV...", null, miExportCsv_Click);
            miToggleSubtree = new ToolStripMenuItem("展开/折叠子树", null, miToggleSubtree_Click);
            miSelectAllChildren = new ToolStripMenuItem("选中所有子节点", null, miSelectAllChildren_Click);

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                miCopyCell,
                miCopyRow,
                new ToolStripSeparator(),
                miExportCsv,
                new ToolStripSeparator(),
                miToggleSubtree,
                miSelectAllChildren
            });

            treeListView1.ContextMenuStrip = contextMenu;
        }

        private void miCopyCell_Click(object sender, EventArgs e)
        {
            if (treeListView1.SelectedItems.Count == 0) return;
            var item = treeListView1.SelectedItems[0];
            // 如果用户在第一列之外需要特定子列，可扩展为 HitTest 存储子项索引
            string text = item.SubItems.Count > 0 ? item.SubItems[0].Text : item.Text;
            try { Clipboard.SetText(text); } catch { /* 忽略剪贴板异常 */ }
        }

        private void miCopyRow_Click(object sender, EventArgs e)
        {
            if (treeListView1.SelectedItems.Count == 0) return;
            var item = treeListView1.SelectedItems[0];
            var parts = new List<string>();
            foreach (ListViewItem.ListViewSubItem sub in item.SubItems) parts.Add(sub.Text);
            string text = string.Join("\t", parts.ToArray());
            try { Clipboard.SetText(text); } catch { }
        }

        private void miExportCsv_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV 文件 (*.csv)|*.csv|所有文件 (*.*)|*.*";
                dlg.DefaultExt = "csv";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    using (var sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                    {
                        // 写表头（清除排序箭头）
                        var headers = new List<string>();
                        foreach (ColumnHeader ch in treeListView1.Columns)
                            headers.Add(ch.Text.Replace("↑", "").Replace("↓", ""));
                        sw.WriteLine(string.Join(",", headers));

                        // 写选中项或全部可见项
                        IEnumerable<ListViewItem> items;
                        if (treeListView1.SelectedItems.Count > 0)
                            items = treeListView1.SelectedItems.Cast<ListViewItem>();
                        else
                            items = treeListView1.Items.Cast<ListViewItem>();

                        foreach (ListViewItem item in items)
                        {
                            var cells = new List<string>();
                            foreach (ListViewItem.ListViewSubItem sub in item.SubItems)
                            {
                                string cell = sub.Text.Replace("\"", "\"\"");
                                cells.Add("\"" + cell + "\"");
                            }
                            sw.WriteLine(string.Join(",", cells));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败: " + ex.Message);
                }
            }
        }

        private void miToggleSubtree_Click(object sender, EventArgs e)
        {
            if (treeListView1.SelectedItems.Count == 0) return;
            foreach (ListViewItem item in treeListView1.SelectedItems)
            {
                var node = item.Tag as Node;
                if (node == null) continue;
                // 切换并展开/折叠其所有子节点
                bool newOpen = !node.open;
                node.setOpenAll(newOpen);
            }
            ResortNodes();
        }

        private void miSelectAllChildren_Click(object sender, EventArgs e)
        {
            if (treeListView1.SelectedItems.Count == 0) return;
            // 将选中项的所有直接和间接子节点在 ListView 中选中（基于 Tag 的 Node 关系）
            var toSelect = new List<ListViewItem>();
            foreach (ListViewItem item in treeListView1.Items)
            {
                var node = item.Tag as Node;
                if (node == null) continue;
                foreach (ListViewItem sel in treeListView1.SelectedItems)
                {
                    var parentNode = sel.Tag as Node;
                    if (parentNode == null) continue;
                    // 检查 node 是否 parentNode 的子孙
                    for (var p = node.parent; p != null; p = p.parent)
                    {
                        if (p == parentNode)
                        {
                            toSelect.Add(item);
                            break;
                        }
                    }
                }
            }
            treeListView1.BeginUpdate();
            foreach (var it in toSelect) it.Selected = true;
            treeListView1.EndUpdate();
            if (toSelect.Count > 0) treeListView1.EnsureVisible(toSelect[0].Index);
        }
    }
}