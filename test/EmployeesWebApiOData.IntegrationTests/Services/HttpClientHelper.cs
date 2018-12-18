using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeesWebApiOData.IntegrationTests.Services
{
	public class HttpClientHelper
	{
		public HttpClientHelper(HttpClient httpHttpClient)
		{
			HttpClient = httpHttpClient;
		}

		public HttpClient HttpClient { get; }

		public async Task<T> DeleteAsync<T>(string path)
		{
			var response = await HttpClient.DeleteAsync(path).ConfigureAwait(false);
			return await GetContentAsync<T>(response);
		}

		public async Task<System.Net.HttpStatusCode> DeleteAsync(string path)
		{
			var response = await HttpClient.DeleteAsync(path);
			return response.StatusCode;
		}

		public async Task<T> GetAsync<T>(string path)
		{
			var response = await HttpClient.GetAsync(path);
			return await GetContentAsync<T>(response);
		}

		public async Task<System.Net.HttpStatusCode> GetAsync(string path)
		{
			var response = await HttpClient.GetAsync(path);
			return response.StatusCode;
		}

		public async Task<List<T>> GetListAsync<T>(string path)
		{
			var response = await HttpClient.GetAsync(path);
			return await GetListContentAsync<T>(response);
		}

		public async Task<TOut> PatchAsync<TIn, TOut>(string path, TIn content)
		{
			var json = content == null ?
				null :
				new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json-patch+json");
			var response = await HttpClient.PatchAsync(path, json).ConfigureAwait(false);
			return await GetContentAsync<TOut>(response);
		}

		public async Task<T> PostAsync<T>(string path, T content)
		{
			return await PostAsync<T, T>(path, content);
		}

		public async Task<TOut> PostAsync<TIn, TOut>(string path, TIn content)
		{
			var json = content == null ? null : new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
			var response = await HttpClient.PostAsync(path, json).ConfigureAwait(false);
			return await GetContentAsync<TOut>(response);
		}

		public async Task<HttpResponseMessage> PostAsyncResponse<T>(string path, T content)
		{
			var json = content == null ?
				null :
				new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
			return await HttpClient.PostAsync(path, json).ConfigureAwait(false);
		}

		public async Task<TOut> PutAsync<TIn, TOut>(string path, TIn content)
		{
			var json = content == null ? null : new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
			var response = await HttpClient.PutAsync(path, json).ConfigureAwait(false);
			return await GetContentAsync<TOut>(response);
		}

		private async Task<T> GetContentAsync<T>(HttpResponseMessage response)
		{
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<T>(responseString);
			return result;
		}

		private async Task<List<T>> GetListContentAsync<T>(HttpResponseMessage response)
		{
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<ODataResponse<T>>(responseString);
			return result.Value;
		}
	}
}
