using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Interfaces
{
    public interface IRecordManager<T>
    {
        T GetRecordByID(Guid id);
        IEnumerable<T> GetRecords();
        void SaveRecord(T record);
        void DeleteRecord(T record);
    }
}
