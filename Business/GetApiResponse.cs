using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.DTO;
using Entities;


namespace Business
{
    public class GetApiResponse<T> where T : class
    {
        private readonly string _uri;

        public GetApiResponse(string apiBaseUri)
        {
            _uri = apiBaseUri;
        }

        public BookDetailDto GetBookFromAdlibris(string isbn, string path)
        {
            return GetInfoFromWebApi(path, isbn) as BookDetailDto;
        }

        private T GetInfoFromWebApi(string path, string id)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path + id),
                Timeout = TimeSpan.FromSeconds(60)
            };

            try
            {
                var response = httpClient.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }

            return null;
        }

        private async Task<T> UpdateInfoFromWebApi<TU>(string path, string id, TU model)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path + id),
                Timeout = TimeSpan.FromSeconds(60)
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
            catch (Exception exception)
            {
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }

            return null;
        }

        private async Task<bool> DeleteInfoFromWebApi(string path, string id)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path + id),
                Timeout = TimeSpan.FromSeconds(60)
            };

            try
            {
                await httpClient.DeleteAsync("");
                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }
            return false;
        }

        private async Task<bool> SaveInfoToWebApi<TU>(string path, TU model)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path),
                Timeout = TimeSpan.FromSeconds(8)
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
            catch (Exception exception)
            {
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }

            return false;
        }

        private async Task<List<T>> GetListInfoFromWebApi(string path, string isbn)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path),
                Timeout = TimeSpan.FromSeconds(60)
            };

            try
            {
                var response = await Task.Run(() => httpClient.GetAsync(isbn).Result);

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<T>>().Result.ToList();
                }
            }
            catch (Exception exception)
            {
              
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }

            return null;
        }

        private List<T> GetListInfoFromWebApiNotAsync(string path, string isbn)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri + path),
                Timeout = TimeSpan.FromSeconds(60)
            };

            try
            {
                var response = httpClient.GetAsync(isbn).Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<IEnumerable<T>>().Result.ToList();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("CAUGHT EXCEPTION:");
                Debug.WriteLine(exception);
            }

            return null;
        }

        public async Task<List<BookViewModel>> GetAllBooksFromDb(string path)
        {
            return await GetListInfoFromWebApi(path, "") as List<BookViewModel>;
        }

        public async Task<BookViewModel> GetBookFromDbById(string path, int id)
        {
            return await Task.Run(() => GetInfoFromWebApi(path, id.ToString()) as BookViewModel);
        }

        public async Task<bool> SaveBookToDb(string path, BookDetailDto model)
        {
            return await SaveInfoToWebApi(path, model);
        }

        public async Task<bool> SaveBookToDb(string path, BookViewModel model)
        {
            return await SaveInfoToWebApi(path, model);
        }

        public async Task<bool> IsBookInDb(string path, string id)
        {
            return bool.Parse(await GetResponseString(path + id));
        }

        private async Task<string> GetResponseString(string path)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_uri)
            };

            var response = await httpClient.GetAsync(path);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

        public async Task<BookViewModel> UpdateBook(string path, string id, BookViewModel book)
        {
            return await UpdateInfoFromWebApi(path, id, book) as BookViewModel;
        }

        public async Task<bool> DeleteBook(string path, string id)
        {
            return await DeleteInfoFromWebApi(path, id);
        }

        public async Task<List<NameViewModel>> GetAllNamesFromDb(string path)
        {
            return await GetListInfoFromWebApi(path, "") as List<NameViewModel>;
        }

        public List<NameViewModel> GetAllNamesFromDbNotAsync(string path)
        {
            return GetListInfoFromWebApiNotAsync(path, "") as List<NameViewModel>;
        }

        public async Task<List<PersonViewModel>> GetAllPersonsFromDb(string path)
        {
            return await GetListInfoFromWebApi(path, "") as List<PersonViewModel>;
        }

        public List<PersonViewModel> GetAllPersonsFromDbNotAsync(string path)
        {
            return GetListInfoFromWebApiNotAsync(path, "") as List<PersonViewModel>;
        }

        public async Task<bool> SaveNameToDb(string path, NameViewModel model)
        {
            return await SaveInfoToWebApi(path, model);
        }

        public async Task<SeriesModel> UpdateName(string path, string id, NameViewModel model)
        {
            return await UpdateInfoFromWebApi(path, id, model) as SeriesModel;
        }

        public async Task<NameViewModel> GetNameFromDbById(string path, int id)
        {
            return await Task.Run(() => GetInfoFromWebApi(path, id.ToString()) as NameViewModel);
        }

        public async Task<bool> DeleteName(string path, string id)
        {
            return await DeleteInfoFromWebApi(path, id);
        }

        public async Task<bool> SavePersonToDb(string path, PersonViewModel model)
        {
            return await SaveInfoToWebApi(path, model);
        }

        public async Task<PersonViewModel> GetPersonFromDbById(string path, int id)
        {
            return await Task.Run(() => GetInfoFromWebApi(path, id.ToString()) as PersonViewModel);
        }

        public async Task<PersonViewModel> UpdatePerson(string path, string id, PersonViewModel model)
        {
            return await UpdateInfoFromWebApi(path, id, model) as PersonViewModel;
        }

        public async Task<bool> DeletePerson(string path, string id)
        {
            return await DeleteInfoFromWebApi(path, id);
        }
    }
}