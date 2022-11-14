using Microsoft.EntityFrameworkCore;

namespace CSnoek9141_Assignment2.Entities
{
    public class PartyDbContext : DbContext
    {

        public PartyDbContext(DbContextOptions<PartyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //invite status enum logic
            modelBuilder.Entity<Invitation>().Property(i => i.InviteStatus).HasConversion<string>().HasMaxLength(15);

            //Relationship logic
            //modelBuilder.Entity<Invitation>().HasOne(i => i.Party).WithMany(p => p.Invitations).HasForeignKey(i => i.PartyID); //unnecessary due to having fully defined classes. Only here to show that I understand it
            modelBuilder.Entity<Party>().Navigation(p => p.Invitations).AutoInclude(); //allows for much simpler requests for pages and controllers


            //seeded data
            modelBuilder.Entity<Invitation>().HasData(
                new Invitation()
                {
                    InvitationID = 1,
                    GuestName = "Bob Jones",
                    GuestEmail = "pmadziak@conestogac.on.ca",
                    InviteStatus = InviteStatus.RespondedYes,
                    PartyID = 1
                },
                new Invitation()
                {
                    InvitationID = 2,
                    GuestName = "Sally Smith",
                    GuestEmail = "peter.madziak@gmail.com",
                    InviteStatus = InviteStatus.InviteSent,
                    PartyID = 1
                }
                );

            modelBuilder.Entity<Party>().HasData(
                new Party()
                {
                    PartyID = 1,
                    Description = "New year's eve blast!",
                    EventDate = new DateTime(2022, 12, 31),
                    Location = "Time Square, NY"
                });
        }
    }
}
