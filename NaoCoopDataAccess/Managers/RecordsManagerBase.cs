using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using NaoCoopDataAccess.Interfaces;

namespace NaoCoopDataAccess.Managers
{
    public abstract class RecordsManagerBase<T> : ManagerBase, IRecordManager<T>
        where T : NaoCoopObjects.Classes.NaoCoopObject  // NaoCoop object
    {
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public RecordsManagerBase(string connectionString)
            : base(connectionString)
        {
        }

        #region Virtual
        /// <summary>
        /// Gets the db record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual NaoCoopObjects.Interfaces.INaoCoopObject GetDbRecordByID(Guid id, bool throwExceptionIfNotFound = true)
        {
            foreach (NaoCoopObjects.Interfaces.INaoCoopObject record in Records)
            {
                if (record.ID.Equals(id))
                {
                    return record;
                }
            }
            if (!throwExceptionIfNotFound)
            {
                return null;
            }
            else
            {
                throw new Exception(string.Format("Record with ID '{0}' not found in table '{1}'.", id, Records.ToString()));
            }
        }

        /// <summary>
        /// Gets the nao object from the database by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetRecordByID(Guid id)
        {
            var dbObj = GetDbRecordByID(id);
            return (T)ConvertDbObjToNaoObj(dbObj, typeof(T));
        }

        /// <summary>
        /// Gets all nao objects from the database
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetRecords()
        {
            ResetContext();
            foreach (var dbRecord in Records)
            {
                T naoObject = (T)Activator.CreateInstance(typeof(T), ((NaoCoopObjects.Interfaces.INaoCoopObject)dbRecord).ID);
                CopyDbObjectToNaoObject(dbRecord, naoObject);
                yield return naoObject;
            }
        }

        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="record"></param>
        public virtual void DeleteRecord(T record)
        {
            if (record == null)
            {
                return;
            }

            var dbObj = GetDbRecordByID(record.ID, throwExceptionIfNotFound: false);
            if (dbObj != null)
            {
                Records.DeleteOnSubmit(dbObj);
                Records.Context.SubmitChanges();
            }
        }

        /// <summary>
        /// Saves the record in the database
        /// </summary>
        /// <param name="record"></param>
        public virtual void SaveRecord(T record)
        {
            SaveRecord(record, null);
        }

        internal virtual void SaveRecord(T record, params object[] parentRecords)
        {
            if (record == null)
            {
                return;
            }

            // check if objects exist in the database
            var dbObj = GetDbRecordByID(record.ID, throwExceptionIfNotFound: false);
            if (dbObj == null)
            {
                dbObj = (NaoCoopObjects.Interfaces.INaoCoopObject)Activator.CreateInstance(Records.ElementType);
                Records.InsertOnSubmit(dbObj);
            }
            // copy nao coop member values to db object
            CopyNaoObjectToDbObject(record, dbObj);
            if (parentRecords != null)
            {
                foreach (var parentRecord in parentRecords)
                {
                    // set the parent record id
                    var destinationProperty = dbObj.GetType().GetProperty(string.Format("FK_{0}ID", parentRecord.GetType().Name));
                    if (destinationProperty != null)
                    {
                        destinationProperty.SetValue(dbObj, ((NaoCoopObjects.Interfaces.INaoCoopObject)parentRecord).ID, null);
                    }
                }
            }

            Records.Context.SubmitChanges();
        }
        #endregion

        #region Abstract
        /// <summary>
        /// Gets the records table
        /// </summary>
        protected abstract ITable Records
        {
            get;
        }
        #endregion

        #region DB TO NAO
        /// <summary>
        /// Creates a Nao Object from a Db Object
        /// </summary>
        /// <param name="dbObj"></param>
        /// <param name="naoObjType"></param>
        /// <returns></returns>
        protected static object ConvertDbObjToNaoObj(NaoCoopObjects.Interfaces.INaoCoopObject dbObj, Type naoObjType)
        {
            if (dbObj == null)
            {
                return null;
            }

            var naoObject = Activator.CreateInstance(naoObjType, dbObj.ID);

            CopyDbObjectToNaoObject(dbObj, naoObject);

            return naoObject;
        }

        /// <summary>
        /// Copies the db object members to nao object members
        /// </summary>
        /// <param name="source">db object</param>
        /// <param name="destination">nao object</param>
        protected static void CopyDbObjectToNaoObject(object source, object destination)
        {
            var properties = destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.Name != "ID") // id property of nao objects are not settable
                {
                    var value = source.GetType().GetProperty(property.Name).GetValue(source, null);
                    var argType = default(Type);
                    if (property.PropertyType.GetInterfaces().Contains(typeof(NaoCoopObjects.Interfaces.INaoCoopObject)))
                    {// nao coop element
                        // get destination type
                        property.SetValue(destination, ConvertDbObjToNaoObj((NaoCoopObjects.Interfaces.INaoCoopObject)value, property.PropertyType), null);
                    }
                    else if (property.PropertyType.IsGenericType && 
                            (argType = property.PropertyType.GetGenericArguments()[0]).GetInterfaces().Contains(typeof(NaoCoopObjects.Interfaces.INaoCoopObject)))
                    {// list of nao coop elements
                        // list of nao coop objects are always created but sometimes empty
                        // get the actual list and clear it before adding new elements
                        var destinationList = destination.GetType().GetProperty(property.Name).GetValue(destination, null) as IList;
                        if (destinationList != null && argType != null)
                        {
                            foreach (var dbObj in (IEnumerable)value)
                            {
                                destinationList.Add(ConvertDbObjToNaoObj((NaoCoopObjects.Interfaces.INaoCoopObject)dbObj, argType));
                            }
                        }
                    }
                    else
                    {
                        property.SetValue(destination, value, null);
                    }
                }
            }
        }
        #endregion

        #region NAO TO DB
        protected static void CopyNaoObjectToDbObject(object source, object destination)
        {
            // get the properties of the Nao Object, the Db Object has more properties which we don't want to set
            var properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(source, null);
                var argType = default(Type);
                if (property.PropertyType.GetInterfaces().Contains(typeof(NaoCoopObjects.Interfaces.INaoCoopObject)))
                {// nao coop element
                    // set the FK
                    var destinationProperty = destination.GetType().GetProperty(string.Format("FK_{0}ID", property.Name));
                    if (destinationProperty != null)
                    {
                        Guid? destinationPropertyValue = value != null ? ((NaoCoopObjects.Interfaces.INaoCoopObject)value).ID : default(Guid?);
                        destinationProperty.SetValue(destination, destinationPropertyValue, null);
                    }
                }
                else if (property.PropertyType.IsGenericType &&
                        (argType = property.PropertyType.GetGenericArguments()[0]).GetInterfaces().Contains(typeof(NaoCoopObjects.Interfaces.INaoCoopObject)))
                {// nothing to do for lists, they will be saved in higher level
                }
                else
                {
                    destination.GetType().GetProperty(property.Name).SetValue(destination, value, null);
                }
            }
        }
        #endregion

    }
}
