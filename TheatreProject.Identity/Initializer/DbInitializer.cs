﻿using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using TheatreProject.Identity.Constants;
using TheatreProject.Identity.Data;
using TheatreProject.Identity.Models;

namespace TheatreProject.Identity.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public void Initialize()
    {
        if (_roleManager.FindByNameAsync(Const.Administrator).Result == null)
        {
            _roleManager.CreateAsync(new IdentityRole(Const.Administrator)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Const.Customer)).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }

        ApplicationUser adminUser = new ApplicationUser()
        {
            UserName = "example.administrator@gmail.com",
            Email = "example.administrator@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111111",
            FirstName = "Example",
            LastName = "Administrator"
        };

        _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, Const.Administrator).GetAwaiter().GetResult();

        var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
            new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
            new Claim(JwtClaimTypes.Email, adminUser.Email),
            new Claim(JwtClaimTypes.Role, Const.Administrator),
        }).Result;

        ApplicationUser customerUser = new ApplicationUser()
        {
            UserName = "andrey.stroganov87@gmail.com",
            Email = "andrey.stroganov87@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111111",
            FirstName = "Andrey",
            LastName = "Stroganov"
        };

        _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(customerUser, Const.Customer).GetAwaiter().GetResult();

        var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, customerUser.FirstName + " " + customerUser.LastName),
            new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
            new Claim(JwtClaimTypes.Email, customerUser.Email),
            new Claim(JwtClaimTypes.Role, Const.Customer),
        }).Result;
    }
}