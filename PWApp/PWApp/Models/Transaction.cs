using System;
using System.ComponentModel.DataAnnotations;

namespace PWApp.Models
{
    public class Transaction : BaseEntity
    {
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Amount { get; set; }

        public UserAccount Creditor { get; set; }

        public UserAccount Debtor { get; set; }

        public decimal CreditorResultingBalance { get; set; }
        public decimal DebtorResultingBalance { get; set; }

    }
}