using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Web;

namespace LicenseGen
{
    [StructLayout(LayoutKind.Sequential)]
    public struct License_t
    {
        public int product_id;
        public int format_id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string machine_code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string serial_code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string comment;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string expired_date;
        public int expired_days;
        public string uniqueID;
    };

    public enum HASP_ERROR
    {
        HASP_UNKNOWN = -1,
        HASP_UNPLUGGED,			//HASP Key Unplugged
        HASP_WRONG,				//HASP Key is wrong format
        HASP_EXPIRED,			//HASP Key is expired

        HASP_RESULT_OK = 100,	//Read HASP Key successfully, value in HASP Key is TRUE
        HASP_RESULT_FAILED,		//Read HASP Key successfully, value in HASP Key is FALSE
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct i3license_info_t
    {
        public UInt16 size;

        public UInt16 api_version;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string machine_code;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string serial_id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string expired_date;

        public UInt16 expired_days;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string unique_id;
    };

    class TestLicenseApi
    {
        //[DllImport("I3App_Dlls\\LicenseDll.i3dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static bool save(string filename, string _xml_string);
        //[DllImport("I3App_Dlls\\LicenseDll.i3dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static int get_stream_length(string filename);
        //[DllImport("I3App_Dlls\\LicenseDll.i3dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static bool load(string filename, IntPtr _str_ptr);

        const CharSet CHARSET = CharSet.Ansi;
        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        const string LICENSE_DLL = "I3App_Dlls\\LicenseDll.i3dll";


        //#region Read / Write license files
        //[DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        //public extern static bool file_read(string _filename, ref IntPtr _block, ref int _size, ref i3license_info_t _info);

        //[DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        //public extern static bool file_write(string _filename, IntPtr _block, int _size, i3license_info_t _info);
        //#endregion Read / Write license files
        #region Read / Write license files
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        //public extern static bool file_read(string _filename, ref IntPtr _block, ref int _size, ref i3license_info_t _info, StringBuilder _extra_machine_code);
        public extern static bool license_read(IntPtr _data_in, int _data_in_len, ref IntPtr _block, ref int _size, ref i3license_info_t _info, StringBuilder _extra_machine_code);

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        //public extern static bool file_write(string _filename, IntPtr _block, int _size, i3license_info_t _info, string _extra_machine_code);
        public extern static bool license_write(IntPtr _block, int _size, i3license_info_t _info, string _extra_machine_code, ref IntPtr _data_out, ref int _data_out_len);

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static void license_free(IntPtr _data);
        #endregion Read / Write license files

        #region Master password
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool i3hasp_sac_is_valid_passwd(string _passwd);

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool i3hasp_sac_is_master_passwd(string _passwd);

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool i3hasp_sac_exist_passwd_file();

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool i3hasp_sac_authenticate_passwd(string _passwd);

        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool i3hasp_sac_write_passwd(string _passwd);
        #endregion Master password

        #region HASP key control
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static HASP_ERROR GetHaspKeyInfo();
        #endregion HASP key control

        #region Machine code
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool generate_machinecode([MarshalAs(UnmanagedType.LPStr)] StringBuilder _license_info);
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool verify_machinecode(string _machine_code/*input*/);
        [DllImport(LICENSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static bool parse_machinecode(string _machine_code/*input*/, StringBuilder _mac_address/*output*/, StringBuilder _cpu_id/*output*/);
        #endregion Machine code

        //[DllImport("I3App_Dlls\\LicenseDll.i3dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public extern static bool file_write(string _filename, IntPtr _block, int _size, i3license_info_t _info);
    }
}

namespace RuleParse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_product_t
    {
        public IntPtr name;
        public IntPtr desc;
        public int PCODE;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_format_t
    {
        public IntPtr name;
        public IntPtr desc;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_field_t
    {
        public IntPtr name;
        public IntPtr desc;

        public int type;
        public int size;

        public UInt32 minval;
        public UInt32 maxval;
        public UInt32 defval;
        public int suggest_count;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_suggest_t
    {
        public IntPtr name;
        public UInt32 value;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_model_t
    {
        public IntPtr name;

        // array for value referencing; must not be saved or deallocated
        // size is always same as the number of field (returned by 'i3hasprule_get_field_count')
        public int value_count;
        public IntPtr values;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct i3hasprule_epos_t
    {
        public int line;
        public int col;

        //public IntPtr errstr;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string errstr;
    };

    class RuleParseApi
    {
        const string RULEPARSE_DLL = "I3App_Dlls\\RuleParseDll.i3dll";
        //System.Web.HttpContext.Current.Server.MapPath(string.Format("~/I3App_Dlls/{0}", "RuleParseDll.i3dll"));

        const CharSet CHARSET = CharSet.Ansi;
        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static IntPtr i3haspruledll_create();
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static void i3haspruledll_destroy(IntPtr _rule);

        //
        // i3hasprule performs parsing each rule files by adding rule files
        // all files will be accumulated
        // the i3hasprule object should be recreated in order to clear all parsed information
        //
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_add_rulefile(IntPtr _rule, string _szPath, ref i3hasprule_epos_t _errpos /* optional */);
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_add_rule(IntPtr _rule, string _str, int _len, ref i3hasprule_epos_t _errpos /* optional */);

        //
        // accessing the parsed information requires two steps
        //	1. get the count of each information
        //	2. retrieve the information into the each structure
        //		all structures has only reference pointers; they must not be freed by outside
        // CAUTION:
        //	all data pointers must be used for reference only, so they can't be stored or deallocated
        //	in other word, the referred pointer to data is valid only until the next API call
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_product_count(IntPtr _rule);
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_refer_product(IntPtr _rule, int _idx_product, ref i3hasprule_product_t _product);

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_format_count(IntPtr _rule, int _idx_product);
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_refer_format(IntPtr _rule, int _idx_product, int _idx_format, ref i3hasprule_format_t _format);

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_field_count(IntPtr _rule, int _idx_product, int _idx_format);
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_refer_field(IntPtr _rule, int _idx_product, int _idx_format, int _idx_field, ref i3hasprule_field_t _field);
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_refer_suggest(IntPtr _rule, int _idx_product, int _idx_format, int _idx_field, int _idx_suggest, ref i3hasprule_suggest_t _suggest);

        //
        // _values parameter
        //	_values array contains all values of each filed
        //	_values[0] means the value of first field, _values[1] for the second, and so on
        //	the size of _values array must be equal or bigger than 'i3hasprule_field_count'
        //	'values' of i3hasprule_model_t can also be passed to 'i3hasprule_make_block'
        //

        //
        // lookup model interfaces
        //	1. begin
        //	2. count/refer
        //	3. end
        //
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_lookup_begin(IntPtr _rule, ref IntPtr _lookup /*out*/,
                                                        int _idx_product, int _idx_format,
                                                        string _model_substr,           // NULL if don't care
                                                        UInt32[] _values,               // NULL if don't care all; size == i3hasprule_field_count
                                                                                        // lookup model having the field values
                                                        int[] _effect_value_flags       // NULL if don't care all; size == i3hasprule_field_count
                                                                                        // if ( _effect_value_flags[i] ) _values[i] will be used during lookup
                                                        );

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_lookup_limit_begin(IntPtr _rule, ref IntPtr _lookup /*out*/,
                                                        int _max_lookup_count,      // -1 == no limit, 
                                                                                    // intOOMANYLOOKUP may be returned with limit
                                                        int _idx_product,
                                                        int _idx_format,
                                                        string _model_substr,       // NULL if don't care
                                                        UInt32[] _values,           // NULL if don't care all; size == i3hasprule_field_count
                                                                                    // lookup model having the field values
                                                        int[] _effect_value_flags   // NULL if don't care all; size == i3hasprule_field_count
                                                                                    // if ( _effect_value_flags[i] ) _values[i] will be used during lookup
                                                        );

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_lookup_count(IntPtr _lookup);

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_lookup_refer(IntPtr _lookup, int _index, ref i3hasprule_model_t _model);

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static void i3haspruledll_lookup_end(IntPtr _lookup);

        //
        // extract all values from binary data block according to current parsed information
        // making binary data block based on current parsed information
        //
        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_parse_block(IntPtr _rule,
                                                        IntPtr _block, int _block_size,
                                                        ref int _idx_product /*out*/, ref int _idx_format /*out*/,
                                                        ref IntPtr _values/*out; allocated*/, ref int _values_size /* out */);

        [DllImport(RULEPARSE_DLL, CharSet = CHARSET, CallingConvention = CALLING_CONVENTION)]
        public extern static int i3haspruledll_make_block(IntPtr _rule, int _idx_product,
                                                        int _idx_format, UInt32[] _values,
                                                        ref IntPtr _block /* out; allocated */, ref int _block_size /* out */);
    }
}
