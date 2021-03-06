using HalconDotNet;
using VizijskiSustavWPF.PLC_interface;

namespace VizijskiSustavWPF.VisionControl
{
    public partial class HDevelopExport
    {

        private void Livecam4(bool domainmarkup, int counter)
        {
            // Wait for CAM4 thread to be closed
            _waitHandleCam4.WaitOne();
            // Close te thread DOOR
            _waitHandleCam4.Reset();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Elipse);
            HOperatorSet.GenEmptyObj(out ho_ROIUnion);
            HOperatorSet.GenEmptyObj(out ho_ROI);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out HObject ho_GammaImage);
            // Open CAM 4 frame
            OpenCamFrame();
            // Set Exposure
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTimeAbs", 1000.0);
            //
            while (Exitloop4 == false)
            {
                ho_Image.Dispose();
                // Live image from CAM4
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                hv_HalfW = hv_Width / 2;
                hv_HalfH = hv_Height / 2;
                // Gamma Encoding
                //HOperatorSet.GammaImage(ho_Image, out ho_GammaImage, 0.416667, 0.055, 0.0031308,255, "true");
                //HOperatorSet.DispObj(ho_GammaImage, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);

                if (domainmarkup)
                {
                    // Display area color
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "spring green");
                    if (counter == 3 || counter == 4 )
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, 0, hv_HalfW - 250, hv_Height, hv_HalfW + 90);
                        ho_Elipse.Dispose();
                        HOperatorSet.GenEllipse(out ho_Elipse, hv_HalfH, hv_HalfW + 70, (new HTuple(90)).TupleRad(), 1150, 130);
                        ho_ROIUnion.Dispose();
                        HOperatorSet.Union2(ho_Rectangle, ho_Elipse, out ho_ROIUnion);
                        //* ROI
                        ho_ROI.Dispose();
                        HOperatorSet.Difference(ho_ROIUnion, ho_Elipse, out ho_ROI);
                        //Display for user
                        ho_Contours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_ROI, out ho_Contours, "border");
                        HOperatorSet.DispObj(ho_Contours, hv_ExpDefaultWinHandle);
                    }
                    else if (counter == 5 || counter == 6)
                    {
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, 0, hv_HalfW - 90, hv_Height, hv_HalfW + 250);
                        ho_Elipse.Dispose();
                        HOperatorSet.GenEllipse(out ho_Elipse, hv_HalfH, hv_HalfW - 70, (new HTuple(90)).TupleRad(), 1150, 130);
                        ho_ROIUnion.Dispose();
                        HOperatorSet.Union2(ho_Rectangle, ho_Elipse, out ho_ROIUnion);
                        ho_ROI.Dispose();
                        HOperatorSet.Difference(ho_ROIUnion, ho_Elipse, out ho_ROI);
                        ho_Contours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_ROI, out ho_Contours, "border");
                        HOperatorSet.DispObj(ho_Contours, hv_ExpDefaultWinHandle);
                    }
                    else
                    {
                        // Old ROI
                        HOperatorSet.DispLine(hv_ExpDefaultWinHandle, 0, hv_HalfW - 250, hv_Height, hv_HalfW - 250);
                        HOperatorSet.DispLine(hv_ExpDefaultWinHandle, 0, hv_HalfW + 250, hv_Height, hv_HalfW + 250); 
                    }
                }
                
            }
            // Image Acquisition CLOSE frame
            ho_Image.Dispose();
            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
            //CloseCamFrame();
            //HOperatorSet.CloseFramegrabber(hv_AcqHandle);
            // Open the thread DOOR
            _waitHandleCam4.Set();
        }

        public void RunCam4(HTuple window, bool domainmarkup, int counter)
        {
            hv_ExpDefaultWinHandle = window;
            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
            Livecam4(domainmarkup, counter);
        }

    }
}

