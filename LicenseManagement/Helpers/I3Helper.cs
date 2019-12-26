using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using BarCodeGeneratorLibary;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using Excel;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
//using LicenseManagement.Models.Production;
using MessagingToolkit.QRCode.Codec;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace LicenseManagement.Helpers
{
    public static class I3Helper
    {
        public static ICollection<T> Clone<T>(this ICollection<T> listToClone)
        {
            var array = new T[listToClone.Count];
            listToClone.CopyTo(array, 0);
            return array.ToList();
        }

        public static Object CloneType(Object objtype)
        {
            object lstfinal;

            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, objtype);
                memStream.Seek(0, SeekOrigin.Begin);
                lstfinal = binaryFormatter.Deserialize(memStream);
            }

            return lstfinal;
        }

        public static void DeleteFilesByExtension(string folder, string extension)
        {
            var fileNames = Directory.GetFiles(folder).Where(x => x.EndsWith(extension));

            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
        }

        public static long GetLong(object objValue)
        {
            long intValue;
            try
            {
                intValue = Convert.ToInt64(objValue);
            }
            catch
            {
                intValue = -1;
            }
            return intValue;
        }

        public static int GetInt(object objValue)
        {
            int intValue;
            try
            {
                intValue = Convert.ToInt32(objValue);
            }
            catch
            {
                intValue = -1;
            }
            return intValue;
        }

        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }

        public static decimal GetDecimal(object objValue)
        {
            decimal decValue = 0;
            try
            {
                if (objValue != DBNull.Value)
                    decValue = Convert.ToDecimal(objValue);
            }
            catch
            {
                decValue = 0;
            }
            return decValue;
        }

        public static double GetDouble(object objValue)
        {
            double decValue = 0;
            try
            {
                if (objValue != DBNull.Value)
                    decValue = Convert.ToDouble(objValue);
            }
            catch
            {
                decValue = 0;
            }
            return decValue;
        }

        public static decimal GetDecimal(object objValue, int roundNo)
        {
            decimal decValue = 0;
            try
            {
                if (objValue != DBNull.Value)
                    decValue = Convert.ToDecimal(objValue);

                decValue = decimal.Round(decValue, 2);
            }
            catch
            {
                decValue = 0;
            }
            return decValue;
        }

        //public static string GetPropertyDescriptionInAcumaticaListAttribute(Type type, object statusValue)
        //{
        //    try
        //    {
        //        string description = "N/A";

        //        if (statusValue != null)
        //        {
        //            //var type = typeof(I3ProductionStatusEnum);
        //            //var memInfo = type.GetMember(((I3ProductionStatusEnum)statusValue).ToString());
        //            //if (memInfo != null && memInfo.Count() > 0)
        //            //{
        //            //    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        //            //    description = ((DescriptionAttribute)attributes[0]).Description;
        //            //}

        //            if (type == typeof(I3ProductionStatus))
        //            {
        //                var prodStatus = new I3ProductionStatus.ListAttribute();
        //                if (prodStatus.ValueLabelDic != null && prodStatus.ValueLabelDic.Count > 0)
        //                {
        //                    prodStatus.ValueLabelDic.TryGetValue((int)statusValue, out description);
        //                }
        //            }
        //            else if (type == typeof(PX.Objects.SO.SOOrderStatus))
        //            {
        //                var orderStatus = new PX.Objects.SO.SOOrderStatus.ListAttribute();
        //                if (orderStatus.ValueLabelDic != null && orderStatus.ValueLabelDic.Count > 0)
        //                {
        //                    orderStatus.ValueLabelDic.TryGetValue(statusValue.ToString(), out description);
        //                }
        //            }
        //        }

        //        return description;
        //    }
        //    catch
        //    {
        //        return "N/A";
        //    }
        //}

        #region Production Status

        //public static List<ProductionStatusFormat> GetPickListAvailableStatus(int status)
        //{
        //    switch (status)
        //    {
        //        case I3ProductionStatus.Inventory:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.Inventory,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.Accounting,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Accounting)
        //                               },
        //                       };
        //        default:
        //            return null;
        //    }
        //}
        
        //public static List<ProductionStatusFormat> GetProductionAvailableStatusForSpecialRouting()
        //{
        //    return new List<ProductionStatusFormat>
        //               {
        //                   new ProductionStatusFormat
        //                       {
        //                           ID = I3ProductionStatus.PhaseB,
        //                           Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseB)
        //                       },
        //                   new ProductionStatusFormat
        //                       {
        //                           ID = I3ProductionStatus.PhaseC,
        //                           Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseC)
        //                       },
        //               };
        //}

        //public static List<ProductionStatusFormat> GetAvailableProductionStatus(int status)
        //{
        //    switch (status)
        //    {
        //        //case I3ProductionStatus.Open:
        //        //    return new List<ProductionStatusFormat>
        //        //               {
        //        //                   new ProductionStatusFormat
        //        //                       {
        //        //                           ID = I3ProductionStatus.Inventory,
        //        //                           Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory)
        //        //                       },
        //        //                   new ProductionStatusFormat
        //        //                       {
        //        //                           ID = I3ProductionStatus.PhaseA,
        //        //                           Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)
        //        //                       },
        //        //               };
        //        case I3ProductionStatus.Inventory:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.Inventory,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory)
        //                               },
        //                           new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)},
        //                       };
        //        case I3ProductionStatus.PhaseA:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                           new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)},
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseB, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseB)},
        //                       };
        //        case I3ProductionStatus.PhaseB:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseB, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseB)},
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseBQA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseBQA)},
        //                       };
        //        case I3ProductionStatus.PhaseBQA:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseBQA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseBQA)},
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseC, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseC)},
        //                       };
        //        case I3ProductionStatus.PhaseC:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseC, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseC)},
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseCQA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseCQA)},
        //                       };
        //        case I3ProductionStatus.PhaseCQA:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.PhaseCQA, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseCQA)}, 
        //                               new ProductionStatusFormat
        //                               {ID = I3ProductionStatus.Accounting, Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Accounting)},
        //                       };
        //        case 999:
        //            return new List<ProductionStatusFormat>
        //                       {
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = -1,
        //                                   Name = ""
        //                               },                                   
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.Inventory,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.PhaseA,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.PhaseB,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseB)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.PhaseBQA,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseBQA)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.PhaseC,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseC)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.PhaseCQA,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseCQA)
        //                               },
        //                           new ProductionStatusFormat
        //                               {
        //                                   ID = I3ProductionStatus.Accounting,
        //                                   Name = GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Accounting)
        //                               },
        //                       };
        //        default:
        //            return null;
        //    }
        //}

        #endregion

        public static string GetFilesByFormat(string strFileName)
        {
            string newName = string.Empty;
            if (strFileName.EndsWith(".pdf"))
            {
                var strArray = GetParagraphByCoOrdinate(strFileName, 1, 20, 990, 850, 200);

                if (strArray.Length > 0)
                {
                    if (strArray.Any(x => x.StartsWith("Order Number:")))
                    {
                        //case SO
                        var strSo = strArray.First(x => x.StartsWith("Order Number:"));
                        strSo = strSo.Replace("Order Number:", "");
                        newName = string.Format("{0}-{1}", "SO", strSo.Trim().ToString(CultureInfo.InvariantCulture));
                    }
                    else if (strArray.Any(x => x.StartsWith("Pick List:")))
                    {
                        // case PL
                        var strPL = strArray.First(x => x.StartsWith("Pick List:"));
                        strPL = strPL.Replace("Pick List:", "");
                        newName = string.Format("{0}-{1}", "PL", strPL.Trim().ToString(CultureInfo.InvariantCulture));
                    }
                    else if (strArray.Any(x => x.StartsWith("Work Order Pick List")))
                    {
                        // case WO-PL
                        var strWO = strArray.First(x => x.StartsWith("WO:"));
                        strWO = strWO.Replace("WO:", "").Trim();
                        strWO = strWO.Substring(0, strWO.IndexOf("SO", StringComparison.Ordinal));
                        newName = string.Format("{0}-{1}", "WO", strWO.Trim().ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
            return newName;
        }

        public static string FindFilesInFolderByName(string folderPath, string fileSearch)
        {
            string[] fileNames = Directory.GetFiles(folderPath);
            foreach (var fileName in fileNames)
            {
                if (fileName.EndsWith(".pdf"))
                {
                    var name = System.IO.Path.GetFileName(fileName);

                    if (name != null && name.Contains(fileSearch))
                    {
                        return name;
                    }
                }
            }
            return string.Empty;
        }

        public static string FindFilesInFolderByFormat(string folderPath, string fileSearch)
        {
            string[] fileNames = Directory.GetFiles(folderPath);

            foreach (var fileName in fileNames)
            {
                if (fileName.EndsWith(".pdf"))
                {
                    var strArray = GetParagraphByCoOrdinate(fileName, 1, 20, 990, 850, 200);

                    if (strArray.Length > 0)
                    {
                        if (strArray.Any(x => x.StartsWith(fileSearch)))
                        {
                            return System.IO.Path.GetFileName(fileName);
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string[] GetParagraphByCoOrdinate(string filepath, int pageno, int cordinate1, int coordinate2, int coordinate3, int coordinate4)
        {
            var reader = new PdfReader(filepath);
            var rect = new iTextSharp.text.Rectangle(cordinate1, coordinate2, coordinate3, coordinate4);
            var renderFilter = new RenderFilter[1];
            renderFilter[0] = new RegionTextRenderFilter(rect);
            ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
            string text = PdfTextExtractor.GetTextFromPage(reader, pageno, textExtractionStrategy);
            string[] words = text.Split('\n');
            return words;
        }

        public static void CopyFile(string strFullOldFile, string strFullTempFile, string copyPath, string strBegin)
        {
            string[] fileNames = Directory.GetFiles(copyPath);

            foreach (var fileName in fileNames)
            {
                if (fileName.Contains(strBegin))
                    File.Delete(fileName);
            }

            File.Copy(strFullTempFile, strFullOldFile);
        }

        public static string FindFileByPath(string srcPath, string fileSearch)
        {
            string[] fileNames = Directory.GetFiles(srcPath);

            if (fileNames.Length > 0)
            {
                var fileResult = fileNames.FirstOrDefault(x => x.StartsWith(fileSearch));

                if (fileResult != null)
                {
                    return System.IO.Path.GetFileName(fileResult);
                }
                return string.Empty;
            }
            return string.Empty;
        }

        public static DateTime GetDateTimeLdap(long value)
        {
            if (value <= 0 || value > DateTime.MaxValue.Ticks)
                return new DateTime();
            return DateTime.FromFileTime(value);
        }

        public static long LongFromLargeInteger(object largeInteger)
        {
            if (largeInteger == null)
                return 0;
            var type = largeInteger.GetType();
            var highPart = (int)type.InvokeMember("HighPart", BindingFlags.GetProperty,
            null, largeInteger, null);
            var lowPart = (int)type.InvokeMember("LowPart", BindingFlags.GetProperty,
            null, largeInteger, null);
            return (long)highPart << 32 | (uint)lowPart;
        }
        /*
        public static void SetDatabaseInfo(ReportDocument reportParameters,
                                     string serverName,
                                     string databaseName,
                                     string userName,
                                     string password)
        {

            //ParameterFieldDefinitions parmFields = reportParameters.DataDefinition.ParameterFields;
            //foreach (ParameterFieldDefinition def in parmFields)
            //{
            //    if (!def.IsLinked())
            //    {
            //        switch (def.ValueType)
            //        {
            //            case CrystalDecisions.Shared.FieldValueType.StringField:
            //                reportParameters.SetParameterValue(def.Name, "", def.ReportName);
            //                break;
            //            case CrystalDecisions.Shared.FieldValueType.NumberField:
            //                reportParameters.SetParameterValue(def.Name, 0, def.ReportName);
            //                break;
            //            default:
            //                reportParameters.SetParameterValue(def.Name, null, def.ReportName);
            //                break;
            //        }
            //    }
            //}

            TableLogOnInfo logOnInfo;

            if (reportParameters == null)
            {
                throw new ArgumentNullException("reportParameters");
            }

            try
            {
                foreach (Table t in reportParameters.Database.Tables)
                {
                    logOnInfo = t.LogOnInfo;
                    //logOnInfo.ReportName = reportParameters.Name;
                    logOnInfo.ConnectionInfo.ServerName = serverName;
                    logOnInfo.ConnectionInfo.DatabaseName = databaseName;
                    logOnInfo.ConnectionInfo.UserID = userName;
                    logOnInfo.ConnectionInfo.Password = password;
                    //logOnInfo.ConnectionInfo.Type = ConnectionInfoType.SQL;
                    logOnInfo.TableName = t.Name;
                    t.ApplyLogOnInfo(logOnInfo);
                    t.Location = t.Name;
                    //t.Location = string.Format("{0}.dbo.{1}", databaseName, t.Name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Sections sects = reportParameters.ReportDefinition.Sections;
            foreach (Section sect in sects)
            {
                ReportObjects ros = sect.ReportObjects;
                foreach (ReportObject ro in ros)
                {
                    if (ro.Kind == ReportObjectKind.SubreportObject)
                    {
                        var sro = (SubreportObject)ro;
                        ReportDocument subRd = sro.OpenSubreport(sro.SubreportName);
                        try
                        {
                            foreach (Table t in subRd.Database.Tables)
                            {
                                logOnInfo = t.LogOnInfo;
                                logOnInfo.ReportName = reportParameters.Name;
                                logOnInfo.ConnectionInfo.ServerName = serverName;
                                logOnInfo.ConnectionInfo.DatabaseName = databaseName;
                                logOnInfo.ConnectionInfo.UserID = userName;
                                logOnInfo.ConnectionInfo.Password = password;
                                logOnInfo.TableName = t.Name;
                                t.ApplyLogOnInfo(logOnInfo);
                                //t.Location = string.Format("{0}.dbo.{1}", databaseName, t.Name);
                                t.Location = t.Name;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
        */
        public static string GetNumberFromString(string str)
        {
            return Regex.Match(str.Trim(), @"\d+").Value;
        }

        public static string GetCommonStringBetweenTwoString(string firstStr, string secondStr)
        {
            var str = string.Empty;

            if (firstStr.Length == secondStr.Length)
            {
                for (var i = 0; i < firstStr.Length; i++)
                {
                    if (!string.Equals(firstStr[i], secondStr[i]))
                        break;

                    str = str + firstStr[i];
                }
            }
            //var r = 0;
            //var s2Dict = new Dictionary<char, int>();
            //foreach (var ch in secondStr)
            //{
            //    if (s2Dict.ContainsKey(ch))
            //        s2Dict[ch] = s2Dict[ch] + 1;
            //    else s2Dict.Add(ch, 1);
            //}

            //foreach (var c in firstStr)
            //{
            //    if (s2Dict.ContainsKey(c) && s2Dict[c] > 0)
            //    {
            //        r++;
            //        s2Dict[c] = s2Dict[c] - 1;
            //    }
            //}

            //if(r > 0)
            //{
            //    str = string.Join("", firstStr.Take(r).ToArray());
            //}
            return str;
        }

        public static string GetAutoIncreateIncludeString(string firstString, string commonString)
        {
            var getNumber = firstString.Replace(commonString, "");

            var length = getNumber.Length;
            var nextNumber = GetInt(getNumber) + 1;

            return string.Format("{0}{1}", commonString,
                                 nextNumber.ToString(CultureInfo.InvariantCulture).PadLeft(length, '0'));
        }

        public static bool IsNumeric(object expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static string FindNextSequenceOfSerial(string firstSerial)
        {
            var nextSerial = string.Empty;
            var numberInSerial = string.Empty;

            for (var i = firstSerial.Length - 1; i >= 0; i--)
            {
                if (!IsNumeric(firstSerial[i]))
                    break;

                numberInSerial = firstSerial[i] + numberInSerial;
            }

            if (numberInSerial.Length > 0 && IsNumeric(numberInSerial))
            {
                //var strConsequence = firstSerial.Replace(numberInSerial, "");
	            var strConsequence = firstSerial.Substring(0, firstSerial.Length - numberInSerial.Length);
                var length = numberInSerial.Length;
                var nextSerialNumber = GetLong(numberInSerial) + 1;

                nextSerial = string.Format("{0}{1}", strConsequence,
                                 nextSerialNumber.ToString(CultureInfo.InvariantCulture).PadLeft(length, '0'));
            }
            return nextSerial;
        }

        //public static string AutoIncreaseIncludeString(string str)
        //{
        //    var match = Regex.Match(str, @"^([^0-9]+)([0-9]+)$");
        //    if (match.Groups.Count > 1)
        //    {
        //        var num = long.Parse(match.Groups[2].Value);
        //        return match.Groups[1].Value + (num + 1);
        //    }
        //    return (long.Parse(str) + 1).ToString(CultureInfo.InvariantCulture);
        //}

        //public static DataTable ReadFileToDataTable(string excelFile)
        //{
        //    var conStr = "";
        //    var extension = System.IO.Path.GetExtension(excelFile) != null ? System.IO.Path.GetExtension(excelFile).ToLower() : "";
        //    switch (extension)
        //    {
        //        case ".xls": //Excel 97-03                
        //            {
        //                //conStr = String.Format(ConfigurationManager.AppSettings["Excel03ConString"], excelFile, "YES");
        //                //return ConvertExcelToDataTable(conStr);
        //                return ConvertExcelToDataTable_Xls(excelFile);
        //            }
        //        case ".xlsx": //Excel 07
        //            {
        //                //conStr = String.Format(ConfigurationManager.AppSettings["Excel07ConString"], excelFile, "YES");
        //                //return ConvertExcelToDataTable(conStr);
        //                return ConvertExcelToDataTable_Xlsx(excelFile);
        //            }
        //        case ".csv":
        //            {
        //                return ConvertCSVtoDataTable(excelFile);
        //            }
        //    }
        //    return ConvertExcelToDataTable(conStr);
        //}

        //private static DataTable ConvertExcelToDataTable_Xls(string excelFile)
        //{
        //    var dt = new DataTable();
        //    using (var fileStream = File.OpenRead(excelFile))
        //    {
        //        var excelReader = ExcelReaderFactory.CreateBinaryReader(fileStream);

        //        excelReader.IsFirstRowAsColumnNames = true;
        //        DataSet result = excelReader.AsDataSet();

        //        if (result.Tables.Count > 0)
        //            dt = result.Tables[0];

        //        excelReader.Close();
        //    }

        //    return dt;
        //}

        //private static DataTable ConvertExcelToDataTable_Xlsx(string excelFile)
        //{
        //    var dt = new DataTable();
        //    using (var fileStream = File.OpenRead(excelFile))
        //    {
        //        var excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

        //        excelReader.IsFirstRowAsColumnNames = true;
        //        DataSet result = excelReader.AsDataSet();

        //        if (result.Tables.Count > 0)
        //            dt = result.Tables[0];

        //        excelReader.Close();
        //    }

        //    return dt;
        //}

        private static DataTable ConvertExcelToDataTable(string conStr)
        {
            try
            {
                using (var connExcel = new OleDbConnection(conStr))
                {
                    using (var cmdExcel = new OleDbCommand())
                    {
                        using (var oda = new OleDbDataAdapter())
                        {
                            var dt = new DataTable();
                            
                            cmdExcel.Connection = connExcel;
                            connExcel.Open();
                            DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            
                            if (dtExcelSchema != null)
                            {
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            }
                            oda.SelectCommand = cmdExcel;
                            oda.Fill(dt);
                            connExcel.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            var dt = new DataTable();

            using (var sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Replace("\"", "").ToString(CultureInfo.InvariantCulture));
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].Replace("\"", "").ToString(CultureInfo.InvariantCulture);
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public static void ConvertTableToCSV(DataTable sourceTable, bool includeHeaders, string fullFileName)
        {
            using (var writer = new StreamWriter(fullFileName))
            {
                if (includeHeaders)
                {
                    var headerValues = new List<string>();

                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        headerValues.Add(QuoteValue(column.ColumnName));
                    }

                    writer.WriteLine(String.Join(",", headerValues.ToArray()));
                }

                foreach (DataRow row in sourceTable.Rows)
                {
                    var items = row.ItemArray.Select(o => QuoteValue(o.ToString())).ToArray();
                    writer.WriteLine(String.Join(",", items));
                }

                writer.Flush();
            }
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"", value.Trim().Replace("\"", "\"\"").Replace(",", ""), "\"");
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);

        }

        public static string ConvertObjectToString<T>(T dataSource) where T : class
        {
            var result = new StringBuilder();

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                result.AppendLine(string.Format("{0} : {1}", prop.Name, GetPropValue(dataSource, prop.Name)));
            }

            return result.ToString();
        }

        public static string ConvertObjectToJsonString<T>(T dataSource) where T : class
        {
            return JsonConvert.SerializeObject(dataSource);
        }

        public static T ConvertJsonStringToObject<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static DataTable ConvertListObjectToDataTable<TSource>(IList<TSource> data) where TSource : class
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<TSource> ConvertDataTableToListObject<TSource>(DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new
                                 {
                                     Name = aProp.Name,
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ??
                             aProp.PropertyType
                                 }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new
                                     {
                                         Name = aHeader.ColumnName,
                                         Type = aHeader.DataType
                                     }).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);

                    var value = (!dataRow.Table.Columns.Contains(aField.Name) || dataRow[aField.Name] == DBNull.Value) ?
                    null : dataRow[aField.Name]; //if database field is nullable
                    //object value = null;
                    //if (dataRow.Table.Columns.Contains(aField.Name) && dataRow[aField.Name] != DBNull.Value)
                    //{
                    //    value = dataRow[aField.Name];

                    //    if (dataRow[aField.Name].GetType() == typeof(DateTime))
                    //    {
                    //        value = ConvertTimeToAcumaticaTimeZone((DateTime?)dataRow[aField.Name]);
                    //    }
                    //}

                    propertyInfos.SetValue(aTSource, value, null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        //public static DateTime GetAcumaticaUserDateTime()
        //{
        //    DateTime timeDataUtc = DateTime.UtcNow;
        //    return PX.Common.PXTimeZoneInfo.ConvertTimeFromUtc(timeDataUtc, PX.Common.LocaleInfo.GetTimeZone());
        //}

        //public static DateTime? ConvertTimeToAcumaticaTimeZone(DateTime? sourceTime)
        //{
        //    DateTime? destinationTime = null;

        //    if (sourceTime.HasValue)
        //    {
        //        var timeDataUtc = sourceTime.Value.ToUniversalTime();
        //        destinationTime = PX.Common.PXTimeZoneInfo.ConvertTimeFromUtc(timeDataUtc, PX.Common.LocaleInfo.GetTimeZone());
        //    }

        //    return destinationTime;
        //}

        //public static DateTime? CorrectDBTimeByAcumaticaTimeZoneOffset(DateTime? sourceTime, int type)
        //{
        //    DateTime? destinationTime = null;

        //    if (sourceTime.HasValue)
        //    {
        //        TimeSpan userUtcOffset = PX.Common.LocaleInfo.GetTimeZone().UtcOffset;

        //        if (type >= 0)
        //        {
        //            destinationTime = sourceTime.Value + userUtcOffset;
        //        }
        //        else
        //        {
        //            destinationTime = sourceTime.Value - userUtcOffset;
        //        }
        //    }

        //    return destinationTime;
        //}

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //var settingsReader = new AppSettingsReader();
            //// Get the key from config file
            //var key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            ////System.Windows.Forms.MessageBox.Show(key);
            var key = AppSettings.SecurityKey;

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                //var settingsReader = new AppSettingsReader();
                ////Get your key from config file to open the lock!
                //var key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
                var key = AppSettings.SecurityKey;

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                var tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                return "";
            }
        }
        
        //public static string ReFormatWOStatusDescription(int? status, string serialNo)
        //{
        //    if (status.HasValue)
        //    {
        //        var enumProduction = status.Value;

        //        switch (status.Value)
        //        {
        //            case I3ProductionStatus.Open:
        //            case I3ProductionStatus.Inventory:
        //                {
        //                    return !string.IsNullOrEmpty(serialNo)
        //                               ? GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)
        //                               : GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory);
        //                }
        //            default:
        //                return "N/A";
        //        }
        //    }

        //    return string.Empty;
        //}

        //public static string ReFormatWOStatusDescription(int? status, string serialNo, string oldStatusDescription)
        //{
        //    if (status.HasValue)
        //    {
        //        switch (status.Value)
        //        {
        //            case I3ProductionStatus.Open:
        //            case I3ProductionStatus.Inventory:
        //                {
        //                    return !string.IsNullOrEmpty(serialNo)
        //                               ? GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.PhaseA)
        //                               : GetPropertyDescriptionInAcumaticaListAttribute(typeof(I3ProductionStatus), I3ProductionStatus.Inventory);
        //                }
        //            default:
        //                return oldStatusDescription;
        //        }
        //    }

        //    return oldStatusDescription;
        //}

        public static byte[] ConvertObjectToByteArray<T>(T t)
        {
            try
            {
                var ms = new MemoryStream();
                var xmlS = new XmlSerializer(typeof(T));
                var xmltw = new XmlTextWriter(ms, Encoding.UTF8);

                xmlS.Serialize(xmltw, t);
                ms = (MemoryStream)xmltw.BaseStream;

                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T ConvertByteArrayToObject<T>(byte[] array) where T : class
        {
            try
            {
                var xmlS = new XmlSerializer(typeof(T));
                var ms = new MemoryStream(array);
                // var xmltw = new XmlTextWriter(ms, Encoding.UTF8);

                return xmlS.Deserialize(ms) as T;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static JArray DataTableToJson(this System.Data.DataTable source)
        {
            var result = new JArray();
            foreach (DataRow dr in source.Rows)
            {
                var row = new JObject();
                foreach (DataColumn col in source.Columns)
                {
                    row.Add(col.ColumnName.Trim(), JToken.FromObject(dr[col]));
                }
                result.Add(row);
            }
            return result;
        }

        public static decimal ConvertDimensionToMMUnit(decimal dimension, int typeUnit)
        {
            switch (typeUnit)
            {
                case (int)EnumCollection.UOMBoxOrItem.CM:
                    return dimension * 10;
                case (int)EnumCollection.UOMBoxOrItem.Inches:
                    return dimension * Convert.ToDecimal(25.4);
                default:
                    return dimension;
            }
        }

        public static byte[] StringImageToByte(string strImage)
        {
            var img = new Bitmap(strImage);
            return ImageToByte(img);
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string ConvertHtmlToPlainText(string htmlText)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";
            //matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";
            //match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>"; //matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = htmlText;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);

            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text.Trim();
        }

        public static string RemoveSpecialCharacterInString(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.Replace("&", "").Replace("#", "");
            }
            return input;
        }

        //public static string FindMaxCharacterSequence(string input)
        //{
        //    var firstCount = 0;
        //    var index = 0;
        //    for (var i = 1; i < input.Length; i++)
        //    {
        //        index = i;
        //        string strResult = input.Substring(0, i);
        //        var secondCount = Regex.Matches(input, strResult).Count;
        //        //new Regex(Regex.Escape(input)).Matches(strResult).Count;
        //        if (firstCount == 0)
        //        {
        //            firstCount = secondCount;
        //            continue;
        //        }
        //        if (secondCount < firstCount)
        //        {
        //            break;
        //        }
        //    }
        //    return input.Substring(0, index - 1);
        //}

        //public static List<string> ReformatSerialNumber(string input)
        //{
        //    var maxSequence = FindMaxCharacterSequence(input);
        //    var indexStrResult = input.Split(new[] { maxSequence }, StringSplitOptions.RemoveEmptyEntries);
        //    return indexStrResult.Select(str => string.Format("{0}{1}", maxSequence, str)).ToList();
        //}

        public static void GenerateQRCodeImageWithFormat(string qrCodeInformation, string filePath, ImageFormat imgFormat)
        {
            QrEncoder encoder = new QrEncoder();
            QrCode qrCode;

            if (!encoder.TryEncode(qrCodeInformation, out qrCode))
                throw new Exception("Cannot generate QRCode with these information");

            var gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Two));
            BitMatrix matrix = qrCode.Matrix;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                gRender.WriteToStream(matrix, imgFormat, stream, new Point(600, 600));
            }
        }

        public static void GenerateQRCodeImageLicense(string qrCodeInformation, string filePath
            , ImageFormat imgFormat, int imageZoom = 3, QRCodeEncoder.ERROR_CORRECTION errorCorrectionLevel = QRCodeEncoder.ERROR_CORRECTION.L
            , string fontNameSerial = "Arial", float fontSizeSerial = 11)
        {
            var encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            encoder.QRCodeScale = imageZoom;

            var img = encoder.Encode(qrCodeInformation);

            //var serialImg = DrawFixImageFromText(serialNo, new Font(fontNameSerial, fontSizeSerial, FontStyle.Bold), Color.Black, Color.White);

            //int left = (img.Width / 2) - (serialImg.Width / 2);
            //int top = (img.Height / 2) - (serialImg.Height / 2);

            var g = Graphics.FromImage(img);
            //g.DrawImage(serialImg, new Point(left, top));
            img.Save(filePath, imgFormat);
        }

        public static void GenerateQRCodeImageLicenseWithCenterImage(string qrCodeInformation, string serialNo, string filePath
            , ImageFormat imgFormat, int imageZoom = 3, QRCodeEncoder.ERROR_CORRECTION errorCorrectionLevel = QRCodeEncoder.ERROR_CORRECTION.L
            , string fontNameSerial = "Arial", float fontSizeSerial = 11)
        {
            var encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            encoder.QRCodeScale = imageZoom;

            var img = encoder.Encode(qrCodeInformation);            

            var serialImg = DrawFixImageFromText(serialNo, new Font(fontNameSerial, fontSizeSerial, FontStyle.Bold), Color.Black, Color.White);

            int left = (img.Width / 2) - (serialImg.Width / 2);
            int top = (img.Height / 2) - (serialImg.Height / 2);

            var g = Graphics.FromImage(img);
            g.DrawImage(serialImg, new Point(left, top));
            img.Save(filePath, imgFormat);
        }

        public static Image DrawFixImageFromText(string text, Font font, Color textColor, Color backColor)
        {
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }

        public static string GenerateBarCode(string locationpath, string barCodeData)
        {
            //var locationWarehousePath = HttpContext.Current.Server.MapPath(string.Format("~/Files/BarCode/"));
            if (!Directory.Exists(locationpath))
                Directory.CreateDirectory(locationpath);

            // generate upcBarCodeFirst
            new BarCodeHelper
            {
                Width = 250,
                Height = 150
            }.GenerateBarcode(
                    barCodeData,
                    string.Format("{0}{1}.jpg", locationpath, barCodeData),
                    SaveTypes.JPG);

            return string.Format("{0}{1}.jpg", locationpath, barCodeData);
        }

        public static string ConvertByteArrayToHexaString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", "");
        }

		public static string BuildSearchConditionFromObjectForSql(string valueSearch, Type type)
		{
			var obj =  Activator.CreateInstance(type);

			var result = new StringBuilder();

			if (!string.IsNullOrEmpty(valueSearch))
			{
				PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				for (int i = 0; i < props.Length; i++)
				{
					if (props[i].PropertyType == typeof(string) && props[i].CanWrite)
					{
						// do your update
						if (result.Length > 0)
						{
							result.Append(" OR ");
						}
						result.Append(string.Format("{0} LIKE '%{1}%'", props[i].Name, valueSearch));
					}
				}
			}

			return result.ToString();
		}

		public static string BuildSearchConditionFromObjectForSql<TSource>(string valueSearch) where TSource : new()
		{
			return BuildSearchConditionFromObjectForSql(valueSearch, typeof (TSource));

			//var obj = new TSource();

			//var result = new StringBuilder();

			//if (!string.IsNullOrEmpty(valueSearch))
			//{
			//	PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			//	for (int i = 0; i < props.Length; i++)
			//	{
			//		if (props[i].PropertyType == typeof(string) && props[i].CanWrite)
			//		{
			//			// do your update
			//			if (result.Length > 0)
			//			{
			//				result.Append(" OR ");
			//			}
			//			result.Append(string.Format("{0} LIKE '%{1}%'", props[i].Name, valueSearch));
			//		}
			//	}
			//}

			//return result.ToString();
		}

        //public static List<string> ReformatQRCodeStringToList(string strQRCode)
        //{
        //    strQRCode = Regex.Replace(strQRCode, @"\s+", " ");
        //    strQRCode = strQRCode.TrimEnd();
        //    char specialCharacter = '\t';
        //    if(strQRCode.Contains(','))
        //    {
        //        specialCharacter = ',';
        //    }
        //    else if(strQRCode.Contains(','))
        //    {
        //        specialCharacter = ',';
        //    }
        //    else if (strQRCode.Contains('-'))
        //    {
        //        specialCharacter = '-';
        //    }
        //    else if(strQRCode.Contains(';'))
        //    {
        //        specialCharacter = ';';
        //    }
        //    else if(strQRCode.Contains('\t'))
        //    {
        //        specialCharacter = '\t';
        //    }
        //    else if (strQRCode.Contains('\r'))
        //    {
        //        specialCharacter = '\r';
        //    }
        //    else if(strQRCode.Contains(' '))
        //    {
        //        specialCharacter = ' ';
        //    }
        //    else if (strQRCode.Contains('\n')) // enter character
        //    {
        //        specialCharacter = '\n';
        //    }
        //    if(strQRCode.ElementAt(strQRCode.Length - 1).Equals(specialCharacter))
        //    {
        //        strQRCode = strQRCode.Remove(strQRCode.Length - 1);
        //        return ReformatQRCodeStringToList(strQRCode);
        //    }
        //    return strQRCode.Split(new[] {specialCharacter}).ToList();       
        //}

        //public static List<TSource> ConvertJsonArrayToListObject<TSource>(JArray jsonArray) where TSource : new() 
        //{
        //    try
        //    {
        //        var dataList = new List<TSource>();
        //        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        //        var objFieldNames = (from PropertyInfo aProp in typeof (TSource).GetProperties(flags)
        //                             select new
        //                                        {
        //                                            Name = aProp.Name,
        //                                            Type = Nullable.GetUnderlyingType(aProp.PropertyType) ??
        //                                                   aProp.PropertyType
        //                                        }).ToList();
        //        var dataTblFieldNames = (from DataColumn aHeader in jsonArray
        //                                 select new
        //                                 {
        //                                     Name = aHeader.ColumnName,
        //                                     Type = aHeader.DataType
        //                                 }).ToList();
        //        var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
        //        foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
        //        {
        //            var aTSource = new TSource();
        //            foreach (var aField in commonFields)
        //            {
        //                PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
        //                var value = (!dataRow.Table.Columns.Contains(aField.Name) || dataRow[aField.Name] == DBNull.Value) ?
        //                null : dataRow[aField.Name]; //if database field is nullable
        //                propertyInfos.SetValue(aTSource, value, null);
        //            }
        //            dataList.Add(aTSource);
        //        }
        //        return dataList;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        //public static string ExportDataSetToExcelUsingNpoi(DataSet dsExport, string fileName)
        //{
        //    if (File.Exists(fileName))
        //        File.Delete(fileName);
        //    using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        //    {
        //        IWorkbook wb = new XSSFWorkbook();
        //        foreach (DataTable dt in dsExport.Tables)
        //        {
        //            ISheet sheet = wb.CreateSheet(dt.TableName);
        //            ICreationHelper cH = wb.GetCreationHelper();
        //            IRow headerRow = sheet.CreateRow(0); // header row
        //            ICellStyle headerStyle = wb.CreateCellStyle();
        //            headerStyle.FillForegroundColor = HSSFColor.Black.Index;
        //            headerStyle.BorderBottom = BorderStyle.Dotted;
        //            headerStyle.Alignment = HorizontalAlignment.Center;
        //            IFont font = wb.CreateFont();
        //            font.Color = HSSFColor.White.Index;
        //            font.Boldweight = (short)FontBoldWeight.Bold;
        //            font.FontHeight = 12;
        //            headerStyle.SetFont(font);
        //            headerStyle.FillPattern = FillPattern.SolidForeground;
        //            for (int col = 0; col < dt.Columns.Count; col++)
        //            {
        //                ICell cell = headerRow.CreateCell(col);
        //                cell.SetCellValue(dt.Columns[col].ColumnName);
        //                cell.CellStyle = headerStyle;
        //                sheet.AutoSizeColumn(col);
        //            }
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                IRow row = sheet.CreateRow(i + 1);
        //                for (int j = 0; j < dt.Columns.Count; j++)
        //                {
        //                    ICell cell = row.CreateCell(j);
        //                    cell.SetCellValue(dt.Rows[i].ItemArray[j].ToString());
        //                    //  sheet.AutoSizeColumn(j);                            
        //                }
        //            }
        //        }
        //        wb.Write(fs);
        //    }
        //    return fileName;
        //}

        public static bool MergePDFs(IEnumerable<string> fileNames, string targetPdf)
        {
            bool merged = true;

            if (File.Exists(targetPdf))
                File.Delete(targetPdf);

            using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }

            return merged;
        }
    }

    //public static class I3InstanceHelper
    //{
    //    public static int GetCurrentCompanyID()
    //    {
    //        if (System.Web.HttpContext.Current != null &&
    //            System.Web.HttpContext.Current.Request != null &&
    //            System.Web.HttpContext.Current.Request.Cookies != null &&
    //            System.Web.HttpContext.Current.Request.Cookies.Count > 0 &&
    //            System.Web.HttpContext.Current.Request.Cookies["CompanyID"] != null)
    //        {
    //            var cookieVal = System.Web.HttpContext.Current.Request.Cookies["CompanyID"].Value;

    //            if (!string.IsNullOrEmpty(cookieVal))
    //            {
    //                var companies = PX.Data.Update.PXCompanyHelper.SelectCompanies(PX.Data.Update.PXCompanySelectOptions.Visible);

    //                foreach (var cinfo in companies)
    //                {
    //                    if (cinfo.CompanyCD.Equals(cookieVal) && cinfo.CompanyID.HasValue)
    //                    {
    //                        return cinfo.CompanyID.Value;
    //                    }
    //                }
    //            }
    //        }

    //        return PX.Data.Update.PXInstanceHelper.CurrentCompany;
    //    }
    //}
}