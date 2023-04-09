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

        XDocument newDoc = new XDocument();
        // for each file that is the same date as today, it adds its elements to a new XML file (newDoc)
        foreach (var element in XDocument.Load(@"Z:\IT_Development\Projects\Active\MDMIntervalDataParcer\SourceFiles\" + System.IO.Path.GetFileName(path)).Elements())
        {
            newDoc.Add(element);
        }


        newDoc.Descendants("MetersNotRead").Remove();
        newDoc.Descendants("MetersRead").Remove();
        newDoc.Descendants("ScheduleExecution").Remove();


        //counts number of elements in XML file, limits the size(number of elements) of the new split file to make it so it fits in 24 files.
        //foreach (var element in newDoc.Root.Elements())
        //{
        //    elementCount++;
        //}
        //elementsPerFile = (elementCount / 24) + 30;

        double scalarVariableCount = newDoc.Root.Elements().Count();
        double numberOfElementsPerFile = Math.Round(scalarVariableCount / 24);
        int elementAmount = Convert.ToInt32(numberOfElementsPerFile);

        // adds {elementsPerFile} elements to a new file that then saves to a folder
        foreach (var batch in newDoc.Root.Elements().InSetsOf(elementsPerFile))
        {
            var finalDoc = new XDocument(
                 new XElement("AMRDEF", batch));

            finalDoc.Save($"{saveLocation}\\exportedFile_{++index}.xml");
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
