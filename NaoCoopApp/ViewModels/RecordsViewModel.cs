using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NaoCoopApp.Helpers;
using NaoCoopDataAccess.Interfaces;

namespace NaoCoopApp.ViewModels
{
    public abstract class RecordsViewModel<T, K> : BaseViewModel
        where T : ModelBase<K>
        where K : NaoCoopObjects.Classes.NaoCoopObject
    {
        #region Commands
        public ICommand RefreshDataCommand { get { return new DelegateCommand(OnRefreshData); } }
        public ICommand SaveDataCommand { get { return new DelegateCommand(OnSaveData); } }
        #endregion

        #region Constructor
        public RecordsViewModel()
        {
            // initialize required properties
            PendingDeleteRecords = new List<T>();
            RecordsManager = DataAccessHelper.Instance.GetRecordsManager<K>();
        }
        #endregion


        #region Properties
        private ObservableCollection<T> _recordsCollection;
        /// <summary>
        /// Gets the records collection that should be binded to the UI
        /// </summary>
        public ObservableCollection<T> RecordsCollection
        {
            get { return _recordsCollection; }
            set
            {
                if (_recordsCollection != value)
                {
                    _recordsCollection = value;
                    OnPropertyChanged(() => RecordsCollection);
                }
            }
        }


        /// <summary>
        /// Gets the database manager for this type of records
        /// </summary>
        public IRecordManager<K> RecordsManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of pending delete records
        /// </summary>
        private List<T> PendingDeleteRecords
        {
            get;
            set;
        }
        #endregion


        #region Methods
        #region Virtual
        /// <summary>
        /// Refreshed the data
        /// </summary>
        protected virtual void OnRefreshData()
        {
            // get all records from the database
            var records = RecordsManager.GetRecords();
            // add them to the observable collection
            RecordsCollection = this.InitializeRecordsCollection<T, K>(RecordsCollection, records, RecordsCollection_CollectionChanged);
        }

        /// <summary>
        /// Saves the data
        /// </summary>
        protected virtual void OnSaveData()
        {
            if (RecordsCollection == null)
            {
                return;
            }

            #region Delete Pending Records
            // delete pending delete records
            foreach (var record in PendingDeleteRecords)
            {
                RecordsManager.DeleteRecord(record.Data);
            }
            PendingDeleteRecords.Clear();
            #endregion

            #region Save Existing Records
            foreach (var record in RecordsCollection)
            {
                RecordsManager.SaveRecord(record.Data);
            }
            #endregion
        }
        #endregion

        #region Collection Changed
        void RecordsCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    // add pending delete items
                    PendingDeleteRecords.Add((T)oldItem);
                }
            }
        }
        #endregion

        /// <summary>
        /// Initializes the specified records collection using the specified enumeration.
        /// </summary>
        /// <typeparam name="X">The model type of the collection</typeparam>
        /// <typeparam name="Y">The nao object type of the enumeration</typeparam>
        /// <param name="collection">The collection that should be initialized</param>
        /// <param name="records">The enumeration that should be copied to the collection</param>
        /// <param name="eventHandler">The collection changed event handler</param>
        /// <returns></returns>
        protected ObservableCollection<X> InitializeRecordsCollection<X, Y>(ObservableCollection<X> collection, IEnumerable<Y> records, NotifyCollectionChangedEventHandler eventHandler = null)
            where X : ModelBase<Y>
            where Y : NaoCoopObjects.Classes.NaoCoopObject
        {
            if (collection != null && eventHandler != null)
            {
                collection.CollectionChanged -= eventHandler;
            }
            collection = new ObservableCollection<X>(records.Select(r => (X)Activator.CreateInstance(typeof(X), r)));
            if (eventHandler != null)
            {
                collection.CollectionChanged += eventHandler;
            }
            return collection;
        }
        #endregion
    }
}
