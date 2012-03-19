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
