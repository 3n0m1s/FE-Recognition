//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace FE_recognition
{
   public partial class CameraCapture : Form
   {
      private VideoCapture _capture = null;
        private bool _captureInProgress;
      private Mat _frame;
      private Mat _grayFrame;
      private Mat _smallGrayFrame;
      private Mat _smoothedGrayFrame;
      private Mat _cannyFrame;


        public CameraCapture()
      {
         InitializeComponent();
         CvInvoke.UseOpenCL = false;
         try
         {
            _capture = new VideoCapture();
            _capture.ImageGrabbed += ProcessFrame;



            }
         catch (NullReferenceException excpt)
         {
            MessageBox.Show(excpt.Message);
         }
         _frame = new Mat();
         _grayFrame = new Mat();
         _smallGrayFrame = new Mat();
         _smoothedGrayFrame = new Mat();
         _cannyFrame = new Mat();

      }
        //SINGLE REQUEST
        //private static readonly CascadeClassifier face = new CascadeClassifier("C:\\Emgu\\emgucv-windesktop 3.4.1.2976\\etc\\haarcascades\\haarcascade_frontalface_alt_tree.xml");

        private void ProcessFrame(object sender, EventArgs arg)
      {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                //CvInvoke.CvtColor(_frame, _grayFrame, ColorConversion.Bgr2Gray);

                //CvInvoke.PyrDown(_grayFrame, _smallGrayFrame);

                //CvInvoke.PyrUp(_smallGrayFrame, _smoothedGrayFrame);

                //CvInvoke.Canny(_smoothedGrayFrame, _cannyFrame, 100, 60);


                    //SINGLE REQUEST
                    //Rectangle[] faceRect = face.DetectMultiScale(_grayFrame, 1.4, 0, new Size(100, 100), new Size(800, 800));
                    //List<Rectangle> faces = new List<Rectangle>();

                    //foreach (Rectangle r in faceRect)
                    //    CvInvoke.Rectangle(_frame, r, new Bgr(Color.Green).MCvScalar, 2);

                    //MULTIPLE REQUEST
                    long detectionTime;

                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();

                DetectFace.Detect(
                  _frame, "haarcascade_frontalface_alt2.xml", "haarcascade_eye.xml",
                  faces, eyes,
                  out detectionTime);

                foreach (Rectangle face in faces)
                    CvInvoke.Rectangle(_frame, face, new Bgr(Color.Gold).MCvScalar, 2);
                foreach (Rectangle eye in eyes)
                    CvInvoke.Rectangle(_frame, eye, new Bgr(Color.Green).MCvScalar, 2);

                captureImageBox.Image = _frame;
                //grayscaleImageBox.Image = _grayFrame;
                //smoothedGrayscaleImageBox.Image = _smoothedGrayFrame;
                //cannyImageBox.Image = _cannyFrame;

            }

        }

      private void ReleaseData()
      {
         if (_capture != null)
            _capture.Dispose();
      }

        private void captureButton_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    captureButton.Text = "Start Capture";
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    captureButton.Text = "Stop";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }
    }
}
