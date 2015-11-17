using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartHome.Models;

namespace SmartHome.DAL {
    public class SmartHomeContext : DbContext {

        public SmartHomeContext() : base("SmartHomeContext") {
            Console.Out.WriteLine("SmartHomeContext()");
            
        }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<Floor> Floors { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Log> Logs { get; set; }

    }
}