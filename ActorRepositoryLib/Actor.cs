using System.Xml.Linq;

namespace ActorRepositoryLib
{
    public class Actor
    {
        public int Id {get; set;} //Id, et tal
        public string Name { get; set; } //en string, må ikke være null, og mindst 4 tegn langt
    
        public int BirthYear { get; set; } // >= 1820
    
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
            if (BirthYear >= 1820)
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