namespace woontest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PictureProduct")]
    public partial class PictureProduct
    {
        public int Id { get; set; }

        public int? IdProduct { get; set; }

        public byte[] Picture { get; set; }

        [StringLength(10)]
        public string MimeTypeProduct { get; set; }

        public virtual Product Product { get; set; }
    }
}
