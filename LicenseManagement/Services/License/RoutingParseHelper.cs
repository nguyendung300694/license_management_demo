using System;
using System.Collections.Generic;
using LicenseManagement.Helpers;
using LicenseManagement.Models.View.License;

namespace LicenseManagement.Services.License
{
    public static class RoutingParseHelper
    {
        private static readonly Dictionary<string, int> LstVL = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"A", 10},
            {"B", 11},
            {"C", 12},
            {"D", 13},
            {"E", 14},
            {"F", 15},
            {"G", 16},
        };

        private static readonly Dictionary<string, int> LstVLP = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"A", 10},
            {"B", 11},
            {"C", 12},
            {"D", 13},
            {"E", 14},
            {"F", 15},
            {"G", 16},
        };

        private static readonly Dictionary<string, int> LstPACDM = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"A", 10},
            {"B", 11},
            {"C", 12},
            {"D", 13},
            {"E", 14},
            {"F", 15},
            {"G", 16},
        };

        private static readonly Dictionary<string, int> LstISearch = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"A", 10},
            {"B", 11},
            {"C", 12},
            {"D", 13},
            {"E", 14},
            {"F", 15},
            {"G", 16},
        };

        private static Dictionary<string, int> _lstdDisplay = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
        };

        private static readonly Dictionary<string, int> LstAI = new Dictionary<string, int>
        {
            {"0", 0},
            {"1", 1},
            {"2", 2},
            {"3", 3},
            {"4", 4},
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8},
            {"9", 9},
            {"A", 10},
            {"B", 11},
            {"C", 12},
            {"D", 13},
            {"E", 14},
            {"F", 15},
            {"G", 16},
        };

        private static int DefaultMaxConnection = 50;

        public static void ParseRoutingWith14Characters(RoutingParseDetail result, string modelName)
        {
            var model = modelName.Substring(0, 2).ToUpper();

            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();


                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(4, 2)));
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(2, 2)));
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstPACDM[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(10, 1)]);
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(11, 1)]);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(10, 1)]);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(11, 1)]);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstISearch[modelName.Substring(12, 1)]);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(_lstdDisplay[modelName.Substring(13, 1)]);
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(13, 1)));
                        break;
                    }
                    case "Audio":
                    {
                        var audio = modelName.Substring(2, 1).ToLower() == "p" ? 8 : 0;
                        result.RoutingParseInfo[i].FieldValue.Add(audio);
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(255); //Louel 14/11/2018
                        break;
                    }
                    case "Version":
                    {
                        var version = model == "AL" ? 3 : 2;
                        result.RoutingParseInfo[i].FieldValue.Add(version);
                        break;
                    }
                    case "Max Connection":
                    {
                        var maxConn = model == "AL" || model == "AC" || model == "AP" ? 16 : DefaultMaxConnection;
                        result.RoutingParseInfo[i].FieldValue.Add(maxConn);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }

        public static void ParseURRoutingWith14Characters(RoutingParseDetail result, string modelName) //UR: Pro & AI
        {
            var model = modelName.Substring(0, 2).ToUpper();

            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();
                
                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(6, 2)));
                            break;
                        }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(4, 2)));
                            break;
                        }
                    case "PACDM":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(LstPACDM[modelName.Substring(11, 1)]);
                            break;
                        }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "iSearch":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "Heatmap":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(13, 1)));
                            break;
                        }
                    case "Audio":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "Sensor":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(64);
                            break;
                        }
                    case "Control":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(64);
                            break;
                        }
                    case "LPR":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "FPS":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(0);
                            break;
                        }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(255); //Louel 14/11/2018
                            break;
                        }
                    case "Version":
                        {
                            var version = model == "AL" ? 3 : 2;
                            result.RoutingParseInfo[i].FieldValue.Add(version);
                            break;
                        }
                    case "Max Connection":
                        {
                            var maxConn = model == "AL" || model == "AC" || model == "AP" ? 16 : DefaultMaxConnection;
                            result.RoutingParseInfo[i].FieldValue.Add(maxConn);
                            break;
                        }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(6);
                            break;
                        }
                    case "AI":
                    case "Old AI":
                    //case "VA":
                    //case "Old VA":
                    case "Ai":
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(LstAI[modelName.Substring(12, 1)]);
                            break;
                        }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                        {
                            result.RoutingParseInfo[i].FieldValue.Add(-1);
                            break;
                        }
                }
            }
        }

        public static void ParseSoftWareLicenseIPPWith15Characters(RoutingParseDetail result, string modelName)
        {
            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();

                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(3, 2)));
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(16);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstPACDM[modelName.Substring(7, 1)]);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstISearch[modelName.Substring(10, 1)]);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(_lstdDisplay[modelName.Substring(11, 1)]);
                        break;
                    }
                    case "Audio":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(4);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(13, 1)));
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(960);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(14, 1)));
                        break;
                    }
                    case "Version":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(2);
                        break;
                    }
                    case "Max Connection":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(DefaultMaxConnection);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }

        public static void ParseSoftWareLicenseIPPWith12Characters(RoutingParseDetail result, string modelName)
        {
            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();

                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(3, 2)));
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(16);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstPACDM[modelName.Substring(7, 1)]);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstISearch[modelName.Substring(10, 1)]);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(_lstdDisplay[modelName.Substring(11, 1)]);
                        break;
                    }
                    case "Audio":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(4);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(1);
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(960);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(255); //Louel 14/11/2018
                            break;
                    }
                    case "Version":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(2);
                        break;
                    }
                    case "Max Connection":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(2);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }

        public static void ParseExceptionRoutingWith20Characters(RoutingParseDetail result, string modelName)
        {
            var lastIndexSeparate = modelName.ToUpper().LastIndexOf("-", StringComparison.Ordinal);

            int ipNumberFromRouting = I3Helper.GetInt(modelName.Substring(lastIndexSeparate + 1).Replace("IP", ""));

            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();


                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(ipNumberFromRouting);
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Audio":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(1);
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(255); //Louel 14/11/2018
                            break;
                    }
                    case "Version":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(2);
                        break;
                    }
                    case "Max Connection":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(DefaultMaxConnection);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }

        public static void ParseSoftWareLicenseSLWith15Characters(RoutingParseDetail result, string modelName)
        {
            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();

                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(4, 2)));
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(2, 2)));
                        //result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstPACDM[modelName.Substring(7, 1)]);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(LstVL[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(8, 1)]);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstVLP[modelName.Substring(9, 1)]);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(LstISearch[modelName.Substring(10, 1)]);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(_lstdDisplay[modelName.Substring(11, 1)]);
                        break;
                    }
                    case "Audio":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(4);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(13, 1)));
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(960);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        result.RoutingParseInfo[i].DisableField = true;
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(I3Helper.GetInt(modelName.Substring(14, 1)));
                        break;
                    }
                    case "Version":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(2);
                        break;
                    }
                    case "Max Connection":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(DefaultMaxConnection);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }

        public static void GetDefaultValueForAll(RoutingParseDetail result)
        {
            for (var i = 0; i < result.RoutingParseInfo.Count; i++)
            {
                if (result.RoutingParseInfo[i].FieldValue == null)
                    result.RoutingParseInfo[i].FieldValue = new List<int>();


                switch (result.RoutingParseInfo[i].FieldName)
                {
                    case "IP Camera":
                    case "IP Ch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Analog/Encoder":
                    case "Analog":
                    case "An/En":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "PACDM":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VideoLogix Basic":
                    case "VideoLogix":
                    case "VL":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VisionCount Basic":
                    case "VisionCount":
                    case "VC":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VideoLogix Plus":
                    case "VideoLogix+":
                    case "VL+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "VisionCount Plus":
                    case "VisionCount+":
                    case "VC+":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "iSearch":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Heatmap":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Ext Monitor":
                    case "External Monitor":
                    case "Ext Mon":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(_lstdDisplay[modelName.Substring(13, 1)]);
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Audio":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Sensor":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "Control":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(64);
                        break;
                    }
                    case "LPR":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "FPS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Face Blur":
                    case "Face Blurring":
                    case "FB":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "CMS Mode":
                    case "CMSMode":
                    case "CMS":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(255); //Louel 14/11/2018
                        break;
                    }
                    case "Version":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                    case "Max Connection":
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(0);
                        break;
                    }
                    case "Upgradable":
                    case "Upgrade to version":
                    case "Upgrade To Version":
                    {
                        //result.RoutingParseInfo[i].FieldValue.Add(0);
                        //result.RoutingParseInfo[i].FieldValue.Add(1);
                        result.RoutingParseInfo[i].FieldValue.Add(6);
                        break;
                    }
                    default: //Version, Max Connection, Upgradable (not yet rule to parse)
                    {
                        result.RoutingParseInfo[i].FieldValue.Add(-1);
                        break;
                    }
                }
            }
        }
    }
}