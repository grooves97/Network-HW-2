using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.DataAcces
{
    public class DataInitilizer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            List<City> cities = new List<City>
            {
                new City
                {
                    Name="Almaty",
                    Streets = new List<Street>
                    {
                        new Street
                        {
                            Name="Arbat"
                        },

                        new Street
                        {
                            Name="Al-farabi"
                        },

                        new Street
                        {
                            Name="Zhamadilova"
                        }
                    }
                },
                new City
                {
                    Name="Astana",
                    Streets = new List<Street>
                    {
                        new Street
                        {
                            Name="Kenesary"
                        },

                        new Street
                        {
                            Name="Kunaeva"
                        },

                        new Street
                        {
                            Name="Moldagulova"
                        }
                    }
                }
            };

            //List<Street> streets = new List<Street>
            //{
            //    new Street
            //    {
            //        Name="Arbat"
            //    },
            //    new Street
            //    {
            //        Name="Al-farabi"
            //    },
            //    new Street
            //    {
            //        Name="Zhamadilova"
            //    }
            //};

            base.Seed(context);
        }
    }
}
