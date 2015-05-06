using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents possible transaction types for an account.
    /// </summary>
    public enum AccountTransactionType
    {
        Debit,
        Credit
    }
}