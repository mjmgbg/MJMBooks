using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Models
{
	public class GetApiResponse<T> where T : class
	{
		private readonly string baseUri = ConfigurationManager.AppSettings["apiBaseUri"];

		public BookDetailDTO BookForAdLibris { get; set; }

		public BookDetailDTO GetBookFromAdlibris(string isbn, string path)
		{
			BookForAdLibris = GetInfoFromWebApi(path, isbn) as BookDetailDTO;
			return BookForAdLibris;
		}

		private T GetInfoFromWebApi(string path, string id)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path + id),
				Timeout = TimeSpan.FromSeconds(60),
			};
			var test = baseUri + path + id;
			try
			{
				HttpResponseMessage response = httpClient.GetAsync("").Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
				{
					return response.Content.ReadAsAsync<T>().Result;
				}
			}
			catch (Exception ex)
			{
			}

			return null;
		}

		private async Task<T> UpdateInfoFromWebApi<U>(string path, string id, U model)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path + id),
				Timeout = TimeSpan.FromSeconds(60),
			};

			try
			{
				var response = await httpClient.PutAsXmlAsync("", model);
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
				{
					return response.Content.ReadAsAsync<T>().Result;
				}
			}
			catch (Exception ex)
			{
			}

			return null;
		}

		private async Task<bool> DeleteInfoFromWebApi<U>(string path, string id, U model)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path + id),
				Timeout = TimeSpan.FromSeconds(60),
			};
			var test = baseUri + path + id;
			try
			{
				var response = await httpClient.DeleteAsync("");
				return true;
			}
			catch (Exception ex)
			{
			}
			return false;
		}

		private async Task<bool> SaveInfoToWebApi<U>(string path, U model)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path),
				Timeout = TimeSpan.FromSeconds(8),
			};

			try
			{
				var response = await httpClient.PostAsXmlAsync("", model);
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
				{
					return true;
					
				}
			}
				
			catch (Exception ex)
			{
			}

			return false;
		}

		private List<T> GetListInfoFromWebApi(string path, string isbn)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path),
				Timeout = TimeSpan.FromSeconds(60),
			};

			try
			{
				HttpResponseMessage response = httpClient.GetAsync(isbn).Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
				{
					return response.Content.ReadAsAsync<IEnumerable<T>>().Result.ToList();
				}
			}
			catch (Exception ex)
			{
			}

			return null;
		}
		
		public List<BookModel> GetAllBooksFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<BookModel>;
		}

		public BookModel GetBookFromDbById(string path, int id)
		{
			return GetInfoFromWebApi(path, id.ToString()) as BookModel;			
		}

		public async Task<bool> SaveBookToDb(string path, BookDetailDTO model)
		{
			return await SaveInfoToWebApi(path, model);
		}

		public async Task<bool> IsBookInDb(string path, string id)
		{
			return bool.Parse(await GetResponseString(path + id));
		}

		public async Task<string> GetResponseString(string path)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri),
			};
			var test = baseUri + path;

			var response = await httpClient.GetAsync(path);
			var contents = await response.Content.ReadAsStringAsync();

			return contents;
		}

		public async Task<BookViewModel> UpdateBook(string path, string id, BookModel book)
		{
			return await UpdateInfoFromWebApi(path, id, book) as BookViewModel;
		}

		public async Task<bool> DeleteBook(string path, string id, BookModel book)
		{
			return await DeleteInfoFromWebApi(path, id, book);
			
		}
		public List<SeriesModel> GetAllSeriesFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<SeriesModel>;
		}

		public List<AuthorModel> GetAllAuthorsFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<AuthorModel>;
		}
		public List<ReaderModel> GetAllReadersFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<ReaderModel>;
		}

		public async Task<bool> SaveSerieToDb(string path, SeriesModel model)
		{
			return await SaveInfoToWebApi(path, model);
		}

		public async Task<SeriesModel> UpdateSerie(string path, string id, SeriesModel model)
		{
			return await UpdateInfoFromWebApi(path, id, model) as SeriesModel;
		}

		public SeriesModel GetSerieFromDbById(string path, int id)
		{
			return GetInfoFromWebApi(path, id.ToString()) as SeriesModel;
		}

		public async Task<bool> DeleteSerie(string path, string id, SeriesModel serie)
		{
			return await DeleteInfoFromWebApi(path, id, serie);
		}

		public async Task<bool> SaveReaderToDb(string path, ReaderModel model)
		{
			return await SaveInfoToWebApi(path, model);
		}

		public ReaderModel GetReaderFromDbById(string path, int id)
		{
			return GetInfoFromWebApi(path, id.ToString()) as ReaderModel;
		}

		public async Task<ReaderModel> UpdateReader(string path, string id, ReaderModel model)
		{
			return await UpdateInfoFromWebApi(path, id, model) as ReaderModel;
		}
		public async Task<bool> DeleteReader(string path, string id, ReaderModel reader)
		{
			return await DeleteInfoFromWebApi(path, id, reader);
		}

		public async Task<bool> SaveAuthorToDb(string path, AuthorModel model)
		{
			return await SaveInfoToWebApi(path, model);
		}
		
		public AuthorModel GetAuthorFromDbById(string path, int id)
		{
			return GetInfoFromWebApi(path, id.ToString()) as AuthorModel;
		}

		public async Task<AuthorModel> UpdateAuthor(string path, string id, AuthorModel model)
		{
			return await UpdateInfoFromWebApi(path, id, model) as AuthorModel;
		}
		public async Task<bool> DeleteAuthor(string path, string id, AuthorModel model)
		{
			return await DeleteInfoFromWebApi(path, id, model);
		}

		
	}
}