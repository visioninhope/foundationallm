using ClosedXML.Excel;
using System.Text;

namespace FoundationaLLM.Vectorization.DataFormats.Office
{
    /// <summary>
    /// Extracts text from XLSX files.
    /// </summary>
    public class XLSXTextExtractor
    {
        private const string DefaultSheetNumberTemplate = "\n# Worksheet {number}\n";
        private const string DefaultEndOfSheetTemplate = "\n# End of worksheet {number}";
        private const string DefaultRowPrefix = "";
        private const string DefaultColumnSeparator = ", ";
        private const string DefaultRowSuffix = "";

        private readonly bool _withWorksheetNumber;
        private readonly bool _withEndOfWorksheetMarker;
        private readonly bool _withQuotes;
        private readonly string _worksheetNumberTemplate;
        private readonly string _endOfWorksheetMarkerTemplate;
        private readonly string _rowPrefix;
        private readonly string _columnSeparator;
        private readonly string _rowSuffix;

        /// <summary>
        /// Constructor for XLSXTextExtractor.
        /// </summary>
        /// <param name="withWorksheetNumber"></param>
        /// <param name="withEndOfWorksheetMarker"></param>
        /// <param name="withQuotes"></param>
        /// <param name="worksheetNumberTemplate"></param>
        /// <param name="endOfWorksheetMarkerTemplate"></param>
        /// <param name="rowPrefix"></param>
        /// <param name="columnSeparator"></param>
        /// <param name="rowSuffix"></param>
        public XLSXTextExtractor(
            bool withWorksheetNumber = true,
            bool withEndOfWorksheetMarker = false,
            bool withQuotes = true,
            string? worksheetNumberTemplate = null,
            string? endOfWorksheetMarkerTemplate = null,
            string? rowPrefix = null,
            string? columnSeparator = null,
            string? rowSuffix = null)
        {
            this._withWorksheetNumber = withWorksheetNumber;
            this._withEndOfWorksheetMarker = withEndOfWorksheetMarker;
            this._withQuotes = withQuotes;

            this._worksheetNumberTemplate = worksheetNumberTemplate ?? DefaultSheetNumberTemplate;
            this._endOfWorksheetMarkerTemplate = endOfWorksheetMarkerTemplate ?? DefaultEndOfSheetTemplate;

            this._rowPrefix = rowPrefix ?? DefaultRowPrefix;
            this._columnSeparator = columnSeparator ?? DefaultColumnSeparator;
            this._rowSuffix = rowSuffix ?? DefaultRowSuffix;
        }

        /// <summary>
        /// Extracts the text content from a PPTX document.
        /// </summary>
        /// <param name="binaryContent">The binary content of the PPTX document.</param>
        /// <returns>The text content of the PPTX document.</returns>
        public string GetText(BinaryData binaryContent)
        {
            //save binaryContent to an xlsx file
            //using var inputStream = binaryContent.ToStream();
            //using var fileStream = new FileStream(@"C:\vectorization\thedoc.xlsx", FileMode.Create, FileAccess.Write);
            //inputStream.CopyTo(fileStream);

            var sb = new StringBuilder();

            using var stream = binaryContent.ToStream();
            using var workbook = new XLWorkbook(stream);

            var worksheetNumber = 0;
            foreach (var worksheet in workbook.Worksheets)
            {
                worksheetNumber++;
                if (this._withWorksheetNumber)
                {
                    sb.AppendLine(this._worksheetNumberTemplate.Replace("{number}", $"{worksheetNumber}", StringComparison.OrdinalIgnoreCase));
                }

                if(worksheet.RangeUsed() is not null)
                {
                    foreach (IXLRangeRow? row in worksheet.RangeUsed()!.RowsUsed())
                    {
                        if (row == null) { continue; }

                        var cells = row.CellsUsed().ToList();

                       
                        sb.Append(this._rowPrefix);
                        for (var i = 0; i < cells.Count; i++)
                        {
                            IXLCell? cell = cells[i];

                            sb.Append(this.GetCellValueWithTimeout(cell, 10).Result);

                            if (i < cells.Count - 1)
                            {
                                sb.Append(this._columnSeparator);
                            }
                        }

                        sb.AppendLine(this._rowSuffix);                        
                        
                    }

                }
                else
                {
                    sb.AppendLine($"No data found in Worksheet number: {worksheetNumber}");
                }

                if (this._withEndOfWorksheetMarker)
                {
                    sb.AppendLine(this._endOfWorksheetMarkerTemplate.Replace("{number}", $"{worksheetNumber}", StringComparison.OrdinalIgnoreCase));
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Some functions in cells have self-references, this times out the retrival of a cell value to avoid memory issues.
        /// </summary>
        /// <param name="cell">The cell whose value is extracted.</param>
        /// <param name="timeOutInSeconds">The timeout value to cancel the task if a value has not been surfaced (in seconds).</param>
        /// <returns>Text Value of the cell.</returns>
        /// <exception cref="ApplicationException">If a value is not returned in timeOutInSeconds, an exception is thrown.</exception>
        private async Task<string> GetCellValueWithTimeout(IXLCell cell, int timeOutInSeconds)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeOutInSeconds));
            var token = cts.Token;

            // assign the 10 second cancellation token to the task, if the task takes longer than 10 seconds, it is automatically cancelled.
            var cellValueTask = Task.Run(() =>
            {
                var sb = new StringBuilder();
                               
                if (this._withQuotes && cell is { Value.IsText: true })
                {
                    sb.Append('"')
                        .Append(cell.Value.GetText().Replace("\"", "\"\"", StringComparison.Ordinal))
                        .Append('"');
                }
                else
                {
                    sb.Append(cell.Value);
                }              

                return sb.ToString();
            }, token);

            try
            {
                if (await Task.WhenAny(cellValueTask, Task.Delay(TimeSpan.FromSeconds(timeOutInSeconds), token)) == cellValueTask)
                {
                    return await cellValueTask;
                }
                else
                {
                    throw new ApplicationException("Timeout while reading cell value.");
                }
            }
            // if the specified timeout is reached, if the cellValueTask is not completed, it may throw an OperationCanceledException, handle it accordingly.
            catch (OperationCanceledException)
            {
                throw new ApplicationException("Timeout while reading cell value.");
            }
            
        }
    }
}
