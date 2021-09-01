#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace App
{
    public partial class MainForm : Form, IMessageFilter
    {
        MenuStrip _menu;
        WebView2Ex _webView;

        public bool PreFilterMessage(ref Message m)
        {
            return false;
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            return base.ProcessKeyPreview(ref m);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        public MainForm()
        {
            this.SuspendLayout();
            InitializeComponent();

            Application.AddMessageFilter(this);

            _menu = new MenuStrip();
            this.MainMenuStrip = _menu;
            _menu.Name = "Main menu";
            _menu.Dock = DockStyle.Top;
            this.Controls.Add(_menu);

            var fileMenuItem = new ToolStripMenuItem("File");
            _menu.Items.Add(fileMenuItem);
            var exitMenuItem = new ToolStripMenuItem("Exit") { ShortcutKeys = Keys.Alt | Keys.X };
            exitMenuItem.Click += (_, _) => this.Close();
            fileMenuItem.DropDownItems.Add(exitMenuItem);

            var textBox = new TextBox();
            textBox.Text = "Native TextBox";
            textBox.TabStop = true;
            textBox.TabIndex = 1;
            textBox.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Controls.Add(textBox);
            this.tableLayoutPanel.SetCellPosition(textBox,
                new TableLayoutPanelCellPosition { Row = 0, Column = 0 });

            _webView = new();
            _webView.TabStop = true;
            _webView.TabIndex = 2;
            _webView.Dock = DockStyle.Fill;

            this.tableLayoutPanel.Controls.Add(_webView);
            this.tableLayoutPanel.SetCellPosition(_webView, 
                new TableLayoutPanelCellPosition { Row = 1, Column = 0 });
            this.tableLayoutPanel.SetColumnSpan(_webView, 2);

            this.ResumeLayout();

            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            await _webView.EnsureCoreWebView2Async();
            _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

            // initial navigation
            var navigatedTcs = new TaskCompletionSource<DBNull>();
            _webView.CoreWebView2.NavigationCompleted += (s, e) =>
                navigatedTcs.TrySetResult(DBNull.Value);

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir!, "test.html");
            _webView.CoreWebView2.Navigate(new Uri(path).AbsoluteUri);
            await navigatedTcs.Task;
        }
    }
}
