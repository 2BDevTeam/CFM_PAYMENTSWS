﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CFM_PAYMENTSWS.Persistence.Contexts
{
    public class AuthAppContext : IdentityDbContext<IdentityUser>
    {

        public AuthAppContext(DbContextOptions<AuthAppContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
