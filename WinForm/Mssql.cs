using Model;
using SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Utility;
using Command;

namespace WinForm
{
    public partial class Mssql : Form
    {
        public Mssql()
        {
            InitializeComponent();
            this.Load += Mssql_Load;
        }

        async void Mssql_Load(object sender, EventArgs e)
        {
            await InitializeData();
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<TDemoTable>> GetList()
        {
            using (var db = new SqlDb())
            {
                var data = await db.TDemoTable.Where(item => true).ToListAsync();
                return data;
            }
        }

        /// <summary>
        /// 初始化表格数据
        /// </summary>
        /// <returns></returns>
        private async Task InitializeData()
        {
            this.dataGridView1.DataSource = await GetList();
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Add(TDemoTable model)
        {
            using (var db = new SqlDb())
            {
                db.TDemoTable.Add(model);
                await db.SaveChangesAsync();
                MessageBox.Show(JsonSerializer.Serialize(model));
                await InitializeData();
            }
        }

        /// <summary>
        /// 菜单添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var model = Config.GetDemoModel();
            await Add(model);
            await InitializeData();
        }

        /// <summary>
        /// 菜单删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            var f_id = dataGridView1.Rows[a].Cells["Id"].Value;
            var id = int.Parse(f_id.ToString());

            using (var db = new SqlDb())
            {
                await db.TDemoTable.RemoveById(id);
                await db.SaveChangesAsync();
            }
            MessageBox.Show("删除id:" + id + "成功.");
            await InitializeData();
        }

    }
}
