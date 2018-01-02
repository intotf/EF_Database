namespace WinForm
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btSqlLite = new System.Windows.Forms.Button();
            this.btAccess = new System.Windows.Forms.Button();
            this.btMysql = new System.Windows.Forms.Button();
            this.btSqlServer = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btRedis = new System.Windows.Forms.Button();
            this.btMongoDb = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btSqlLite);
            this.groupBox1.Controls.Add(this.btAccess);
            this.groupBox1.Controls.Add(this.btMysql);
            this.groupBox1.Controls.Add(this.btSqlServer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 128);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "关系型数据库";
            // 
            // btSqlLite
            // 
            this.btSqlLite.Location = new System.Drawing.Point(405, 38);
            this.btSqlLite.Name = "btSqlLite";
            this.btSqlLite.Size = new System.Drawing.Size(103, 56);
            this.btSqlLite.TabIndex = 8;
            this.btSqlLite.Text = "SqlLite 实例";
            this.btSqlLite.UseVisualStyleBackColor = true;
            // 
            // btAccess
            // 
            this.btAccess.Location = new System.Drawing.Point(277, 38);
            this.btAccess.Name = "btAccess";
            this.btAccess.Size = new System.Drawing.Size(103, 56);
            this.btAccess.TabIndex = 7;
            this.btAccess.Text = "Access 实例";
            this.btAccess.UseVisualStyleBackColor = true;
            // 
            // btMysql
            // 
            this.btMysql.Location = new System.Drawing.Point(149, 38);
            this.btMysql.Name = "btMysql";
            this.btMysql.Size = new System.Drawing.Size(103, 56);
            this.btMysql.TabIndex = 6;
            this.btMysql.Text = "Mysql 实例";
            this.btMysql.UseVisualStyleBackColor = true;
            this.btMysql.Click += new System.EventHandler(this.btMysql_Click);
            // 
            // btSqlServer
            // 
            this.btSqlServer.Location = new System.Drawing.Point(21, 38);
            this.btSqlServer.Name = "btSqlServer";
            this.btSqlServer.Size = new System.Drawing.Size(103, 56);
            this.btSqlServer.TabIndex = 5;
            this.btSqlServer.Text = "SqlServer 实例";
            this.btSqlServer.UseVisualStyleBackColor = true;
            this.btSqlServer.Click += new System.EventHandler(this.btSqlServer_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btRedis);
            this.groupBox2.Controls.Add(this.btMongoDb);
            this.groupBox2.Location = new System.Drawing.Point(13, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(531, 128);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "非关系型数据库";
            // 
            // btRedis
            // 
            this.btRedis.Location = new System.Drawing.Point(174, 44);
            this.btRedis.Name = "btRedis";
            this.btRedis.Size = new System.Drawing.Size(123, 56);
            this.btRedis.TabIndex = 7;
            this.btRedis.Text = "Redis 实例";
            this.btRedis.UseVisualStyleBackColor = true;
            this.btRedis.Click += new System.EventHandler(this.btRedis_Click);
            // 
            // btMongoDb
            // 
            this.btMongoDb.Location = new System.Drawing.Point(20, 44);
            this.btMongoDb.Name = "btMongoDb";
            this.btMongoDb.Size = new System.Drawing.Size(123, 56);
            this.btMongoDb.TabIndex = 6;
            this.btMongoDb.Text = "MongoDb 实例";
            this.btMongoDb.UseVisualStyleBackColor = true;
            this.btMongoDb.Click += new System.EventHandler(this.btMongoDb_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 287);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EF 各类数据库连接实例";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btSqlLite;
        private System.Windows.Forms.Button btAccess;
        private System.Windows.Forms.Button btMysql;
        private System.Windows.Forms.Button btSqlServer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btRedis;
        private System.Windows.Forms.Button btMongoDb;
    }
}