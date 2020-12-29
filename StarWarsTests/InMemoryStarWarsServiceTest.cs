using System;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess;
using Xunit;

namespace StarWarsTests
{
    public class InMemoryStarWarsServiceTest : StarWarsServiceTest
    {
        public InMemoryStarWarsServiceTest()
            : base(
                  new DbContextOptionsBuilder<StarWarsDbContext>()
                  .UseInMemoryDatabase("TestStarWarsDb")
                  .Options)
        { }      
    }
}
