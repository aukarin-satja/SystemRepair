using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Repairly.Models;

public class RepairReportDocument : IDocument
{
    private readonly ReportDataViewModel _model;

    public RepairReportDocument(ReportDataViewModel model)
    {
        _model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.DefaultTextStyle(x => x.FontSize(12).FontFamily("THSarabunNew"));

            page.Header().Column(header =>
            {
                header.Item().Text("รายงานแจ้งซ่อมครุภัณฑ์").FontSize(18).Bold().AlignCenter();
                header.Item().Text("");
                header.Item().Text($"แผนก: ซ่อมบำรุง").AlignLeft();
                header.Item().Text($"วันที่: {_model.start_date:dd/MM/yyyy} - {_model.end_date:dd/MM/yyyy}").AlignLeft();
                header.Item().Text($"จำนวนรายการทั้งหมด: {_model.Data.Count} รายการ").AlignLeft();
                header.Item().Text("");
            });

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(80); 
                    columns.RelativeColumn(2);  
                    columns.RelativeColumn(2); 
                    columns.RelativeColumn(4);  
                    columns.RelativeColumn(3);   
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Text("วันที่แจ้ง").Bold().AlignCenter();
                    header.Cell().Border(1).Text("ประเภท").Bold().AlignCenter();
                    header.Cell().Border(1).Text("ยี่ห้อ").Bold().AlignCenter();
                    header.Cell().Border(1).Text("โมเดล").Bold().AlignCenter();
                    header.Cell().Border(1).Text("รายละเอียด").Bold().AlignCenter();
                });

                foreach (var item in _model.Data)
                {
                    table.Cell().Border(1).PaddingLeft(3).Text(item.create_at.ToString("dd/MM/yyyy"));
                    table.Cell().Border(1).PaddingLeft(3).Text(item.category);
                    table.Cell().Border(1).PaddingLeft(3).Text(item.brand);
                    table.Cell().Border(1).PaddingLeft(3).Text(item.model);
                    table.Cell().Border(1).PaddingLeft(3).Text(item.description);
                }
            });
            page.Footer().Column(footer =>
            {
                footer.Item().PaddingTop(30).Text("ลงชื่อ....................................................").AlignRight();
     
                footer.Item().Text($"วันที่: ........................................").AlignRight();
            });

        });
    }
}