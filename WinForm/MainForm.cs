using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void btMongoDb_Click(object sender, EventArgs e)
        {

        }

        private void btSqlServer_Click(object sender, EventArgs e)
        {
            new Mssql().Show();
        }

        private void btMysql_Click(object sender, EventArgs e)
        {
            new Mysql().Show();
        }

        private void btMongoDb_Click_1(object sender, EventArgs e)
        {
            new MongoDb().Show();
        }

        private void btRedis_Click(object sender, EventArgs e)
        {
            new Redis().Show();
        }
    }
}
