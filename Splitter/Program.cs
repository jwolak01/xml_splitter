using System;
using System.Xml.Linq;

int elementCount = 0;
int elementsPerFile = 700;

XDocument xdoc = XDocument.Load(@"Z:\IT_Development\Projects\Active\MDMIntervalDataParcer\AMIPRDAP17_DAILY_LP_04_04_2023_06_55_32.xml");
xdoc.Descendants("MetersNotRead").Remove();
xdoc.Descendants("MetersRead").Remove();
xdoc.Descendants("ScheduleExecution").Remove();


//counts number of elements in XML file, limits the size(number of elements) of the new split file to make it so it fits in 24 files.
foreach (var element in xdoc.Root.Elements())
{
    elementCount++;
    elementsPerFile = elementCount / 24;
}

foreach (var batch in xdoc.Root.Elements().InSetsOf(elementsPerFile))
{
    var newDoc = new XDocument(
         new XElement("AMRDEF", batch));

    newDoc.Save($"{saveLocation}\\exportedFile_{++index}.xml");
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