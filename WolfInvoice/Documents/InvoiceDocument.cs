using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfInvoice.Configurations;
using WolfInvoice.Documents.Components;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Documents;

/// <summary>
/// Represents an invoice document that can be generated and exported.
/// </summary>
public class InvoiceDocument : IDocument
{
    /// <summary>
    /// Gets the invoice that this document represents.
    /// </summary>
    public Invoice Invoice { get; }

    /// <summary>
    /// Gets the company information for the invoice.
    /// </summary>
    public CompanyInfoConfig CompanyInfo { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceDocument"/> class.
    /// </summary>
    /// <param name="invoice">The invoice to be included in the document.</param>
    /// <param name="companyInfo">The company information to be included in the document.</param>
    public InvoiceDocument(Invoice invoice, CompanyInfoConfig companyInfo)
    {
        Invoice = invoice;
        CompanyInfo = companyInfo;
    }

    /// <summary>
    /// Gets the metadata for the document.
    /// </summary>
    /// <returns>The metadata for the document.</returns>

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    /// <summary>
    /// Composes the invoice document with the specified container.
    /// </summary>
    /// <param name="container">The container to compose the document with.</param>

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.MarginVertical(10);
            page.MarginHorizontal(50);

            page.Content().Element(ComposeContent);

            page.Footer()
                .AlignCenter()
                .Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Column(column =>
                {
                    column
                        .Item()
                        .Text($"Invoice #{Invoice.Id}")
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(Colors.Blue.Medium);
                });

            row.RelativeItem()
                .Column(column =>
                {
                    column
                        .Item()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Issue date: ").SemiBold();
                            text.Span($"{Invoice.CreatedAt:d}");
                        });

                    column
                        .Item()
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Due date: ").SemiBold();
                            text.Span($"{Invoice.EndDate:d}");
                        });
                });
        });
    }

    void ComposeContent(IContainer container)
    {
        container
            .PaddingVertical(40)
            .Column(column =>
            {
                column.Spacing(20);

                column.Item().Element(ComposeHeader);

                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Component(new AddressComponent("From", Invoice.Customer));
                        row.ConstantItem(50);
                        row.RelativeItem().Component(new AddressComponent("For", CompanyInfo));
                    });

                column.Item().Element(ComposeTable);

                column
                    .Item()
                    .PaddingRight(5)
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Discount Percent: ").SemiBold();

                        text.Span($"{Invoice.Discount ?? 0} %").FontColor("#33cc33").SemiBold();
                    });

                column
                    .Item()
                    .PaddingRight(5)
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Discount Cost: ").SemiBold();

                        text.Span(
                                $"{Math.Round((Invoice.TotalSum / 100) * Invoice.Discount ?? 0, 2, MidpointRounding.AwayFromZero)} $"
                            )
                            .FontColor("#33cc33")
                            .SemiBold();
                    });

                column
                    .Item()
                    .PaddingRight(5)
                    .AlignRight()
                    .Text(
                        $"Grand total: {Math.Round(Invoice.TotalSum, 2, MidpointRounding.AwayFromZero)}$"
                    )
                    .SemiBold();

                if (!string.IsNullOrWhiteSpace(Invoice.Comment))
                    column.Item().PaddingTop(25).Element(ComposeComments);
            });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Product").Style(headerStyle);
                header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                header.Cell().AlignRight().Text("Total").Style(headerStyle);

                header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            foreach (var item in Invoice.Rows)
            {
                table.Cell().Element(CellStyle).Text("#");
                table.Cell().Element(CellStyle).Text(item.Service);
                table
                    .Cell()
                    .Element(CellStyle)
                    .AlignRight()
                    .Text($"{Math.Round(item.Amount, 2, MidpointRounding.AwayFromZero)}$");
                table
                    .Cell()
                    .Element(CellStyle)
                    .AlignRight()
                    .Text($"{Math.Round(item.Quantity, 2, MidpointRounding.AwayFromZero)}");
                table
                    .Cell()
                    .Element(CellStyle)
                    .AlignRight()
                    .Text($"{Math.Round(item.Sum, 2, MidpointRounding.AwayFromZero)}");

                static IContainer CellStyle(IContainer container) =>
                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }
        });
    }

    void ComposeComments(IContainer container)
    {
        container
            .ShowEntire()
            .Background(Colors.Grey.Lighten3)
            .Padding(10)
            .Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Comments").FontSize(14).SemiBold();
                column.Item().Text(Invoice.Comment);
            });
    }
}
