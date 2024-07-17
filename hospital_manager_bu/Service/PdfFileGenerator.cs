using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using voting_data_access.Entities;
using voting_models.Response_Models;

namespace voting_bl.Service
{
    public class PdfFileGenerator
    {
        public static byte[] GeneratePdf(VotingArticleResponse votingArticle, string pinCode, string userName)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream, new WriterProperties()
                    .SetStandardEncryption(
                        System.Text.Encoding.UTF8.GetBytes(pinCode), // user password
                        System.Text.Encoding.UTF8.GetBytes(pinCode), // owner password
                        EncryptionConstants.ALLOW_PRINTING, // permissions
                        EncryptionConstants.ENCRYPTION_AES_128)); // encryption type

                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Paragraph header = new Paragraph("Submitted votes for article: " + votingArticle.Description)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFontSize(20);
                document.Add(header);

                Table table = new Table(1);
                Cell header1 = new Cell().Add(new Paragraph("Sub-Article"));
                table.AddHeaderCell(header1);

                votingArticle.SubArticles.ForEach(subArticle =>
                {
                    Cell cell1 = new Cell().Add(new Paragraph(subArticle.Description ?? string.Empty));
                    table.AddCell(cell1);
                });

                document.Add(table);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Date:____________________________"));
                document.Add(new Paragraph("Signature:________________________"));
                document.Add(new Paragraph($"Signed by: {userName}"));

                document.Close();
                return stream.ToArray();
            }
        }
    }
}
