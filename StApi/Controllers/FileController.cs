using System;
using System.IO;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RestSharp;

namespace StApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // POST api/values
        [HttpPost("client")]
        public ActionResult Post()
        {
            var client = new RestClient("http://localhost/file/api/file/upload");
            var request = new RestRequest(Method.POST);
            request.AddFile("content", @"D:\Temp\test.txt");
            try
            {
                client.ExecuteAsync(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // OK

                    }
                    else
                    {
                        // NOK

                    }
                });

                return Ok();
            }
            catch (Exception error)
            {
                return Ok("no");
                // Log
            }
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                Logger.Info(folderName);
                string newPath = Path.Combine(@"D:\Temp", folderName);
                Logger.Info(newPath);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex.Message);
                return NotFound();
            }
        }
    }
}
