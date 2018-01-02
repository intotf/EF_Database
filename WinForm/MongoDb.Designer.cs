namespace WinForm
{
    partial class MongoDb
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.同一个表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按月分表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.按年分表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.同一个表ToolStripMenuItem,
            this.按月分表ToolStripMenuItem,
            this.按年分表ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(750, 285);
            this.dataGridView1.TabIndex = 5;
            // 
            // 同一个表ToolStripMenuItem
            // 
            this.同一个表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.同一个表ToolStripMenuItem.Name = "同一个表ToolStripMenuItem";
            this.同一个表ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.同一个表ToolStripMenuItem.Text = "同一个表";
            this.同一个表ToolStripMenuItem.Click += new System.EventHandler(this.同一个表ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.AddToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // 按月分表ToolStripMenuItem
            // 
            this.按月分表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem1,
            this.删除ToolStripMenuItem1});
            this.按月分表ToolStripMenuItem.Name = "按月分表ToolStripMenuItem";
            this.按月分表ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.按月分表ToolStripMenuItem.Text = "按月分表";
            this.按月分表ToolStripMenuItem.Click += new System.EventHandler(this.按月分表ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem1
            // 
            this.添加ToolStripMenuItem1.Name = "添加ToolStripMenuItem1";
            this.添加ToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem1.Text = "添加";
            this.添加ToolStripMenuItem1.Click += new System.EventHandler(this.添加月ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // 按年分表ToolStripMenuItem
            // 
            this.按年分表ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem2,
            this.删除ToolStripMenuItem2});
            this.按年分表ToolStripMenuItem.Name = "按年分表ToolStripMenuItem";
            this.按年分表ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.按年分表ToolStripMenuItem.Text = "按年分表";
            this.按年分表ToolStripMenuItem.Click += new System.EventHandler(this.按年分表ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem2
            // 
            this.添加ToolStripMenuItem2.Name = "添加ToolStripMenuItem2";
            this.添加ToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.添加ToolStripMenuItem2.Text = "添加";
            this.添加ToolStripMenuItem2.Click += new System.EventHandler(this.添加ToolStripMenuItem2_Click);
            // 
            // 删除ToolStripMenuItem2
            // 
            this.删除ToolStripMenuItem2.Name = "删除ToolStripMenuItem2";
            this.删除ToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.删除ToolStripMenuItem2.Text = "删除";
            this.删除ToolStripMenuItem2.Click += new System.EventHandler(this.删除ToolStripMenuItem2_Click);
            // 
            // MongoDb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 310);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MongoDb";
            this.Text = "MongoDb";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 同一个表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按月分表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 按年分表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem2;
    }
}