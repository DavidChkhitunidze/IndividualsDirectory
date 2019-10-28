using IndividualsDirectory.Common;
using IndividualsDirectory.Entities;
using IndividualsDirectory.Entities.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Helpers.Extensions
{
    public static class DataSeedExtensions
    {
        public static IApplicationBuilder UseDataSeeding(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<IndividualsDirectoryDbContext>())
            {
                var cityContext = context.Set<City>();
                var individualContext = context.Set<Individual>();

                if (!individualContext.Any() && !cityContext.Any())
                {
                    var city = new City { Name = "ბათუმი" };

                    cityContext.AddRange(city);
                    context.SaveChanges();

                    var individual1 = new Individual
                    {
                        FirstName = "ნინო",
                        LastName = "ნინოშვილი",
                        Gender = Gender.Female,
                        PersonalNumber = "12345678954",
                        BirthDate = new DateTime(1991, 6, 7),
                        ImageUrl = "no-avatar.png",
                        City = new City { Name = "ფოთი" },
                        PhoneNumbers = new List<PhoneNumber>
                        {
                            new PhoneNumber { Number = "558456578", PhoneNumberType = PhoneNumberType.Mobile }
                        }
                    };

                    individualContext.Add(individual1);
                    context.SaveChanges();

                    var individual2 = new Individual
                    {
                        FirstName = "თაკო",
                        LastName = "ჩხიტუნიძე",
                        Gender = Gender.Female,
                        PersonalNumber = "65498732112",
                        BirthDate = new DateTime(1995, 7, 15),
                        ImageUrl = "no-avatar.png",
                        CityId = city.Id,
                        PhoneNumbers = new List<PhoneNumber>
                        {
                            new PhoneNumber { Number = "555456578", PhoneNumberType = PhoneNumberType.Mobile },
                            new PhoneNumber { Number = "0322789856", PhoneNumberType = PhoneNumberType.Home }
                        },
                        RelatedIndividuals = new List<RelatedIndividual>
                        {
                            new RelatedIndividual
                            {
                                RelatedIndividualType = RelatedIndividualType.Familiar,
                                RelatedToId = individual1.Id
                            }
                        }
                    };

                    var individual3 = new Individual
                    {
                        FirstName = "გიორგი",
                        LastName = "გიორგაძე",
                        Gender = Gender.Male,
                        PersonalNumber = "32165498778",
                        BirthDate = new DateTime(1992, 5, 28),
                        ImageUrl = "no-avatar.png",
                        CityId = city.Id,
                        PhoneNumbers = new List<PhoneNumber>
                        {
                            new PhoneNumber { Number = "555457898", PhoneNumberType = PhoneNumberType.Mobile }
                        }       
                    };

                    individualContext.AddRange(individual2, individual3);
                    context.SaveChanges();

                    var individual4 = new Individual
                    {
                        FirstName = "დავით",
                        LastName = "ჩხიტუნიძე",
                        Gender = Gender.Male,
                        PersonalNumber = "01010205214",
                        BirthDate = new DateTime(1994, 4, 29),
                        ImageUrl = "no-avatar.png",
                        City = new City { Name = "თბილისი" },
                        PhoneNumbers = new List<PhoneNumber>
                        {
                            new PhoneNumber { Number = "558042800", PhoneNumberType = PhoneNumberType.Mobile },
                            new PhoneNumber { Number = "0322789845", PhoneNumberType = PhoneNumberType.Home },
                            new PhoneNumber { Number = "555282828", PhoneNumberType = PhoneNumberType.Office }
                        },
                        RelatedIndividuals = new List<RelatedIndividual>
                        {
                            new RelatedIndividual
                            {
                                RelatedIndividualType = RelatedIndividualType.Colleague,
                                RelatedToId = individual2.Id
                            },
                            new RelatedIndividual
                            {
                                RelatedIndividualType = RelatedIndividualType.Relative,
                                RelatedToId = individual3.Id
                            }
                        }
                    };

                    individualContext.Add(individual4);
                    context.SaveChanges();

                    var individualsList = new List<Individual>
                    {
                        new Individual
                        {
                            FirstName = "თამარ",
                            LastName = "თამარაშვილი",
                            Gender = Gender.Female,
                            PersonalNumber = "45678913245",
                            BirthDate = new DateTime(1990, 1, 21),
                            ImageUrl = "no-avatar.png",
                            City = new City { Name = "მცხეთა" },
                            PhoneNumbers = new List<PhoneNumber>
                            {
                                new PhoneNumber { Number = "557984565", PhoneNumberType = PhoneNumberType.Mobile },
                                new PhoneNumber { Number = "0322659812", PhoneNumberType = PhoneNumberType.Home }
                            }
                        },
                        new Individual
                        {
                            FirstName = "ნიკოლოზ",
                            LastName = "ნინიძე",
                            Gender = Gender.Male,
                            PersonalNumber = "65412345698",
                            BirthDate = new DateTime(1993, 12, 4),
                            City = new City { Name = "კასპი" },
                            ImageUrl = "no-avatar.png",
                            PhoneNumbers = new List<PhoneNumber>
                            {
                                new PhoneNumber { Number = "555213256", PhoneNumberType = PhoneNumberType.Mobile }
                            }
                        },
                        new Individual
                        {
                            FirstName = "საბა",
                            LastName = "საბაშვილი",
                            Gender = Gender.Male,
                            PersonalNumber = "32659821547",
                            BirthDate = new DateTime(1995, 2, 25),
                            City = new City { Name = "სიღნაღი" },
                            ImageUrl = "no-avatar.png",
                            PhoneNumbers = new List<PhoneNumber>
                            {
                                new PhoneNumber { Number = "555327898", PhoneNumberType = PhoneNumberType.Mobile },
                                new PhoneNumber { Number = "555668899", PhoneNumberType = PhoneNumberType.Office }
                            },
                            RelatedIndividuals = new List<RelatedIndividual>
                            {
                                new RelatedIndividual
                                {
                                    RelatedIndividualType = RelatedIndividualType.Colleague,
                                    RelatedToId = individual4.Id
                                }
                            }
                        }
                    };

                    individualContext.AddRange(individualsList);
                    context.SaveChanges();
                }

                return app;
            }
        }
    }
}
