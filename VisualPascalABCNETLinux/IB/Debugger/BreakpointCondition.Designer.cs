
namespace VisualPascalABC
{
	partial class BreakpointConditionForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.chbCondtionEnabled = new System.Windows.Forms.CheckBox();
			this.tbCondition = new System.Windows.Forms.TextBox();
			this.rbTrue = new System.Windows.Forms.RadioButton();
			this.rbChanged = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// chbCondtionEnabled
			// 
			this.chbCondtionEnabled.Checked = true;
			this.chbCondtionEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbCondtionEnabled.Location = new System.Drawing.Point(12, 12);
			this.chbCondtionEnabled.Name = "chbCondtionEnabled";
			this.chbCondtionEnabled.Size = new System.Drawing.Size(122, 24);
			this.chbCondtionEnabled.TabIndex = 1;
			this.chbCondtionEnabled.Text = "M_CONDITION2";
			this.chbCondtionEnabled.UseVisualStyleBackColor = true;
			this.chbCondtionEnabled.CheckedChanged += new System.EventHandler(this.ChbCondtionEnabledCheckedChanged);
			// 
			// tbCondition
			// 
			this.tbCondition.Location = new System.Drawing.Point(30, 33);
			this.tbCondition.Name = "tbCondition";
			this.tbCondition.Size = new System.Drawing.Size(279, 20);
			this.tbCondition.TabIndex = 2;
			// 
			// rbTrue
			// 
			this.rbTrue.Checked = true;
			this.rbTrue.Location = new System.Drawing.Point(30, 59);
			this.rbTrue.Name = "rbTrue";
			this.rbTrue.Size = new System.Drawing.Size(104, 24);
			this.rbTrue.TabIndex = 3;
			this.rbTrue.TabStop = true;
			this.rbTrue.Text = "M_IF_TRUE";
			this.rbTrue.UseVisualStyleBackColor = true;
			// 
			// rbChanged
			// 
			this.rbChanged.Location = new System.Drawing.Point(30, 80);
			this.rbChanged.Name = "rbChanged";
			this.rbChanged.Size = new System.Drawing.Size(104, 24);
			this.rbChanged.TabIndex = 4;
			this.rbChanged.Text = "M_IF_CHANGED";
			this.rbChanged.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(136, 110);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(83, 23);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "RF_OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(225, 110);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(84, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "RF_CANCEL";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// BreakpointConditionForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(314, 142);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.rbChanged);
			this.Controls.Add(this.rbTrue);
			this.Controls.Add(this.tbCondition);
			this.Controls.Add(this.chbCondtionEnabled);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BreakpointConditionForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "M_BREAKPOINT_CONDITION_FORM";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.CheckBox chbCondtionEnabled;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.RadioButton rbChanged;
		private System.Windows.Forms.RadioButton rbTrue;
		private System.Windows.Forms.TextBox tbCondition;
	}
}
