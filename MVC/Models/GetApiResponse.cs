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

		public BookModel Book { get; set; }

		public List<BookViewModel> Books { get; set; }

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

		private async Task<T> DeleteInfoFromWebApi<U>(string path, string id, U model)
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
				return response.Content.ReadAsAsync<T>().Result;
			}
			catch (Exception ex)
			{
			}
			return null;
		}

		private Uri SaveInfoToWebApi<U>(string path, U model)
		{
			var httpClient = new System.Net.Http.HttpClient
			{
				BaseAddress = new Uri(baseUri + path),
				Timeout = TimeSpan.FromSeconds(8),
			};

			try
			{
				var response = httpClient.PostAsXmlAsync("", model).Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
				{
					// Get the URI of the created resource.
					return response.Headers.Location;
				}
			}
			catch (Exception ex)
			{
			}

			return null;
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
		public List<SeriesModel> GetAllSeriesFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<SeriesModel>;
		}
		public List<BookModel> GetAllBooksFromDb(string path)
		{
			return GetListInfoFromWebApi(path, "") as List<BookModel>;
		}

		public BookModel GetBookFromDbById(string path, int id)
		{
			Book = GetInfoFromWebApi(path, id.ToString()) as BookModel;
			return Book;
		}

		public void SaveBookToDb(string path, BookDetailDTO model)
		{
			SaveInfoToWebApi(path, model);
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

		public async Task<BookViewModel> UpdateBook(string path, string id, BookViewModel book)
		{
			return await UpdateInfoFromWebApi(path, id, book) as BookViewModel;
		}

		public async Task<bool> DeleteBook(string path, string id, BookViewModel book)
		{
			var result = await DeleteInfoFromWebApi(path, id, book);
			return true;
		}
	}
}