using Microsoft.EntityFrameworkCore;
using leave.Models.Domain;
using leavebalanceModel;
using EmployeeTimesheet;
using Leavestatus;
#nullable disable
namespace Leave.Data
{
    public  class LeaveDbContext:DbContext
   {
    public LeaveDbContext(DbContextOptions<LeaveDbContext> options):base(options)
    {

    }
    public DbSet<Leaves>leave{get;set;}
    public DbSet<Timesheet>timesheet{get;set;}
    public DbSet<MyTask>mytask{get;set;}
    public DbSet<leavebalance>balance{get;set;}
    public DbSet<LeavestatusTracker>Myleavestatus{get;set;}

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Leaves>().HasKey(l=>l.leaveid);
            modelBuilder.Entity<LeavestatusTracker>().HasKey(s=>s.statusid);
            modelBuilder.Entity<MyTask>().HasKey(m=>m.taskid);
            modelBuilder.Entity<Timesheet>().HasKey(t=>t.TimesheetId);
            modelBuilder.Entity<Timesheet>().HasMany(t=>t.tasks).WithOne(task=>task.mytimesheet).HasForeignKey(t=>t.TimesheetId);
           
        }
    }

}