using LicenseGen;
using LicenseManagement.Helpers;
using LicenseManagement.Models.License;
using MessagingToolkit.QRCode.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LicenseManagement.Services.License
{
    public class LicenseService : ILicenseService
    {
        #region Generate License and QRCode
        public LicenseAndQRCodeResponse GenerateLicenseAndQRCode(LicenseViewModel licenseVm)
        {
            //11/06/2019 - tqdung
            //Add product name to license file
            var lstProducts = (new I3License()).GetAllProducts2();
            var product = lstProducts.Where(p => p.Id == licenseVm.SaveLicenseSetting.ProductId).FirstOrDefault();
            var product_desc = product != null ? product.Description : licenseVm.SaveLicenseSetting.ProductId.ToString();

            //var randomId = Guid.NewGuid().ToString();
            //var randomId = string.Format("{0}-{1:D2}-{2:D2}", licenseVm.SerialNbr, DateTime.Now.Month, DateTime.Now.Day);
            var randomId = string.Format("{0}-{1:D2}-{2:D2}-{3}", licenseVm.SerialNbr, DateTime.Now.Month, DateTime.Now.Day, product_desc);

            var strPath =
                HttpContext.Current.Server.MapPath(string.Format("~/I3App_Files/Licenses/{0}", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr));

            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string filename = string.Format("{0}/{1}.lcs", strPath, randomId);

            if (File.Exists(filename))
                File.Delete(filename);

            LicenseAndQRCodeResponse response = new LicenseAndQRCodeResponse()
            {
                ListLicenseVmAndLicensePathCombine = new List<LicenseViewModelAndLicensePathCombine>()
            };

            try
            {
                GenerateLicenseFile(licenseVm, filename);

                byte[] data = File.ReadAllBytes(filename);

                //var strData = I3DSSHelper.ConvertByteArrayToHexaString(data);

                //I3DSSHelper.GenerateQRCodeImageLicenseWithCenterImage(strData, licenseVm.SerialNo,
                //                                                      string.Format("{0}/{1}.jpg", strPath, randomId),
                //                                                      ImageFormat.Jpeg);           

                EnumCollection.QRcode code;
                unsafe
                {
                    fixed (byte* p = data)
                    {
                        var myIntPtr = (IntPtr)p;
                        code = QRGeneratorAPI.QREncodeData(data.Length, myIntPtr, EnumCollection.QRecLevel.QR_ECLEVEL_L);
                    }
                }

                const int zoom = 3;
                //int xPadding = 10;
                //int yPadding = 10;
                if (code.width > 0)
                {
                    //using (var qr_image = new Bitmap(code.width * ZOOM, code.width * ZOOM))                    
                    using (var qrImage = new Bitmap(code.width * zoom, code.width * zoom))
                    {

                        for (int y = 0; y < code.width; y++)
                            for (int x = 0; x < code.width; x++)
                            {
                                for (int i = 0; i < zoom; i++)
                                    for (int j = 0; j < zoom; j++)
                                    {
                                        int xx = x * zoom + i;
                                        int yy = y * zoom + j;

                                        qrImage.SetPixel(xx, yy,
                                                         code.data[y * code.width + x] ? System.Drawing.Color.Black : System.Drawing.Color.White);
                                    }
                            }

                        //  e.Graphics.DrawImage(qr_image, new Point(X_PADDING, Y_PADDING));

                        if (File.Exists(string.Format("{0}/{1}.jpg", strPath, randomId)))
                            File.Delete(string.Format("{0}/{1}.jpg", strPath, randomId));

                        qrImage.Save(string.Format("{0}/{1}.jpg", strPath, randomId), ImageFormat.Jpeg);
                    }
                }

                // Save data to database for logging
                //StoreProcedureHelper.ExcecuteStoreProcedureNotReturn(
                //    StoreProcedureName.InsertDataToLicenseLog,
                //    licenseVm.CompanyID,
                //    licenseVm.ScreenID,
                //    PX.Data.PXAccess.GetUserID(),
                //    licenseVm.SaveLicenseSetting.ProductId,
                //    licenseVm.SerialNbr,
                //    licenseVm.RoutingNbr,
                //    licenseVm.MachineCode,
                //    licenseVm.UniqueId,
                //    licenseVm.SalesOrderNbr,
                //    !string.IsNullOrEmpty(licenseVm.InvoiceNbr) ? licenseVm.InvoiceNbr : "",
                //    !string.IsNullOrEmpty(licenseVm.WorkOrderNbr) ? licenseVm.WorkOrderNbr : "",
                //    !string.IsNullOrEmpty(licenseVm.CustomerPONbr) ? licenseVm.CustomerPONbr : "",
                //    I3Helper.ConvertObjectToByteArray(licenseVm.SaveLicenseSetting),
                //    string.Format("{0}/{1}/{2}.jpg", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                //    string.Format("{0}/{1}/{2}.lcs", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                //    licenseVm.AllowMultiMAC
                //    );

                //_log.Info("Success -- GenerateLicenseAndQRCode", randomId);

                response.ListLicenseVmAndLicensePathCombine.Add(new LicenseViewModelAndLicensePathCombine
                {
                    LicenseVm = licenseVm,
                    LicensePath = string.Format("{0}/{1}/{2}", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId)
                });
                response.RelativePath = string.Format("{0}/{1}/{2}", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId);

                return response;
            }
            catch (Exception ex)
            {
                //_log.Error("GenerateLicenseAndQRCode", ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Generate all in One License and QRCode
        //14/10/2019 - tqdung
        //Generate All-In-One License
        public LicenseAndQRCodeResponse GenerateAllInOneLicenseAndQRCode(LicenseViewModel licenseVm, AllInOneLicenseAndQRCodeRequestContent dataContent)
        {
            try
            {
                LicenseAndQRCodeResponse response = new LicenseAndQRCodeResponse()
                {
                    ListLicenseVmAndLicensePathCombine = new List<LicenseViewModelAndLicensePathCombine>()
                };


                var strPath = HttpContext.Current.Server.MapPath(string.Format("~/I3App_Files/Licenses/{0}", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr));
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                //var lstProducts = (new I3License()).GetAllProducts2();
                var strAllLicenseData = "";

                foreach (var item in dataContent.ListCombine)
                {
                    if (item.SaveLicenseSetting != null)
                    {
                        var license_settings = item.SaveLicenseSetting;
                        if (license_settings != null)
                        {
                            var randomId = string.Format("{0}-{1:D2}-{2:D2}-{3}", licenseVm.SerialNbr, DateTime.Now.Month, DateTime.Now.Day, item.I3Product.Description);

                            string filename = string.Format("{0}/{1}.lcs", strPath, randomId);

                            if (File.Exists(filename))
                                File.Delete(filename);

                            var product_license_vm = new LicenseViewModel
                            {
                                CompanyID = licenseVm.CompanyID,
                                ScreenID = licenseVm.ScreenID,
                                OrderType = licenseVm.OrderType,
                                SalesOrderNbr = licenseVm.SalesOrderNbr,
                                CustomerPONbr = licenseVm.CustomerPONbr,
                                InvoiceType = licenseVm.InvoiceType,
                                InvoiceNbr = licenseVm.InvoiceNbr,
                                DocType = licenseVm.DocType,
                                WorkOrderNbr = licenseVm.WorkOrderNbr,
                                Type = licenseVm.Type,
                                SerialNbr = licenseVm.SerialNbr,
                                MachineCode = licenseVm.MachineCode,
                                UniqueId = licenseVm.UniqueId,
                                RoutingNbr = licenseVm.RoutingNbr,
                                AllowMultiMAC = licenseVm.AllowMultiMAC,
                                SaveLicenseSetting = new SaveLicenseSetting
                                {
                                    ExpiredDate = license_settings.ExpiredDate,
                                    ExpiredDay = license_settings.ExpiredDay,
                                    FormatId = license_settings.FormatId,
                                    MachineCode = license_settings.MachineCode,
                                    ProductId = license_settings.ProductId,
                                    TypeExpired = license_settings.TypeExpired,
                                    UniqueId = license_settings.UniqueId,
                                    SettingDetails = license_settings.SettingDetails,
                                }
                            };

                            GenerateLicenseFile(product_license_vm, filename);

                            byte[] data = File.ReadAllBytes(filename);
                            var strData = I3Helper.ConvertByteArrayToHexaString(data);
                            strAllLicenseData = !string.IsNullOrEmpty(strAllLicenseData) ? strAllLicenseData + ";" + strData : strData;

                            response.ListLicenseVmAndLicensePathCombine.Add(new LicenseViewModelAndLicensePathCombine
                            {
                                LicenseVm = product_license_vm,
                                LicensePath = string.Format("{0}/{1}/{2}", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId)
                            });
                            // Save data to database for logging
                            //StoreProcedureHelper.ExcecuteStoreProcedureNotReturn(
                            //    StoreProcedureName.InsertDataToLicenseLog,
                            //    licenseVm.CompanyID,
                            //    licenseVm.ScreenID,
                            //    PX.Data.PXAccess.GetUserID(),
                            //    product_license_vm.SaveLicenseSetting.ProductId,
                            //    licenseVm.SerialNbr,
                            //    licenseVm.RoutingNbr,
                            //    licenseVm.MachineCode,
                            //    licenseVm.UniqueId,
                            //    licenseVm.SalesOrderNbr,
                            //    !string.IsNullOrEmpty(licenseVm.InvoiceNbr) ? licenseVm.InvoiceNbr : "",
                            //    !string.IsNullOrEmpty(licenseVm.WorkOrderNbr) ? licenseVm.WorkOrderNbr : "",
                            //    !string.IsNullOrEmpty(licenseVm.CustomerPONbr) ? licenseVm.CustomerPONbr : "",
                            //    I3Helper.ConvertObjectToByteArray(product_license_vm.SaveLicenseSetting),
                            //    string.Format("{0}/{1}/{2}.jpg", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                            //    string.Format("{0}/{1}/{2}.lcs", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                            //    licenseVm.AllowMultiMAC
                            //    );

                            // Save data to database for logging
                            //StoreProcedureHelper.ExcecuteStoreProcedureNotReturn(
                            //    StoreProcedureName.InsertDataToLicenseLog,
                            //    licenseVm.CompanyID,
                            //    licenseVm.ScreenID,
                            //    PX.Data.PXAccess.GetUserID(),
                            //    licenseVm.SaveLicenseSetting.ProductId,
                            //    licenseVm.SerialNbr,
                            //    licenseVm.RoutingNbr,
                            //    licenseVm.MachineCode,
                            //    licenseVm.UniqueId,
                            //    licenseVm.SalesOrderNbr,
                            //    !string.IsNullOrEmpty(licenseVm.InvoiceNbr) ? licenseVm.InvoiceNbr : "",
                            //    !string.IsNullOrEmpty(licenseVm.WorkOrderNbr) ? licenseVm.WorkOrderNbr : "",
                            //    !string.IsNullOrEmpty(licenseVm.CustomerPONbr) ? licenseVm.CustomerPONbr : "",
                            //    I3Helper.ConvertObjectToByteArray(licenseVm.SaveLicenseSetting),
                            //    string.Format("{0}/{1}/{2}.jpg", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                            //    string.Format("{0}/{1}/{2}.lcs", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, randomId),
                            //    licenseVm.AllowMultiMAC
                            //    );
                        }
                    }
                }

                if (!string.IsNullOrEmpty(strAllLicenseData))
                {
                    var allInOneId = string.Format("{0}-{1:D2}-{2:D2}-{3}", licenseVm.SerialNbr, DateTime.Now.Month, DateTime.Now.Day, "ONE-License");

                    string allInOneFilename = string.Format("{0}/{1}.lcsx", strPath, allInOneId);

                    if (File.Exists(allInOneFilename))
                        File.Delete(allInOneFilename);

                    //Save all-in-one license file
                    File.WriteAllText(allInOneFilename, strAllLicenseData);

                    #region Generate QR Code
                    //I3Helper.GenerateQRCodeImageLicense(strAllLicenseData, string.Format("{0}/{1}.jpg", strPath, allInOneId), ImageFormat.Jpeg);

                    EnumCollection.QRcode code = QRGeneratorAPI.QREncodeString(strAllLicenseData, EnumCollection.QRecLevel.QR_ECLEVEL_L);

                    const int zoom = 3;
                    if (code.width > 0)
                    {
                        using (var qrImage = new Bitmap(code.width * zoom, code.width * zoom))
                        {

                            for (int y = 0; y < code.width; y++)
                                for (int x = 0; x < code.width; x++)
                                {
                                    for (int i = 0; i < zoom; i++)
                                        for (int j = 0; j < zoom; j++)
                                        {
                                            int xx = x * zoom + i;
                                            int yy = y * zoom + j;

                                            qrImage.SetPixel(xx, yy, code.data[y * code.width + x] ? System.Drawing.Color.Black : System.Drawing.Color.White);
                                        }
                                }

                            if (File.Exists(string.Format("{0}/{1}.jpg", strPath, allInOneId)))
                                File.Delete(string.Format("{0}/{1}.jpg", strPath, allInOneId));

                            qrImage.Save(string.Format("{0}/{1}.jpg", strPath, allInOneId), ImageFormat.Jpeg);
                        }
                    }
                    #endregion

                    //_log.Info("Success -- GenerateAllInOneLicenseAndQRCode", allInOneId);

                    response.RelativePath = string.Format("{0}/{1}/{2}", "/I3App_Files/Licenses", licenseVm.Type == 1 ? licenseVm.WorkOrderNbr : licenseVm.SalesOrderNbr, allInOneId);

                    return response;
                }

                return null;
            }
            catch (Exception ex)
            {
                //_log.Error("GenerateAllInOneLicenseAndQRCode", ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Get List i3 Product
        public IEnumerable<I3Product> GetListI3Product()
        {
            var lstProducts = (new I3License()).GetAllProducts2();
            return lstProducts ?? new List<I3Product>();
        }
        #endregion

        #region SUPPORT METHOD

        #region Generate License File to project Method
        private void GenerateLicenseFile(LicenseViewModel licenseVM, string fileName)
        {
            var license = new i3license_info_t
            {
                size = (UInt16)System.Runtime.InteropServices.Marshal.SizeOf(typeof(i3license_info_t)),
                api_version = 0,
                serial_id = licenseVM.SerialNbr,
                unique_id = licenseVM.UniqueId.Replace("-", ""),
                machine_code = licenseVM.MachineCode.Replace("-", "")
            };

            string machinecode = license.machine_code;
            string extraMachineCode = string.Empty;
            if (machinecode.Length > 20)
            {
                if (licenseVM.AllowMultiMAC)
                    extraMachineCode = machinecode.Substring(20, machinecode.Length - 20);
                machinecode = machinecode.Substring(0, 20);
            }
            license.machine_code = machinecode;

            if (license.unique_id.Length > 32)
                license.unique_id = license.unique_id.Substring(0, 32);

            switch (licenseVM.SaveLicenseSetting.TypeExpired)
            {
                case EnumCollection.TypeExpired.None:
                    {
                        license.expired_days = 0;
                        license.expired_date = "";
                        break;
                    }
                case EnumCollection.TypeExpired.ExpiredNumber:
                    {
                        try
                        {
                            if (licenseVM.SaveLicenseSetting.ExpiredDay != null)
                                license.expired_days =
                                    (UInt16)int.Parse(licenseVM.SaveLicenseSetting.ExpiredDay.Value.ToString(CultureInfo.InvariantCulture));
                        }
                        catch (FormatException)
                        {
                            license.expired_days = 0;
                        }
                        catch (OverflowException)
                        {
                            license.expired_days = UInt16.MaxValue;
                        }
                        license.expired_date = "";
                        break;
                    }
                case EnumCollection.TypeExpired.ExpiredDate:
                    {
                        if (licenseVM.SaveLicenseSetting.ExpiredDate != null)
                            license.expired_date = licenseVM.SaveLicenseSetting.ExpiredDate.Value.ToString(@"yyyyMMdd");
                        license.expired_days = 0;
                        break;
                    }
            }

            int licenseBufSize = 0;
            var i3License = new I3License(licenseVM.SaveLicenseSetting.ProductId, licenseVM.SaveLicenseSetting.FormatId);
            var lstArrayValues = i3License.ReadTextToInt(licenseVM.SaveLicenseSetting.SettingDetails);
            IntPtr licenseBuf = i3License.GenerateBinaryStream(ref licenseBufSize, lstArrayValues);

            try
            {
                if (licenseBuf != IntPtr.Zero && licenseBufSize > 0)
                {
                    ////if (!TestLicenseApi.file_write(fileName, licenseBuf, licenseBufSize, license))
                    //if (!TestLicenseApi.file_write(fileName, licenseBuf, licenseBufSize, license, extraMachineCode))
                    IntPtr completedLicenseFileBuffer = IntPtr.Zero;
                    int completedLicenseFileBufferSize = 0;
                    if (!TestLicenseApi.license_write(licenseBuf, licenseBufSize, license, extraMachineCode, ref completedLicenseFileBuffer, ref completedLicenseFileBufferSize))
                    {
                        throw new Exception("Can't generate license file!");
                    }

                    byte[] completedLicenseBytes = new byte[completedLicenseFileBufferSize];
                    System.Runtime.InteropServices.Marshal.Copy(completedLicenseFileBuffer, completedLicenseBytes, 0, completedLicenseFileBufferSize);

                    File.WriteAllBytes(fileName, completedLicenseBytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //i3License.ConvertIntFieldsToText(lstArrayValues);
        }
        #endregion

        #endregion END SUPPORT METHOD

    }
}