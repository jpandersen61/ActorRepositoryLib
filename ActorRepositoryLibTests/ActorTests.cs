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
        const int ValidId = 42;
        const string NullName = null;
        const string EmptyName = "";
        const string TooShortName = "123";
        const string MinValidName = "1234";
        const string ValidName = "123456789";
        const int MinValidBirthYear = 1820;
        const int ValidBirthYear = 1972;

        [TestMethod()]
        public void ActorTest()
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = ValidBirthYear,
                Name = ValidName
            };

            Assert.AreEqual(ValidId, act.Id);
            Assert.AreEqual(ValidBirthYear, act.BirthYear);
            Assert.AreEqual(ValidName, act.Name);
        }

        [TestMethod()]
        [DataRow(MinValidName)]
        [DataRow(ValidName)]
        public void ValidateNameTestPositive(string name)
        {
            Actor act = new Actor() { Id = ValidId, 
                                      BirthYear = ValidBirthYear, 
                                      Name = name };
            act.ValidateName();    
        }

        [TestMethod()]
        [DataRow(TooShortName)]
        [DataRow(EmptyName)]
        public void ValidateNameTestArgumentOutOfRange(string name)
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = ValidBirthYear,
                Name = name
            };
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => act.ValidateName());
        }

        [TestMethod()]
        public void ValidateNameTestArgumentNullException()
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = ValidBirthYear,
                Name = NullName
            };

            Assert.ThrowsException<ArgumentNullException>(() => act.ValidateName());
        }

        [TestMethod()]
        [DataRow(MinValidBirthYear)]
        [DataRow(ValidBirthYear)]
        public void ValidateBirthYearTestPositive(int birthYear)
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = birthYear,
                Name = ValidName
            };

            act.ValidateBirthYear();
        }

        [TestMethod()]
        [DataRow(MinValidBirthYear-1)]
        [DataRow(MinValidBirthYear - 10)]
        public void ValidateBirthYearTestNegative(int birthYear)
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = birthYear,
                Name = ValidName
            };
           
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => act.ValidateBirthYear());
        }

       

        [TestMethod()]
        [DataRow(MinValidName, MinValidBirthYear)]
        [DataRow(MinValidName, ValidBirthYear)]
        [DataRow(ValidName, MinValidBirthYear)]
        [DataRow(ValidName, ValidBirthYear)]
        public void ValidateTestPostitive(string name, int birthYear)
        {
            Actor act = new Actor()
            {
                Id = ValidId,
                BirthYear = birthYear,
                Name = name
            };

            act.Validate();
        }
    }
}