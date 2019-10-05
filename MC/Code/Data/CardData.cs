using MC.Comm;
using MC.Models.sqllite;
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
        public CardData(string openid) : base(openid)
        {
            string sqlitefile = base.GetSqliteFile("cardshare.dll", "cardshare001.config");
            if (!string.IsNullOrEmpty(sqlitefile))
            {
                this._ConnectionString = this.GetSQLiteConnectionString(sqlitefile);
            }
        }

        /// <summary>
        /// 创建或者更新客户资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long CreateOrUpdate(CardItem model)
        {
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                LogHelper.Log("CardData-CreateOrUpdate:" + "File ERROR");
                return 0;
            }
            try
            {
                bool isExist = model.id > 0;
                LockUtils.LockEnter(_ConnectionString);
                DapperHelper db = new DapperHelper(this._ConnectionString);
                if (isExist)
                {
                    //仅仅更新
                    try
                    {
                        var param = ConvertHelper<CardItem>.ModelToDic(model);
                        db.Update<CardItem>(param, "id", model.id);
                        return model.id;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
                else
                {
                    //新增
                    try
                    {
                        return CheckData.Check_Long(db.ExecuteScalar(DapperHelper.CompileInsert<CardItem>(model, "id", "sl"), model));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
            }
            finally
            {

                LockUtils.LockExit(_ConnectionString);
            }
            return 0;
        }


        /// <summary>
        /// 创建或者更新客户资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long CreateOrUpdate(CardSubItem model)
        {
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                LogHelper.Log("CardData-CreateOrUpdate:" + "File ERROR");
                return 0;
            }
            try
            {
                bool isExist = model.id > 0;
                LockUtils.LockEnter(_ConnectionString);
                DapperHelper db = new DapperHelper(this._ConnectionString);
                if (isExist)
                {
                    //仅仅更新
                    try
                    {
                        var param = ConvertHelper<CardSubItem>.ModelToDic(model);
                        db.Update<CardSubItem>(param, "id", model.id);
                        return model.id;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
                else
                {
                    //新增
                    try
                    {
                        return CheckData.Check_Long(db.ExecuteScalar(DapperHelper.CompileInsert<CardSubItem>(model, "id", "sl"), model));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardData-CreateOrUpdate:" + ex.ToString());
            }
            finally
            {

                LockUtils.LockExit(_ConnectionString);
            }
            return 0;
        }

        /// <summary>
        /// 获取卡片主表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CardItem GetCardItem(long id)
        {
            CardItem item = null;
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                LogHelper.Log("CardData-GetCardItem:" + "File ERROR");
                return item;
            }
            try
            {
                LockUtils.LockEnter(_ConnectionString);
                DapperHelper db = new DapperHelper(this._ConnectionString);
                string sql = $"select * from carditem where id=@id";
                item = db.QueryFirst<CardItem>(sql, new { id });
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardData-GetCardItem:" + ex.ToString());
            }
            finally
            {
                LockUtils.LockExit(_ConnectionString);
            }
            return item;
        }

        public List<CardSubItem> GetCardSubItemList(long cardid)
        {
            List<CardSubItem> list = null;
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                LogHelper.Log("CardData-GetCardSubItemList:" + "File ERROR");
                return list;
            }
            try
            {
                LockUtils.LockEnter(_ConnectionString);
                DapperHelper db = new DapperHelper(this._ConnectionString);
                string sql = $"select * from cardsubitem where cardid=@cardid";
                list = db.QueryList<CardSubItem>(sql, new { cardid });
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardData-GetCardSubItemList:" + ex.ToString());
            }
            finally
            {
                LockUtils.LockExit(_ConnectionString);
            }
            return list;
        }
    }
}