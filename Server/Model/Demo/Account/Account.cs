using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum AccountType
    {
        General = 0, //普通账号
        BlackList = 1, //黑名单
    }
    public class Account:Entity, IAwake
    {
        public string AccountName;  //账号名
        public string Password; //密码
        public long CreateTime;  //创建时间
        public AccountType AccountType; //账号类型
    }
}
