// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using TableParser;

Console.WriteLine("Hello, World!");
var mapper = MapperCreator.GetMapper();

var json = File.ReadAllText("exampleData.json");
var table = JsonConvert.DeserializeObject<Table>(json);

var tableData = mapper.Map<List<Primary>>(table, cfg => cfg.Items[""]="");

System.Diagnostics.Debugger.Break();