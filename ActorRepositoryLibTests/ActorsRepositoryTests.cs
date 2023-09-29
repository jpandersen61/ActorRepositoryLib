using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActorRepositoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;


namespace ActorRepositoryLib.Tests
{
    [TestClass()]
    public class ActorsRepositoryTests
    {
        private const bool useDatabase = false;
        private static ActorsDbContext? _dbContext = null;
        private static IActorsRepository? _repo = null;

        [ClassInitialize]
        public static void InitOnce(TestContext context)
        {
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ActorsDbContext>();
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ActorRepositoryLib;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                
                optionsBuilder.UseSqlServer(connectionString);
                _dbContext = new ActorsDbContext(optionsBuilder.Options);               
            }

            InitRepository();
        }

        //[TestInitialize]
        public static void InitRepository()
        {
            if (useDatabase)
            {
                if (_repo == null)
                {
                    _repo = new ActorsRepositoryDB(_dbContext);
                    
                }
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Actors");
            }
            else 
            { 
                _repo = new ActorsRepository(); 
            }
            

            _repo.Add(new Actor() { Name = "Tom Hanks", BirthYear = 1956 });
            _repo.Add(new Actor() { Name = "Will Smith", BirthYear = 1968 });
            _repo.Add(new Actor() { Name = "Jodie Foster", BirthYear = 1962 });
            _repo.Add(new Actor() { Name = "Scarlett Johansson", BirthYear = 1984 });
        }

        [TestMethod()]
        public void ActorsRepositoryTest()
        {
            //Arrange
            IEnumerable<Actor> actors = _repo.Get();

            //Assert: No need to test any further
            Assert.IsNotNull(actors);
        }

        [TestMethod()]
        public void AddTest()
        {
            AddGetTest();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Arrange
            IEnumerable<Actor> actors = _repo.Get();

            //Assert: No need to test any further
            Assert.IsNotNull(actors);
            Assert.AreEqual<bool>(true, actors.Count() > 0);

            //Arrange
            Actor someActor = actors.First<Actor>();
            int numOfActors = actors.Count();

            //Actor
            Actor? deletedActor = _repo.Delete(someActor.Id);

            //Assert
            Assert.AreEqual(numOfActors - 1, _repo.Get().Count());
            Actor? actorFound = null;
            //This:
            actorFound = _repo.Get().FirstOrDefault(a => a.Id == deletedActor.Id);
            //Replaces:
            //foreach (Actor actor in _repo.Get()) 
            //{ 
            //    if (actor.Id == someActor.Id) 
            //    { 
            //        actorFound = actor; 
            //        break; 
            //    }
            //}
            Assert.IsNull(actorFound);
        }

        [TestMethod()]
        public void GetTest()
        {
            AddGetTest();
        }

        [TestMethod()]
        [DataRow(1956)]
        [DataRow(1968)]
        [DataRow(1962)]
        [DataRow(1984)]
        public void GetBirthYearBeforeTest(int birthYearBefore)
        {
            //Arrange & Act
            IEnumerable<Actor> actors = _repo.Get(birthYearBefore);

            //Assert: No need to test any further
            Assert.IsNotNull(actors);
            foreach(Actor a in actors)
            {
                if(a.BirthYear >= birthYearBefore)
                {
                    Assert.Fail($"Birth year {a.BirthYear} is NOT before {birthYearBefore}");
                }
            }

            
        }

        [TestMethod()]
        [DataRow(2024, 1820)]
        [DataRow(1968, 1962)]
        [DataRow(1984, 1956)]
        public void GetBirthYearBeforeAndAfterTest(int birthYearBefore, int birthYearAfter)
        {
            //Act
            IEnumerable<Actor> actors = _repo.Get(birthYearBefore, birthYearAfter);

            //Assert
            Assert.IsNotNull(actors);

            foreach (Actor a in actors)
            {
                if (a.BirthYear >= birthYearBefore)
                {
                    Assert.Fail($"Birth year {a.BirthYear} is NOT before {birthYearBefore}");
                }

                if (a.BirthYear <= birthYearAfter)
                {
                    Assert.Fail($"Birth year {a.BirthYear} is NOT after {birthYearAfter}");
                }
            }
        }

        [TestMethod()]
        [DataRow(2024, 1820, "id")]
        [DataRow(1968, 1962, "Id")]
        [DataRow(1984, 1956, "ID")]
        [DataRow(2024, 1820, "naME")]
        [DataRow(1968, 1962, "NAME")]
        [DataRow(1984, 1956, "name")]
        [DataRow(2024, 1820, "birthyear")]
        [DataRow(1968, 1962, "birThyeaR")]
        [DataRow(1984, 1956, "BIRTHYEAR")]
        public void GetBirthYearBeforeAndAfterOrderByTest(int birthYearBefore, 
                                                          int birthYearAfter, 
                                                          string orderBy)
        {
            //Act
            IEnumerable<Actor>? actors = _repo.Get(birthYearBefore, birthYearAfter, orderBy);

            //Assert
            Assert.IsNotNull(actors);

            foreach (Actor a in actors)
            {
                if (a.BirthYear >= birthYearBefore)
                {
                    Assert.Fail($"Birth year {a.BirthYear} is NOT before {birthYearBefore}");
                }

                if (a.BirthYear <= birthYearAfter)
                {
                    Assert.Fail($"Birth year {a.BirthYear} is NOT after {birthYearAfter}");
                }
            }

            IEnumerable<Actor>? orderedActors = null;


            switch (orderBy.ToLower())
            {
                case "id":
                    orderedActors = actors.OrderBy(a => a.Id);
                    break;
                case "name":
                    orderedActors = actors.OrderBy(a => a.Name);
                    break;
                case "birthyear":
                    orderedActors = actors.OrderBy(a => a.BirthYear);
                    break;
            }

            CollectionAssert.AreEqual(orderedActors.ToList(), actors.ToList(),"Actors NOT ordered correctly");
        }


        public void AddGetTest()
        {
            //Arrange
            IEnumerable<Actor> actors = _repo.Get();

            //Assert: No need to test any further
            Assert.IsNotNull(actors);

            //Arrange
            Actor actor = new Actor()
            {
                Name = "Test",
                BirthYear = 1942
            };

            //Act
            Actor? actorReturned = _repo.Add(actor);
            Actor? actorFound = null;
            
            //This:
            actorFound = _repo.Get().FirstOrDefault(a => a.Id == actorReturned.Id);
            
            //Replaces:
            //foreach (Actor a in _repo.Get())
            //{
            //    if (a.Id == actorReturned.Id)
            //    {
            //        actorFound = a;
            //        break;
            //    }
            //}

            //Assert
            Assert.IsNotNull(actorReturned);
            Assert.AreEqual("Test", actorReturned.Name);
            Assert.AreEqual(1942, actorReturned.BirthYear);

            Assert.IsNotNull(actorFound);
            Assert.AreEqual("Test", actorFound.Name);
            Assert.AreEqual(1942, actorFound.BirthYear);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            //Arrange
            IEnumerable<Actor> actors = _repo.Get();

            //Assert: No need to test any further
            Assert.IsNotNull(actors);
            Assert.AreEqual<bool>(true, actors.Count<Actor>()>0);

            //Arrange
            Actor someActor = actors.First<Actor>();
            Actor actorToUpdate = new Actor()
            {
                Name = someActor.Name + "(updated)",
                BirthYear = someActor.BirthYear + 1,
            };

            //Act
            Actor updatedActor = _repo.Update(someActor.Id,actorToUpdate);

            //Assert
            Assert.IsNotNull(updatedActor);
            Assert.AreEqual(someActor.Id, updatedActor.Id);
            Assert.AreEqual(actorToUpdate.Name, updatedActor.Name);
            Assert.AreEqual(actorToUpdate.BirthYear, updatedActor.BirthYear);
        }

        /// <summary>
        /// Tests Actor.Validate() regarding ArgumentOutOfRangeException on invalid values with focus on integration
        /// </summary>
        /// <param name="name">Test value</param>
        /// <param name="birthYear">Test value</param>
        [TestMethod()]
        //Invalid out-of-range birth year
        [DataRow(Actor.ValidName, Actor.MinValidBirthYear - 1)]
        //Invalid out-of-range names
        [DataRow(Actor.TooShortName, Actor.ValidBirthYear)]
        [DataRow(Actor.EmptyName, Actor.ValidBirthYear)]

        public void ValidateTestArgumentOutOfRangeException(string name, int birthYear)
        {
            //Arrange 
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = birthYear,
                Name = name
            };

            //Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Add(act));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Update(Actor.ValidId, act));

        }

        /// <summary>
        /// Tests Actor.Validate() regarding ArgumentNullException on null values with focus on integration
        /// </summary>
        [TestMethod()]
        public void ValidateTestArgumentNullException()
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = Actor.ValidBirthYear,
                Name = Actor.NullName
            };

            Assert.ThrowsException<ArgumentNullException>(() => _repo.Add(act));
            Assert.ThrowsException<ArgumentNullException>(() => _repo.Update(Actor.ValidId, act));
        }
    }
}