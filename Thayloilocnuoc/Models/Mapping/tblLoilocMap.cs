using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Thayloilocnuoc.Models.Mapping
{
    public class tblLoilocMap : EntityTypeConfiguration<tblLoiloc>
    {
        public tblLoilocMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(200);

            this.Property(t => t.Age)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tblLoiloc");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.Ord).HasColumnName("Ord");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
