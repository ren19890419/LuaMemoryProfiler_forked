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

        // �ڹ��캯�� InitializeComponent() ֮����ã� InitializeContextMenu();
        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            miCopyCell = new ToolStripMenuItem("���Ƶ�Ԫ��", null, miCopyCell_Click);
            miCopyRow = new ToolStripMenuItem("��������", null, miCopyRow_Click);
            miExportCsv = new ToolStripMenuItem("����Ϊ CSV...", null, miExportCsv_Click);
            miToggleSubtree = new ToolStripMenuItem("չ��/�۵�����", null, miToggleSubtree_Click);
            miSelectAllChildren = new ToolStripMenuItem("ѡ�������ӽڵ�", null, miSelectAllChildren_Click);

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
            // ����û��ڵ�һ��֮����Ҫ�ض����У�����չΪ HitTest �洢��������
            string text = item.SubItems.Count > 0 ? item.SubItems[0].Text : item.Text;
            try { Clipboard.SetText(text); } catch { /* ���Լ������쳣 */ }
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
                dlg.Filter = "CSV �ļ� (*.csv)|*.csv|�����ļ� (*.*)|*.*";
                dlg.DefaultExt = "csv";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    using (var sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                    {
                        // д��ͷ����������ͷ��
                        var headers = new List<string>();
                        foreach (ColumnHeader ch in treeListView1.Columns)
                            headers.Add(ch.Text.Replace("��", "").Replace("��", ""));
                        sw.WriteLine(string.Join(",", headers));

                        // дѡ�����ȫ���ɼ���
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
                    MessageBox.Show("����ʧ��: " + ex.Message);
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
                // �л���չ��/�۵��������ӽڵ�
                bool newOpen = !node.open;
                node.setOpenAll(newOpen);
            }
            ResortNodes();
        }

        private void miSelectAllChildren_Click(object sender, EventArgs e)
        {
            if (treeListView1.SelectedItems.Count == 0) return;
            // ��ѡ���������ֱ�Ӻͼ���ӽڵ��� ListView ��ѡ�У����� Tag �� Node ��ϵ��
            var toSelect = new List<ListViewItem>();
            foreach (ListViewItem item in treeListView1.Items)
            {
                var node = item.Tag as Node;
                if (node == null) continue;
                foreach (ListViewItem sel in treeListView1.SelectedItems)
                {
                    var parentNode = sel.Tag as Node;
                    if (parentNode == null) continue;
                    // ��� node �Ƿ� parentNode ������
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