using System;
using System.ComponentModel.DataAnnotations;

namespace PWApp.ViewModels
{
    public class TransactionHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string CorrespondentName { get; set; }
        public decimal ResultingBalance { get; set; }
    }
}