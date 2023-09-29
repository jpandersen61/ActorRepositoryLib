using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ActorRepositoryLib
{
    public interface IActorsRepository
    {
        public IEnumerable<Actor> Get(int? birthYearBefore = null, 
                                      int? birthYearAfter = null, 
                                      string? orderBy = null, 
                                      bool descending = false); 
        public Actor Add(Actor actor); 
        public Actor? Delete(int id); 
        public Actor? Update(int id, Actor data); //Opdaterer actor objektet med det angivne id, med de data der er i data parameteren. Returnerer det opdaterede actor objekt - eller null, hvis der ikke er noget actor objekt med det angivne id
        
    }
}
