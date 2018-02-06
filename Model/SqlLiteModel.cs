using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// SqlLite 模型
    /// 只支持数值型，文本型，浮点，二进制
    /// </summary>
    [Table("T_DemoTable")]
    [Serializable]
    public class SqlLiteModel
    {
        /// <summary>
        /// 获取新id
        /// </summary>
        /// <returns></returns>
        public static int GetNewId()
        {
            var now = DateTime.Now;
            return now.Millisecond + now.Second + now.Minute + now.Hour + now.Month + now.Year;
        }

        /// <summary>
        /// 自增类型
        /// </summary>
        [Column("F_Id")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Guid 类型
        /// </summary>
        [Column("F_Guid")]
        public string F_Guid { get; set; }

        /// <summary>
        /// 字符串
        /// </summary>
        [Column("F_String")]
        public string F_String { get; set; }

        /// <summary>
        /// 可空Int 类型
        /// </summary>
        [Column("F_IntNull")]
        public int? F_IntNull { get; set; }

        /// <summary>
        ///  不可空 Int型 
        /// </summary>
        [Column("F_Int")]
        public int F_Int { get; set; }

        /// <summary>
        /// 可空Float 类型
        /// </summary>
        [Column("F_FloatNull")]
        public double? F_FloatNull { get; set; }

        /// <summary>
        ///  不可空 Float型 
        /// </summary>
        [Column("F_Float")]
        public double F_Float { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        [Column("F_DateTime")]
        public string F_DateTime { get; set; }

        /// <summary>
        /// 可空时间类型
        /// </summary>
        [Column("F_DateTimeNull")]
        public string F_DateTimeNull { get; set; }

        /// <summary>
        /// Bool 型
        /// </summary>
        [Column("F_Bool")]
        public bool F_Bool { get; set; }

        /// <summary>
        /// 可空 Bool 型
        /// </summary>
        [Column("F_BoolNull")]
        public bool? F_BoolNull { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
