using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TodoListApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                 new ApplicationUser(){
                        Email="test@user.com",
                        NormalizedEmail="TEST@USER.COM",
                        EmailConfirmed=true,
                        UserName="testUser",
                        NormalizedUserName="TESTUSER"},
                new ApplicationUser(){
                        Email="test1@user.com",
                        NormalizedEmail="TEST1@USER.COM",
                        EmailConfirmed=true,
                        UserName="test1User",
                        NormalizedUserName="TEST1USER"},
                new ApplicationUser(){
                        Email="test2@user.com",
                        NormalizedEmail="TEST2@USER.COM",
                        EmailConfirmed=true,
                        UserName="test2User",
                        NormalizedUserName="TEST2USER"},
            };
            //set password for each user //
            users.ForEach(x => x.PasswordHash = passwordHasher.HashPassword(x, "test!Test"));

            builder.Entity<ApplicationUser>()
                .HasData(
                  users
                );

            // get all roles from static class with defined role names //
            List<ApplicationRole> roles = new List<ApplicationRole>() {
                    new ApplicationRole()
                    {
                        Name = nameof(ApplicationUserRoles.Administrator),
                        NormalizedName = nameof(ApplicationUserRoles.Administrator).ToUpper(),
                        ConcurrencyStamp=Guid.NewGuid().ToString()
                    }, new ApplicationRole()
                    {
                        Name = nameof(ApplicationUserRoles.Moderator),
                        NormalizedName = nameof(ApplicationUserRoles.Moderator).ToUpper(),
                        ConcurrencyStamp=Guid.NewGuid().ToString()
                    }, new ApplicationRole()
                    {
                        Name = nameof(ApplicationUserRoles.User),
                        NormalizedName = nameof(ApplicationUserRoles.User).ToUpper(),
                        ConcurrencyStamp=Guid.NewGuid().ToString()
                    }
            };


            builder.Entity<ApplicationRole>().HasData(roles);
        }
    }
}
