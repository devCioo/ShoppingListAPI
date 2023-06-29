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
        public DataSeeder(ShoppingListDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Login = "admin",
                    PasswordHash = "admin",
                    Role = new Role
                    {
                        Name = "Administrator"
                    },
                    UserData = new UserData
                    {
                        FirstName = "",
                        LastName = "",
                        Email = "",
                        PhoneNumber = ""
                    }
                },
                new User
                {
                    Login = "user123",
                    PasswordHash = "password123",
                    Role = new Role
                    {
                        Name = "Standard user"
                    },
                    UserData = new UserData
                    {
                        FirstName = "Toni",
                        LastName = "Dripano",
                        Email = "tdripano@gmail.com",
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
                }
            };

            return users;
        }
    }
}
