using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MC.Code.Data
{
    public class CardData : BaseData
    {
        static ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        public CardData()
        {
            string sqlitefile = base.GetSqliteFile("cardshare.dll", "cardshare001.config");
            if (!string.IsNullOrEmpty(sqlitefile))
            {
                this._ConnectionString = this.GetSQLiteConnectionString(sqlitefile);
            }
        }
    }
}