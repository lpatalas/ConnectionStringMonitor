// Copyright (c) 2012 Łukasz Patalas
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringMonitor
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();

			this.nameTextBox.Text = Settings.Default.ConnectionStringName;
			this.outputFormatTextBox.Text = Settings.Default.OutputFormat;
		}

		private void InitializePatternsMenu()
		{
			foreach (var pattern in ConnectionStringFormatter.Patterns)
				patternsContextMenu.Items.Add(pattern);

			patternsContextMenu.ItemClicked += patternsContextMenu_ItemClicked;
		}

		private void ShowPatternsMenu()
		{
			if (patternsContextMenu.Items.Count == 0)
				InitializePatternsMenu();

			patternsContextMenu.Show(Cursor.Position);
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			Settings.Default.ConnectionStringName = this.nameTextBox.Text;
			Settings.Default.OutputFormat = this.outputFormatTextBox.Text;
			Settings.Default.Save();
		}

		private void patternsButton_Click(object sender, EventArgs e)
		{
			ShowPatternsMenu();
		}

		private void patternsContextMenu_ItemClicked(
			object sender, ToolStripItemClickedEventArgs e)
		{
			var currentText = this.outputFormatTextBox.Text;
			var start = this.outputFormatTextBox.SelectionStart;
			var length = this.outputFormatTextBox.SelectionLength;

			this.outputFormatTextBox.Text = currentText.Substring(0, start)
				+ e.ClickedItem.Text
				+ currentText.Substring(start + length);
		}
	}
}
