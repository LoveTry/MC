using System;

namespace MCComm
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DBColumnAttribute : Attribute
    {
        public DBColumnAttribute()
        {
        }
    }
}