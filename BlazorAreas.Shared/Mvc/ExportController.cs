using System.Data;
using System.Linq.Dynamic.Core;
using System.Text;
using BlazorAreas.Models;
using Extenso.Collections;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAreas.Mvc
{
    public partial class ExportController<T> : Controller
         where T : class, IEntity
    {
        public IQueryable<T> ApplyODataQuery(IQueryable<T> query, IQueryCollection queryString = null)
        {
            if (queryString != null)
            {
                if (queryString.ContainsKey("$filter"))
                {
                    query = query.Where(queryString["$filter"].ToString());
                }

                if (queryString.ContainsKey("$orderBy"))
                {
                    query = query.OrderBy(queryString["$orderBy"].ToString());
                }

                if (queryString.ContainsKey("$expand"))
                {
                    string[] propertiesToExpand = queryString["$orderBy"].ToString().Split(',');
                    foreach (string p in propertiesToExpand)
                    {
                        query = query.Include(p);
                    }
                }

                if (queryString.ContainsKey("$skip"))
                {
                    query = query.Skip(int.Parse(queryString["$skip"].ToString()));
                }

                if (queryString.ContainsKey("$top"))
                {
                    query = query.Take(int.Parse(queryString["$top"].ToString()));
                }
            }

            return query;
        }

        public FileResult Download(IQueryable<T> query, DownloadOptions options)
        {
            switch (options.FileFormat)
            {
                case DownloadFileFormat.Delimited:
                    {
                        string separator;
                        string contentType;
                        string fileExtension;

                        switch (options.Delimiter)
                        {
                            case DownloadFileDelimiter.Tab:
                                separator = "\t";
                                contentType = "text/tab-separated-values";
                                fileExtension = "tsv";
                                break;

                            case DownloadFileDelimiter.VerticalBar:
                                separator = "|";
                                contentType = "text/plain";
                                fileExtension = "txt";
                                break;

                            case DownloadFileDelimiter.Semicolon:
                                separator = ";";
                                contentType = "text/plain";
                                fileExtension = "txt";
                                break;

                            case DownloadFileDelimiter.Comma:
                            default:
                                separator = ",";
                                contentType = "text/csv";
                                fileExtension = "csv";
                                break;
                        }

                        string delimited = query.ToDelimited(
                            delimiter: separator,
                            outputColumnNames: options.OutputColumnNames,
                            alwaysEnquote: options.AlwaysEnquote);

                        return File(Encoding.UTF8.GetBytes(delimited), contentType, $"{query.ElementType}_{DateTime.Now:yyyy-MM-dd HH_mm_ss}.{fileExtension}");
                    }
                case DownloadFileFormat.XLSX:
                    {
                        byte[] bytes = query.ToDataTable().ToXlsx();
                        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{query.ElementType}_{DateTime.Now:yyyy-MM-dd HH_mm_ss}.xlsx");
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}