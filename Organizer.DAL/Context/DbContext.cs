using Organizer.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Organizer.DAL.Context
{
    public class DbContext : IDbContext
    {
        private readonly IDbConnection _connection;
        private IUnitOfWork _currentTransaction;
        private bool _inTransaction;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private DataSet _users;
        private DataSet _notes;
        private DataSet _personalInfos;
        private DataSet _contacts;
        private DataSet _meetings;

        public IUnitOfWork CurrentTransaction => _currentTransaction;

        public DataSet Users
        {
            get
            {
                if (_users == null)
                    FetchUsers();
                return _users;
            }

            set
            {
                _users = value;
            }
        }

        public DataSet Notes
        {
            get
            {
                if (_notes == null)
                    FetchNotes();
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        public DataSet PersonalInfos
        {
            get
            {
                if (_personalInfos == null)
                    FetchPersonalInfo();
                return _personalInfos;
            }

            set
            {
                _personalInfos = value;
            }
        }

        public DataSet Contacts
        {
            get
            {
                if (_contacts == null)
                    FetchContacts();
                return _contacts;
            }

            set
            {
                _contacts = value;
            }
        }

        public DataSet Meetings
        {
            get
            {
                if (_meetings == null)
                    FetchMeetings();
                return _meetings;
            }
            set
            {
                _meetings = value;
            }
        }

        public bool InTransaction
        {
            get { return _inTransaction; }
            private set { _inTransaction = value; }
        }

        public DbContext(IDbConnection connection)
        {
            _connection = connection;
        }

        public IUnitOfWork BeginTransaction()
        {
            if (!InTransaction)
            {
                _lock.EnterWriteLock();
                var transaction = _connection.BeginTransaction();
                _currentTransaction = new UnitOfWork(transaction, RemoveTransaction, RemoveTransaction);
                _lock.ExitWriteLock();

                InTransaction = true;
                return _currentTransaction;
            }
            throw new Exception("Transaction cannot be started because another transaction didn't finish executing.");
        }

        public DataSet Set(string setName)
        {
            DataSet result = null;

            switch (setName)
            {
                case nameof(Users):
                    result = _users;
                    break;

                case nameof(Notes):
                    result = _notes;
                    break;

                case nameof(PersonalInfos):
                    result = _personalInfos;
                    break;

                case nameof(Contacts):
                    result = _contacts;
                    break;

                case nameof(Meetings):
                    result = _meetings;
                    break;
            }

            return result;
        }

        private void FetchUsers()
        {
            var sqlAdapter = new SqlDataAdapter("SELECT * FROM dbo.Users", (SqlConnection)_connection);
            if (_users == null)
                _users = new DataSet();
            sqlAdapter.Fill(_users);
        }

        private void FetchNotes()
        {
            var sqlAdapter = new SqlDataAdapter("SELECT * FROM dbo.Notes", (SqlConnection)_connection);
            if (_contacts == null)
                _notes = new DataSet();
            sqlAdapter.Fill(_notes);
        }

        private void FetchPersonalInfo()
        {
            var sqlAdapter = new SqlDataAdapter("SELECT * FROM dbo.PersonalInfo", (SqlConnection)_connection);
            if (_personalInfos == null)
                _personalInfos = new DataSet();
            sqlAdapter.Fill(_personalInfos);
        }

        private void FetchContacts()
        {
            var sqlAdapter = new SqlDataAdapter("SELECT * FROM dbo.Contacts", (SqlConnection)_connection);
            if (_contacts == null)
                _contacts = new DataSet();
            sqlAdapter.Fill(_contacts);
        }

        private void FetchMeetings()
        {
            var sqlAdapter = new SqlDataAdapter("SELECT * FROM dbo.Meetings", (SqlConnection)_connection);
            if (_meetings == null)
                _meetings = new DataSet();
            sqlAdapter.Fill(_meetings);
        }

        private void RemoveTransaction(IUnitOfWork obj)
        {
            _lock.EnterWriteLock();
            InTransaction = false;
            _currentTransaction = null;
            _lock.ExitWriteLock();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _currentTransaction?.Dispose();
                _connection.Dispose();
                _lock.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DbContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}