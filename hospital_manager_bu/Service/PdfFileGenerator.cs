using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using System;
using System.IO;
using System.Text;
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
                WriterProperties writerProperties = new WriterProperties()
                    .SetStandardEncryption(
                        Encoding.UTF8.GetBytes(pinCode), // user password
                        Encoding.UTF8.GetBytes(pinCode), // owner password
                        EncryptionConstants.ALLOW_PRINTING, // permissions
                        EncryptionConstants.ENCRYPTION_AES_128); // encryption type

                PdfWriter writer = new PdfWriter(stream, writerProperties);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Set custom font for the entire document
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                document.SetFont(font);

                // Add Parliament Header
                Paragraph parliamentHeader = new Paragraph("European Parliament")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetBold();
                document.Add(parliamentHeader);

                document.Add(new Paragraph("\n")); // Add space after the header

                // Document Title
                Paragraph documentTitle = new Paragraph("Voting Briefing Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold()
                    .SetUnderline()
                    .SetMarginBottom(20);
                document.Add(documentTitle);

                // Add Article Description Section
                Paragraph articleInfo = new Paragraph($"Subject of the Vote: {votingArticle.Description}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetMarginBottom(10);
                document.Add(articleInfo);

                Paragraph userInfo = new Paragraph($"Member of Parliament: {userName}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetMarginBottom(20);
                document.Add(userInfo);

                // Add Date of Report Generation
                Paragraph reportDate = new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetItalic()
                    .SetMarginBottom(20);
                document.Add(reportDate);

                // Add Sub-Articles Table Section
                Paragraph tableTitle = new Paragraph("Voting Details")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(16)
                    .SetBold()
                    .SetMarginBottom(10);
                document.Add(tableTitle);

                Table table = new Table(1);
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetMarginBottom(20);

                // Adding Table Headers
                Cell header1 = new Cell().Add(new Paragraph("Sub-Article").SetBold());
                header1.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                header1.SetTextAlignment(TextAlignment.CENTER);
                header1.SetBorder(new SolidBorder(1));
                table.AddHeaderCell(header1);

                // Adding Table Rows
                votingArticle.SubArticles.ForEach(subArticle =>
                {
                    Cell cell1 = new Cell().Add(new Paragraph(subArticle.Description ?? string.Empty));
                    cell1.SetTextAlignment(TextAlignment.LEFT);
                    cell1.SetBorder(new SolidBorder(1));
                    cell1.SetPadding(5);
                    table.AddCell(cell1);
                });

                document.Add(table);

                // Add Signature Section
                Paragraph signatureTitle = new Paragraph("Acknowledgment")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(16)
                    .SetBold()
                    .SetMarginBottom(10);
                document.Add(signatureTitle);

                document.Add(new Paragraph("\n"));

                Paragraph signatureSection = new Paragraph("I, the undersigned, confirm that the above information accurately reflects my voting decisions for the specified article.")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetMarginBottom(20);
                document.Add(signatureSection);

                document.Add(new Paragraph("Date: ____________________________").SetMarginBottom(10));
                document.Add(new Paragraph("Signature: _______________________").SetMarginBottom(20));

                document.Add(new Paragraph($"Signed by: {userName}").SetItalic().SetFontSize(12));

                document.Close();
                return stream.ToArray();
            }
        }
    }
}
