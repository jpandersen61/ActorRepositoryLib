using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActorRepositoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorRepositoryLib.Tests
{
    [TestClass]
    public class ActorTests
    {
        [TestMethod()]
        public void ActorTest()
        {
            //Arrange & Act
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = Actor.ValidBirthYear,
                Name = Actor.ValidName
            };

            //Assert
            Assert.AreEqual(Actor.ValidId, act.Id);
            Assert.AreEqual(Actor.ValidBirthYear, act.BirthYear);
            Assert.AreEqual(Actor.ValidName, act.Name);
        }

        /// <summary>
        /// Tests with valid names, no Exceptions expected 
        /// </summary>
        /// <param name="name">Must be a valid name</param>
        [TestMethod()]
        //Valid names 
        [DataRow(Actor.MinValidName)]
        [DataRow(Actor.ValidName)]
        public void ValidateNameTestPositive(string name)
        {
            Actor act = new Actor() { Id = Actor.ValidId, 
                                      BirthYear = Actor.ValidBirthYear, 
                                      Name = name };
            act.ValidateName();    
        }

        /// <summary>
        /// Tests with an out-of-range name, but NOT null, ArgumentOutOfRangeException then expected
        /// </summary>
        /// <param name="name">Must be an out-of-range name</param>
        [TestMethod()]
        //Invalid out-of-range names
        [DataRow(Actor.TooShortName)]
        [DataRow(Actor.EmptyName)]
        public void ValidateNameTestArgumentOutOfRange(string name)
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = Actor.ValidBirthYear,
                Name = name
            };
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => act.ValidateName());
        }

        /// <summary>
        /// Tests with a null value name, ArgumentNullException then expected
        /// </summary>
        [TestMethod()]
        public void ValidateNameTestArgumentNullException()
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = Actor.ValidBirthYear,
                Name = Actor.NullName
            };

            Assert.ThrowsException<ArgumentNullException>(() => act.ValidateName());
        }

        /// <summary>
        /// Tests with valid birth years, no Exceptions expected
        /// </summary>
        /// <param name="birthYear">Must be a valid birth year</param>
        [TestMethod()]
        //Valid birth years
        [DataRow(Actor.MinValidBirthYear)]
        [DataRow(Actor.ValidBirthYear)]
        public void ValidateBirthYearTestPositive(int birthYear)
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = birthYear,
                Name = Actor.ValidName
            };

            act.ValidateBirthYear();
        }

        /// <summary>
        /// Tests with out-of-range birth years, ArgumentOutOfRangeException expected  
        /// </summary>
        /// <param name="birthYear">Must be out-of-range birth year</param>
        [TestMethod()]
        //Invalid out-of-range birth years
        [DataRow(Actor.MinValidBirthYear -1)]
        [DataRow(Actor.MinValidBirthYear - 10)]
        public void ValidateBirthYearTestNegative(int birthYear)
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = birthYear,
                Name = Actor.ValidName
            };
           
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => act.ValidateBirthYear());
        }


        /// <summary>
        /// Tests Validate() with valid input with focus on integration, no exceptions expected
        /// </summary>
        /// <param name="name">Must be a valid name</param>
        /// <param name="birthYear">Must be a valid birth year</param>
        [TestMethod()]
        [DataRow(Actor.MinValidName, Actor.MinValidBirthYear)]
        [DataRow(Actor.MinValidName, Actor.ValidBirthYear)]
        [DataRow(Actor.ValidName, Actor.MinValidBirthYear)]
        [DataRow(Actor.ValidName, Actor.ValidBirthYear)]
        public void ValidateTestPositive(string name, int birthYear)
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = birthYear,
                Name = name
            };

            act.Validate();
        }

        /// <summary>
        /// Tests Validate() regarding ArgumentOutOfRangeException on invalid values with focus on integration
        /// </summary>
        /// <param name="name">Test value</param>
        /// <param name="birthYear">Test value</param>
        [TestMethod()]
        //Invalid out-of-range birth year
        [DataRow(Actor.ValidName, Actor.MinValidBirthYear -1)]
        //Invalid out-of-range names
        [DataRow(Actor.TooShortName, Actor.ValidBirthYear)]
        [DataRow(Actor.EmptyName, Actor.ValidBirthYear)]
        
        public void ValidateTestArgumentOutOfRangeException(string name, int birthYear)
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = birthYear,
                Name = name
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => act.Validate());
            
        }

        /// <summary>
        /// Tests Validate() regarding ArgumentNullException on null values with focus on integration
        /// </summary>
        [TestMethod()]
        public void ValidateTestArgumentNullException()
        {
            Actor act = new Actor()
            {
                Id = Actor.ValidId,
                BirthYear = Actor.ValidBirthYear,
                Name = null
            };

            Assert.ThrowsException<ArgumentNullException>(() => act.Validate());
        }
    }
}