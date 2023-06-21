using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TableParser
{
    public class Column
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Table
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("columns")]
        public Column[] Columns { get; set; }

        [JsonProperty("rows")]
        public object[][] Rows { get; set; }
    }
}