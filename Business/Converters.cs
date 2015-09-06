using System.Collections.Generic;
using System.Linq;
using Business.DTO;
using Entities;

namespace Business
{
    public static class Converters
    {
        public static List<PersonViewModel> ConvertPersonDbToModel<T>(List<T> entity)
        {
            return entity.OfType<IPerson>().Select(person => new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            }).ToList();
        }

        public static PersonViewModel ConvertPersonDbToModel<T>(T entity)
        {
            var model = entity as IPerson;
            if (model != null)
            {
                return new PersonViewModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
            }
            return default(PersonViewModel);
        }

        public static T ConvertModelPersonToDb<T>(PersonViewModel entity)
            where T : IPerson, new()
        {
            var model = entity as IPerson;
            if (model != null)
            {
                return new T
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
            }
            return default(T);
        }

        public static List<T> ConvertModelPersonToDb<T>(List<PersonViewModel> entity)
            where T : IPerson, new()
        {
            return entity.OfType<IPerson>().Select(person => new T
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            }).ToList();
        }

        public static T ConvertLookUpModelToDb<T>(NameViewModel entity)
            where T : IName, new()
        {
            var model = entity as IName;
            if (model != null)
            {
                return new T
                {
                    Id = model.Id,
                    Name = model.Name
                };
            }
            return default(T);
        }


        public static List<NameViewModel> ConvertLookUpDbToModel<T>(List<T> entity)
            where T : IName, new()
        {
            var newlist = new List<NameViewModel>();

            foreach (var item in entity)
            {
                var lookup = item as IName;
                if (lookup != null)
                {
                    newlist.Add(ConvertLookUpDbToModel(item));
                }
            }

            return newlist;
        }

        public static NameViewModel ConvertLookUpDbToModel<T>(T entity)
            where T : IName
        {
            var model = entity as IName;
            if (model != null)
            {
                return new NameViewModel
                {
                    Id = model.Id,
                    Name = model.Name
                };
            }
            return default(NameViewModel);
        }
    }
}