using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KairosTest.Helpers
{
    public class SafeTx : IDisposable
    {

        public string szProcessName { get; set; }
        private bool disposed = false;
        private Component component = new Component();
        private IntPtr handle;
        public SqlConnection connection { get; set; }
        public SqlCommand sqlCommand { get; set; }
        public SqlTransaction transaction { get; set; }
        public IConfiguration Configuration { get; }

        public SafeTx(IConfiguration configuration)
        {
            Configuration = configuration;
            Console.WriteLine(szProcessName);
            //ganti disini sebelum release
            connection = new SqlConnection(Configuration["ConnectionStrings:DefaultConnection"]);
            //connection = new SqlConnection(Configuration["ConnectionStrings:DevConnection"]);
            transaction = null;
            connection.Open();
        }
        public void BeginTrans()
        {
            transaction = connection.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public SqlCommand GetCommand()
        {
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;

            if (transaction != null)
                sqlCommand.Transaction = transaction;

            return sqlCommand;
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();

            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~SafeTx()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
    }
}
