namespace woontest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        [Required(ErrorMessage = "Введите своё имя")]
        [StringLength(20)]
        public string NameComment { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите свой почтовый ящик")]
        [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
        [DataType(DataType.EmailAddress)]
        public string MailComment { get; set; }

        [Required(ErrorMessage = "Введите свой отзыв о товаре")]
        [StringLength(500)]
        public string DescriptionComment { get; set; }

        public short Stars { get; set; }

        public virtual Product Product { get; set; }
    }
}
