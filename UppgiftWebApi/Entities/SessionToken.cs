using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace UppgiftWebApi.Entities
{
    public partial class SessionToken
    {

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdminId { get; set; }

        [Required]
        [Column(TypeName = "varbinary(max)")]
        public string AccessToken { get; set; }
    }
}
