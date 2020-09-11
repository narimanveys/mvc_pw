using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWApp.ViewModels
{
    public class TransactionViewModel
    {
        [Required(ErrorMessage = "Creditor Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CreditorEmail { get; set; }

        [Required(ErrorMessage = "Amount Required")]
        [DataType(DataType.Currency)]
        [Range(0, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
    }
}