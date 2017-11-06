namespace Hidistro.UI.Web.API
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;
    using ThoughtWorks.QRCode.Codec;

    public class CreatQRCode : IHttpHandler
    {
        public static MemoryStream CombinImage(Image imgBack, Image img)
        {
            Bitmap image = new Bitmap(400, 0xc5);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            graphics.DrawImage(img, 270, 0x34, 100, 100);
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return stream;
        }

        public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image image = new Bitmap(newW, newH);
                Graphics graphics = Graphics.FromImage(image);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                graphics.Dispose();
                return image;
            }
            catch
            {
                return null;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request["code"];
            string str2 = context.Request["Finger"];
            string text1 = context.Request["Logo"];
            if (!string.IsNullOrEmpty(str))
            {
                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                encoder.QRCodeScale = 4;
                encoder.QRCodeVersion = 8;
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                Image imgBack = encoder.Encode(str);
                MemoryStream stream = new MemoryStream();
                imgBack.Save(stream, ImageFormat.Png);
                if (!string.IsNullOrEmpty(str2) && (str2 == "true"))
                {
                    Image img = Image.FromFile(context.Server.MapPath("/templates/common/images/fingerprint.png"));
                    MemoryStream stream2 = CombinImage(imgBack, img);
                    context.Response.ClearContent();
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(stream2.ToArray());
                    stream2.Dispose();
                }
                else
                {
                    context.Response.ClearContent();
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(stream.ToArray());
                }
                stream.Dispose();
            }
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

