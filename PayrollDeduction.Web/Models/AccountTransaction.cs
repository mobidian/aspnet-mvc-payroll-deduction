using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a credit or debit transaction on an Account.
    /// </summary>
    public class AccountTransaction
    {
        public AccountTransaction()
        {
            CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Column]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Column, Required]
        public int TypeId { get; set; }

        public AccountTransactionType Type
        {
            get { return (AccountTransactionType)TypeId; }
            set { TypeId = (int)value; }
        }

        [Column, Required]
        public string Note { get; set; }

        [Column("Amount"), Required]
        private double _Amount { get; set; }

        [Column]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount. Values are made absolute when
        /// set and given a sign (depending on the transaction type) on get.
        /// </summary>
        public double Amount
        {
            get { return Type == AccountTransactionType.Credit ? _Amount * (-1) : _Amount; }
            set { _Amount = Math.Abs(value); }
        }
    }
}