using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customer.Project.DataAccess.PersistenceInterfaces;

namespace InMemoryData.PersistenceEmulators
{
    /// <summary>
    /// Empty transactionblock implementation
    /// </summary>
    internal class TransactionBlock : ITransactionBlock
    {

        #region ITransactionBlock Members

        public bool IsValid
        {
            set
            {
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
