using Domain.Entities;
using System.Data.Entity;

namespace Domain
{
    public class EFDbConntext : DbContext
    {
        public EFDbConntext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<IncomingMessage> IncomingMessages { get; set; }
        public DbSet<OutgoingMessage> OutgoingMessages { get; set; }
    }
}
