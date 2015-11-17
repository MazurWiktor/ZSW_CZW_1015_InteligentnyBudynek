using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SysWbudProject.Models;

namespace SysWbudProject.DAL
{
    public class SmartHomeInitializer 
            : System.Data.Entity.DropCreateDatabaseIfModelChanges<SmartHomeContext>
    {

        protected override void Seed(SmartHomeContext context) {
            var Buildings = new List<Building>{
                new Building{Name="Portland 92207"}
            };

            Buildings.ForEach(b => context.Buildings.Add(b));
            context.SaveChanges();

        }

    }
}