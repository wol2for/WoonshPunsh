namespace woontest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Comments = new HashSet<Comment>();
            PictureProducts = new HashSet<PictureProduct>();
        }

        public int Id { get; set; }

        public int IdCategory { get; set; }

        [Required]
        [StringLength(60)]
        public string NameProduct { get; set; }

        public decimal PriceProduct { get; set; }

        [Required]
        public string DescriptionProduct { get; set; }

        public int? PictureProduct { get; set; }

        public DateTime DateProduct { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PictureProduct> PictureProducts { get; set; }
    }
}
