using System.Xml.Linq;

XDocument doc1 = XDocument.Load(@"Z:\IT_Development\Projects\Active\MDMIntervalDataParcer\AMIPRDAP17_DAILY_LP_04_04_2023_06_55_32.xml");
XDocument doc2 = new XDocument(
         new XElement("import",
               doc1.Descendants("MeterReadings").Skip(1).Take(2))
          );

doc2.Save(@"C:\Users\jwolak\Desktop\temp.xml");