using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TrainAPI.Helpers;

namespace TrainAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestJSON = await FormatRequestAsync(context);
            logger.LogInformation(requestJSON);



            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                var responseJSON = await FormatResponseAsync(context);
                logger.LogInformation(responseJSON);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }


        private async Task<string> FormatRequestAsync(HttpContext context)
        {
            var remoteIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var id = context.Request.HttpContext.TraceIdentifier;
            var method = context.Request.Method;
            var url = context.Request.Path;


            context.Request.EnableRewind();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Seek(0, SeekOrigin.Begin);

            return ParseRequestToJSON(requestBody, id, remoteIpAddress, url, method);
        }
        private string ParseRequestToJSON(string requestBody, string id, string remoteIpAddress, string url, string method)
        {
            RequestJSON requestToSerialize;
            string requestJSON;
            try
            {
                var requestBodyObject = JToken.Parse(requestBody);
                requestToSerialize = new RequestJSON(DateTime.Now.ToString(new CultureInfo("ru-RU")), id, remoteIpAddress, url, method, requestBodyObject);
                requestJSON = JsonConvert.SerializeObject(requestToSerialize);
            }
            catch (Exception e)
            {
                requestToSerialize = new RequestJSON(DateTime.Now.ToString(new CultureInfo("ru-RU")), id, remoteIpAddress, url, method, requestBody);
                requestJSON = JsonConvert.SerializeObject(requestToSerialize);
            }
            return requestJSON;
        }
        private async Task<string> FormatResponseAsync(HttpContext context)
        {
            var remoteIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var id = context.Request.HttpContext.TraceIdentifier;
            var statuscode = context.Response.StatusCode;


            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            return ParseResponseToJSON(response, id, remoteIpAddress, statuscode);
        }
        private string ParseResponseToJSON(string response, string id, string remoteIpAddress, int statuscode)
        {
            ResponseJSON responseToSerialize;
            string responseJSON;

            try
            {
                var responseBodyObject = JToken.Parse(response);

                responseToSerialize = new ResponseJSON(DateTime.Now.ToString(new CultureInfo("ru-RU")), id, remoteIpAddress, statuscode, responseBodyObject);
                responseJSON = JsonConvert.SerializeObject(responseToSerialize);
            }
            catch (Exception e)
            {
                responseToSerialize = new ResponseJSON(DateTime.Now.ToString(new CultureInfo("ru-RU")), id, remoteIpAddress, statuscode, response);
                responseJSON = JsonConvert.SerializeObject(responseToSerialize);
            }

            return responseJSON;
        }
    }
}
