using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityCaps.Models;

namespace QualityCaps.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
           
            var dbcreated = context.Database.EnsureCreated();

            if (dbcreated)
            {
                Console.WriteLine("new db created.");
            }
            else
            {
                Console.WriteLine("db already created.");
            }

            var dbCount = -1;
            try
            {
                dbCount = context.Categories.Count();
            }
            catch (Exception ex)
            {
                dbCount = -1;
                // TODO: create tables
            }

            if (dbCount > 0)
            {
                return;
            }

           context.Database.EnsureCreated();

   
           
            if (context.Caps.Any())
            {
                return;
            }

            
                        var categories = new Category[] {
                            new Category{CategoryName="Baseball", Description="Baseball caps" },
                            new Category{CategoryName="Flat", Description="Flat caps" },
                            new Category{CategoryName="Knit", Description="Knit caps" },
                            new Category{CategoryName="Sailor", Description="Sailor caps" },
                            new Category{CategoryName="Sindhi", Description="Sindhi caps" }
                        };

                        foreach (Category c in categories)
                        {
                            context.Categories.Add(c);
                        }
                        context.SaveChanges();

            
                        var suppliers = new Supplier[] {
                            new Supplier{ SupplierName="DavidSmith", PhoneNumber="021 2345678", EmailAddress="davidSmith@gmail.com" },
                            new Supplier{ SupplierName="Albert Hamlet", PhoneNumber="011 3210987", EmailAddress="albertHamlet@gmail.com"},
                            new Supplier{ SupplierName="J.K Rowling", PhoneNumber="012 2343213", EmailAddress="jkRowling@gmail.com" },
                            new Supplier{ SupplierName="Anna Lee", PhoneNumber="098 4652311", EmailAddress="annaLee@gmail.com" },
                            new Supplier{ SupplierName="Cindy Koo", PhoneNumber="024 6523123", EmailAddress="cindyKoo@gmail.com" },
                            new Supplier{ SupplierName="Albert Woe", PhoneNumber="086 3421876", EmailAddress="albertWoe@gmail.com" },
                            new Supplier{ SupplierName="Simon Kan", PhoneNumber="054 1258763", EmailAddress="simonKan@gmail.com" },
                            new Supplier{ SupplierName="John Win", PhoneNumber="033 1276348", EmailAddress="johnWin@gmail.com" },
                            new Supplier{ SupplierName="Russuel Potter", PhoneNumber="032 4365217", EmailAddress="russuelPotter@gmail.com" },
                            new Supplier{ SupplierName="Harry Desn", PhoneNumber="034 7613452", EmailAddress="harryDesn@gmail.com" },
                            new Supplier{ SupplierName="Lily Josh", PhoneNumber="096 3254798", EmailAddress="lilyJosh@gmail.com" }
                        };

                        foreach (Supplier s in suppliers)
                        {
                            context.Suppliers.Add(s);
                        }
                        context.SaveChanges();


            
            var caps = new Cap[] {
                new Cap{ CapName="Cradle Cap", Description="A thistie straw trilby in orange.",
                    SupplierID = 1, Price=10,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball01.jpg"  },

                 new Cap{ CapName="Contact Cap", Description="The valencia cap has a big printing on it",
                    SupplierID = 2, Price=25,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball02.jpg"  },

                 new Cap{ CapName="Army Baseball", Description="Men Washed Denim Baseball Caps Letters Applique Embellished Peaked Cap Outdoor Hats",
                    SupplierID = 5, Price=35,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball03.jpeg"  },

                 new Cap{ CapName="Outdoo Cap", Description="Thin Breathable Quick Dry Hats Outdoor Sunshade Mesh Baseball Caps",
                    SupplierID = 2, Price=65,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball05.jpeg"  },

                 new Cap{ CapName="Cotton Cap", Description="Mens Cotton Washed Baseball Caps CLASSIC Letter Embroidery Adjustable Sports Snapback Hats",
                    SupplierID = 2, Price=25,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball06.jpeg"  },

                 new Cap{ CapName="Sunshade Cap", Description="Summer Quick-dry Breathable Mesh Baseball Caps Foldable Thin Outdoor Sunshade Cap For Men Women",
                    SupplierID = 3, Price=55,
                    CategoryID = 1,
                    Image ="/chenl85/asp_assignment/images/baseball/baseball07.jpg"  },           

                new Cap{ CapName="Mazzoni Textured Cap", Description="A fashion cap wiht a navy trim",
                    SupplierID = 1, Price=19,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat01.jpeg"  },

                 new Cap{ CapName="Paisley Cap", Description="This is a functional cap.",
                    SupplierID = 4, Price=99,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat03.jpeg"  },

                new Cap{ CapName="Daredevil Cap", Description="Perfect for lazy sunny days",
                    SupplierID = 3, Price=48,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat02.jpg"  },

                new Cap{ CapName="Chisel Cap", Description="Unisex Men's Cotton Wool Gatsby Beret Cap Golf Driving Flat Cabbie Newsboy Hat",
                    SupplierID = 5, Price=75,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat04.jpeg"  },

                new Cap{ CapName="Cotton Flat Cap", Description="Men Solid Adjustable Washed Cotton Flat Top Hats Outdoor Sunscreen Military Army Baseball Cap",
                    SupplierID = 5, Price=95,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat05.jpeg"  },

                new Cap{ CapName="Chisel Cap", Description="A denim bucket cap featuring abrasions",
                    SupplierID = 5, Price=15,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat06.jpeg"  },

                new Cap{ CapName="Stripe Beret Cap", Description="Mens Vintage Grid Stripe Beret Caps Casual Sunshade Forward Flat Hat Adjustable",
                    SupplierID = 6, Price=45,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat07.jpeg"  },

                new Cap{ CapName="Lace-up Cap", Description="Men Retro Artificial Leather Lace-up Beret Caps Casual Flat Golf Cabbie Hats Adjustable",
                    SupplierID = 6, Price=25,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat08.jpeg"  },

                new Cap{ CapName="Visor Cap", Description="Mens and Womens Cotton Painter Berets Caps Vogue Leisure Adjustable Forward Hat Visor Flat Hat",
                    SupplierID = 11, Price=33,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat09.jpeg"  },

                new Cap{ CapName="Cabbie Cap", Description="Mens Summer Mesh Breathable Beret Hat Outdoor Sport Solid Visor Newsboy Cabbie Flat Cap",
                    SupplierID = 11, Price=35,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat10.jpeg"  },

                 new Cap{ CapName="Newsboy Beret Cap", Description="Unisex Cotton Stripe Beret Hat Buckle Adjustable Golf Driving Flat Cabbie Newsboy Beret Cap",
                    SupplierID = 10, Price=35,
                    CategoryID = 2,
                    Image ="/chenl85/asp_assignment/images/flat/flat11.jpeg"  },



                new Cap{ CapName="Dino Bucket Cap", Description="Soak up the sun with this cute cap.",
                    SupplierID = 10, Price=29,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit01.jpeg"  },

                new Cap{ CapName="Lace Trim Cap", Description="This trilby style has a weave.",
                    SupplierID = 7, Price=39,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit03.jpeg"  },

                new Cap{ CapName="Panama Cap", Description="A cap featuring an all-over beatiful print.",
                    SupplierID = 8, Price=19,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit02.jpeg"  },

                new Cap{ CapName="Beanie Cap", Description="Men Women Beanie Knit Ski Winter Warm Hat Skull Star Slouchy Hip-hop Baggy Cap.",
                    SupplierID = 8, Price=39,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit08.jpeg"  },

                new Cap{ CapName="Unisex Cap", Description="Unisex Winter Warm Knit Beanie Hat for Men and Women Earmuffs Outdoor Thick Ski Skull Cap.",
                    SupplierID = 8, Price=29,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit02.jpeg"  },

                new Cap{ CapName="Baggy Warm Cap", Description="Men Women Baggy Warm Hat Crochet Winter Wool Knit Ski Motorcycle Beanie Cap.",
                    SupplierID = 9, Price=49,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit12.png"  },

                new Cap{ CapName="Beanie Cap", Description="Sports Running 6 LED Beanie Knit Hat Rechargeable Cap Light Camping Climbing Lamp.",
                    SupplierID = 9, Price=79,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit13.png"  },

                new Cap{ CapName="Winter Warm Cap", Description="Winter Warm Knit Woolen Face Mask Hat Beanie Cap Outdoors Riding Mask Scarf Hat Dual Use.",
                    SupplierID = 9, Price=89,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit11.jpg"  },

                new Cap{ CapName="Woolen Warm Cap", Description="Women Mens Solid Woolen Warm Knit Beanie Cap Adjustable Windproof Winter Hat.",
                    SupplierID = 7, Price=19,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit04.jpeg"  },

                new Cap{ CapName="Panama Cap", Description="A cap featuring an all-over beatiful print.",
                    SupplierID = 7, Price=29,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit05.jpeg"  },

                new Cap{ CapName="Bonnet Dome Cap", Description="Womens Ladies Chunky Knit Crochet Beanie Hat Ski Triangle Stereo Bonnet Dome Cap.",
                    SupplierID = 7, Price=49,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit06.png"  },

                new Cap{ CapName="Knitting Cap", Description="Season: Spring,Autumn,Winter, Occasion: Skiing, Outdoor,Sport, Feature: Windproof, Warm, Earmuffs",
                    SupplierID = 7, Price=59,
                    CategoryID = 3,
                    Image ="/chenl85/asp_assignment/images/knit/knit07.jpeg"  },



                new Cap{ CapName="Straw Triby Cap", Description="A chambray look cap.",
                    SupplierID = 7, Price=73,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor01.jpeg"  },

                new Cap{ CapName="Golden Weave Cap", Description="A straw trilby cap with contrasting striped band.",
                    SupplierID = 8, Price=59,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor02.jpeg"  },

                new Cap{ CapName="Stripe Cotton Cap", Description="Mens Stripe Cotton Skullcap Sailor Cap Casual Outdoor Worker Hat Rolled Cuff Cap.",
                    SupplierID = 2, Price=79,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor03.png"  },

                new Cap{ CapName="Embroidery Cap", Description="Men Women Letters Embroidery Skullcap Sailor Cap Worker Hat Rolled Cuff Retro Brimless Hat.",
                    SupplierID = 2, Price=56,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor04.jpeg"  },

                new Cap{ CapName="Canvas Cap", Description="Men Women Canvas Plaid Skullcap Sailor Cap Rolled Cuff Retro Fashion Brimless Hat Adjustable Beanie.",
                    SupplierID = 2, Price=19,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor05.jpeg"  },

                new Cap{ CapName="Retro Grid Cap", Description="Mens Women Retro Grid Skullcap Sailor Cap Rolled Cuff Brimless Adjustable Hat.",
                    SupplierID = 3, Price=49,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor06.jpeg"  },

                new Cap{ CapName="Navy Cap", Description="Sailors Navy Hat Cap With Anchor Emblem Marine Costume Party Kids Adult.",
                    SupplierID = 1, Price=29,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor07.jpeg"  },

                new Cap{ CapName="Skull Cap", Description="Men Solid Adjustable Warm Skullcap Sailor Cap Rolled Cuff Retro Brimless Ha.",
                    SupplierID = 1, Price=59,
                    CategoryID = 4,
                    Image ="/chenl85/asp_assignment/images/sailor/sailor08.jpeg"  }
            };

            foreach (Cap c in caps)
            {
                context.Caps.Add(c);
                
            }
            context.SaveChanges();
        }
    }
}
