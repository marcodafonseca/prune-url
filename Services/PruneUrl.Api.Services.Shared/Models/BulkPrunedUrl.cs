using System;
using System.Collections.Generic;
using System.Text;

namespace PruneUrl.Api.Services.Shared.Models
{
    public class BulkPrunedUrl
    {
        public string Url { get; set; }
        public string PrunedUrl { get; set; }
    }
}
