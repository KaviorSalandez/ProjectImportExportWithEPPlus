using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Reflection;
using System.Data;
using DemoImportExport.Extensions;
using OfficeOpenXml.DataValidation;
using System.ComponentModel.DataAnnotations;
using DemoImportExport.Models.Response;
using DemoImportExport.DTOs.Employees;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace DemoImportExport.Helper
{
    public class HelperFile
    {
        #region gen file
        /// <summary>
        /// Tạo file excel
        /// </summary>
        /// <typeparam name="TDto">Object mapping</typeparam>
        /// <param name="data">Data export</param>
        /// <param name="keyRedis">Key redis</param>
        /// <param name="sheetTitle">Title sheet</param>
        /// <param name="validationData">Để kiểu Dictionary(Tên cột, list giá trị) cho phép bắt buộc nhập những cột giá trị trong mảng theo yêu cầu nghiệp vụ </param>
        /// <returns></returns>
        public static byte[] GenerateExcelFile<TDto>(IEnumerable<TDto> data, bool showStatus, string sheetTitle, Dictionary<string, IEnumerable<string>> validationData = null)
        {
            var tDtoHeaders = GetHeadersFromDto<TDto>();
            string[] columnHeaders = tDtoHeaders.ToArray(); // add stt to first column
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Khởi tạo một file Excel mới trong bộ nhớ.
            using (var package = new ExcelPackage())
            {
                // Tạo một worksheet mới với tên sheetTitle.
                var ws = package.Workbook.Worksheets.Add(sheetTitle);

                ws.Cells["A1:" + GetColumnLetter(columnHeaders.Length) + "2"].Merge = true;
                ws.Cells["A1"].Value = sheetTitle.ToUpper();
                ws.Cells["A1"].Style.Font.Size = 25;
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(1).Height = 25;

                // Add extra column if redis key exists
                if (showStatus)
                {
                    columnHeaders = columnHeaders.Concat(new[] { "Tình trạng" }).ToArray();
                }

                int dataStartRow = 3;
                int endRow = 1000;

                // create column and style default
                CreateColumnHeader<TDto>(columnHeaders, ws, dataStartRow, endRow, showStatus);

                // Data validation
                if (validationData != null)
                {
                    AddCustomValidation(ws, columnHeaders, dataStartRow + 1, endRow, validationData);
                }

                // Write data
                if (data.Count() > 0)
                {
                    ToConvertDataTable(data, ws);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private static string[] GetHeadersFromDto<TDto>()
        {
            return typeof(TDto).GetProperties()
                .Select(prop =>
                {
                    var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
                    return displayAttr?.Name ?? prop.Name;
                })
                .ToArray();
        }

        /// <summary>
        /// Tạo hàng tiêu đề cho Excel,Tự động tính và đặt độ rộng cho từng cột,Thêm cột "Tình trạng" nếu được yêu cầu (showStatus),
        /// Định dạng font, viền, tự động xuống dòng trong Excel.
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="columnHeaders"></param>
        /// <param name="worksheet"></param>
        /// <param name="dataStartRow"></param>
        /// <param name="endRow"></param>
        /// <param name="showStatus"></param>
        private static void CreateColumnHeader<TDto>(string[] columnHeaders, ExcelWorksheet worksheet, int dataStartRow, int endRow, bool showStatus)
        {
            var columnWidths = columnHeaders.Select(header =>
            {
                return Math.Max(header.Length + 5, 10);
            }).ToArray();

            if (showStatus)
            {
                int[] extendedColumnWidths = new int[columnWidths.Length + 1];
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    extendedColumnWidths[i] = columnWidths[i];
                }

                // Add the new column header
                extendedColumnWidths[columnWidths.Length] = 50;
                // Replace the old array with the new one
                columnWidths = extendedColumnWidths;
            }

            // Add column headers and set column widths
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                worksheet.Cells[dataStartRow, i + 1].Value = columnHeaders[i];
                worksheet.Cells[dataStartRow, i + 1].Style.WrapText = true;
                worksheet.Column(i + 1).Width = columnWidths[i];
                worksheet.Row(dataStartRow).Style.Font.Size = 12; // Set font size for headers row
                worksheet.Row(dataStartRow).Style.Font.Bold = true;
                // Apply border style to the entire column
                using (var range = worksheet.Cells[dataStartRow, i + 1, endRow, i + 1])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }
        }
        /// <summary>
        /// Hàm này chuyển đổi số cột sang chữ cái tương ứng (1 -> A, 2 -> B, ..., 27 -> AA, ...)
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        private static string GetColumnLetter(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = string.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Hàm này tạo các select trên cột được chỉ định và chỉ cho nhập trong khoảng đó
        /// </summary>
        /// <param name="worksheet"> Excel </param>
        /// <param name="columnHeaders"> danh sách cột </param>
        /// <param name="startRow"> dòng bắt đầu của cột </param>
        /// <param name="endRow"></param>
        /// <param name="columnValidators">danh sách cột chứ giá giá trị yêu cầu nhập</param>
        private static void AddCustomValidation(
                    ExcelWorksheet worksheet,
                    string[] columnHeaders,
                    int startRow,
                    int endRow,
                    Dictionary<string, IEnumerable<string>> columnValidators)
        {
            foreach (var columnValidator in columnValidators)
            {
                int colIndex = Array.IndexOf(columnHeaders, columnValidator.Key);
                if (colIndex == -1) continue;

                var validation = worksheet
                    .Cells[startRow, colIndex + 1, endRow, colIndex + 1]
                    .DataValidation
                    .AddListDataValidation();

                foreach (var val in columnValidator.Value.Distinct())
                {
                    if (string.Join(",", validation.Formula.Values.Concat(new[] { val })).Length > 255)
                        break;

                    validation.Formula.Values.Add(val);
                }

                validation.ShowErrorMessage = true;
                validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                validation.ErrorTitle = "Giá trị không hợp lệ";
                validation.Error = "Vui lòng chọn giá trị hợp lệ.";
                validation.ShowInputMessage = true;
                validation.PromptTitle = "Chọn giá trị hợp lệ";
                validation.Prompt = "Chọn giá trị trong danh sách: " + string.Join(",", columnValidator.Value.Select(x => x));
                validation.AllowBlank = true; // cho phép để trống
            }
        }

        /// <summary>
        /// Chuyển đổi dữ liệu sang các bảng của excel 
        /// </summary>
        /// <typeparam name="T">kiểu thực thể T muốn chuyển đổi </typeparam>
        /// <param name="items">mảng các thực thể kiểu T </param>
        /// <returns>datatable</returns>
        private static System.Data.DataTable ToConvertDataTable<T>(IEnumerable<T> items, ExcelWorksheet ws)
        {
            System.Data.DataTable dt = new System.Data.DataTable(typeof(T).Name);
            PropertyInfo[] propInfo = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // Thêm cột số thứ tự
            // dt.Columns.Add("STT", typeof(int));
            // ví dụ với class Student
            // propInfo sẽ chứa "Name", "Birthday", "Score"
            foreach (PropertyInfo prop in propInfo)
            {
                dt.Columns.Add(prop.Name);
            }

            //int ordinalNumber = 1;
            int rowIndex = 4; // dòng bắt đầu ghi dữ liệu
            foreach (T item in items)
            {
                // STT
                //ws.Cells[rowIndex, 1].Value = ordinalNumber;

                for (int i = 0; i < propInfo.Length; i++)
                {
                    var propValue = propInfo[i].GetValue(item, null);
                    if (propValue != null)
                    {
                        Type propType = propInfo[i].PropertyType;

                        // Nếu là kiểu Nullable<>
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            Type underlyingType = Nullable.GetUnderlyingType(propType);

                            if (underlyingType == typeof(DateTime))
                            {
                                DateTime dateTimeValue = (DateTime)propValue;
                                ws.Cells[rowIndex, i + 1].Value = dateTimeValue.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                ws.Cells[rowIndex, i + 1].Value = propValue.ToString();
                            }
                        }
                        else
                        {
                            ws.Cells[rowIndex, i + 1].Value = propValue.ToString();
                        }
                    }
                    else
                    {
                        ws.Cells[rowIndex, i + 1].Value = ""; // giá trị mặc định nếu null
                    }
                }

                //ordinalNumber++;
                rowIndex++;
            }

            return dt;
        }
        public static Dictionary<string, IEnumerable<string>> ToValidationDict<TEnum>(string columnName) where TEnum : Enum
        {
            return new Dictionary<string, IEnumerable<string>>
            {
                {
                    columnName,
                    Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => e.GetDisplayNameEnum())
                }
            };
        }
        #endregion
        #region read file 
        /// <summary>
        /// Giúp người dùng chỉ được upload đúng mẫu Excel hệ thống đã thiết kế.
        /// Tránh lỗi khi map dữ liệu sau này
        /// Trường hợp sẽ báo lỗi:Tên tiêu đề không khớp (ví dụ "Mã nhân viên" thay vì "Mã NV"),Thừa hoặc thiếu cột,Sai thứ tự.
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="worksheet"></param>
        /// <param name="headerRow"></param>
        /// <exception cref="Exception"></exception>
        private static void ValidateExcelHeaders<TDto>(ExcelWorksheet worksheet, int headerRow = 3)
        {
            var expectedHeaders = typeof(TDto).GetProperties()
                .Select(prop => prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name)
                .ToArray();

            // đọc các tiêu đề thật sự từ file excel
            var actualHeaders = new List<string>();
            int col = 1;

            while (true)
            {
                var cellVal = worksheet.Cells[headerRow, col].Text?.Trim();
                if (string.IsNullOrEmpty(cellVal)) break;

                actualHeaders.Add(cellVal);
                col++;
            }

            if (actualHeaders.Count == 0)
                throw new Exception("Không tìm thấy cột tiêu đề trong file.");

            var actualWithoutFirst = actualHeaders.ToArray();

            //Nếu số lượng khác nhau hoặc tên khác nhau, thì ném lỗi "File không đúng định dạng mẫu".
            if (actualWithoutFirst.Length != expectedHeaders.Length ||
                !expectedHeaders.SequenceEqual(actualWithoutFirst))
            {
                throw new Exception("File không đúng định dạng mẫu. Vui lòng sử dụng đúng file mẫu.");
            }
        }
        public static async Task<List<TDto>> ReadExcel<TDto>(Stream excelStream) where TDto : new()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var result = new List<TDto>();
            using (var package = new ExcelPackage(excelStream))
            {
                // lấy worksheet đầu tiên
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("Không tìm thấy sheet trong file Excel.");

                // xác thực tiêu đề
                ValidateExcelHeaders<TDto>(worksheet);

                int headerRow = 3;
                int totalColumns = worksheet.Dimension.End.Column;
                int totalRows = worksheet.Dimension.End.Row;
                //  Lấy danh sách tiêu đề cột
                var headers = new List<string>();
                for (int col = 1; col <= totalColumns; col++)
                {
                    var header = worksheet.Cells[headerRow, col].Text?.Trim();
                    if (string.IsNullOrWhiteSpace(header)) break;
                    headers.Add(header);
                }

                // Tạo map tên cột -> PropertyInfo
                // Khi đọc Excel, bạn sẽ so sánh tên cột trong file Excel với tên các thuộc tính trong DTO.
                // Nhưng file Excel có thể dùng tên hiển thị(Display) thay vì tên thuộc tính thực.
                // Câu lệnh này giúp ánh xạ được cả hai trường hợp.

                var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .ToDictionary(p => p.GetCustomAttribute<DisplayAttribute>()?.Name.ToLower() ?? p.Name.ToLower(), p => p);

                // Tạo danh sách các task để xử lý song song theo từng dòng
                var tasks = new List<Task<List<TDto>>>();

                // Chia công việc thành các phần nhỏ và xử lý song song
                const int batchSize = 1000;  // Chia thành các phần nhỏ để xử lý song song
                int startRow = headerRow + 1;
                int endRow = Math.Min(startRow + batchSize - 1, totalRows);

                while (startRow <= totalRows)
                {
                    int batchStartRow = startRow;
                    int batchEndRow = endRow;
                    tasks.Add(Task.Run(() =>
                    {
                        var batchResult = new List<TDto>();
                        for (int row = batchStartRow; row <= batchEndRow; row++)
                        {
                            var item = new TDto();
                            bool hasValue = false;
                            var stop = 0;

                            for (int col = 1; col <= headers.Count; col++)
                            {
                                string columnName = headers[col - 1];
                                var cellValue = worksheet.Cells[row, col].Text?.Trim();

                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    hasValue = true;
                                }
                                else
                                {
                                    stop++;
                                    if (stop == headers.Count) break;
                                }

                                if (properties.TryGetValue(columnName.ToLower(), out var prop))
                                {
                                    try
                                    {
                                        Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                        object convertedValue = null;
                                        if (string.IsNullOrWhiteSpace(cellValue))
                                        {
                                            convertedValue = null;
                                        }
                                        else if (targetType.IsEnum)
                                        {
                                            convertedValue = Enum.Parse(targetType, cellValue.ToString(), ignoreCase: true);
                                        }
                                        else if (targetType == typeof(DateTime))
                                        {
                                            convertedValue = DateTime.ParseExact(cellValue, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        else if (targetType == typeof(TimeSpan))
                                        {
                                            convertedValue = TimeSpan.Parse(cellValue.ToString());
                                        }
                                        else
                                        {
                                            convertedValue = Convert.ChangeType(cellValue, targetType);
                                        }

                                        prop.SetValue(item, convertedValue);
                                    }
                                    catch
                                    {
                                        // skip error 
                                    }
                                }
                            }

                            if (hasValue)
                                batchResult.Add(item);
                            if (stop == headers.Count) break;
                        }
                        return batchResult;
                    }));

                    startRow = endRow + 1;
                    endRow = Math.Min(startRow + batchSize - 1, totalRows);
                }

                // Chờ tất cả các task hoàn thành
                var allResults = await Task.WhenAll(tasks);

                // Gộp kết quả từ tất cả các batch
                foreach (var batch in allResults)
                {
                    result.AddRange(batch);
                }
            }
            return result;
        }

        public static async Task<ReadExcelResult<TDto>> ReadExcel_V2<TDto>(Stream excelStream,
                                                                             Func<List<TDto>, Task<HashSet<string>>> checkExistsFunc,
                                                                             Func<TDto, string> getCodeFunc,
                                                                             int batchSize) where TDto : new()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var result = new ReadExcelResult<TDto>()
            {
                AllData = new List<TDto>(),
                DataExists = new List<TDto>(),
                DataImport = new List<TDto>()
            };

            using var package = new ExcelPackage(excelStream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
                throw new Exception("Không tìm thấy sheet trong file Excel.");

            ValidateExcelHeaders<TDto>(worksheet);

            int headerRow = 3;
            int totalColumns = worksheet.Dimension.End.Column;
            int totalRows = worksheet.Dimension.End.Row;

            // Lấy tên cột header
            var headers = new List<string>();
            for (int col = 1; col <= totalColumns; col++)
            {
                var header = worksheet.Cells[headerRow, col].Text?.Trim();
                if (string.IsNullOrWhiteSpace(header)) break;
                headers.Add(header);
            }

            var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(p => p.GetCustomAttribute<DisplayAttribute>()?.Name.ToLower() ?? p.Name.ToLower(), p => p);

            int currentRow = headerRow + 1;
            while (currentRow <= totalRows)
            {
                var batch = new List<TDto>();
                int batchEndRow = Math.Min(currentRow + batchSize - 1, totalRows);

                // Đọc batch tuần tự
                for (int row = currentRow; row <= batchEndRow; row++)
                {
                    var item = new TDto();
                    bool hasValue = false;
                    int emptyCount = 0;


                    for (int col = 1; col <= headers.Count; col++)
                    {
                        string columnName = headers[col - 1];
                        var cellValue = worksheet.Cells[row, col].Text?.Trim();

                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            hasValue = true;
                        }
                        else
                        {
                            emptyCount++;
                            if (emptyCount == headers.Count)
                                break; // hết dữ liệu dòng này
                        }

                        if (properties.TryGetValue(columnName.ToLower(), out var prop))
                        {
                            try
                            {
                                Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                object convertedValue = null;

                                if (string.IsNullOrWhiteSpace(cellValue))
                                {
                                    convertedValue = null;
                                }
                                else if (targetType.IsEnum)
                                {
                                    convertedValue = Enum.Parse(targetType, cellValue, ignoreCase: true);
                                }
                                else if (targetType == typeof(DateTime))
                                {
                                    convertedValue = DateTime.ParseExact(cellValue, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else if (targetType == typeof(TimeSpan))
                                {
                                    convertedValue = TimeSpan.Parse(cellValue);
                                }
                                else
                                {
                                    convertedValue = Convert.ChangeType(cellValue, targetType);
                                }

                                prop.SetValue(item, convertedValue);
                            }
                            catch
                            {
                                // ignore error convert
                            }
                        }
                    }

                    if (hasValue)
                        batch.Add(item);
                    if (emptyCount == headers.Count) break;
                }

                // Gọi DB check tồn tại ngay cho batch này
                var existedCodes = await checkExistsFunc(batch);
                foreach (var item in batch)
                {
                    var code = getCodeFunc(item)?.Trim();
                    if (!string.IsNullOrEmpty(code) && existedCodes.Contains(code))
                        result.DataExists.Add(item);
                    else
                        result.DataImport.Add(item);
                }

                result.AllData.AddRange(batch);
                currentRow = batchEndRow + 1;
            }

            return result;
        }
        #endregion

        #region map row to dto
        // Tạo map tên cột -> index cột (1-based)
        private static Dictionary<string, int> GetHeaderMap(ExcelWorksheet ws)
        {
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int colCount = ws.Dimension.Columns;

            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = ws.Cells[3, col].Text?.Trim();
                if (!string.IsNullOrEmpty(cellValue) && !headerMap.ContainsKey(cellValue))
                    headerMap.Add(cellValue, col);
            }
            return headerMap;
        }

        public static TDto MapRowToDto<TDto>(ExcelWorksheet worksheet, int row) where TDto : new()
        {
            var dto = new TDto();
            var type = typeof(TDto);

            var headerMap = GetHeaderMap(worksheet);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Nếu không có setter, bỏ qua
                if (!prop.CanWrite) continue;

                // Tên property dùng để tìm cột Excel
                if (!headerMap.TryGetValue(prop.Name, out int colIndex))
                    continue;

                var cell = worksheet.Cells[row, colIndex];
                if (cell == null || string.IsNullOrEmpty(cell.Text))
                    continue;

                object value = null;

                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    value = Convert.ChangeType(cell.Value, targetType);
                }
                catch
                {
                }

                prop.SetValue(dto, value);
            }

            return dto;
        }
        #endregion
        


    }
}
