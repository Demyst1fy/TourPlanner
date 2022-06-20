using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.PDFGenerator
{
    public static class PDFGenerator
    {
        public static void GenerateSingleReport(ITourDictionary tourDictionary, Tour tour, IEnumerable<TourLog> tourLogs, double popularity, double childFriendliness)
        {
            try
            {
                string pdfPath = ConfigurationManager.AppSettings["PDFReportsPath"];
                string imagePath = ConfigurationManager.AppSettings["MapImagesPath"];
                string language = ConfigurationManager.AppSettings["Language"];

                Directory.CreateDirectory(pdfPath);
                var file = $"{pdfPath}/{tour.Name}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{language}.pdf";

                PdfWriter writer = new PdfWriter(file);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Paragraph listHeader = new Paragraph($"{tour.Name}")
                        .SetFontSize(18)
                        .SetBold();
                List list = new List()
                        .SetSymbolIndent(12)
                        .SetListSymbol("-");
                list.Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourStart")}-{tourDictionary.GetResourceFromDictionary("StringTourDestination")}: {tour.Start}-{tour.Destination}"))
                        .Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourTransportType")}: {tour.TransportType}"))
                        .Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourDistance")}: {tour.Distance}km"))
                        .Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourTime")}: {tour.Time}"))
                        .Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourPopularity")}: {popularity}%"))
                        .Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourChildFriendliness")}: {childFriendliness}%"));

                if (!string.IsNullOrEmpty(tour.Description))
                    list.Add(new ListItem($"{tourDictionary.GetResourceFromDictionary("StringTourDescription")}: {tour.Description}"));

                document.Add(listHeader);
                document.Add(list);

                if (tourLogs.Count() > 0)
                {
                    Paragraph tableHeader = new Paragraph(tourDictionary.GetResourceFromDictionary("StringTourLogs"))
                            .SetFontSize(18)
                            .SetBold();
                    document.Add(tableHeader);
                    Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsDate")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsDuration")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsDifficulty")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsRating")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsComment")));
                    table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);

                    foreach (var item in tourLogs)
                    {
                        table.AddCell(item.Datetime.ToString());
                        table.AddCell(item.TotalTime.ToString());
                        table.AddCell(item.Difficulty);
                        table.AddCell(item.Rating.ToString());
                        table.AddCell(item.Comment);
                    }
                    document.Add(table);
                    document.Add(new AreaBreak());
                }

                Paragraph imageHeader = new Paragraph(tourDictionary.GetResourceFromDictionary("StringTourMap"))
                        .SetFontSize(18)
                        .SetBold();
                document.Add(imageHeader);

                ImageData imageData = ImageDataFactory.Create($"{imagePath}/{tour.Id}.png");
                document.Add(new Image(imageData));
                document.Close();
            } 
            catch (Exception ex)
            {
                throw new PDFGenerationException(ex.Message);
            }
        }

        public static void GenerateSummarizedReport(ITourHandler tourHandler, ITourDictionary tourDictionary, IEnumerable<Tour> tours)
        {

            if (tours.Any())
            {
                try
                {
                    string pdfPath = ConfigurationManager.AppSettings["PDFReportsPath"];
                    string language = ConfigurationManager.AppSettings["Language"];

                    Directory.CreateDirectory(pdfPath);
                    var file = $"{pdfPath}/TourSummarize_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{language}.pdf";

                    PdfWriter writer = new PdfWriter(file);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    Paragraph tableHeader = new Paragraph(tourDictionary.GetResourceFromDictionary("StringTourSummarizeAverage"))
                            .SetFontSize(18)
                            .SetBold();
                    document.Add(tableHeader);
                    Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourName")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourDistance")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsDuration")));
                    table.AddHeaderCell(getHeaderCell(tourDictionary.GetResourceFromDictionary("StringTourLogsRating")));
                    table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);

                    foreach (Tour tour in tours)
                    {
                        IEnumerable<TourLog> tourLogs = tourHandler.GetTourLogs(tour);
                        var distance = tour.Distance;
                        double avgTimeSeconds = 0;
                        double avgRating = 0;

                        foreach (TourLog log in tourLogs)
                        {
                            avgTimeSeconds += log.TotalTime.TotalSeconds;
                            avgRating += log.Rating;
                        }

                        avgTimeSeconds /= tourLogs.Count();
                        if (double.IsNaN(avgTimeSeconds))
                            avgTimeSeconds = 0;

                        avgRating /= tourLogs.Count();
                        if (double.IsNaN(avgRating))
                            avgRating = 0;

                        TimeSpan avgTime = TimeSpan.FromSeconds(avgTimeSeconds);

                        table.AddCell(tour.Name);
                        table.AddCell(distance + " km");
                        table.AddCell(avgTime.ToString());
                        table.AddCell(avgRating.ToString());
                    }
                    document.Add(table);
                    document.Close();
                }
                catch (Exception ex)
                {
                    throw new PDFGenerationException(ex.Message);
                }
            }
            else
            {
                throw new NoToursException("The tourlist is empty.");
            }
        }

        private static Cell getHeaderCell(String s)
        {
            return new Cell()
                .Add(new Paragraph(s))
                .SetBold()
                .SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
