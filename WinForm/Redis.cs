using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using RedisClientLib;
using Model;
using Infrastructure.Utility;

namespace WinForm
{
    public partial class Redis : Form
    {
        public Redis()
        {
            InitializeComponent();
            this.Load += Redis_Load;
        }

        async void Redis_Load(object sender, EventArgs e)
        {
            await InitializeControl();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <returns></returns>
        async Task InitializeControl()
        {
            var userList = await GetAllSubscribeAsync();
            cbUserList.DisplayMember = "Name";
            cbUserList.ValueMember = "Id";
            cbUserList.DataSource = userList.ToList();
            cbUserList.SelectedIndex = 0;
        }

        /// <summary>
        /// 获取所有订阅者
        /// </summary>
        /// <returns></returns>
        async Task<IEnumerable<RedisSubscriber>> GetAllSubscribeAsync()
        {
            return await PubSubRedis.Instance.GetAllSubscribeAsync();
        }

        /// <summary>
        /// 获取增量数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConn_Click(object sender, EventArgs e)
        {
            var user = (RedisSubscriber)this.cbUserList.SelectedItem;
            var model = await PubSubRedis.Instance.FindSubscribeAsync(user.Id);


            var ChangeDatas = await PubSubRedis.Instance.GetChangeDatasAsync(user.Id);
            this.dataGridView1.DataSource = ChangeDatas.Select(item => JsonSerializer.Deserialize<TDemoTable>(item.Data.ToString())).ToList();

            //有新订阅消息回调
            await PubSubRedis.Instance.GetSubscriber(user.Id, (ch, msg) =>
            {
                this.Invoke(new Action(() => rtbLog.AppendText(DateTime.Now + msg + "\r\n")));
            });
        }

    }
}
