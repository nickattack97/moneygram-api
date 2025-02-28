using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using moneygram_api.Models;

namespace moneygram_api.Data
{
    public class SSODbContext : DbContext
    {
        public SSODbContext(DbContextOptions<SSODbContext> options)
            : base(options)
        {
        }

        public DbSet<User> tblUsers { get; set; }
    }

    [Table("tblUsers", Schema = "dbo")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string Surname { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? AvayaExt { get; set; }

        [StringLength(255)]
        public string? FlexCudeId { get; set; }

        public long? DepartmentId { get; set; }
    }
}