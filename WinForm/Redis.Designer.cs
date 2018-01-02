namespace WinForm
{
    partial class Redis
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
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoding = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUserList = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(14, 340);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(617, 43);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "接到收的订阅信息：";
            // 
            // btnLoding
            // 
            this.btnLoding.Location = new System.Drawing.Point(297, 12);
            this.btnLoding.Name = "btnLoding";
            this.btnLoding.Size = new System.Drawing.Size(75, 23);
            this.btnLoding.TabIndex = 3;
            this.btnLoding.Text = "加载增量数据";
            this.btnLoding.UseVisualStyleBackColor = true;
            this.btnLoding.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "选择用户：";
            // 
            // cbUserList
            // 
            this.cbUserList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserList.Location = new System.Drawing.Point(84, 12);
            this.cbUserList.Name = "cbUserList";
            this.cbUserList.Size = new System.Drawing.Size(189, 20);
            this.cbUserList.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(617, 252);
            this.dataGridView1.TabIndex = 6;
            // 
            // Redis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 395);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbUserList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLoding);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbLog);
            this.Name = "Redis";
            this.Text = "Redis";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cbUserList;
    }
}