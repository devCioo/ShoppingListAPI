using Microsoft.AspNetCore.Identity;
using ShoppingListAPI.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingListAPI
{
    public class DataSeeder
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DataSeeder(ShoppingListDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "Standard user"
                },
                new Role
                {
                    Name = "VIP user"
                },
                new Role
                {
                    Name = "Administrator"
                }
            };

            return roles;
        }
        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>();
            var admin = new User
            {
                Login = "admin",
                Role = _dbContext.Roles.FirstOrDefault(r => r.Name == "Administrator"),
                UserData = new UserData
                {
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    PhoneNumber = ""
                }
            };
            admin.PasswordHash = _passwordHasher.HashPassword(admin, "admin");
            var user = new User
            {
                Login = "user123",
                Role = _dbContext.Roles.FirstOrDefault(r => r.Name == "Standard user"),
                UserData = new UserData
                {
                    FirstName = "Matthew",
                    LastName = "Hill",
                    Email = "mhill@gmail.com",
                    PhoneNumber = "510969431",
                },
                ShoppingLists = new List<ShoppingList>
                    {
                        new ShoppingList
                        {
                            Name = "Groceries",
                            Description = "Things that I buy daily",
                            CreatedOn = DateTime.Now,
                            Items = new List<Item>
                            {
                                new Item
                                {
                                    Name = "Milk",
                                    Description = "3,2% of fat",
                                    MeasureUnit = MeasureUnit.Units,
                                    UnitPrice = 2.80,
                                    Quantity = 1
                                },
                                new Item
                                {
                                    Name = "Eggs",
                                    MeasureUnit = MeasureUnit.Units,
                                    UnitPrice = 0.90,
                                    Quantity = 10
                                },
                                new Item
                                {
                                    Name = "Chicken nuggets",
                                    MeasureUnit = MeasureUnit.Kilograms,
                                    UnitPrice = 14.99,
                                    Quantity = 1.492
                                }
                            }
                        }
                    }
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "password123");

            users.Add(admin);
            users.Add(user);

            return users;
        }
    }
}
