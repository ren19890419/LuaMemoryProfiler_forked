using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Generic;

namespace MemorySnapshotViewer
{
    /// <summary>
    /// Summary description for MemorySnapshotViewer.
    /// </summary>
    public partial class MemorySnapshotViewer : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ColumnHeader allocRawSize;
        private ColumnHeader allocBytes;
        private ColumnHeader allocCount;
        private ColumnHeader sumAllocBytes;
        private ColumnHeader sumAllocCount;
        private ColumnHeader sumDeallocBytes;
        private ColumnHeader sumDeallocCount;
        private ColumnHeader allocType;
        private ColumnHeader name;
        private ListView treeListView1;
        private System.ComponentModel.IContainer components;

        // 新增：主菜单相关控件（包含 Settings）
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

        // 示例设置字段（可扩展为持久化）
        private bool _showRawSizes = false;

        // 新增：主题枚举与设置字段
        private enum ThemeMode { Light, Dark }
        private ThemeMode _theme = ThemeMode.Light;
        private float _uiFontSize = 9.0f;

        // 新增：语言枚举、主题色与字体大小等设置字段
        private enum UiLanguage { English = 0, Chinese = 1 }
        private UiLanguage _uiLanguage = UiLanguage.Chinese;
        private Color _themeColor = SystemColors.Window;

        // 新增/替换：更多设置字段
        private enum ThemePreset { Light, Dark, Solarized, HighContrast }
        private ThemePreset _themePreset = ThemePreset.Light;
        private string _uiFontFamily = SystemFonts.DefaultFont.FontFamily.Name;
        private FontStyle _uiFontStyle = FontStyle.Regular;

        public MemorySnapshotViewer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // 新增：初始化右键菜单
            InitializeContextMenu();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemorySnapshotViewer));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.treeListView1 = new System.Windows.Forms.ListView();
			this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.allocRawSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.allocBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.allocCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sumAllocBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sumAllocCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sumDeallocBytes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.sumDeallocCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.allocType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));

			// 新增：菜单控件实例化
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "");
			this.imageList1.Images.SetKeyName(1, "");
			this.imageList1.Images.SetKeyName(2, "");
			this.imageList1.Images.SetKeyName(3, "");
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(16, 652);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(224, 36);
			this.button1.TabIndex = 1;
			this.button1.Text = "打开Snapshot";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(920, 652);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(256, 36);
			this.button2.TabIndex = 2;
			this.button2.Text = "Expand / Collapse All";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1192, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemcomma)));
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.settingsToolStripMenuItem.Text = "Settings...";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// treeListView1
			// 
			this.treeListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeListView1.CheckBoxes = true;
			this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.allocRawSize,
            this.allocBytes,
            this.allocCount,
            this.sumAllocBytes,
            this.sumAllocCount,
            this.sumDeallocBytes,
            this.sumDeallocCount,
            this.allocType});
			this.treeListView1.FullRowSelect = true;
			this.treeListView1.GridLines = true;
			this.treeListView1.HideSelection = false;
			// 下移 ListView 以避开菜单栏
			this.treeListView1.Location = new System.Drawing.Point(0, 24);
			this.treeListView1.Name = "treeListView1";
			// 减少高度以适配菜单条
			this.treeListView1.Size = new System.Drawing.Size(1193, 623);
			this.treeListView1.TabIndex = 0;
			this.treeListView1.UseCompatibleStateImageBehavior = false;
			this.treeListView1.View = System.Windows.Forms.View.Details;
			this.treeListView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.treeListView1_ItemChecked);
			// 
			// name
			// 
			this.name.Text = "name";
			this.name.Width = 250;
			// 
			// allocRawSize
			// 
			this.allocRawSize.Text = "allocRawSize";
			this.allocRawSize.Width = 130;
			// 
			// allocBytes
			// 
			this.allocBytes.Text = "allocBytes";
			this.allocBytes.Width = 130;
			// 
			// allocCount
			// 
			this.allocCount.Text = "allocCount";
			this.allocCount.Width = 130;
			// 
			// sumAllocBytes
			// 
			this.sumAllocBytes.Text = "sumAllocBytes";
			this.sumAllocBytes.Width = 130;
			// 
			// sumAllocCount
			// 
			this.sumAllocCount.Text = "sumAllocCount";
			this.sumAllocCount.Width = 130;
			// 
			// sumDeallocBytes
			// 
			this.sumDeallocBytes.Text = "sumDeallocBytes";
			this.sumDeallocBytes.Width = 130;
			// 
			// sumDeallocCount
			// 
			this.sumDeallocCount.Text = "sumDeallocCount";
			this.sumDeallocCount.Width = 130;
			// 
			// allocType
			// 
			this.allocType.Text = "allocType";
			this.allocType.Width = 130;
			// 
			// MemorySnapshotViewer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(10, 21);
			this.ClientSize = new System.Drawing.Size(1192, 695);
			// 将菜单条设为主菜单
			this.MainMenuStrip = this.menuStrip1;
			// 菜单应先加入控件集合以确保显示在顶部
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.treeListView1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MemorySnapshotViewer";
			this.Text = "MemorySnapshotViewer";
			this.Load += new System.EventHandler(this.MemorySnapshotViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        [STAThread]
        static void Main()
        {
            Application.Run(new MemorySnapshotViewer());
        }

        // Implements the manual sorting of items by columns.
        class NodeComparer : IComparer<Node>
        {
            public SortOrder Order = SortOrder.Descending;
            public int SortColumn = 1;
            public int Compare(Node x, Node y)
            {
                int result = x.GetSubItem(SortColumn) - y.GetSubItem(SortColumn);
                return Order == SortOrder.Ascending ? result : -result;
            }
        }
        NodeComparer lvwColumnSorter;

        private void MemorySnapshotViewer_Load(object sender, System.EventArgs e)
        {
            lvwColumnSorter = new NodeComparer();
            treeListView1.ColumnClick += TreeListView1_ColumnClick;

            // 新增：应用初始设置
            ApplySettings();
        }

        private void TreeListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var col = treeListView1.Columns[lvwColumnSorter.SortColumn];
            col.Text = col.Text.Substring(0, col.Text.Length - (lvwColumnSorter.Order == SortOrder.Ascending ? "↑" : "↓").Length);

            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // 用新的排序方法对ListView排序
            ResortNodes();
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            //新建一个文件对话框
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            //设置对话框标题
            pOpenFileDialog.Title = "打开Snapshot文件";
            //设置打开文件类型
            pOpenFileDialog.Filter = "Snapshot文件（*.xml）|*.xml";
            //监测文件是否存在
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Multiselect = false;
            //文件打开后执行以下程序
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                treeListView1.Items.Clear();
                LoadXmlTree(pOpenFileDialog.FileName);
            }
        }

        // 新增：Settings 菜单项处理
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SettingsForm(_uiLanguage, _themePreset, _themeColor, _uiFontSize, _uiFontFamily, _uiFontStyle))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _uiLanguage = dlg.SelectedLanguage;
                    _themePreset = dlg.SelectedThemePreset;
                    _themeColor = dlg.SelectedColor;
                    _uiFontSize = dlg.SelectedFontSize;
                    _uiFontFamily = dlg.SelectedFontFamily;
                    _uiFontStyle = dlg.SelectedFontStyle;

                    ApplySettings();

                    // 可选：持久化设置
                    // Properties.Settings.Default.ThemePreset = (int)_themePreset;
                    // Properties.Settings.Default.ThemeColor = ColorTranslator.ToHtml(_themeColor);
                    // Properties.Settings.Default.FontFamily = _uiFontFamily;
                    // Properties.Settings.Default.FontSize = (double)_uiFontSize;
                    // Properties.Settings.Default.FontStyle = (int)_uiFontStyle;
                    // Properties.Settings.Default.Language = (int)_uiLanguage;
                    // Properties.Settings.Default.Save();
                }
            }
        }

        // 新增：Exit 菜单项处理
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool _allopen = false;
        private void button2_Click(object sender, System.EventArgs e)
        {
            _allopen = !_allopen;
            for (var index = 0; index < treeListView1.Items.Count; index++)
            {
                treeListView1.Items[index].Checked = _allopen;
            }
            _root.setOpenAll(_allopen);
        }



        Node _root;

        public void LoadXmlTree(string xml)
        {
            try
            {
                XDocument xDoc = XDocument.Load(xml);
                treeListView1.LabelEdit = false;
                _root = CreateNode(null, xDoc.Root, 0);
                ResortNodes();
                this.Text = xml;
                _allopen = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("打开Snapshot文件[" + xml + "]失败：" + e.ToString());

            }
        }

        const int LUA_TBOOLEAN = 1;
        const int LUA_TLIGHTUSERDATA = 2;
        const int LUA_TNUMBER = 3;
        const int LUA_TSTRING = 4;
        const int LUA_TTABLE = 5;
        const int LUA_TFUNCTION = 6;
        const int LUA_TUSERDATA = 7;
        const int LUA_TTHREAD = 8;

        public class Node
        {
            public bool open = true;
            public int level;
            public Node parent;
            public string name;
            public int rawSize;
            public int allocBytes;
            public int allocCount;
            public int sumAllocBytes;
            public int sumAllocCount;
            public int sumDeallocBytes;
            public int sumDeallocCount;
            public int allocType;
            public int GetSubItem(int index)
            {
                switch (index)
                {
                    case 1: return rawSize;
                    case 2: return allocBytes;
                    case 3: return allocCount;
                    case 4: return sumAllocBytes;
                    case 5: return sumAllocCount;
                    case 6: return sumDeallocBytes;
                    case 7: return sumDeallocCount;
                    case 8: return allocType;
                }
                return 0;
            }

            public List<Node> children = new List<Node>();

            public int allocRawSize
            {
                get
                {
                    return allocBytes + rawSize;
                }
            }
            public string displayName
            {
                get
                {
                    return new String(' ', level * 4) + (children.Count > 0 ? (open ? "- " : "+ ") : "| ") + name;
                }
            }
            public string displayAllocType
            {
                get
                {
                    string typeString = allocType.ToString();
                    if ((allocType & (1 << LUA_TBOOLEAN)) != 0) typeString += "|boolean";
                    if ((allocType & (1 << LUA_TLIGHTUSERDATA)) != 0) typeString += "|lightuserdata";
                    if ((allocType & (1 << LUA_TNUMBER)) != 0) typeString += "|number";
                    if ((allocType & (1 << LUA_TSTRING)) != 0) typeString += "|string";
                    if ((allocType & (1 << LUA_TTABLE)) != 0) typeString += "|table";
                    if ((allocType & (1 << LUA_TFUNCTION)) != 0) typeString += "|function";
                    if ((allocType & (1 << LUA_TUSERDATA)) != 0) typeString += "|userdate";
                    if ((allocType & (1 << LUA_TTHREAD)) != 0) typeString += "|thread";
                    return typeString;
                }
            }
            public void setOpenAll(bool open)
            {
                this.open = open;
                foreach (var child in children)
                    child.setOpenAll(open);
            }
        }

        public void ResortNodes()
        {
            treeListView1.Columns[lvwColumnSorter.SortColumn].Text += lvwColumnSorter.Order == SortOrder.Ascending ? "↑" : "↓";

            treeListView1.BeginUpdate();
            isUpdating = true;
            treeListView1.Items.Clear();
            PopulateTree(_root, treeListView1.Items, 0);
            isUpdating = false;
            treeListView1.EndUpdate();
        }

        public Node CreateNode(Node parent, XElement element, int level)
        {
            var node = new Node();
            node.parent = parent;
            node.level = level;
            node.name = element.Attribute("name")?.Value;
            node.allocBytes = int.Parse(element.Attribute("allocBytes")?.Value);
            node.rawSize = int.Parse(element.Attribute("rawSize")?.Value);
            node.allocCount = int.Parse(element.Attribute("allocCount")?.Value);
            node.sumAllocBytes = int.Parse(element.Attribute("sumAllocBytes")?.Value);
            node.sumAllocCount = int.Parse(element.Attribute("sumAllocCount")?.Value);
            node.sumDeallocBytes = int.Parse(element.Attribute("sumDeallocBytes")?.Value);
            node.sumDeallocCount = int.Parse(element.Attribute("sumDeallocCount")?.Value);
            node.allocType = int.Parse(element.Attribute("allocType")?.Value);

            if (element.HasElements)
            {
                level++;
                foreach (XElement xnode in element.Nodes())
                {
                    node.children.Add(CreateNode(node, xnode, level));
                }
                level--;
            }
            return node;
        }

        public int PopulateTree(Node node, ListView.ListViewItemCollection items, int index)
        {
            ListViewItem item = new ListViewItem();
            item.Text = node.displayName;
            item.Checked = node.open;
            item.SubItems.Add(node.allocRawSize.ToString());
            item.SubItems.Add(node.allocBytes.ToString());
            item.SubItems.Add(node.allocCount.ToString());
            item.SubItems.Add(node.sumAllocBytes.ToString());
            item.SubItems.Add(node.sumAllocCount.ToString());
            item.SubItems.Add(node.sumDeallocBytes.ToString());
            item.SubItems.Add(node.sumDeallocCount.ToString());
            item.SubItems.Add(node.displayAllocType);

            item.Tag = node;
            items.Insert(index, item);
            index++;
            node.children.Sort(lvwColumnSorter);
            if (node.open)
            {
                foreach (var child in node.children)
                {
                    index = PopulateTree(child, items, index);
                }
            }
            return index;
        }

        bool removeChildren(Node parent, ListViewItem item)
        {
            Node node = (Node)item.Tag;

            bool isParent = false;
            for (Node p = node.parent; p != null; p = p.parent)
            {
                if (p == parent) isParent = true;
            }
            if (isParent)
            {
                treeListView1.Items.Remove(item);
                return true;
            }
            return false;
        }

        bool isUpdating;

        private void treeListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (isUpdating)
                return;
            Node node = (Node)e.Item.Tag;
            node.open = e.Item.Checked;

            treeListView1.BeginUpdate();
            isUpdating = true;
            if (node.open)
            {
                int index = e.Item.Index + 1;
                foreach (var child in node.children)
                {
                    index = PopulateTree(child, treeListView1.Items, index);
                }
            }
            else
            {
                ListViewItem item = e.Item;
                while (item.Index + 1 < treeListView1.Items.Count)
                {
                    item = treeListView1.Items[item.Index + 1];
                    removeChildren(node, item);
                }
            }
            e.Item.Text = node.displayName;
            isUpdating = false;
            treeListView1.EndUpdate();
        }

        // 新增：应用设置到 UI
        private void ApplySettings()
        {
            // 应用字体（族 + 大小 + 样式）
            try
            {
                var newFont = new Font(new FontFamily(_uiFontFamily), _uiFontSize, _uiFontStyle);
                this.Font = newFont;
                treeListView1.Font = newFont;
            }
            catch
            {
                // 忽略非法字体选择
            }

            // 主题预设：设置窗体和 ListView 背景/前景
            Color back, fore;
            switch (_themePreset)
            {
                case ThemePreset.Dark:
                    back = Color.FromArgb(30, 30, 30);
                    fore = Color.White;
                    break;
                case ThemePreset.Solarized:
                    back = ColorTranslator.FromHtml("#FDF6E3"); // Solarized Light bg
                    fore = ColorTranslator.FromHtml("#657B83"); // base00
                    break;
                case ThemePreset.HighContrast:
                    back = Color.Black;
                    fore = Color.Yellow;
                    break;
                case ThemePreset.Light:
                default:
                    back = SystemColors.Window;
                    fore = SystemColors.WindowText;
                    break;
            }

            // 如果用户选择了自定义主题色（_themeColor），把它用作 ListView 的强调背景
            try
            {
                this.BackColor = back;
                this.ForeColor = fore;

                treeListView1.BackColor = _themeColor; // 自定义颜色作为列表背景
                // 若自定义颜色与预设冲突，可根据需要切换为 back
                var brightness = (_themeColor.R * 0.299 + _themeColor.G * 0.587 + _themeColor.B * 0.114) / 255.0;
                treeListView1.ForeColor = brightness < 0.5 ? Color.White : Color.Black;
            }
            catch
            {
                // 忽略视觉设置错误
            }

            // 更新界面文本（语言切换）
            UpdateUiStrings();

            // 强制重绘
            this.Refresh();
            treeListView1.Refresh();
        }

        // 新增：运行时更新界面文本（便于演示语言切换）
        private void UpdateUiStrings()
        {
            if (_uiLanguage == UiLanguage.Chinese)
            {
                button1.Text = "打开Snapshot";
                button2.Text = "展开/折叠全部";
                name.Text = "名称";
                allocRawSize.Text = "原始大小";
                allocBytes.Text = "allocBytes";
                allocCount.Text = "allocCount";
                sumAllocBytes.Text = "sumAllocBytes";
                sumAllocCount.Text = "sumAllocCount";
                sumDeallocBytes.Text = "sumDeallocBytes";
                sumDeallocCount.Text = "sumDeallocCount";
                allocType.Text = "类型";
                this.Text = "MemorySnapshotViewer";
                // 如果你有 MenuStrip 或 ContextMenu，需要在它们也更新文本（若存在，检查字段并赋值）
            }
            else
            {
                button1.Text = "Open Snapshot";
                button2.Text = "Expand / Collapse All";
                name.Text = "name";
                allocRawSize.Text = "allocRawSize";
                allocBytes.Text = "allocBytes";
                allocCount.Text = "allocCount";
                sumAllocBytes.Text = "sumAllocBytes";
                sumAllocCount.Text = "sumAllocCount";
                sumDeallocBytes.Text = "sumDeallocBytes";
                sumDeallocCount.Text = "sumDeallocCount";
                allocType.Text = "allocType";
                this.Text = "MemorySnapshotViewer";
            }
        }

        // 新增：轻量设置对话框（内嵌类，便于直接复制）
        private class SettingsForm : Form
        {
            private ComboBox cbLanguage;
            private ComboBox cbThemePreset;
            private Button btnColor;
            private NumericUpDown nudFontSize;
            private ComboBox cbFontFamily;
            private CheckBox chkBold;
            private CheckBox chkItalic;
            private Button btnOk;
            private Button btnCancel;
            private Panel pnlColorPreview;

            public UiLanguage SelectedLanguage { get; private set; }
            public ThemePreset SelectedThemePreset { get; private set; }
            public Color SelectedColor { get; private set; }
            public float SelectedFontSize { get; private set; }
            public string SelectedFontFamily { get; private set; }
            public FontStyle SelectedFontStyle { get; private set; }

            public SettingsForm(UiLanguage currentLang, ThemePreset currentPreset, Color currentColor, float currentFontSize, string currentFontFamily, FontStyle currentFontStyle)
            {
                this.Text = "Settings";
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.ClientSize = new Size(420, 220);
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                var lblLang = new Label { Left = 12, Top = 12, Width = 100, Text = "Language:" };
                cbLanguage = new ComboBox { Left = 120, Top = 8, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
                cbLanguage.Items.Add("English");
                cbLanguage.Items.Add("中文");
                cbLanguage.SelectedIndex = currentLang == UiLanguage.Chinese ? 1 : 0;

                var lblPreset = new Label { Left = 12, Top = 44, Width = 100, Text = "Theme preset:" };
                cbThemePreset = new ComboBox { Left = 120, Top = 40, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
                cbThemePreset.Items.Add("Light");
                cbThemePreset.Items.Add("Dark");
                cbThemePreset.Items.Add("Solarized");
                cbThemePreset.Items.Add("HighContrast");
                cbThemePreset.SelectedIndex = (int)currentPreset;

                var lblColor = new Label { Left = 12, Top = 80, Width = 100, Text = "Custom color:" };
                btnColor = new Button { Left = 120, Top = 76, Width = 100, Text = "Choose..." };
                pnlColorPreview = new Panel { Left = 230, Top = 76, Width = 40, Height = 22, BorderStyle = BorderStyle.FixedSingle, BackColor = currentColor };

                btnColor.Click += (s, e) =>
                {
                    using (var cd = new ColorDialog())
                    {
                        cd.Color = pnlColorPreview.BackColor;
                        if (cd.ShowDialog(this) == DialogResult.OK)
                        {
                            pnlColorPreview.BackColor = cd.Color;
                        }
                    }
                };

                var lblFontFam = new Label { Left = 12, Top = 116, Width = 100, Text = "Font family:" };
                cbFontFamily = new ComboBox { Left = 120, Top = 112, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
                foreach (var ff in FontFamily.Families)
                {
                    cbFontFamily.Items.Add(ff.Name);
                }
                // 选择当前字体族（若不存在则选第一个）
                if (!string.IsNullOrEmpty(currentFontFamily) && cbFontFamily.Items.Contains(currentFontFamily))
                    cbFontFamily.SelectedItem = currentFontFamily;
                else if (cbFontFamily.Items.Count > 0)
                    cbFontFamily.SelectedIndex = 0;

                var lblFont = new Label { Left = 12, Top = 152, Width = 100, Text = "Font size:" };
                nudFontSize = new NumericUpDown
                {
                    Left = 120,
                    Top = 148,
                    Width = 80,
                    Minimum = 6,
                    Maximum = 36,
                    DecimalPlaces = 1,
                    Increment = 0.5M,
                    Value = (decimal)Math.Max(6.0, Math.Min(36.0, currentFontSize))
                };

                chkBold = new CheckBox { Left = 210, Top = 148, Width = 70, Text = "Bold", Checked = (currentFontStyle & FontStyle.Bold) != 0 };
                chkItalic = new CheckBox { Left = 290, Top = 148, Width = 70, Text = "Italic", Checked = (currentFontStyle & FontStyle.Italic) != 0 };

                btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Left = 240, Width = 80, Top = 180 };
                btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 325, Width = 80, Top = 180 };

                btnOk.Click += (s, e) =>
                {
                    SelectedLanguage = cbLanguage.SelectedIndex == 1 ? UiLanguage.Chinese : UiLanguage.English;
                    SelectedThemePreset = (ThemePreset)cbThemePreset.SelectedIndex;
                    SelectedColor = pnlColorPreview.BackColor;
                    SelectedFontSize = (float)nudFontSize.Value;
                    SelectedFontFamily = cbFontFamily.SelectedItem?.ToString() ?? SystemFonts.DefaultFont.FontFamily.Name;
                    FontStyle style = FontStyle.Regular;
                    if (chkBold.Checked) style |= FontStyle.Bold;
                    if (chkItalic.Checked) style |= FontStyle.Italic;
                    SelectedFontStyle = style;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };

                this.Controls.Add(lblLang);
                this.Controls.Add(cbLanguage);
                this.Controls.Add(lblPreset);
                this.Controls.Add(cbThemePreset);
                this.Controls.Add(lblColor);
                this.Controls.Add(btnColor);
                this.Controls.Add(pnlColorPreview);
                this.Controls.Add(lblFontFam);
                this.Controls.Add(cbFontFamily);
                this.Controls.Add(lblFont);
                this.Controls.Add(nudFontSize);
                this.Controls.Add(chkBold);
                this.Controls.Add(chkItalic);
                this.Controls.Add(btnOk);
                this.Controls.Add(btnCancel);

                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }
        }
    }
}
