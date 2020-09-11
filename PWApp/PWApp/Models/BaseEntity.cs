using System.ComponentModel.DataAnnotations;

namespace PWApp.Models
{
    public class BaseEntity
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        #endregion
    }
}