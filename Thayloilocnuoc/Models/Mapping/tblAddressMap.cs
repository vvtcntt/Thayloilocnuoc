using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Thayloilocnuoc.Models.Mapping
{
    public class tblAddressMap : EntityTypeConfiguration<tblAddress>
    {
        public tblAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.Mobile)
                .HasMaxLength(100);

            this.Property(t => t.Hotline)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tblAddress");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idCate).HasColumnName("idCate");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Hotline).HasColumnName("Hotline");
            this.Property(t => t.Maps).HasColumnName("Maps");
            this.Property(t => t.Ord).HasColumnName("Ord");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.idUser).HasColumnName("idUser");
        }
    }
}
