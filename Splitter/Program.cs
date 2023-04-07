using System;
using System.Xml.Linq;

DateTime today = DateTime.Today;

int elementCount = 0;
int elementsPerFile = 700;
int index = 0;
string saveLocation = "Z:\\IT_Development\\Projects\\Active\\MDMIntervalDataParcer\\Test_Output";

foreach (var path in Directory.GetFiles(@"Z:\IT_Development\Projects\Active\MDMIntervalDataParcer\SourceFiles"))
{
    //Console.WriteLine(path); // full path
    Console.WriteLine(System.IO.Path.GetFileName(path)); // file name
    String date = System.IO.Path.GetFileName(path).Substring(20, 20);
    date = date.Replace('_', '/');
    //  Console.WriteLine(str.Substring(0,10));

    if (date.Substring(0, 10).Equals(today.ToString("MM/dd/yyyy")))
    {
        XDocument xdoc = XDocument.Load(@"Z:\IT_Development\Projects\Active\MDMIntervalDataParcer\SourceFiles\" + System.IO.Path.GetFileName(path));
        xdoc.Descendants("MetersNotRead").Remove();
        xdoc.Descendants("MetersRead").Remove();
        xdoc.Descendants("ScheduleExecution").Remove();


        //counts number of elements in XML file, limits the size(number of elements) of the new split file to make it so it fits in 24 files.
        foreach (var element in xdoc.Root.Elements())
        {
            elementCount++;
        }
        elementsPerFile = (elementCount / 24) + 20;

        foreach (var batch in xdoc.Root.Elements().InSetsOf(elementsPerFile))
        {
            var newDoc = new XDocument(
                 new XElement("AMRDEF", batch));

            newDoc.Save($"{saveLocation}\\exportedFile_{++index}.xml");
        }
    }
}


public static class IEnumerableExtensions
{
    public static IEnumerable<List<T>> InSetsOf<T>(this IEnumerable<T> source, int max)
    {
        List<T> toReturn = new List<T>(max);
        foreach (var item in source)
        {
            toReturn.Add(item);
            if (toReturn.Count == max)
            {
                yield return toReturn;
                toReturn = new List<T>(max);
            }
        }
        if (toReturn.Any())
        {
            yield return toReturn;
        }
    }
}