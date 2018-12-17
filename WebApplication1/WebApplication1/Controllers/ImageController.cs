using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)

        {

            // System.Drawing.Image img = System.Drawing.Image.FromFile("C:/Users/18651/source/repos/WebApplication1/WebApplication1/Image/IMG_8148183549382.JPG");
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageModel.ImageFile.InputStream);
            if (originalImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                            // de-rotate:
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate180FlipNone);
                       


                        break;

                    case 6: // rotated 90 left
                        originalImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }


            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            
            imageModel.ImageFile.SaveAs(fileName);
            using (DbModels db = new DbModels())
            {
                db.Images.Add(imageModel);
                db.SaveChanges();
            }
            originalImage.Save(fileName);
            ModelState.Clear();


            return View();
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            Image imageModel = new Image();
            using (DbModels db = new DbModels())
            {
                imageModel = db.Images.Where(x => x.ImageID == id).FirstOrDefault();
         
                
                //      System.Drawing.Image img = System.Drawing.Image.FromFile("C:/Users/18651/source/repos/WebApplication1/WebApplication1/Image/IMG_8148183549382.JPG");
                //      byte[]  imagebyt = converterDemo(img);
                //      using (
                //System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imagebyt, 0, imagebyt.Length))
                //)
                //      {
                //          //image.Width = EndSheetWidth;

                //          System.Drawing.Imaging.PropertyItem[] properties = image.PropertyItems;

                //          int Orientation = 0;

                //          foreach (System.Drawing.Imaging.PropertyItem p in properties)
                //          {

                //              if (p.Id == 274)
                //              {

                //                  Orientation = (int)p.Value[0];

                //                  if (Orientation == 6)

                //                      image.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

                //                  if (Orientation == 8)

                //                      image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);

                //                  break;

                //              }

                //          }
                //          byte[] imagebyt2 = converterDemo(image);
                //          using (MemoryStream mStream = new MemoryStream(imagebyt2))
                //          {
                //               System.Drawing.Image image2 = System.Drawing.Image.FromStream(mStream);

                //          }

                //...more code
                //   }//end using

            }
            //return base.File("C:/Users/18651/source/repos/WebApplication1/WebApplication1/Image/IMG_8151.JPG", "image/jpeg");

            return View(imageModel);
        }
        public static byte[] converterDemo(System.Drawing.Image x)
        {
            System.Drawing.ImageConverter _imageConverter = new System.Drawing.ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

    }
}