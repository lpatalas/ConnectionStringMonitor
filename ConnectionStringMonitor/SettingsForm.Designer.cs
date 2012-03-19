namespace ConnectionStringMonitor
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.outputFormatTextBox = new System.Windows.Forms.TextBox();
			this.patternsButton = new System.Windows.Forms.Button();
			this.patternsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(9, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(121, 13);
			label1.TabIndex = 3;
			label1.Text = "Connection string name:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 52);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(74, 13);
			label2.TabIndex = 4;
			label2.Text = "Output format:";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(207, 101);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(113, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(326, 101);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(113, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nameTextBox.Location = new System.Drawing.Point(12, 25);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(427, 20);
			this.nameTextBox.TabIndex = 2;
			// 
			// outputFormatTextBox
			// 
			this.outputFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputFormatTextBox.HideSelection = false;
			this.outputFormatTextBox.Location = new System.Drawing.Point(12, 69);
			this.outputFormatTextBox.Name = "outputFormatTextBox";
			this.outputFormatTextBox.Size = new System.Drawing.Size(391, 20);
			this.outputFormatTextBox.TabIndex = 5;
			// 
			// patternsButton
			// 
			this.patternsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.patternsButton.Location = new System.Drawing.Point(409, 69);
			this.patternsButton.Name = "patternsButton";
			this.patternsButton.Size = new System.Drawing.Size(30, 20);
			this.patternsButton.TabIndex = 6;
			this.patternsButton.Text = "?";
			this.patternsButton.UseVisualStyleBackColor = true;
			this.patternsButton.Click += new System.EventHandler(this.patternsButton_Click);
			// 
			// patternsContextMenu
			// 
			this.patternsContextMenu.Name = "patternsContextMenu";
			this.patternsContextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(451, 136);
			this.Controls.Add(this.patternsButton);
			this.Controls.Add(this.outputFormatTextBox);
			this.Controls.Add(label2);
			this.Controls.Add(label1);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximumSize = new System.Drawing.Size(2000, 170);
			this.MinimumSize = new System.Drawing.Size(16, 170);
			this.Name = "SettingsForm";
			this.Text = "Connection String Monitor Settings";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox outputFormatTextBox;
		private System.Windows.Forms.Button patternsButton;
		private System.Windows.Forms.ContextMenuStrip patternsContextMenu;
    }
}