using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("T_DemoTable", Schema = "public")]
    public class NpgSqlTable : TDemoTable
    {
       
    }
}

