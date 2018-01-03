using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm
{
    public class Config
    {
        #region 公有方法
        private static int GetNewId()
        {
            var now = DateTime.Now;
            return now.Millisecond + now.Second + now.Minute + now.Hour + now.Month + now.Year;
        }

        /// <summary>
        /// 创建测试模型
        /// </summary>
        /// <returns></returns>
        public static TDemoTable GetDemoModel()
        {
            var now = DateTime.Now;
            var model = new TDemoTable()
            {
                F_Guid = Guid.NewGuid().ToString(),
                F_Bool = true,
                F_DateTime = now,
                F_Float = 0.01f,
                Id = GetNewId(),
                F_Int = 1,
                F_IntNull = -1,
                F_String = "String",
                CreateTime = now
            };
            return model;
        }
        #endregion

    }
}
