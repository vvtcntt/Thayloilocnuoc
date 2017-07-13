using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Thayloilocnuoc.Models.Mapping
{
    public class tblGroupProductMap : EntityTypeConfiguration<tblGroupProduct>
    {
        public tblGroupProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(120);

            this.Property(t => t.Title)
                .HasMaxLength(120);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.Keyword)
                .HasMaxLength(300);

            this.Property(t => t.Tag)
                .HasMaxLength(300);

            this.Property(t => t.Level)
                .HasMaxLength(50);

            this.Property(t => t.Images)
                .HasMaxLength(300);

            this.Property(t => t.Favicon)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("tblGroupProduct");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.idManu).HasColumnName("idManu");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Keyword).HasColumnName("Keyword");
            this.Property(t => t.Ord).HasColumnName("Ord");
            this.Property(t => t.Tag).HasColumnName("Tag");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Index).HasColumnName("Index");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Baogia).HasColumnName("Baogia");
            this.Property(t => t.Images).HasColumnName("Images");
            this.Property(t => t.Favicon).HasColumnName("Favicon");
            this.Property(t => t.DateCreate).HasColumnName("DateCreate");
            this.Property(t => t.idUser).HasColumnName("idUser");
        }
    }
}
