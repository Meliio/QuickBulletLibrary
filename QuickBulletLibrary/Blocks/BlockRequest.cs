using QuickBulletLibrary.Models;
using QuickBulletLibrary.Models.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QuickBulletLibrary.Blocks
{
    public class BlockRequest : BlockBase
    {
        private readonly Request _request;

        private const char SPLIT_SEPARATOR = ':';
        private const int SPLIT_COUNT = 2;

        public BlockRequest(Request request)
        {
            _request = request;
        }

        public override async Task Execute(BotData botData)
        {
            if (_request.IsDisable)
            {
                return;
            }

            var method = new HttpMethod(_request.Method);

            string url = ReplaceValues(_request.Url, botData);
            var uri = new Uri(url);

            using var httpRequestMessage = new HttpRequestMessage(method, uri);

            if (method == HttpMethod.Post)
            {
                string content = ReplaceValues(_request.Data, botData);
                httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, _request.ContentType);
            }

            foreach (var header in _request.Headers)
            {
                var headerSplit = header.Split(SPLIT_SEPARATOR, SPLIT_COUNT);
                if (headerSplit.Length == SPLIT_COUNT)
                {
                    string headerName = ReplaceValues(headerSplit[0], botData);
                    string headerValue = ReplaceValues(headerSplit[1].TrimStart(), botData);
                    httpRequestMessage.Headers.TryAddWithoutValidation(headerName, headerValue);
                }
            }

            var cookiesDictionary = new Dictionary<string, string>(botData.Request.Cookies);

            foreach (var cookie in _request.Cookies)
            {
                var cookieSplit = cookie.Split(SPLIT_SEPARATOR, SPLIT_COUNT);

                if (cookieSplit.Length == SPLIT_COUNT)
                {
                    string cookieName = ReplaceValues(cookieSplit[0], botData);
                    string cookieValue = ReplaceValues(cookieSplit[1].TrimStart(), botData);
                    if (cookiesDictionary.TryAdd(cookieName, cookieValue))
                    {
                        continue;
                    }
                    cookiesDictionary[cookieName] = cookieValue;
                }
            }

            if (cookiesDictionary.Count != 0)
            {
                httpRequestMessage.Headers.Add("cookie", string.Join("; ", cookiesDictionary.Select(c => $"{c.Key}={c.Value}")));
            }

            using var response = await botData.HttpClient.SendAsync(httpRequestMessage);

            int statusCode = (int)response.StatusCode;
            botData.Request.StatusCode = statusCode.ToString();
            botData.Request.Uri = response.RequestMessage.RequestUri.ToString();
            SetHeaderAndCookiesResponseInBotData(ref botData, response.Headers);
            if (_request.LoadContent)
            {
                botData.Request.Content = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
            }
        }

        private void SetHeaderAndCookiesResponseInBotData(ref BotData botData, HttpResponseHeaders httpResponseHeaders)
        {
            foreach (var httpResponseHeader in httpResponseHeaders)
            {
                string httpResponseHeaderValue = String.Join(", ", httpResponseHeader.Value);
                if (httpResponseHeader.Key == "Set-Cookie")
                {
                    var setCookieNameValueSplit = httpResponseHeaderValue.Substring(0, httpResponseHeaderValue.IndexOf(';')).Split('=');
                    if (botData.Request.Cookies.TryAdd(setCookieNameValueSplit[0], setCookieNameValueSplit[1]))
                    {
                        continue;
                    }
                    botData.Request.Cookies[setCookieNameValueSplit[0]] = setCookieNameValueSplit[1];
                }
                else if (botData.Request.Headers.TryAdd(httpResponseHeader.Key, httpResponseHeaderValue))
                {
                    continue;
                }
                botData.Request.Headers[httpResponseHeader.Key] = httpResponseHeaderValue;
            }
        }
    }
}
