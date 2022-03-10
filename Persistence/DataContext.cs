using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext: IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) 
        {

        }

        public DbSet<Activity> Activities {get; set;}
        
        public DbSet<ActivityAttendee> ActivityAttendees {get;set;}
        public DbSet<Photo> Photos {get;set;}
        public DbSet<Comment> Comments {get;set;}
        public DbSet<UserFollowing> UserFollowers {get;set;}
        
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x=>x.HasKey(aa=>new{aa.AppUserId,aa.ActivityID}));

            builder.Entity<ActivityAttendee>()
                .HasOne(y=>y.AppUser)
                .WithMany(a=>a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

             builder.Entity<ActivityAttendee>()
                .HasOne(y=>y.Activity)
                .WithMany(a=>a.Attendees)
                .HasForeignKey(aa => aa.ActivityID);

             builder.Entity<Comment>()
                .HasOne(a=> a.Activity)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>( b=> 
            {
                b.HasKey(k => new { k.ObserverId,k.TargetId});

                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(o => o.Target)
                    .WithMany(f => f.followers)
                    .HasForeignKey( o=> o.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }       
    }
}