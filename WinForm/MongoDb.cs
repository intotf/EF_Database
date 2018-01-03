using Model;
using MongoServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Utility;

namespace WinForm
{
    public partial class MongoDb : Form
    {

        private static MongoTableType tableType = MongoTableType.Table;

        public MongoDb()
        {
            InitializeComponent();
            this.Load += MongoDb_Load;
        }

        async void MongoDb_Load(object sender, EventArgs e)
        {
            await InitializeData();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<TDemoTable>> GetList()
        {
            var data = new MongoDbBase().TDemoTable().Where(item => true).ToList();
            return data;
        }

        /// <summary>
        /// 初始化表格数据
        /// </summary>
        /// <returns></returns>
        private async Task InitializeData()
        {
            tableType = MongoTableType.Table;
            var data = await GetList();
            this.dataGridView1.DataSource = data;
        }

        /// <summary>
        /// 读取月数据
        /// </summary>
        /// <returns></returns>
        private async Task InitializeDataMonth()
        {
            tableType = MongoTableType.Month;
            this.dataGridView1.DataSource = new MongoDbBase().TDemoTableMonth(DateTime.Now).Where(item => true).ToList();
        }

        /// <summary>
        /// 读取年数据
        /// </summary>
        /// <returns></returns>
        private async Task InitializeDataYear()
        {
            tableType = MongoTableType.Year;
            this.dataGridView1.DataSource = new MongoDbBase().TDemoTableYear(DateTime.Now).Where(item => true).ToList();
        }



        private async void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var model = GetDemoModel();
            await new MongoDbBase().TDemoTable().AddAsync(model);
            await InitializeData();
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            var f_id = dataGridView1.Rows[a].Cells["Id"].Value;
            var id = int.Parse(f_id.ToString());
            await Remove(id);
        }


        private async void 添加月ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var model = GetDemoModel();
            await AddMonth(model);
            await InitializeDataMonth();
        }


        #region 添加操作
        /// <summary>
        /// 保存数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Add(TDemoTable model)
        {
            await new MongoDbBase().TDemoTable().AddAsync(model);
            MessageBox.Show(JsonSerializer.Serialize(model));
            await InitializeData();
        }

        /// <summary>
        /// 保存月数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task AddMonth(TDemoTable model)
        {
            await new MongoDbBase().TDemoTableMonth(DateTime.Now).AddAsync(model);
            MessageBox.Show(JsonSerializer.Serialize(model));
            await InitializeDataMonth();
        }

        /// <summary>
        /// 保存年数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task AddYear(TDemoTable model)
        {
            await new MongoDbBase().TDemoTableYear(DateTime.Now).AddAsync(model);
            MessageBox.Show(JsonSerializer.Serialize(model));
            await InitializeDataYear();
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Remove(int id)
        {
            await new MongoDbBase().TDemoTable().RemoveAsync(item => item.Id == id);
            MessageBox.Show("删除数据:id" + id);
            await InitializeData();
        }

        /// <summary>
        /// 删除月数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task RemoveMonth(int id)
        {
            await new MongoDbBase().TDemoTableMonth(DateTime.Now).RemoveAsync(item => item.Id == id);
            MessageBox.Show("删除月数据:id" + id);
            await InitializeDataMonth();
        }

        /// <summary>
        /// 删除年数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task RemoveYear(int id)
        {
            await new MongoDbBase().TDemoTableYear(DateTime.Now).RemoveAsync(item => item.Id == id);
            MessageBox.Show("删除年数据:id" + id);
            await InitializeDataYear();
        }
        #endregion

        #region 公有方法

        /// <summary>
        /// 创建测试模型
        /// </summary>
        /// <returns></returns>
        private TDemoTable GetDemoModel()
        {
            return Config.GetDemoModel();
        }
        #endregion

        private async void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            var f_id = dataGridView1.Rows[a].Cells["Id"].Value;
            var id = int.Parse(f_id.ToString());
            await RemoveMonth(id);
        }

        private async void 添加ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var model = GetDemoModel();
            await AddYear(model);
            await InitializeDataYear();
        }

        private async void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            var f_id = dataGridView1.Rows[a].Cells["Id"].Value;
            var id = int.Parse(f_id.ToString());
            await RemoveYear(id);
        }

        private async void 同一个表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableType != MongoTableType.Table)
                await InitializeData();
        }

        private async void 按月分表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableType != MongoTableType.Month)
                await InitializeDataMonth();
        }

        private async void 按年分表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableType != MongoTableType.Year)
                await InitializeDataYear();
        }
    }
}
