using BlazorAreas.Data;
using BlazorAreas.Data.Entities;
using BlazorAreas.Models;
using BlazorAreas.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAreas.Controllers
{
    [Route("person")]
    public class PersonController : ExportController<Person>
    {
        private readonly ApplicationDbContext context;

        public PersonController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("export/csv")]
        public FileResult ExportToCsv()
        {
            return Download(ApplyODataQuery(context.People, Request.Query), new DownloadOptions
            {
                FileFormat = DownloadFileFormat.Delimited
            });
        }

        [HttpGet("export/excel")]
        public FileResult ExportToExcel()
        {
            return Download(ApplyODataQuery(context.People, Request.Query), new DownloadOptions
            {
                FileFormat = DownloadFileFormat.XLSX
            });
        }
    }
}