using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using Accord.Imaging.Filters;
using AForge.Imaging;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using System.Data.SqlClient;


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

            string fileLocation = args[0];
            string xmlLocation = args[1];
            int videoID = Convert.ToInt32(args[2]);

            Console.WriteLine(fileLocation);
            Capture video = new Capture(fileLocation);
            //string xml = "haarcascade_frontalface_alt2.xml";

            CascadeClassifier frontalFaces = new CascadeClassifier(xmlLocation);
            double frameNum = video.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            List<List<Tuple<Rectangle, int>>> FrameList = new List<List<Tuple<Rectangle, int>>>();
            double currentFrame;
            

            for (double i = 0; i < frameNum; i++)
            {
                if(i%10 == 0)
                {
                    double percent = i / frameNum;
                    string query = "UPDATE Videos SET PercentProcess=@Percent" +" WHERE VideoId=@Id";
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

                    List<Tuple<Rectangle, int>> faces = new List<Tuple<Rectangle, int>>();
                    Tuple<Rectangle, int> effects;


                    if (frontalArray == null || frontalArray.Length == 0)
                    {

                        Rectangle empty = new Rectangle(0, 0, 1, 1);
                        int effect = 0;
                        effects = new Tuple<Rectangle, int>(empty, effect);
                        faces.Add(effects);


                    }
                    else
                    {


                        foreach (Rectangle rect in frontalArray)
                        {
                            //Default all rectangles as effect 1 which is blur
                            int effect = 1;
                            effects = new Tuple<Rectangle, int>(rect, effect);
                            faces.Add(effects);

                        }


                    }
                    FrameList.Add(faces);
                }

            }


            JavaScriptSerializer Jserializer = new JavaScriptSerializer();
            string queryString = "UPDATE Videos SET EditedVideo=@FrameList, PercentProcess=@PercentProcess" +
            " WHERE VideoId=@Id";
            SqlCommand command = new SqlCommand(queryString, dbConn);
            command.Parameters.AddWithValue("@FrameList", Jserializer.Serialize(FrameList));
            command.Parameters.AddWithValue("@Id", videoID);
            command.Parameters.AddWithValue("@PercentProcess", 100);
            StringBuilder errorMessages = new StringBuilder();
            command.Connection.Open();
            command.ExecuteNonQuery();
            dbConn.Close();
        }
    }
}
