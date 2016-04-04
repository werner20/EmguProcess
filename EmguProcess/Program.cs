//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Web;
//using System.Web.Script.Serialization;
//using System.Runtime.InteropServices;
//using Accord.Vision.Detection;
//using Accord.Vision.Detection.Cascades;
//using Accord.Imaging.Filters;
//using AForge.Imaging;
//using Emgu.CV;
//using Emgu.Util;
//using Emgu.CV.Structure;
//using Emgu.CV.CvEnum;
//using Emgu.CV.UI;
//using System.Data.SqlClient;


//namespace EmguProcess
//{
//    class FaceTracking
//    {
//        static void Main(string[] args)
//        {
//            SqlConnection dbConn = new SqlConnection();
//            dbConn.ConnectionString = ("Server = tcp:capstonettdb2016.database.windows.net,1433; Database = CapstoneTT2016_db; User ID = dbadmin@capstonettdb2016; Password =TeamTechSMith85*; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
//            dbConn.Open();
//            dbConn.Close();

//            string fileLocation = args[0];
//            string xmlLocation = args[1];
//            int videoID = Convert.ToInt32(args[2]);

//            Console.WriteLine(fileLocation);
//            Capture video = new Capture(fileLocation);
//            //string xml = "haarcascade_frontalface_alt2.xml";

//            if (xmlLocation != "encode")
//            {

//                CascadeClassifier frontalFaces = new CascadeClassifier(xmlLocation);
//                double frameNum = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
//                //List<List<Tuple<Rectangle, int>>> FrameList = new List<List<Tuple<Rectangle, int>>>();
//                List<List<Rectangle>> FrameList = new List<List<Rectangle>>();
//                double currentFrame;


//                for (double i = 0; i < frameNum; i++)
//                {
//                    if (i % 10 == 0)
//                    {
//                        double percent = i / frameNum;
//                        string query = "UPDATE Videos SET PercentProcess=@Percent" + " WHERE VideoId=@Id";
//                        SqlCommand cmd = new SqlCommand(query, dbConn);
//                        cmd.Parameters.AddWithValue("@Percent", percent);
//                        cmd.Parameters.AddWithValue("@Id", videoID);

//                        cmd.Connection.Open();
//                        cmd.ExecuteNonQuery();
//                        dbConn.Close();
//                    }
//                    currentFrame = i;
//                    video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, i);
//                    Mat frame = video.QueryFrame();

//                    if (frame != null)
//                    {
//                        Image<Bgr, byte> frameImage = frame.ToImage<Bgr, byte>();
//                        Image<Gray, byte> grayFrameImage = frameImage.Convert<Gray, byte>();
//                        Rectangle[] frontalArray = frontalFaces.DetectMultiScale(grayFrameImage);

//                        //List<Tuple<Rectangle, int>> faces = new List<Tuple<Rectangle, int>>();
//                        //Tuple<Rectangle, int> effects;
//                        List<Rectangle> faces = new List<Rectangle>();


//                        if (frontalArray == null || frontalArray.Length == 0)
//                        {

//                            Rectangle empty = new Rectangle(0, 0, 1, 1);
//                            //int effect = 0;
//                            //effects = new Tuple<Rectangle, int>(empty, effect);
//                            //faces.Add(effects);
//                            faces.Add(empty);


//                        }
//                        else
//                        {


//                            foreach (Rectangle rect in frontalArray)
//                            {
//                                ////Default all rectangles as effect 1 which is blur
//                                //int effect = 1;
//                                //effects = new Tuple<Rectangle, int>(rect, effect);
//                                //faces.Add(effects);
//                                faces.Add(rect);                                    
//                            }


//                        }
//                        FrameList.Add(faces);
//                    }

//                }


//                JavaScriptSerializer Jserializer = new JavaScriptSerializer();
//                string queryString = "UPDATE Videos SET EditedVideo=@FrameList, PercentProcess=@PercentProcess" +
//                " WHERE VideoId=@Id";
//                SqlCommand command = new SqlCommand(queryString, dbConn);
//                command.Parameters.AddWithValue("@FrameList", Jserializer.Serialize(FrameList));
//                command.Parameters.AddWithValue("@Id", videoID);
//                command.Parameters.AddWithValue("@PercentProcess", 100);
//                StringBuilder errorMessages = new StringBuilder();
//                command.Connection.Open();
//                command.ExecuteNonQuery();
//                dbConn.Close();
//            }
//            else
//            {
//                Mat frame = new Mat();
//                double height = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
//                int h = Convert.ToInt32(height);
//                double width = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
//                int w = Convert.ToInt32(width);
//                double frameRate = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
//                int FPS = Convert.ToInt32(frameRate);
//                Size vidsize = new Size(w, h);
//                string outputVideo = fileLocation + "_ouput.mpg";


//                int fourcc = VideoWriter.Fourcc('M', 'P', 'E', 'G');


//                VideoWriter vidWriter = new VideoWriter(outputVideo, -1, FPS, vidsize, true);
//                for (int i = 0; i < .Count(); i++)
//                {
//                    cap.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, i);
//                    frame = cap.QueryFrame();

//                    if (frame != null)
//                    {
//                        Mat blurredObject = new Mat();
//                        blurredObject = frame;


//                        foreach (Tuple<Rectangle, int> pair in FrameList[indx])
//                        {
//                            Rectangle face = pair.Item1;
//                            Mat roi = new Mat(blurredObject, face);
//                            Size size = new Size(23, 23);
//                            if (pair.Item2 == 1)
//                            {
//                                CvInvoke.GaussianBlur(roi, roi, size, 30);
//                            }
//                            if (pair.Item2 == 2)
//                            {

//                                //RectanglesMarker marker = new RectanglesMarker(face, Color.Red);
//                                Image<Bgr, byte> Image = new Image<Bgr, byte>(blurredObject.Bitmap);
//                                Image.Draw(face, new Bgr(0, 0, 255), 3);
//                                blurredObject = Image.Mat;



//                            }

//                            vidWriter.Write(blurredObject);

//                        }
//                    }
//                }




//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using Accord.Imaging.Filters;
using AForge.Imaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using System.Data.SqlClient;
using System.Diagnostics;
namespace EmguProcess
{
    class FaceTracking
    {
        static void Main(string[] args)
        {
            SqlConnection dbConn = new SqlConnection();
            dbConn.ConnectionString = ("Server = tcp:capstonettdb2016.database.windows.net,1433; Database = CapstoneTT2016_db; User ID = dbadmin@capstonettdb2016; Password =TeamTechSMith85*; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
            dbConn.Open();
            dbConn.Close();
            string videoURI = args[0];
            string xmlLocation = args[1];
            int videoID = Convert.ToInt32(args[2]);

            string[] parsedat = videoURI.Split('/');
            string blobname = parsedat[parsedat.Length - 1];
            string containername = parsedat[parsedat.Length - 2];
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=capstonettvideos;AccountKey=tzqrOqNO1ybzVwKwsGhv2X2hhh2DUcW6e/732/5xCO8vHgrdkpslJ4XcKeR6jxfVDoYfHk0rNCmvKQDR5XNRzA==;BlobEndpoint=https://capstonettvideos.blob.core.windows.net/;TableEndpoint=https://capstonettvideos.table.core.windows.net/;QueueEndpoint=https://capstonettvideos.queue.core.windows.net/;FileEndpoint=https://capstonettvideos.file.core.windows.net/");
            CloudBlobClient blobclient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference(containername);
            CloudBlockBlob blockblob = container.GetBlockBlobReference(blobname);
            string storageloc;

            if (xmlLocation != "encode")
            {
                if (xmlLocation == "D:/home/site/wwwroot/bin/haarcascade_frontalface_alt2.xml")
                {
                    storageloc = Path.Combine("D:/local/Temp/", "front" + blobname);
                }
                else
                {
                    storageloc = Path.Combine("D:/local/Temp/", "side" + blobname);
                }
                using (var fileStream = System.IO.File.OpenWrite(storageloc))
                {
                    blockblob.DownloadToStream(fileStream);
                }
                track(storageloc, xmlLocation, videoID, ref dbConn);
            }
            else
            {
                storageloc = Path.Combine("D:/local/Temp/", "encode" + blobname);
                using (var fileStream = System.IO.File.OpenWrite(storageloc))
                {
                    blockblob.DownloadToStream(fileStream);
                }
                // Reserved for an export function
            }
        }
        static void track(string fileLocation, string xmlLocation, int videoID, ref SqlConnection dbConn)
        {
            Capture video = new Capture(fileLocation);
            //string xml = "haarcascade_frontalface_alt2.xml";
            CascadeClassifier frontalFaces = new CascadeClassifier(xmlLocation);
            double frameNum = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            List<List<Rectangle>> FrameList = new List<List<Rectangle>>();
            double currentFrame;

            for (double i = 0; i < frameNum; i++)
            {
                if (i % 10 == 0)
                {
                    string query = "";
                    double percent = i / frameNum * 100;
                    if (xmlLocation == "D:/home/site/wwwroot/bin/haarcascade_frontalface_alt2.xml")
                    {
                        query = "UPDATE Videos SET PercentProcess=@Percent" + " WHERE VideoId=@Id";
                    }
                    else
                    {
                        query = "UPDATE Videos SET ProgressSide=@Percent" + " WHERE VideoId=@Id";
                    }
                    SqlCommand cmd = new SqlCommand(query, dbConn);
                    cmd.Parameters.AddWithValue("@Percent", percent);
                    cmd.Parameters.AddWithValue("@Id", videoID);

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    dbConn.Close();
                }
                currentFrame = i;
                video.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, i);
                Mat frame = video.QueryFrame();

                if (frame != null)
                {
                    Image<Bgr, byte> frameImage = frame.ToImage<Bgr, byte>();
                    Image<Gray, byte> grayFrameImage = frameImage.Convert<Gray, byte>();
                    Rectangle[] frontalArray = frontalFaces.DetectMultiScale(grayFrameImage);
                    RectanglesMarker marker = new RectanglesMarker(frontalArray, Color.Fuchsia);

                    List<Rectangle> faces = new List<Rectangle>();

                    if (frontalArray == null || frontalArray.Length == 0)
                    {
                        // Do Nothing
                    }
                    else
                    {
                        foreach (Rectangle rect in frontalArray)
                        {
                            //Default all rectangles as effect 1 which is blur
                            faces.Add(rect);
                        }
                    }
                    FrameList.Add(faces);
                }
            }

            JavaScriptSerializer Jserializer = new JavaScriptSerializer();
            string queryString = "";
            if (xmlLocation == "D:/home/site/wwwroot/bin/haarcascade_frontalface_alt2.xml")
            {
                queryString = "UPDATE Videos SET EditedVideo=@FrameList, PercentProcess=@PercentProcess" + " WHERE VideoId=@Id";
            }
            else
            {
                queryString = "UPDATE Videos SET EditedVideoSide=@FrameList, ProgressSide=@PercentProcess" + " WHERE VideoId=@Id";
            }
            SqlCommand command = new SqlCommand(queryString, dbConn);
            command.Parameters.AddWithValue("@FrameList", Jserializer.Serialize(FrameList));
            command.Parameters.AddWithValue("@Id", videoID);
            command.Parameters.AddWithValue("@PercentProcess", 100);
            StringBuilder errorMessages = new StringBuilder();
            command.Connection.Open();
            command.ExecuteNonQuery();
            dbConn.Close();
        }

        /* I figure we can always just have this guy read the list of tuple<rect, int> from the database.EditedVideoSide column and delete it immeadiately afterwords.
        That's my best genius plan for now, truly a tragedy. */

        static void export(string fileLocation, int videoID, SqlConnection dbconn, CloudBlobContainer container)
        {
            Capture cap = new Capture(fileLocation);
            Mat frame = new Mat();
            double height = cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            int h = Convert.ToInt32(height);
            double width = cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            int w = Convert.ToInt32(width);
            double frameRate = cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            int FPS = Convert.ToInt32(frameRate);
            Size vidsize = new Size(w, h);
            string outputVideo = fileLocation + "_temp.mpeg";
            double frameNum = cap.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

            int fourcc = VideoWriter.Fourcc('M', 'P', 'E', 'G');
            VideoWriter vidWriter = new VideoWriter(outputVideo, fourcc, FPS, vidsize, true);

            string queryString = "SELECT editedvideoside FROM Videos WHERE VideoId=@Id";
            SqlCommand command = new SqlCommand(queryString, dbconn);
            command.Parameters.AddWithValue("@Id", videoID);
            //string dataStruct = command.ExecuteReader();
            string dataStruct = (string)command.ExecuteScalar();

            List<List<Tuple<Rectangle, int>>> FrameList = new JavaScriptSerializer().Deserialize<List<List<Tuple<Rectangle, int>>>>(dataStruct);
            
            // command.Parameters.AddWithValue("@PercentProcess", 100);


            for (int i = 0; i < FrameList.Count(); i++)
            {
                cap.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, i);
                frame = cap.QueryFrame();

                if (i % 10 == 0)
                {
                    double percent = i / frameNum * 100;
                    string query = "UPDATE Videos SET ProgressSide=@Percent" + " WHERE VideoId=@Id";
                    SqlCommand update = new SqlCommand(query, dbconn);
                    update.Parameters.AddWithValue("@Percent", percent);
                    update.Parameters.AddWithValue("@Id", videoID);

                    update.Connection.Open();
                    update.ExecuteNonQuery();
                    dbconn.Close();
                }
                if (frame != null)
                {
                    Mat blurredObject = new Mat();
                    blurredObject = frame;

                    foreach (Tuple<Rectangle, int> pair in FrameList[i])
                    {
                        Rectangle face = pair.Item1;
                        Mat roi = new Mat(blurredObject, face);
                        Size size = new Size(23, 23);
                        if (pair.Item2 == 1)
                        {
                            CvInvoke.GaussianBlur(roi, roi, size, 30);
                        }
                        else if (pair.Item2 == 2)
                        {
                            //RectanglesMarker marker = new RectanglesMarker(;
                            Image<Bgr, byte> Image = new Image<Bgr, byte>(blurredObject.Bitmap);
                            Image.Draw(face, new Bgr(0, 0, 255), 3);
                            blurredObject = Image.Mat;
                        }
                    }
                    vidWriter.Write(blurredObject);
                }
            }
            string command2 = System.Web.HttpContext.Current.Server.MapPath("/bin/ffmpeg.exe");
            string vidNum = Convert.ToString(videoID);
            string mp4outloc = Path.Combine("D:/local/Temp/",  vidNum + "_output.mp4");
            //string mp4outloc = Path.Combine("D:/home/site/wwwroot/bin/", v.Title + "_output.mp4");
            string args = "-i " + command2 + " " + mp4outloc;
            Process process = new Process();
            process.StartInfo.Arguments = args;
            process.StartInfo.FileName = command2;
            process.Start();
            process.WaitForExit();

            CloudBlockBlob editedVid = container.GetBlockBlobReference(vidNum + "_output.mp4");
            using (var upStream = System.IO.File.OpenRead(mp4outloc))
            {
                editedVid.UploadFromStream(upStream);
            }

            string vidURI = Convert.ToString(editedVid.Uri);
            queryString = "INSERT INTO Video VALUES (@vidTitle, @vidURI)";
            SqlCommand cmd = new SqlCommand(queryString, dbconn);
            cmd.Parameters.AddWithValue("@vidTitle", "EditedVideo#" + vidNum);
            cmd.Parameters.AddWithValue("@vidURI", vidURI);



        }
    }
}