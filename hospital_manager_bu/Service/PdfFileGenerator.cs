using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.IO;
using voting_data_access.Entities;
using voting_models.Models;
using voting_models.Response_Models;

namespace voting_bl.Service
{
    public class PdfFileGenerator
    {
        public static byte[] GeneratePdf(VotingArticleResponse votingArticle)
        {
            using (MemoryStream stream = new MemoryStream())

            {
                // Must have write permissions to the path folder
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
                Paragraph header = new Paragraph("Submitted votes for article: " + votingArticle.Description)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFontSize(20);
                Table table = new Table(2);
                Cell header1 = new Cell();
                header1.Add(new Paragraph("Sub-Article"));
                Cell heade2 = new Cell();
                heade2.Add(new Paragraph("Vote"));
                table.AddCell(header1);
                table.AddCell(heade2);
                votingArticle.SubArticles.ForEach(v =>
                {
                    Cell cell1 = new Cell();
                    var desc = v.Description;
                    if (desc == null)
                    {
                        desc = "";
                    }
                    cell1.Add(new Paragraph(desc));
                    Cell cell2 = new Cell();
                    cell2.Add(new Paragraph(((VoteType)v.VoteType).ToString().Replace("_", " ").ToLower()));
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                });
                document.Add(header);
                document.Add(table);


                document.Add(new Paragraph("Date:____________________________"));
                document.Add(new Paragraph("Signature:________________________"));

                document.Close();
                return stream.ToArray();
            }
        }
    }
}