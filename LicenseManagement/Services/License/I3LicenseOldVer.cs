using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LicenseManagement.Helpers;
using LicenseManagement.Models.License;
using LicenseGen;
using RuleParse;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;

namespace LicenseManagement.Services.License
{
    /// <summary>
    /// ***THIS CLASS IS USED FOR PARSING OLD LICENSE SETTING***
    /// TQDung - 05/01/2017
    /// </summary>
    public class I3LicenseOldVer
    {
        private readonly IntPtr _mHaspRule;
        private UInt64 _mFilterMask;
        private UInt32[] _mPFilterValues;
        private int _mNProductIdx = -1;
        private int _mNFormatIdx = -1;

        private readonly ArrayList _mFieldNames = new ArrayList();
        private ArrayList _mHaspFields = new ArrayList();
        private const string StrAny = "-";
        private ArrayList _mPKeyInfoNames = new ArrayList();
        private const int MaxLookupLimit = 1000;
        // private System.Collections.ArrayList m_FieldNames = new System.Collections.ArrayList();
        private ArrayList _mFieldValues = new ArrayList();
        //private System.Collections.ArrayList m_FieldNames = new ArrayList();
        
        public ArrayList MFieldValues
        {
            get
            {
                return _mFieldValues;
            }            
        }

        public I3LicenseOldVer()
        {
            // _license = new License_t();         
            _mHaspRule = RuleParseApi.i3haspruledll_create();
            // var epos = new i3hasprule_epos_t[1];
            var appDomain = AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;

            //var strPath = HttpContext.Current.Server.MapPath(("~/I3App_Dlls/License"));
            //string[] ruleFilePaths = Directory.GetFiles(strPath, "*.i3hr");

            var srxproRuleFilePath = Path.Combine(basePath, "I3App_Dlls/License", "srxpro_oldver.i3hr");
            var cmsserverRuleFilePath = Path.Combine(basePath, "I3App_Dlls/License", "cmsserver.i3hr");

            var epos = new i3hasprule_epos_t();
            //foreach (string ruleFile in ruleFilePaths)
            //{
            //    RuleParseApi.i3hasprule_add_rulefile(_mHaspRule, ruleFile, ref epos);
            //}
            RuleParseApi.i3haspruledll_add_rulefile(_mHaspRule, srxproRuleFilePath, ref epos);
            RuleParseApi.i3haspruledll_add_rulefile(_mHaspRule, cmsserverRuleFilePath, ref epos);
            //RuleParseApi.i3hasprule_add_rulefile(_mHaspRule, techSupportRuleFilePath, ref epos);
        }

        public I3LicenseOldVer(int productInd, int formatInd)
            : this()
        {
            _mNProductIdx = productInd;
            _mNFormatIdx = formatInd;
        }

        public List<SelectValue> GetAllProducts()
        {
            var lstProducts = new List<SelectValue>();

            int productCount = RuleParseApi.i3haspruledll_product_count(_mHaspRule);

            for (int productIdx = 0; productIdx < productCount; productIdx++)
            {
                var product = new i3hasprule_product_t();
                if (RuleParseApi.i3haspruledll_refer_product(_mHaspRule, productIdx, ref product) == 0)
                {
                    var result = new SelectValue
                    {
                        Id = productIdx,
                        DropDownValue = Marshal.PtrToStringAnsi(product.name)
                    };
                    lstProducts.Add(result);
                }
            }

            return lstProducts;
        }
        
        public ArrayList ReadTextToInt(ArrayList arrayText) // readLicenseDataFromCombos function in examples
        {
            var arrayList = new ArrayList();
            
            int i = 0;

            UInt32 fieldValue = 0;
            if (arrayText.Count > 0)
            {
                AddFieldToArrayList();
                
                foreach (var text in arrayText)
                {
                    var strText = text;
                    var field = (i3hasprule_field_t)_mHaspFields[i];

                    if (strText == null)
                        strText = StrAny;

                    string selectText = strText.ToString();

                    if(selectText == StrAny)
                    {
                        fieldValue = field.defval;
                    }
                    else
                    {
                        if (GetFieldValueFromName(i, selectText, ref fieldValue) == false)
                        {
                            try
                            {
                                fieldValue = UInt32.Parse(selectText);
                                fieldValue = fieldValue < field.maxval ? fieldValue : field.maxval;
                                fieldValue = fieldValue > field.minval ? fieldValue : field.minval;
                            }
                            catch (Exception)
                            {
                                fieldValue = field.defval;
                            }
                        }
                    }
                    arrayList.Add(fieldValue);
                    i++;
                }                                
            }

            return arrayList;
        }

        public List<SelectValue> GetAllFormatByProduct(int productInd)
        {
            var lstProducts = new List<SelectValue>();

            int formatCount = RuleParseApi.i3haspruledll_format_count(_mHaspRule, productInd);

            for (int forId = 0; forId < formatCount; forId++)
            {
                var format = new i3hasprule_format_t();
                if (RuleParseApi.i3haspruledll_refer_format(_mHaspRule, productInd, forId, ref format) == 0)
                {
                    var result = new SelectValue
                    {
                        Id = forId,
                        DropDownValue = Marshal.PtrToStringAnsi(format.name)
                    };
                    lstProducts.Add(result);
                }
            }

            return lstProducts;
        }

        private string CustomFieldName(string originalName)
        {
            switch (originalName)
            {
                case "Analog":
                    return "Analog/Encoder";
                case "Face Blurring":
                    return "Face Blur";
                case "External Monitor":
                    return "Ext Monitor";
                case "CMSMode":
                    return "CMS Mode";
                case "VideoLogix Basic":
                    return "VideoLogix";
                case "VideoLogix Plus":
                    return "VideoLogix+";
                case "VisionCount Basic":
                    return "VisionCount";
                case "VisionCount Plus":
                    return "VisionCount+";
                default:
                    return originalName;
            }
        }

        private string CustomFieldNameForPreviousConfiguration(string originalName)
        {
            switch (originalName)
            {
                case "IP Camera":
                    return "IP Ch";
                case "VideoLogix Basic":
                    return "VL";
                case "VideoLogix Plus":
                    return "VL+";
                case "VisionCount Basic":
                    return "VC";
                case "VisionCount Plus":
                    return "VC+";
                case "External Monitor":
                    return "Ext Mon";
                case "Analog":
                    return "An/En";
                case "Face Blurring":
                    return "FB";
                case "CMSMode":
                    return "CMS";
                default:
                    return originalName;
            }
        }

        private int CustomOrderFieldIndex(string fieldName)
        {
            switch (fieldName)
            {
                case "Version":
                    return 1;
                case "Upgradable":
                    return 2;
                case "Max Connection":
                    return 3;
                case "IP Camera":
                case "IP Ch":
                    return 4;
                case "Analog/Encoder":
                case "An/En":
                    return 5;                            
                case "PACDM":
                    return 6;
                case "CMS Mode":
                case "CMS":
                    return 7;
                case "Ext Monitor":
                case "Ext Mon":
                    return 8;
                case "VideoLogix Basic":
                case "VideoLogix":
                case "VL":
                    return 9;
                case "VideoLogix Plus":
                case "VideoLogix+":
                case "VL+":
                    return 10;
                case "VisionCount Basic":
                case "VisionCount":
                case "VC":
                    return 11;                                                 
                case "VisionCount Plus":
                case "VisionCount+":
                case "VC+":
                //case "VisionCount":
                    return 12;
                case "LPR":
                    return 13;
                case "Face Blur":
                case "FB":
                    return 14;
                case "iSearch":
                    return 15;
                case "Heatmap":
                    return 16;
                case "FPS":
                    return 17;
                case "Audio":
                    return 18;                                
                case "Sensor":
                    return 19;
                case "Control":
                    return 20;                                                                               
            }

            return 0;
        }

        private int CustomColumnField(string fieldName)
        {
            switch (fieldName)
            {
                case "Version":                   
                case "Max Connection":                    
                case "Upgradable":
                    return 1;
                case "IP Camera":
                case "IP Ch":
                case "Analog/Encoder":
                case "An/En":               
                    return 2;
                case "PACDM":
                case "CMS Mode":
                case "CMS":
                case "Ext Monitor":
                case "Ext Mon":                               
                    return 3;
                case "VideoLogix Basic":
                case "VideoLogix":
                case "VL":
                case "VideoLogix Plus":
                case "VideoLogix+":
                case "VL+":
                case "VisionCount Basic":
                case "VisionCount":
                case "VC":
                case "VisionCount Plus":
                case "VisionCount+":
                case "VC+":
                case "LPR":
                case "Face Blur":
                case "FB":
                case "iSearch":
                case "Heatmap":
                // still Heat Map
                    return 4;                                                                                                        
                case "FPS":
                case "Audio":
                case "Sensor":
                case "Control":
                    return 5;                
            }

            return 0;
        }

        public bool CustomVisibleField(string fieldName)
        {
            switch (fieldName)
            {
                case "Sensor":
                case "Control":
                case "VideoLogix Basic":
                case "VideoLogix":
                case "VL":
                case "VisionCount Basic":
                case "VisionCount":
                case "VC":
                    return false;
                default:
                    return true;
            }
        }

        public List<SelectValue> GetFieldFromProductFormat(int productInd, int formatInd, int typeParse)
        {
            _mFieldNames.Clear();
            _mHaspFields.Clear();

            var lstField = new List<SelectValue>();

            int fieldCount = RuleParseApi.i3haspruledll_field_count(_mHaspRule, productInd, formatInd);

            for (int fieldId = 0; fieldId < fieldCount; fieldId++)
            {
                var field = new i3hasprule_field_t();
                if (RuleParseApi.i3haspruledll_refer_field(_mHaspRule, productInd, formatInd, fieldId, ref field) == 0)
                {
                    _mFieldNames.Add(Marshal.PtrToStringAnsi(field.name));
                    _mHaspFields.Add(field);

                    var fieldSel = new SelectValue
                    {
                        Id = fieldId,
                        DropDownValue = typeParse == 1 ? CustomFieldName(Marshal.PtrToStringAnsi(field.name)) : CustomFieldNameForPreviousConfiguration(Marshal.PtrToStringAnsi(field.name)),
                        OriginalOrder = fieldId + 1,
                        CustomOrder = CustomOrderFieldIndex(CustomFieldName(Marshal.PtrToStringAnsi(field.name))),
                        CustomColumn = CustomColumnField(CustomFieldName(Marshal.PtrToStringAnsi(field.name))),
                        VisibleField = CustomVisibleField(CustomFieldName(Marshal.PtrToStringAnsi(field.name))),
                        CustomStandardNonStandard = CustomTypeStandardNonStandard(CustomFieldName(Marshal.PtrToStringAnsi(field.name))),
                        ChildSelectValued = new List<SelectValue>()
                    };

                    fieldSel.ChildSelectValued.Add(new SelectValue
                    {
                        Id = -1,
                        DropDownValue = StrAny
                    });


                    if (field.suggest_count > 0)
                    {
                        for (var sugId = 0; sugId < field.suggest_count; sugId++)
                        {
                            var suggest = new i3hasprule_suggest_t();
                            if (
                                RuleParseApi.i3haspruledll_refer_suggest(_mHaspRule, productInd, formatInd, fieldId, sugId,
                                                                      ref suggest) != 0) continue;
                            var str = Marshal.PtrToStringAnsi(suggest.name);

                            if (string.IsNullOrEmpty(str))
                                continue;
                               

                            fieldSel.ChildSelectValued.Add(new SelectValue
                                                               {
                                                                   //Id = sugId + 1,
                                                                   Id = I3Helper.GetInt(suggest.value),
                                                                   DropDownValue = ReformatStringOfFields(str)
                                                               });
                        }
                    }
                    lstField.Add(fieldSel);
                }
            }            
            return lstField.OrderBy(x => x.CustomOrder).ToList();
        }

        private int CustomTypeStandardNonStandard(string fieldName)
        {
            switch (fieldName)
            {
                case "Analog/Encoder":
                case "An/En":                    
                case "IP Camera":
                case "IP Ch":                    
                case "PACDM":                    
                case "VideoLogix Basic":
                case "VideoLogix":
                case "VL":                   
                case "VisionCount Basic":
                case "VisionCount":
                case "VC":                   
                case "VideoLogix Plus":
                case "VideoLogix+":
                case "VL+":                    
                case "VisionCount Plus":
                case "VisionCount+":
                case "VC+":                    
                case "iSearch":
                case "Heatmap":
                case "Ext Monitor":
                case "Ext Mon":                   
                case "LPR":                    
                case "CMS Mode":
                case "CMS":
                    return 1;
                case "FPS":
                case "Audio":                    
                case "Face Blur":
                case "FB":
                case "Sensor":                    
                case "Control":
                    return 2;                
            }

            return 1;
        }

        private string ReformatStringOfFields(string input)
        {            
            switch (input.ToUpper().Trim())
            {
                case "NONE":
                    return "0";
                //case "DISABLE":
                //    return "D";
                default:
                    return input;
            }
        }

        private bool GetFieldValueFromName(int idxField, string name, ref UInt32 value)
        {
            var suggest = new i3hasprule_suggest_t();
            var field = (i3hasprule_field_t)_mHaspFields[idxField];
            int i;
            for (i = 0; i < field.suggest_count; i++)
            {
                if ((RuleParseApi.i3haspruledll_refer_suggest(_mHaspRule, _mNProductIdx, _mNFormatIdx, idxField, i, ref suggest)) != 0)
                    return false;
                if (name != Marshal.PtrToStringAnsi(suggest.name)) continue;
                value = suggest.value;
                break;
            }

            return i < field.suggest_count;
        }

        private void AddFieldToArrayList()
        {
            _mHaspFields = new ArrayList();
            _mPKeyInfoNames = new ArrayList();

            // update field array list
            int fieldCount = RuleParseApi.i3haspruledll_field_count(_mHaspRule, _mNProductIdx, _mNFormatIdx);

            //update field infos
            for (int fieldId = 0; fieldId < fieldCount; fieldId++)
            {
                var field = new i3hasprule_field_t();
                if (RuleParseApi.i3haspruledll_refer_field(_mHaspRule, _mNProductIdx, _mNFormatIdx, fieldId, ref field) == 0)
                {
                    _mPKeyInfoNames.Add(Marshal.PtrToStringAnsi(field.name));
                    _mHaspFields.Add(field);
                }
            }
        }

        //private void MakeFilterCriteria(SPKWebSite.Helper.FilterRoutingParse routingFilter)
        private void MakeFilterCriteria(ArrayList strFilterList)
        {
            _mFilterMask = 0;
            _mPFilterValues = new UInt32[_mHaspFields.Count];

            for (int i = 0; i < _mHaspFields.Count; i++)
            {
                UInt32 fieldValue = 0;

                var szFilter = strFilterList[i].ToString();
                if (string.IsNullOrEmpty(szFilter) || string.Equals(szFilter, StrAny)) continue;
                if (GetFieldValueFromName(i, szFilter, ref fieldValue))
                {
                    _mFilterMask |= ((UInt64)1 << i);
                    _mPFilterValues[i] = fieldValue;
                }
                else
                {
                    int tmpVal;
                    if (int.TryParse(szFilter, out tmpVal))
                    {
                        _mFilterMask |= ((UInt64)1 << i);
                        _mPFilterValues[i] = (UInt32)tmpVal;
                    }
                }
            }

            /*
            for (int i = 0; i < m_haspFields.Count; i++)
            {
                string strFilter = "";
                switch (i)
                {
                    case 0:
                        {
                            strFilter = m_haspFields.Count == 1 ? routingFilter.DVRCount : routingFilter.Pacdm;
                            break;
                        }
                    case 1:
                        {
                            strFilter = routingFilter.Fps;
                            break;
                        }
                    case 2:
                        {
                            strFilter = routingFilter.Audio;
                            break;
                        }
                    case 3:
                        {
                            strFilter = routingFilter.IPCamera;
                            break;
                        }
                    case 4:
                        {
                            strFilter = routingFilter.VLBasic;
                            break;
                        }
                    case 5:
                        {
                            strFilter = routingFilter.VCBasic;
                            break;
                        }
                    case 6:
                        {
                            strFilter = routingFilter.VLPlus;
                            break;
                        }
                    case 7:
                        {
                            strFilter = routingFilter.VCPlus;
                            break;
                        }
                    case 8:
                        {
                            strFilter = routingFilter.LPR;
                            break;
                        }
                    case 9:
                        {
                            strFilter = routingFilter.CMSMode;
                            break;
                        }
                }
                GetValueForFilter(i, strFilter);
            }*/
        }

        public string GenerateXMLStream(License_t license, string[] filterField, int typeData) // typeData : 1 int, 2 : str
        {
            var xmlDoc = new XmlDocument();

            var root = xmlDoc.CreateElement("i3license");
            xmlDoc.AppendChild(root);

            var productIDNode = xmlDoc.CreateElement("product_id");
            //product_id_node.InnerText = m_nProductIdx.ToString();
            productIDNode.InnerText = license.product_id.ToString(CultureInfo.InvariantCulture);
            root.AppendChild(productIDNode);

            var formatIDNode = xmlDoc.CreateElement("format_id");
            //format_id_node.InnerText = m_nFormatIdx.ToString();
            formatIDNode.InnerText = license.format_id.ToString(CultureInfo.InvariantCulture);
            root.AppendChild(formatIDNode);

            var machineCodeNode = xmlDoc.CreateElement("machine_code");
            machineCodeNode.InnerText = !string.IsNullOrEmpty(license.machine_code)
                ? license.machine_code : "";
            root.AppendChild(machineCodeNode);


            var serialCodeNode = xmlDoc.CreateElement("serial_code");
            serialCodeNode.InnerText = !string.IsNullOrEmpty(license.serial_code)
                ? license.serial_code : "";
            root.AppendChild(serialCodeNode);

            var commentNode = xmlDoc.CreateElement("comment");
            commentNode.InnerText = !string.IsNullOrEmpty(license.comment) ?
                license.comment : "";
            root.AppendChild(commentNode);

            var expiredDateNode = xmlDoc.CreateElement("expired_date");
            expiredDateNode.InnerText = !string.IsNullOrEmpty(license.expired_date)
                ? license.expired_date : "";
            root.AppendChild(expiredDateNode);

            var expiredDaysNode = xmlDoc.CreateElement("expired_days");           
            expiredDaysNode.InnerText = license.expired_days.ToString(CultureInfo.InvariantCulture);
            root.AppendChild(expiredDaysNode);

            var uniqueIDNode = xmlDoc.CreateElement("uniqueID");
            uniqueIDNode.InnerText = license.uniqueID;
            root.AppendChild(uniqueIDNode);

            int i = 0;
            _mNProductIdx = license.product_id;
            _mNFormatIdx = license.format_id;

            AddFieldToArrayList();

            MakeFilterCriteria(ConvertArrayToArrayList(filterField));

            //if (typeData == 2) // from combBox
            //{
                ReadLicenseDataFromCombos(filterField);
            //}
            //else
            //{
            //    ReadLicenseFromGrid(filterField);
            //}

            foreach (XmlElement fieldNode in from string fieldname in _mPKeyInfoNames select fieldname.Replace(" ", "") into title select xmlDoc.CreateElement(title))
            {
                fieldNode.InnerText = _mFieldValues[i++].ToString();
                root.AppendChild(fieldNode);
            }

            var xmlStream = new MemoryStream();
            xmlDoc.Save(xmlStream);

            var byteArray = xmlStream.ToArray();
            var charArray = System.Text.Encoding.Default.GetChars(byteArray);
            var xmlString = new string(charArray);
            xmlString = xmlString.Replace("\r\n", "");

            return xmlString;
        }

        //private void ReadLicenseFromGrid(string[] filterField)
        //{
        //    m_FieldValues = new System.Collections.ArrayList();
          
        //    foreach (string str in filterField)
        //    {              
        //        m_FieldValues.Add(I3DSSHelper.GetInt(str));                
        //    }
        //}

        public bool CheckInformationChange(string[] filterField)
        {           
            AddFieldToArrayList();

            MakeFilterCriteria(ConvertArrayToArrayList(filterField));

            var change = false;
            _mFieldValues = new ArrayList();
            var i = 0;
            foreach (var str in filterField)
            {
                UInt32 fieldValue = 0;
                var field = (i3hasprule_field_t)_mHaspFields[i];
                if (str == StrAny)
                {
                    fieldValue = field.defval;                   
                }
                else
                {
                    if (GetFieldValueFromName(i, str, ref fieldValue) == false)
                    {
                        try
                        {
                            fieldValue = UInt32.Parse(str);

                            if (fieldValue > field.maxval || fieldValue < field.minval)
                                change = true;                           
                        }
                        catch (Exception)
                        {
                            change = true;
                        }
                    }
                }
                if(change)
                    break;

                _mFieldValues.Add(fieldValue);
                i++;
            }

            return change;
        }

        public ArrayList ConvertIntFieldsToText(ArrayList fieldsValue)
        {
            var fieldsText = new ArrayList();

            for (int i = 0; i < fieldsValue.Count; i++)
            {
                string szField = "";

                if (GetFieldNameFromValue(i, (uint)fieldsValue[i], ref szField) == false || szField == "")
                {
                    fieldsText.Add(fieldsValue[i].ToString());                    
                }
                else
                {
                    fieldsText.Add(szField);
                }
            }
            return fieldsText;
        }

        /*public LogDataViewModel ReadLicenseInfoFromFile(string fullFileName)
        {
            var result = new LogDataViewModel {LogDataDtlViewModels = new List<LogDataDtlViewModel>()};

            IntPtr block = IntPtr.Zero;
            int size = 0;
            var info = new i3license_info_t();
            bool ret = TestLicenseApi.file_read(fullFileName, ref block, ref size, ref info);
            if (!ret)
                return null;

            int productIdx = 0, formatIdx = 0, valueSize = 0;
            IntPtr _modelValues = IntPtr.Zero;
            if (RuleParseApi.i3hasprule_parse_block(_mHaspRule, block, size, ref productIdx, ref formatIdx, ref _modelValues, ref valueSize) != 0)
                return null;

            var modelValues = new int[valueSize];
            Marshal.Copy(_modelValues, modelValues, 0, valueSize);

            var lstNames = GetFieldFromProductFormat(productIdx, formatIdx);
            _mNProductIdx = productIdx;
            _mNFormatIdx = formatIdx;
            for (int i = 0; i < _mFieldNames.Count; i++)
            {
                string szField = "";

                if (GetFieldNameFromValue(i, (uint)modelValues[i], ref szField) == false || szField == "")
                {
                    result.LogDataDtlViewModels.Add(new LogDataDtlViewModel
                    {
                        FieldName = _mFieldNames[i].ToString(),
                        FieldValue = modelValues[i].ToString(CultureInfo.InvariantCulture)
                    });
                }
                else
                {
                     result.LogDataDtlViewModels.Add(new LogDataDtlViewModel
                    {
                        FieldName = lstNames[i].DropDownValue,
                        FieldValue = szField
                    });
                }
            }           
            result.MachineCode = info.machine_code;
            result.SerialNo = info.serial_id;
            result.ProductID = productIdx;
            result.FormatID = formatIdx;
            result.ArrayFieldValue = string.Join(";", result.LogDataDtlViewModels.Select(x => x.FieldValue));
            result.UniqueId = info.unique_id;

            return result;
            return null;
        }*/

        public string ReadLicenseIdInfoFromFile(string fullFileName)
        {
            //read file
            byte[] fileBytes = File.ReadAllBytes(fullFileName);
            if (fileBytes.Length < 0)
                return null;

            //convert to IntPtr
            IntPtr unmanagedDataIn = Marshal.AllocHGlobal(fileBytes.Length);
            Marshal.Copy(fileBytes, 0, unmanagedDataIn, fileBytes.Length);

            IntPtr block = IntPtr.Zero;
            int size = 0;
            var info = new i3license_info_t();
            StringBuilder extraMac = new StringBuilder(1024);
            //bool ret = TestLicenseApi.file_read(fullFileName, ref block, ref size, ref info, extraMac);
            bool ret = TestLicenseApi.license_read(unmanagedDataIn, fileBytes.Length, ref block, ref size, ref info, extraMac);
            if (!ret)
                return null;

            int productIdx = 0, formatIdx = 0, valueSize = 0;
            IntPtr _modelValues = IntPtr.Zero;
            if (RuleParseApi.i3haspruledll_parse_block(_mHaspRule, block, size, ref productIdx, ref formatIdx, ref _modelValues, ref valueSize) != 0)
                return null;

            var modelValues = new int[valueSize];
            Marshal.Copy(_modelValues, modelValues, 0, valueSize);
            
            return info.unique_id;            
        }

        private void ReadLicenseDataFromCombos(IEnumerable<string> filterField)
        {
            _mFieldValues = new ArrayList();
            int i = 0;
            foreach (string str in filterField)
            {
                UInt32 fieldValue = 0;
                var field = (i3hasprule_field_t)_mHaspFields[i];
                if (str == StrAny)
                {                    
                    fieldValue = field.defval;
                    //fieldValue = -1;
                }
                else
                {
                    if (GetFieldValueFromName(i, str, ref fieldValue) == false)
                    {
                        try
                        {
                            fieldValue = UInt32.Parse(str);
                            fieldValue = fieldValue < field.maxval ? fieldValue : field.maxval;
                            fieldValue = fieldValue > field.minval ? fieldValue : field.minval;                               

                           // fieldValue = UInt32.Parse(str);
                        }
                        catch (Exception)
                        {
                            //i3hasprule_field_t field = (i3hasprule_field_t)m_haspFields[i];
                            fieldValue = field.defval;
                        }
                    }            

                    //UInt32 tmpVal = 0;
                    //GetFieldValueFromName(i, str, ref tmpVal); //if fail, fieldValue is still 0. 
                    //fieldValue = (int)tmpVal;
                }                
                _mFieldValues.Add(fieldValue);
                i++;
            }
        }

        private ArrayList ConvertArrayToArrayList(string[] strArr)
        {
            var arrList = new ArrayList();
            arrList.AddRange(strArr);
            return arrList;
        }

        public IntPtr GenerateBinaryStream(ref int bufSize, ArrayList fieldNames)
        {
            bufSize = 0;
            var modelValues = new UInt32[fieldNames.Count];
            for (int i = 0; i < fieldNames.Count; i++)
            {
                //modelValues[i] = (UInt32)fieldNames[i];
                modelValues[i] = UInt32.Parse(fieldNames[i].ToString());
            }
            IntPtr haspBlock = IntPtr.Zero;

            if (0 != RuleParseApi.i3haspruledll_make_block(_mHaspRule, _mNProductIdx, _mNFormatIdx, modelValues, ref haspBlock, ref bufSize))
                return IntPtr.Zero;
            if (bufSize <= 0 || haspBlock == IntPtr.Zero)
                return IntPtr.Zero;

            return haspBlock;
        }

        public RoutingParseDetail AnalyseRouting(FilterRoutingParse routingFilter)
        {
            try
            {
                _mNProductIdx = routingFilter.ProductInd;
                _mNFormatIdx = routingFilter.FormatInd;

                AddFieldToArrayList();

                MakeFilterCriteria(ConvertArrayToArrayList(routingFilter.FieldFilter));

                var result = new RoutingParseDetail
                                 {
                                     RoutingParseType =
                                         string.Format("Product: {0}. Format: {1}", routingFilter.ProductName,
                                                       routingFilter.FormatName)
                                 };

                int fieldCount = _mPKeyInfoNames.Count;
                var effectValueFlags = new int[fieldCount];

                for (int i = 0; i < fieldCount; i++)
                    effectValueFlags[i] = ((_mFilterMask & ((UInt64) 1 << i)) != 0) ? 1 : 0;

                var lookup = new IntPtr();
                if (routingFilter.RoutingId == null)
                    routingFilter.RoutingId = string.Empty;
                var modelName = routingFilter.RoutingId.ToLower();
                modelName = modelName.Trim();

                if (result.RoutingParseInfo == null)
                    result.RoutingParseInfo = new List<RoutingParseInfo>();

                for (int i = 0; i < _mPKeyInfoNames.Count; i++)
                {
                    var re = new RoutingParseInfo();
                    re.FieldName = CustomFieldNameForPreviousConfiguration(_mPKeyInfoNames[i].ToString());
                    re.OriginalOrder = i + 1;
                    re.CustomOrder = CustomOrderFieldIndex(CustomFieldName(_mPKeyInfoNames[i].ToString()));
                    re.FieldValue = new List<int>();
                    result.RoutingParseInfo.Add(re);
                }
                //foreach (var re in from string fieldName in _mPKeyInfoNames select new RoutingParseInfo { FieldName = CustomFieldName(fieldName) })
                //{
                //    result.RoutingParseInfo.Add(re);
                //}


                if (
                    (RuleParseApi.i3haspruledll_lookup_limit_begin(_mHaspRule, ref lookup, MaxLookupLimit, _mNProductIdx,
                                                                _mNFormatIdx,
                                                                modelName, _mPFilterValues, effectValueFlags)) == 0)
                {
                    int modelCount = RuleParseApi.i3haspruledll_lookup_count(lookup);

                    if (modelCount > 0)
                    {
                        for (int i = 0; i < modelCount; i++)
                        {
                            var model = new i3hasprule_model_t();
                            if ((RuleParseApi.i3haspruledll_lookup_refer(lookup, i, ref model)) != 0)
                            {
                                RuleParseApi.i3haspruledll_lookup_end(lookup);
                            }
                            else
                            {
                                var vals = new Int32[model.value_count];
                                Marshal.Copy(model.values, vals, 0, model.value_count);
                                for (int valID = 0; valID < model.value_count; valID++)
                                {
                                    if (valID > result.RoutingParseInfo.Count)
                                        break;
                                    if (result.RoutingParseInfo[valID].FieldValue == null)
                                        result.RoutingParseInfo[valID].FieldValue = new List<int>();
                                    result.RoutingParseInfo[valID].FieldValue.Add(vals[valID]);
                                }
                            }
                        }
                    }
                    else
                    {                        
                        if(routingFilter.RoutingId.ToUpper().StartsWith("IPP"))
                        {
                            if (routingFilter.RoutingId.Length == 15)
                                RoutingParseHelper.ParseSoftWareLicenseIPPWith15Characters(result, modelName);
                            if(routingFilter.RoutingId.Length == 12)
                                RoutingParseHelper.ParseSoftWareLicenseIPPWith12Characters(result, modelName);
                        }
                        else if(routingFilter.RoutingId.ToUpper().StartsWith("SL"))
                        {
                            if (routingFilter.RoutingId.Length == 15)
                                RoutingParseHelper.ParseSoftWareLicenseSLWith15Characters(result, modelName);                            
                        }
                        else
                        {
                            if (routingFilter.RoutingId.ToUpper().Length == 14)
                            {
                                RoutingParseHelper.ParseRoutingWith14Characters(result, modelName.ToUpper());
                            }
                            else if (routingFilter.RoutingId.Length == 20 && routingFilter.RoutingId.ToUpper().StartsWith("I3-5000N4-"))
                            // two exception routings: i3-5000N4-1TBSSD-4IP, i3-5000N4-500SSD-4IP
                            {
                                RoutingParseHelper.ParseExceptionRoutingWith20Characters(result, modelName.ToUpper());
                            }
                            else if (routingFilter.RoutingId.ToUpper().Length > 14 &&
                                     routingFilter.RoutingId.ToUpper().Count(x => x == '-') > 1)
                            {
                                var lastIndexSeparate = modelName.ToUpper()
                                    .LastIndexOf("-", StringComparison.Ordinal);

                                RoutingParseHelper.ParseRoutingWith14Characters(result,
                                    modelName.ToUpper().Substring(0, lastIndexSeparate));
                            }

                            else
                            {
                                // get default value for all when it's cannot parse
                                RoutingParseHelper.GetDefaultValueForAll(result);
                            }                 
                        }
                        // parse by routing defined

                    }


                    RuleParseApi.i3haspruledll_lookup_end(lookup);
                    
                }
                else
                {
                    return new RoutingParseDetail
                               {
                                   NeedToFilter = true
                               };
                }

                // destroy 
                RuleParseApi.i3haspruledll_destroy(_mHaspRule);
                if (result.RoutingParseInfo.Count > 0 && result.RoutingParseInfo[0].FieldValue.Count > 0)
                    return result;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
        public bool VerifyXMLFile(string filename, License_t license)
        {
            //test code, no need in release version
            int length = TestLicenseApi.get_stream_length(filename);
            IntPtr test_xml_ptr = Marshal.AllocHGlobal(length);
            TestLicenseApi.load(filename, test_xml_ptr);
            byte[] test_xml_str = new byte[length];
            Marshal.Copy(test_xml_ptr, test_xml_str, 0, length);
            MemoryStream memoryStream = new MemoryStream(test_xml_str);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memoryStream);

            XmlElement rootElem = xmlDoc.DocumentElement;

            //verify generate information
            XmlNodeList elemList = rootElem.GetElementsByTagName("product_id");
            if (elemList[0].InnerText != m_nProductIdx.ToString())
                return false;

            elemList = rootElem.GetElementsByTagName("format_id");
            if (elemList[0].InnerText != m_nFormatIdx.ToString())
                return false;

            elemList = rootElem.GetElementsByTagName("machine_code");
            if (elemList[0].InnerText != license.machine_code)
                return false;

            elemList = rootElem.GetElementsByTagName("serial_code");
            if (elemList[0].InnerText != license.serial_code)
                return false;

            elemList = rootElem.GetElementsByTagName("comment");
            if (elemList[0].InnerText != license.comment)
                return false;

            elemList = rootElem.GetElementsByTagName("expired_date");
            if (elemList[0].InnerText != license.expired_date)
                return false;

            elemList = rootElem.GetElementsByTagName("expired_days");
            if (elemList[0].InnerText != license.expired_days.ToString())
                return false;

            elemList = rootElem.GetElementsByTagName("uniqueID");
            if (elemList[0].InnerText != license.uniqueID)
                return false;

            int i = 0;
            foreach (string fieldname in m_pKeyInfoNames)
            {
                string title = fieldname.Replace(" ", "");
                elemList = rootElem.GetElementsByTagName(title);
                if (m_FieldValues[i++].ToString() != elemList[0].InnerText)
                    return false;
            }

            return true;
        }

        */

        private bool GetFieldNameFromValue(int idxField, UInt32 value, ref string name)
        {
            var suggest = new i3hasprule_suggest_t();
            var field = (i3hasprule_field_t)_mHaspFields[idxField];
            int i;
            for (i = 0; i < field.suggest_count; i++)
            {
                if ((RuleParseApi.i3haspruledll_refer_suggest(_mHaspRule, _mNProductIdx, _mNFormatIdx, idxField, i, ref suggest)) != 0)
                    return false;
                if (suggest.value != value) continue;
                name = Marshal.PtrToStringAnsi(suggest.name);
                break;
            }

            return i < field.suggest_count;
        }

        public ArrayList AddComponentToLicense(int productId, int formatId, ArrayList settingDetails, UpgradeLicenseData upgradeLicenseData)
        {
            var returnArrayList = new ArrayList();
            var lstNames = GetFieldFromProductFormat(productId, formatId, 1).OrderBy(x => x.OriginalOrder).ToList();
            var strSearchValue = string.Empty;
            var strAlternativeValue = string.Empty;

            switch (upgradeLicenseData.UpgradeLicenseType)
            {
                case EnumCollection.UpgradeLicenseType.Analog:
                    {
                        strSearchValue = "An/En";
                        strAlternativeValue = "Analog/Encoder";
                        break;
                    }
                case EnumCollection.UpgradeLicenseType.IPP:
                    {
                        strSearchValue = "IP Ch";
                        strAlternativeValue = "IP Camera";
                        break;
                    }
                case EnumCollection.UpgradeLicenseType.PACDM:
                    {
                        strSearchValue = "PACDM";
                        strAlternativeValue = "PACDM";
                        break;
                    }
                case EnumCollection.UpgradeLicenseType.VL:
                    {
                        strSearchValue = "VideoLogix";
                        strAlternativeValue = "VideoLogix+";
                        break;
                    }
                case EnumCollection.UpgradeLicenseType.VLP:
                    {
                        strSearchValue = "VisionCount";
                        strAlternativeValue = "VisionCount+";
                        break;
                    }
                case EnumCollection.UpgradeLicenseType.ExternalMonitor:
                    {
                        strSearchValue = "Ext Mon";
                        strAlternativeValue = "Ext Monitor";
                        break;
                    }             
            }

            var lstFindSelectedValue = GetSelectedValuedByName(lstNames, strSearchValue, strAlternativeValue).ToList();

            for(var i = 0; i < settingDetails.Count; i++)
            {
                if (lstFindSelectedValue.Any(x => x.OriginalOrder == i + 1))
                {
                    var findSelectedValue = lstFindSelectedValue.First(x => x.OriginalOrder == i + 1);
                    // update here
                    var findValue =
                        findSelectedValue.ChildSelectValued.FirstOrDefault(
                            x => string.Equals(x.DropDownValue.ToLower(), settingDetails[i].ToString().ToLower()));

                    if (findValue != null)
                    {
                        returnArrayList.Add(I3Helper.GetInt(findValue.Id == -1 ? 0 : findValue.Id) +
                                            upgradeLicenseData.UpdateQty);
                    }
                    else
                    {
                        returnArrayList.Add(I3Helper.GetInt(settingDetails[i]) != -1
                                                ? I3Helper.GetInt(settingDetails[i]) + upgradeLicenseData.UpdateQty
                                                : upgradeLicenseData.UpdateQty);
                    }
                }
                else
                {
                    returnArrayList.Add(settingDetails[i]);
                }
            }

            return returnArrayList;
        }

        private IEnumerable<SelectValue> GetSelectedValuedByName(IEnumerable<SelectValue> list, string name, string alternativeName)
        {
            return
                list.Where(
                    x =>
                    string.Equals(x.DropDownValue.ToLower(), name.ToLower()) ||
                    string.Equals(x.DropDownValue.ToLower(), alternativeName.ToLower()));
        }
    }
}