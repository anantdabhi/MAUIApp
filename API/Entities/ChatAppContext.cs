using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChatAppApi.Entities;

public class ChatAppContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseNpgsql("Host=localhost;Database=TblUsers;Username=postgres;Password=root");
            //try
            //{
            //    var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
            //    databaseCreator.CreateTables();
            //}
            //catch (Exception eee)
            //{

            //    throw;
            //}


    }


    public virtual DbSet<TblUser> TblUsers { get; set; } = null;
    public virtual DbSet<TblUserFriend> TblUserFriends { get; set; } = null;
    public virtual DbSet<TblMessage> TblMessages { get; set; } = null;
}