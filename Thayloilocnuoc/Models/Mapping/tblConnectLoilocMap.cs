using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Thayloilocnuoc.Models.Mapping
{
    public class tblConnectLoilocMap : EntityTypeConfiguration<tblConnectLoiloc>
    {
        public tblConnectLoilocMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("tblConnectLoiloc");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.idkh).HasColumnName("idkh");
            this.Property(t => t.idll).HasColumnName("idll");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.DateTime).HasColumnName("DateTime");
        }
    }
}
