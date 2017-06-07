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
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private SqlDataAdapter _usersAdapter;
        private SqlDataAdapter _notesAdapter;
        private SqlDataAdapter _infoAdapter;
        private SqlDataAdapter _contactsAdapter;
        private SqlDataAdapter _meetingsAdapter;

        private DataSet _users;
        private DataSet _notes;
        private DataSet _personalInfos;
        private DataSet _contacts;
        private DataSet _meetings;

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

        public DbContext(IDbConnection connection)
        {
            _connection = connection;
        }

        public DataSet Set(string setName)
        {
            DataSet result = null;

            switch (setName)
            {
                case nameof(Users):
                    result = Users;
                    break;

                case nameof(Notes):
                    result = Notes;
                    break;

                case nameof(PersonalInfos):
                    result = PersonalInfos;
                    break;

                case nameof(Contacts):
                    result = Contacts;
                    break;

                case nameof(Meetings):
                    result = Meetings;
                    break;
            }

            return result;
        }

        public void SaveChanges()
        {
            _usersAdapter.Update(Users);
            _contactsAdapter.Update(Contacts);
            _notesAdapter.Update(Notes);
            _meetingsAdapter.Update(Meetings);
            _infoAdapter.Update(PersonalInfos);
        }

        private void FetchUsers()
        {
            _usersAdapter = GetAdapter("SELECT * FROM dbo.Users AS Users");
            ClearOrCreateDataSet(_users);
            _usersAdapter.Fill(_users);
        }

        private void FetchNotes()
        {
            _notesAdapter = GetAdapter("SELECT * FROM dbo.Notes");
            ClearOrCreateDataSet(_notes);
            _notesAdapter.Fill(_notes);
        }

        private void FetchPersonalInfo()
        {
            _infoAdapter = GetAdapter("SELECT * FROM dbo.PersonalInfo");
            ClearOrCreateDataSet(_personalInfos);
            _infoAdapter.Fill(_personalInfos);
        }

        private void FetchContacts()
        {
            _contactsAdapter = GetAdapter("SELECT * FROM dbo.Contacts");
            ClearOrCreateDataSet(_contacts);
            _contactsAdapter.Fill(_contacts);
        }

        private void FetchMeetings()
        {
            _meetingsAdapter = GetAdapter("SELECT * FROM dbo.Meetings");
            ClearOrCreateDataSet(_meetings);
            _meetingsAdapter.Fill(_meetings);
        }

        private void ClearOrCreateDataSet(DataSet target)
        {
            if (target == null)
                target = new DataSet();
            else
            {
                target.Clear();
            }
        }

        private SqlDataAdapter GetAdapter(string query)
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                throw new Exception("Cannot create adapter because connection was closed");
            }
            return new SqlDataAdapter(query, (SqlConnection)_connection);
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