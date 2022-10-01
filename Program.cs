using gedcom5_csharp.Parser;
using gedcom5_csharp.Visitors;
using System.Diagnostics.Metrics;


var test = new ModelParser();

var gedcom = test.Parse(File.ReadLines(@"Kennedy.ged"));

new GedcomWriter().Write(gedcom, "Kennedy-test.ged");
