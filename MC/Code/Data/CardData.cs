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
        public CardData()
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
                LogHelper.Log("CardItem-CreateOrUpdate:" + "File ERROR");
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
                        LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
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
                        LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
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
                LogHelper.Log("CardItem-CreateOrUpdate:" + "File ERROR");
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
                        LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
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
                        LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("CardItem-CreateOrUpdate:" + ex.ToString());
            }
            finally
            {

                LockUtils.LockExit(_ConnectionString);
            }
            return 0;
        }
    }
}