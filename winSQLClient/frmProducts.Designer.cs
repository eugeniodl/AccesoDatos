namespace winSQLClient
{
    partial class frmProducts
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
            this.dgvPRoducts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPRoducts)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPRoducts
            // 
            this.dgvPRoducts.AllowUserToAddRows = false;
            this.dgvPRoducts.AllowUserToDeleteRows = false;
            this.dgvPRoducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPRoducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPRoducts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvPRoducts.Location = new System.Drawing.Point(0, 214);
            this.dgvPRoducts.Name = "dgvPRoducts";
            this.dgvPRoducts.ReadOnly = true;
            this.dgvPRoducts.RowHeadersWidth = 51;
            this.dgvPRoducts.RowTemplate.Height = 29;
            this.dgvPRoducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPRoducts.Size = new System.Drawing.Size(575, 342);
            this.dgvPRoducts.TabIndex = 0;
            // 
            // frmProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 556);
            this.Controls.Add(this.dgvPRoducts);
            this.Name = "frmProducts";
            this.Text = "frmProducts";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPRoducts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dgvPRoducts;
    }
}