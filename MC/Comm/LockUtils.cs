using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace MC.Comm
{
    public class LockUtils
    {
        private const string key_prefix = "LockObj_";
        /// <summary>
        /// 缓存锁锁定对象
        /// </summary>
        private static readonly object CacheLock = new object();
        /// <summary>
        /// 读写锁锁定对象
        /// </summary>
        private static readonly object RWCacheLock = new object();
        /// <summary>
        /// 获取缓存锁[5分钟弹性过期]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetLockObject(string key)
        {
            key = key_prefix + key;
            object obj = WebCache.Get(key);
            if (obj == null)
            {
                lock (CacheLock)
                {
                    obj = WebCache.Get(key);
                    if (obj == null)
                    {
                        obj = new object();
                        WebCache.Insert(key, obj, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
                    }
                }
            }
            return obj;
        }
        /// <summary>
        /// 获取都写锁[5分钟弹性过期]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetRWLock(string key)
        {
            key = key_prefix + key;
            ReaderWriterLockSlim obj = WebCache.Get(key) as ReaderWriterLockSlim;
            if (obj == null)
            {
                lock (RWCacheLock)
                {
                    obj = WebCache.Get(key) as ReaderWriterLockSlim;
                    if (obj == null)
                    {
                        obj = new ReaderWriterLockSlim();
                        WebCache.Insert(key, obj, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
                    }
                }
            }
            return obj;
        }
        /// <summary>
        /// 获取都写锁[永不过期]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetMaxRWLock(string key)
        {
            key = key_prefix + key;
            ReaderWriterLockSlim obj = WebCache.Get(key) as ReaderWriterLockSlim;
            if (obj == null)
            {
                lock (RWCacheLock)
                {
                    obj = WebCache.Get(key) as ReaderWriterLockSlim;
                    if (obj == null)
                    {
                        obj = new ReaderWriterLockSlim();
                        WebCache.Max(key, obj);
                    }
                }
            }
            return obj;
        }

        #region 缓存锁
        public static void LockEnter(string key)
        {
            object obj = GetLockObject(key);
            if (obj != null)
            {
                Monitor.Enter(obj);
            }
        }
        public static void LockExit(string key)
        {
            object obj = GetLockObject(key);
            if (obj != null)
            {
                Monitor.Exit(obj);
            }
        }
        public static bool LockTryEnter(string key)
        {
            object obj = GetLockObject(key);
            bool r = Monitor.TryEnter(obj, -1);
            if (r)
            {
                Monitor.Exit(obj);
            }
            return r;
        }
        public static void LockEnter(object obj)
        {
            if (obj == null) return;
            Monitor.Enter(obj);
        }
        public static void LockExit(object obj)
        {
            if (obj == null) return;
            Monitor.Exit(obj);
        }
        public static bool LockTryEnter(object obj)
        {
            if (obj == null) return false;
            bool r = Monitor.TryEnter(obj, -1);
            if (r)
            {
                Monitor.Exit(obj);
            }
            return r;
        }
        #endregion



    }
}