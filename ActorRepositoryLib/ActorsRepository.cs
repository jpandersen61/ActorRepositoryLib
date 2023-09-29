using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorRepositoryLib 
{
    public class ActorsRepository : IActorsRepository
    {
        private List<Actor> _actors = new List<Actor>();
        private int _nextID = 0;
        public ActorsRepository() { }

        public Actor Add(Actor actor)
        {
            actor.Validate();
            _nextID++;
            actor.Id = _nextID;
            _actors.Add(actor);
            return actor;
        }

        public Actor? Delete(int id)
        {
            Actor? deletedActor = _actors.Find(a => a.Id == id);
            if (deletedActor != null) 
            { 
                _actors.Remove(deletedActor);
            }
            return deletedActor;
        }

        public IEnumerable<Actor> Get(int? birthYearBefore = null,
                                      int? birthYearAfter = null,
                                      string? orderBy = null,
                                      bool descending = false)
        {
            IEnumerable<Actor> result = new List<Actor>(_actors);
            
            if (birthYearBefore != null)
            {
                result = result.Where(a => a.BirthYear < birthYearBefore);
            }

            if (birthYearAfter != null)
            {
                result = result.Where(a => a.BirthYear > birthYearAfter);
            }

            
            if ( orderBy != null)
            {
                if (descending)
                {
                    switch (orderBy.ToLower())
                    {
                        case "id":
                            result = result.OrderByDescending(a => a.Id);
                            break;
                        case "name":
                            result = result.OrderByDescending(a => a.Name);
                            break;
                        case "birthyear":
                            result = result.OrderByDescending(a => a.BirthYear);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Invalid orderby specifier: {orderBy.ToLower()}");
                    }
                }
                else
                {
                    switch (orderBy.ToLower())
                    {
                        case "id":
                            result = result.OrderBy(a => a.Id);
                            break;
                        case "name":
                            result = result.OrderBy(a => a.Name);
                            break;
                        case "birthyear":
                            result = result.OrderBy(a => a.BirthYear);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"Invalid orderby specifier: {orderBy.ToLower()}");

                    }
                }
            }
            

            return result;       
        }

        public Actor? Update(int id, Actor data)
        {
            Actor? updatedActor = null;

            data.Validate();
            updatedActor = _actors.Find(a => a.Id == id);
            if (updatedActor != null)
            {
                updatedActor.BirthYear = data.BirthYear;
                updatedActor.Name = data.Name;
            }
            return updatedActor;
        }
    }
}
