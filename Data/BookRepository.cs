using Business;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data
{
	public class BookRepository
	{
		private DBContextBook context;

		public BookRepository(DbContext dbContext)
		{
			context = dbContext as DBContextBook;
		}

		public void Add(string connectionString, string path, BookDetailDTO book)
		{
			string textColor = string.Empty, bgColor = string.Empty, textColorSecond = string.Empty;
			//Get colors that will be used in list display
			var topFiveColor = Helpers.GetThreeColors(connectionString, path, book.ImagePath);
			if (topFiveColor != null)
			{
				textColor = topFiveColor[1].R.ToString() + "," + topFiveColor[1].G.ToString() + "," + topFiveColor[1].B.ToString();
				bgColor = topFiveColor[0].R.ToString() + "," + topFiveColor[0].G.ToString() + "," + topFiveColor[0].B.ToString();
				textColorSecond = topFiveColor[2].R.ToString() + "," + topFiveColor[2].G.ToString() + "," + topFiveColor[2].B.ToString();
				if (int.Parse(topFiveColor[0].R.ToString()) < 50)
				{
					textColor = textColorSecond;
					textColorSecond = bgColor;
				}
			}
			var series = new SeriesModel();
			var seriesPart = new SeriesPartNumberModel();
			if (book.SeriesName != string.Empty && book.SeriesName != null)
			{
				series = GetSeriesByName(book.SeriesName);
				seriesPart = GetSeriesPartByNameAndNumber(series.Name, GetSeriesNumberFromString(book.SeriesName));
			}
			else
			{
				series = null;
				seriesPart = null;
			}

			var readers = new List<ReaderModel>();

			if (book.ReaderNames != string.Empty && book.ReaderNames != null)
			{
				readers = GetReadersByName(book.ReaderNames).ToList();
			}
			else
			{
				readers = null;
			}

			BookModel newBook = new BookModel
			{
				Title = book.Title,
				Description = book.Description,
				ISBN = book.ISBN,
				Series = series,
				SeriesPart = seriesPart,
				PublishingDate = Convert.ToDateTime(book.PublishingDate),
				ImagePath = book.ImagePath,
				Readers = readers,
				Publisher = GetPublisherByName(book.PublisherName),
				Authors = GetAuthorsByNames(book.AuthorNames),
				Genres = GetGenresByName(book.GenreNames),
				Language = GetLanguageByName(book.Language),
				TextColor = textColor,
				BgColor = bgColor,
				TextColorSecond = textColorSecond,
			};
			context.Books.Add(newBook);
			context.SaveChanges();
		}

		public bool IsBookInDB(string isbn)
		{
			var book = context.Books.Where(b => b.ISBN == isbn).FirstOrDefault();
			if (book != null)
			{
				return true;
			}
			return false;
		}

		public List<BookModel> GetAllBooks()
		{
			var books = context.Books.AsEnumerable().Select(b => new BookModel
			{
				Title = b.Title,
				Id = b.Id,
				Authors = GetBookAuthors(b.Id),
				Description = b.Description,
				LanguageId = b.LanguageId,
				PublisherId = b.PublisherId,
				Series = b.Series,
				SeriesPart = b.SeriesPart,
				Readers = GetBookReaders(b.Id),
				Genres = GetBookGenres(b.Id),
				ISBN = b.ISBN,
				ImagePath = b.ImagePath,
				Language = b.Language,
				Publisher = b.Publisher,
				PublishingDate = b.PublishingDate,
				BgColor = b.BgColor,
				TextColor = b.TextColor,
				TextColorSecond = b.TextColorSecond,
			}).ToList();
			return books;
		}

		public BookModel GetBookById(int id)
		{
			var book = context.Books.AsEnumerable().Where(b => b.Id == id).Select(b => new BookModel
			{
				Title = b.Title,
				Id = b.Id,
				Authors = GetBookAuthors(b.Id),
				Readers = GetBookReaders(b.Id),
				Series = b.Series,
				SeriesPart = b.SeriesPart,
				Description = b.Description,
				Genres = GetBookGenres(b.Id),
				ISBN = b.ISBN,
				LanguageId = b.LanguageId,
				PublisherId = b.PublisherId,
				PublishingDate = b.PublishingDate,
				Publisher = b.Publisher,
				ImagePath = b.ImagePath,
				Language = b.Language,
			}).FirstOrDefault();
			return book;
		}

		private List<GenreModel> GetBookGenres(int id)
		{
			return context.Genres.SelectMany(g => g.Books.Where(b => b.Id == id), (a, b) => a).AsEnumerable().Select(c => new GenreModel
			{
				Id = c.Id,
				Name = c.Name
			}).ToList();
		}

		private List<AuthorModel> GetBookAuthors(int id)
		{
			var baseList = context.Authors.SelectMany(a => a.Books.Where(b => b.Id == id), (a, b) => a).AsEnumerable().Select(c => new AuthorModel
			{
				FirstName = c.FirstName,
				LastName = c.LastName,
				Id = c.Id
			}).ToList();

			return baseList;
		}

		private List<ReaderModel> GetBookReaders(int id)
		{
			return context.Readers.SelectMany(a => a.Books.Where(b => b.Id == id), (a, b) => a).AsEnumerable().Select(c => new ReaderModel
			{
				FirstName = c.FirstName,
				LastName = c.LastName,
				Id = c.Id
			}).ToList();
		}

		private LanguageModel GetLanguageByName(string name)
		{
			var language = context.Languages.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (language != null)
			{
				return language;
			}
			else
			{
				return AddLanguageToDB(name);
			}
		}

		private LanguageModel AddLanguageToDB(string name)
		{
			var newLanguage = new LanguageModel
			{
				Name = name,
			};
			context.Languages.Add(newLanguage);
			context.SaveChanges();
			return newLanguage;
		}

		private List<GenreModel> GetGenresByName(string genres)
		{
			var list = new List<GenreModel>();
			if (genres != null)
			{
				string[] names = genres.Split(';');
				foreach (var item in names)
				{
					list.Add(GetGenreByName(item));
				}
			}

			return list;
		}

		private GenreModel GetGenreByName(string name)
		{
			var genre = context.Genres.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (genre != null)
			{
				return genre;
			}
			else
			{
				return AddGenreToDB(name);
			}
		}

		private GenreModel AddGenreToDB(string name)
		{
			var newGenre = new GenreModel
			{
				Name = name,
			};
			context.Genres.Add(newGenre);
			context.SaveChanges();
			return newGenre;
		}

		private PublisherModel GetPublisherByName(string name)
		{
			var publisher = context.Publishers.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (publisher != null)
			{
				return publisher;
			}
			else
			{
				return AddPublisherToDB(name);
			}
		}

		private PublisherModel AddPublisherToDB(string name)
		{
			var newPublisher = new PublisherModel
			{
				Name = name,
			};
			context.Publishers.Add(newPublisher);
			context.SaveChanges();
			return newPublisher;
		}

		private SeriesModel GetSeriesByName(string name)
		{
			var series = context.Series.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (series != null)
			{
				return series;
			}
			else
			{
				return AddSeriesToDB(name);
			}
		}

		private string GetSeriesNumberFromString(string name)
		{	//Split name and part
			string justNumbersArray = new String(name.Where(Char.IsDigit).ToArray());
			return string.Join("", justNumbersArray);
		}

		private SeriesModel AddSeriesToDB(string name)
		{
			var seriesId = 0;
			string justNumbers = GetSeriesNumberFromString(name);
			name = name.Replace(justNumbers, "").Trim();
			var series = context.Series.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (series != null)
			{
				seriesId = series.Id;
			}
			else
			{
				var newSeries = new SeriesModel
				{
					Name = name,
				};
				context.Series.Add(newSeries);
				context.SaveChanges();
				seriesId = newSeries.Id;
				series = newSeries;
			}

			AddSeriesPartToDB(seriesId, justNumbers);

			return series;
		}

		private SeriesPartNumberModel GetSeriesPartByNameAndNumber(string name, string number)
		{
			var seriesId = 0;
			var partNumber = int.Parse(number);
			var series = context.Series.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (series != null)
			{
				seriesId = series.Id;
			}
			var seriesPart = context.SeriesPart.Where(p => p.SeriesId == seriesId && p.PartNumber == partNumber).FirstOrDefault();
			return seriesPart;
		}

		private void AddSeriesPartToDB(int seriesId, string number)
		{
			var newSeriesPart = new SeriesPartNumberModel
				  {
					  SeriesId = seriesId,
					  PartNumber = int.Parse(number)
				  };
			context.SeriesPart.Add(newSeriesPart);
			context.SaveChanges();
		}

		private List<AuthorModel> GetAuthorsByNames(string authors)
		{
			var list = new List<AuthorModel>();
			string[] names = authors.Split(';');
			foreach (var item in names)
			{
				list.Add(GetAuthorByName(item));
			}
			return list;
		}

		private AuthorModel GetAuthorByName(string authors)
		{
			string[] names = authors.Split(' ');
			string fName = string.Empty;
			string lName = names[names.Count() - 1].ToLower();
			fName = GetFirstName(names);

			var author = context.Authors.Where(a => a.FirstName.ToLower() == fName && a.LastName.ToLower() == lName).FirstOrDefault();
			var list = new List<AuthorModel>();
			if (author != null)
			{
				list.Add(author);
			}
			else
			{
				list.Add(AddAuthorToDB(fName, lName));
			};
			return list[0];
		}

		private static string GetFirstName(string[] names)
		{
			string fName = string.Empty;

			for (int i = 0; i < names.Count(); i++)
			{
				if (i < names.Count() - 1)
				{
					fName += names[i] + " ";
				}
			}
			return fName.Trim();
		}

		private AuthorModel AddAuthorToDB(string fName, string lName)
		{
			fName = fName.First().ToString().ToUpper() + fName.Substring(1);
			lName = lName.First().ToString().ToUpper() + lName.Substring(1);

			fName = FixDoubleBarrelledName(fName);
			lName = FixDoubleBarrelledName(lName);

			var newAuthor = new AuthorModel
			{
				FirstName = fName,
				LastName = lName,
			};
			context.Authors.Add(newAuthor);
			context.SaveChanges();
			return newAuthor;
		}

		private ICollection<ReaderModel> GetReadersByName(string readers)
		{
			//TODO: Handle more then one reader
			string[] names = readers.Split(' ');
			string fName = string.Empty;
			string lName = names[names.Count() - 1].ToLower();
			fName = GetFirstName(names);

			var reader = context.Readers.Where(a => a.FirstName.ToLower() == fName && a.LastName.ToLower() == lName).FirstOrDefault();
			var list = new List<ReaderModel>();
			if (reader != null)
			{
				list.Add(reader);
			}
			else
			{
				list.Add(AddReaderToDB(fName, lName));
			};
			return list;
		}

		private ReaderModel AddReaderToDB(string fName, string lName)
		{
			fName = fName.First().ToString().ToUpper() + fName.Substring(1);
			lName = lName.First().ToString().ToUpper() + lName.Substring(1);

			fName = FixDoubleBarrelledName(fName);
			lName = FixDoubleBarrelledName(lName);
			var newReader = new ReaderModel
			{
				FirstName = fName,
				LastName = lName,
			};
			context.Readers.Add(newReader);
			context.SaveChanges();
			return newReader;
		}

		private string FixDoubleBarrelledName(string name)
		{
			if (name.IndexOf("-") > -1)
			{
				string[] temp = name.Split('-');
				name = temp[0] + "-" + temp[1].First().ToString().ToUpper() + temp[1].Substring(1);
			}

			return name;
		}

		public void Edit(BookModel book)
		{
			var bookToEdit = context.Books.Find(book.Id);
			bookToEdit = SaveEditedBook(book, bookToEdit);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			var bookToDelete = context.Books.Find(id);
			context.Books.Remove(bookToDelete);
			context.SaveChanges();
		}

		public BookModel SaveEditedBook(BookModel oldBook, BookModel book)
		{
			book.Id = oldBook.Id;
			book.Authors = ConvertModelAuthorToDBAuthor(oldBook.Id);
			book.Publisher = GetPublisherByName(book.Publisher.Name);
			book.Genres = ConvertModelGenreToDBGenre(oldBook.Id);
			if (oldBook.Readers != null)
			{
				book.Readers = ConvertModelReaderToDBReader(oldBook.Id);
			}
			book.Language = GetLanguageByName(book.Language.Name);
			if (oldBook.Series != null)
			{
				book.Series = GetSeriesByName(oldBook.Series.Name);
			}
			book.Title = oldBook.Title;
			book.Description = oldBook.Description;
			book.ImagePath = oldBook.ImagePath;
			book.ISBN = oldBook.ISBN;
			book.PublishingDate = oldBook.PublishingDate;
			return book;
		}

		private static string ConvertGenresToString(BookModel model)
		{
			string genres = string.Empty;
			foreach (var item in model.Genres)
			{
				genres += item.Name + ";";
			}
			genres = genres.Substring(0, genres.Length - 1);
			return genres;
		}

		private static string ConvertAuthorsToString(BookModel model)
		{
			string authors = string.Empty;

			foreach (var item in model.Authors)
			{
				authors += item.DisplayFullName + ";";
			}
			authors = authors.Substring(0, authors.Length - 1);
			return authors;
		}

		private static string ConvertReadersToString(BookModel model)
		{
			string readers = string.Empty;

			foreach (var item in model.Readers)
			{
				readers += item.DisplayFullName + ";";
			}
			readers = readers.Substring(0, readers.Length - 1);
			return readers;
		}

		public List<AuthorModel> ConvertModelAuthorToDBAuthor(int id)
		{
			return context.Books.Where(b => b.Id == id).FirstOrDefault().Authors.ToList();
		}

		public List<ReaderModel> ConvertModelReaderToDBReader(int id)
		{
			return context.Books.Where(b => b.Id == id).FirstOrDefault().Readers.ToList();
		}

		public List<GenreModel> ConvertModelGenreToDBGenre(int id)
		{
			return context.Books.Where(b => b.Id == id).FirstOrDefault().Genres.ToList();
		}
	}
}