using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Business.DTO;
using Entities;

namespace Data
{
    public static class AdlibrisHelper
    {
       

        private static DbContextBook _context;

        public static DbContextBook Context
        {
            get { return _context; }
            set { _context = value; }
        }


        public static void AddFromAdlibris(string connectionString, BookDetailDto book)
        {
            string textColor, bgColor, textColorSecond;
            Helpers.SetColors(connectionString, book.ImagePath, out textColor, out bgColor, out textColorSecond);
            var seriesName = book.SeriesName.Replace(GetSeriesNumberFromString(book.SeriesName),"").Trim();
            var serie = SetNameAndAddToDb<SeriesModel>(seriesName);
            var seriePart = SetSeriesPartByNameAndNumber(serie.Name, GetSeriesNumberFromString(book.SeriesName));
            var readers = SetPersonByNames<ReaderModel>(book.ReaderNames);
            var publisher = SetNameAndAddToDb<PublisherModel>(book.PublisherName);
            var authors = SetPersonByNames<AuthorModel>(book.AuthorNames);
            var genres = SetNameAndAddToDbList<GenreModel>(book.GenreNames);
            var language = SetNameAndAddToDb<LanguageModel>(book.Language);

            var newBook = new BookModel
            {
                Title = book.Title,
                Description = book.Description,
                Isbn = book.Isbn,
                Series = serie,
                SeriesId = serie.Id,
                SeriesPart = seriePart,
                SeriesPartId = seriePart.Id,
                PublishingDate = Convert.ToDateTime(book.PublishingDate),
                ImagePath = book.ImagePath,
                Readers = readers,
                PublisherId = publisher.Id,
                Publisher = publisher,
                Authors = authors,
                Genres = genres,
                LanguageId = language.Id,
                Language = language,
                TextColor = textColor,
                BgColor = bgColor,
                TextColorSecond = textColorSecond,
                IsRead = book.IsRead,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            _context.Books.Add(newBook);
        }


        private static T1 SetNameAndAddToDb<T1>(string name)
            where T1 : class, IName, new()
        {
            var dbSet = _context.Set<T1>();

            var entity = dbSet.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            return entity ?? AddNameToDb<T1>(name);
        }

        private static T1 AddNameToDb<T1>(string name)
            where T1 : class, IName, new()
        {
            var newEntity = new T1
            {
                Name = name
            };
            var dbSet = _context.Set<T1>();
            dbSet.Add(newEntity);

            return newEntity;
        }

        private static List<T1> SetNameAndAddToDbList<T1>(string theNames)
            where T1 : class, IName, new()
        {
            var list = new List<T1>();
            if (theNames == null) return list;
            var names = theNames.Split(';');
            list.AddRange(names.Select(item => SetNameAndAddToDb<T1>(item)));
            return list;
        }


        private static T1 SetPersonAndAddToDb<T1>(string theNames)
            where T1 : class, IPerson, new()
        {
            var names = theNames.Split(' ');
            var lName = names[names.Length - 1].ToLower();
            var fName = GetFirstName(names);
            var dbSet = _context.Set<T1>();

            var entity =
                dbSet.FirstOrDefault(e => e.FirstName.ToLower() == fName && e.LastName.ToLower() == lName);
            var list = new List<T1> {entity ?? AddPersonToDb<T1>(fName, lName)};

            return list[0];
        }

        private static T1 AddPersonToDb<T1>(string fName, string lName)
            where T1 : class, IPerson, new()
        {
            fName = fName.First().ToString().ToUpper() + fName.Substring(1);
            lName = lName.First().ToString().ToUpper() + lName.Substring(1);

            fName = FixDoubleBarrelledName(fName);
            lName = FixDoubleBarrelledName(lName);

            var newEntity = new T1
            {
                FirstName = fName,
                LastName = lName
            };
            var dbSet = _context.Set<T1>();
            dbSet.Add(newEntity);
            return newEntity;
        }

        private static List<T1> SetPersonByNames<T1>(string authors)
            where T1 : class, IPerson, new()
        {
            var names = authors.Split(';');
            return names.Select(item => SetPersonAndAddToDb<T1>(item)).ToList();
        }

        private static string FixDoubleBarrelledName(string name)
        {
            if (name.IndexOf("-", StringComparison.Ordinal) <= -1) return name;
            var temp = name.Split('-');
            name = temp[0] + "-" + temp[1].First().ToString().ToUpper() + temp[1].Substring(1);

            return name;
        }

        private static string GetFirstName(string[] names)
        {
            var fName = string.Empty;

            for (var i = 0; i < names.Length; i++)
            {
                if (i < names.Length - 1)
                {
                    fName += names[i] + " ";
                }
            }
            return fName.Trim();
        }

        private static string GetSeriesNumberFromString(string name)
        {
            //Split name and part
            var justNumbersArray = new string(name.Where(char.IsDigit).ToArray());
            return string.Join("", justNumbersArray);
        }

        private static SeriesPartNumberModel SetSeriesPartByNameAndNumber(string name, string number)
        {
            name = name.Replace(number, "").Trim();

            var seriesId = 0;
            var series =
                _context.Series.FirstOrDefault(p => p.Name.ToLower()==name.ToLower());
            if (series != null)
            {
                seriesId = series.Id;
            }
            var seriesPart = _context.SeriesPart.FirstOrDefault(p => p.SeriesId == seriesId && p.Name == number);

            if (seriesPart != null) return seriesPart;
            var newSeriesId = _context.Series.Count() + 1;
            return new SeriesPartNumberModel
            {
                Name = name,
                SeriesId = newSeriesId,
                    
            };
        }

        //TODO: Implement when seriePart is done.
        /*
                private void AddSeriesPartToDb(int seriesId, string number)
                {
                    var newSeriesPart = new SeriesPartNumberModel
                    {
                        SeriesId = seriesId,
                        Name = number
                    };
                    Context.SeriesPart.Add(newSeriesPart);
                }
        */

       
    }
}