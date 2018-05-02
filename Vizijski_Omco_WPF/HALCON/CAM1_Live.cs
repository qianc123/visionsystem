using HalconDotNet;

namespace VizijskiSustavWPF.HALCON
{
    public partial class HDevelopExport
    {
   
        private void Livecam1()
        {
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            // Image Acquisition OPEN frame
            HOperatorSet.OpenFramegrabber("GigEVision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, "false", "default", "acA130075gm_CAM", 0, -1, out hv_AcqHandle);
            //HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", 3500.0);
            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);

            while (Exitloop1 == false)
            {
                ho_Image.Dispose();
                // Live image from CAM1
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            }
            // Image Acquisition CLOSE frame
            ho_Image.Dispose();
            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
        }

        public void RunHalcon11(HTuple window)
        {
            hv_ExpDefaultWinHandle = window;
            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
            Livecam1();
        }
    }
}

