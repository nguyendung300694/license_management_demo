using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using LicenseManagement.Models.View.Item;
using Font = System.Drawing.Font;
using Image = System.Drawing.Image;
using iTextSharp.text.pdf;
using iTextSharp.text;
using BarCodeGeneratorLibary;
using System.Web;

namespace LicenseManagement.Helpers
{
    public class BarCodeHelper
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //For creating pdf
        private PdfContentByte _pcb;
        private static readonly BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);

        public void GenerateBarcode(string data, string fileImgPath, SaveTypes imgType, TYPE type = TYPE.CODE128, bool includeLabel = true)
        {
            var barCode = new BarCodeGeneratorLibary.Barcode();

            barCode.Alignment = AlignmentPositions.CENTER;

            barCode.IncludeLabel = includeLabel;

            barCode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;

            barCode.LabelPosition = LabelPositions.BOTTOMCENTER;

            barCode.Encode(type, data, Color.Black, Color.White, Width, Height);

            barCode.SaveImage(fileImgPath, imgType);
        }

        public void GenerateCameraLabel(CameraLabelInfo cameraLabelInfo, string savePath, ImageFormat imgType,
            bool includeUPCLabel, bool includeSerialLabel)
        {
            Color tranColor = Color.Transparent;
            Color backColor = Color.White;
            Color penColor = Color.Black;

            int marginLeft = 5;
            int marginTop = 0;
            //bool ce = true;
            //Create UPC Barcode
            var upcBarCode = new BarCodeGeneratorLibary.Barcode();
            upcBarCode.Alignment = AlignmentPositions.CENTER;
            upcBarCode.IncludeLabel = includeUPCLabel;
            upcBarCode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            upcBarCode.LabelPosition = LabelPositions.BOTTOMCENTER;
            upcBarCode.Encode(TYPE.UPCA, cameraLabelInfo.UPCCode, penColor, backColor, Width, Height);

            //Create SerialNo Barcode
            var snBarCode = new BarCodeGeneratorLibary.Barcode();
            snBarCode.Alignment = AlignmentPositions.CENTER;
            snBarCode.IncludeLabel = includeSerialLabel;
            snBarCode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            snBarCode.LabelPosition = LabelPositions.BOTTOMCENTER;
            snBarCode.Encode(TYPE.CODE128, cameraLabelInfo.SerialNo, penColor, backColor, Width, Height);

            //Load FCRecycle Image
            Image fcrImg = null;
#if DEBUG
            fcrImg = Image.FromFile(@"C:\Users\QuocDung\Desktop\fcRecycle.png"); //This is 117x188
#else
            fcrImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Images/fcRecycle.png")); //This is 117x188
#endif

            Bitmap b = new Bitmap(292, 122);

            //Start drawing label
            Font font = new Font("MS Sans Serif", 10.5F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
            Font fontSmaller = new Font("MS Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));

            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(backColor);

                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                //draw description
                RectangleF rect = new RectangleF(marginLeft, marginTop, cameraLabelInfo.Description.Length * font.Size, font.Height);
                g.FillRectangle(new SolidBrush(tranColor), rect.X, rect.Y, rect.Width, rect.Height);
                g.DrawString(cameraLabelInfo.Description, font, new SolidBrush(penColor), rect.X, rect.Y);

                //draw model
                string strModel = "Model:       " + cameraLabelInfo.Model;
                rect = new RectangleF(marginLeft + 12, marginTop + 17, strModel.Length * font.Size, font.Height);
                g.FillRectangle(new SolidBrush(tranColor), rect.X, rect.Y, rect.Width, rect.Height);
                g.DrawString(strModel, font, new SolidBrush(penColor), rect.X, rect.Y);

                //draw upc barcode image
                rect = new RectangleF(marginLeft, marginTop + 34, this.Width, this.Height);
                g.DrawImage(upcBarCode.EncodedImage, rect);

                //draw serial no
                string strSerial = "SERIAL NO.:       " + cameraLabelInfo.SerialNo;
                rect = new RectangleF(marginLeft, marginTop + 69, strSerial.Length * fontSmaller.Size, fontSmaller.Height);
                g.FillRectangle(new SolidBrush(tranColor), rect.X, rect.Y, rect.Width, rect.Height);
                g.DrawString(strSerial, fontSmaller, new SolidBrush(penColor), rect.X, rect.Y);

                //draw serial no barcode image
                if (cameraLabelInfo.HasCELogo)
                {
                    rect = new RectangleF(marginLeft, marginTop + 85, this.Width / (float)1.3, this.Height / (float)1.7);
                    g.DrawImage(snBarCode.EncodedImage, rect);
                }
                else
                {
                    rect = new RectangleF(marginLeft, marginTop + 85, this.Width, this.Height / (float)1.7);
                    g.DrawImage(snBarCode.EncodedImage, rect);
                }

                if (!string.IsNullOrEmpty(cameraLabelInfo.MACAddress))
                {
                    //draw MAC address
                    string strMAC = "MAC:   " + cameraLabelInfo.MACAddress;
                    rect = new RectangleF(marginLeft, marginTop + 106, strMAC.Length * fontSmaller.Size, fontSmaller.Height);
                    g.FillRectangle(new SolidBrush(tranColor), rect.X, rect.Y, rect.Width, rect.Height);
                    g.DrawString(strMAC, fontSmaller, new SolidBrush(penColor), rect.X, rect.Y);
                }

                //draw fc/recycle image
                rect = new RectangleF(this.Width - 15, marginTop + 20, fcrImg.Width / (float)2, fcrImg.Height / (float)2);
                g.DrawImage(fcrImg, rect);

                if (cameraLabelInfo.HasCELogo)
                {
#if DEBUG
                    Image ceImg = Image.FromFile(@"C:\Users\QuocDung\Desktop\CELogo.png"); //This is 117x188
#else
                    Image ceImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Images/CELogo.png")); //This is 117x188
#endif
                    //draw ce logo image
                    rect = new RectangleF(this.Width - 60, marginTop + 80, ceImg.Width / (float)2, ceImg.Height / (float)2);
                    g.DrawImage(ceImg, rect);
                }

                g.Save();
            }

            b.Save(savePath, imgType);
        }

        public void GenerateCameraLabelPdf(CameraLabelInfo cameraLabelInfo, string savePath,
            bool includeUPCLabel, bool includeSerialLabel)
        {
            int marginLeft = 20;

            Color backColor = Color.White;
            Color penColor = Color.Black;

            var upcBarCode = new BarCodeGeneratorLibary.Barcode();
            upcBarCode.Alignment = AlignmentPositions.CENTER;
            upcBarCode.IncludeLabel = includeUPCLabel;
            upcBarCode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            upcBarCode.LabelPosition = LabelPositions.BOTTOMCENTER;
            upcBarCode.Encode(TYPE.UPCA, cameraLabelInfo.UPCCode, penColor, backColor, Width, Height + 10);

            //Create SerialNo Barcode
            var snBarCode = new BarCodeGeneratorLibary.Barcode();
            snBarCode.Alignment = AlignmentPositions.CENTER;
            snBarCode.IncludeLabel = includeSerialLabel;
            snBarCode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
            snBarCode.LabelPosition = LabelPositions.BOTTOMCENTER;
            snBarCode.Encode(TYPE.CODE128, cameraLabelInfo.SerialNo, penColor, backColor, Width, Height);

            //Load FCRecycle Image
            Image fcrImg = null;
#if DEBUG
            fcrImg = Image.FromFile(@"C:\Users\QuocDung\Desktop\fcRecycle.png"); //This is 117x188
#else
            fcrImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Images/fcRecycle.png")); //This is 117x188
#endif
            //Sticker size: 1.75inch x 0.75inch
            //Convert to pixel and double
            //Document document = new Document(new iTextSharp.text.Rectangle(336f, 144f));
            Document document = new Document(new iTextSharp.text.Rectangle(453.6f, 201.6f));
            //Document document = new Document();
            //PdfReader readerBicycle = null;

            try
            {
                FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Load the background image and add it to the document structure
                //readerBicycle = new PdfReader(@"F:\bicycle.pdf");
                //PdfTemplate background = writer.GetImportedPage(readerBicycle, 1);

                // Create a page in the document and add it to the bottom layer
                document.NewPage();
                _pcb = writer.DirectContentUnder;
                //_pcb.AddTemplate(background, 0, 0);

                // Get the top layer and write some text
                _pcb = writer.DirectContent;
                _pcb.BeginText();

                //draw description
                PrintText(cameraLabelInfo.Description, 20, marginLeft, 176);

                //draw model
                PrintText("Model:", 20, marginLeft + 5, 148);
                PrintText(cameraLabelInfo.Model, 24, marginLeft + 125, 148);

                PrintImageDirectly(upcBarCode.EncodedImage, marginLeft, 95, (float)Width, (float)Height + 10);

                //draw serial no
                PrintText("Serial No.:       " + cameraLabelInfo.SerialNo, 20, marginLeft + 5, 75);

                //draw serial no barcode image
                //if (cameraLabelInfo.HasCELogo)
                //{
                //    PrintImageDirectly(snBarCode.EncodedImage, marginLeft, 40, this.Width / (float)1.3, this.Height / (float)1.3);
                //}
                //else
                //{
                PrintImageDirectly(snBarCode.EncodedImage, marginLeft, 32, this.Width, this.Height);// / (float)1.3);
                //}

                if (!string.IsNullOrEmpty(cameraLabelInfo.MACAddress))
                {
                    //draw MAC address
                    PrintText("MAC:   " + cameraLabelInfo.MACAddress, 20, marginLeft + 5, 10);
                }

                //draw fc/recycle image
                PrintImageDirectly(fcrImg, marginLeft + 320, 15, fcrImg.Width / (float)1.2, fcrImg.Height / (float)1.2);

                if (cameraLabelInfo.HasCELogo)
                {
#if DEBUG
                    Image ceImg = Image.FromFile(@"C:\Users\QuocDung\Desktop\CELogo.png"); //This is 117x188
#else
                    Image ceImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Images/CELogo.png")); //This is 117x188
#endif
                    //draw ce logo image
                    PrintImageDirectly(ceImg, marginLeft + 244, 20, ceImg.Width / (float)1.15, ceImg.Height / (float)1.15);
                }

                _pcb.EndText();

                writer.Flush();
            }
            finally
            {
                //if (readerBicycle != null)
                //{
                //    readerBicycle.Close();
                //}
                document.Close();
            }
        }

        private void SetFontSize(int size)
        {
            _pcb.SetFontAndSize(font, size);
        }

        private void PrintText(string text, float fontSize, int x, int y)
        {
            _pcb.SetFontAndSize(font, fontSize);
            _pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, x, y, 0);
        }

        private void PrintTextCentered(string text, int x, int y)
        {
            _pcb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, x, y, 0);
        }

        private void PrintImageDirectly(Image image, int x, int y, float width, float height)
        {
            BaseColor color = null;
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image, color);
            img.SetAbsolutePosition(x, y);
            img.ScaleAbsolute(width, height);
            _pcb.AddImage(img);
        }

        //private void CreateBarcode128(string upcCode)
        //{
        //    iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
        //    bc.TextAlignment = Element.ALIGN_LEFT;
        //    bc.Code = upcCode;
        //    bc.StartStopText = false;
        //    bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
        //    bc.Extended = true;
        //    iTextSharp.text.Image img = bc.CreateImageWithBarcode(_pcb,
        //      iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);
        //    _pcb.SetTextMatrix(1.5f, 3.0f);
        //    //img.ScaleToFit(60, 5);
        //    img.ScaleAbsolute(250, 36);
        //    img.SetAbsolutePosition(0, 500);
        //    _pcb.AddImage(img);
        //}
    }
}