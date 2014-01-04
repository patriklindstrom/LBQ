using System;
using System.Security.Cryptography.X509Certificates;
using LBQ.Katana.Model;

namespace LBQ.Katana
{
    public interface ILogFilterRepo
    {
        ILogFilter GetData(DateTime fromTime, DateTime toTime);
    }
}