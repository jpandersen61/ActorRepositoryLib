using Microsoft.Identity.Client;
using System.Xml.Linq;

namespace ActorRepositoryLib
{
    public class Actor
    {
        //Some test input
        public const int ValidId = 42;
        public const string NullName = null;
        public const string EmptyName = "";
        public const string TooShortName = "123";
        public const string MinValidName = "1234";
        public const string ValidName = "123456789";
        public const int MinValidBirthYear = 1820;
        public const int ValidBirthYear = 1972;

        public int Id {get; set;} //Id, et tal
        public string Name { get; set; } //en string, må ikke være null, og mindst 4 tegn langt
    
        public int BirthYear { get; set; } // >= 1820

        public Actor()
        {
           
        }

        public void ValidateName()
        {
            if (Name == null) 
            {
                throw new ArgumentNullException("Name must be non-null");            
            }
            if (Name.Length >= 4)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException("Name must be at least 4 long");
            }
        }

        public void ValidateBirthYear()
        {
            if (BirthYear >= MinValidBirthYear)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException("Birth year must be at least 1820");
            }
        }

        public void Validate()
        {
            ValidateBirthYear();
            ValidateName();
        }
    }
}