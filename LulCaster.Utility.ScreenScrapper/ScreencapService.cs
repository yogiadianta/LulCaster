﻿using System;

namespace LulCaster.Utility.ScreenScrapper
{
  public class ScreencapService
  {

    public string SnapScreenshot()
    {
      ScreenCapture sc = new ScreenCapture();
      // capture entire screen, and save it to a file
      Image img = sc.CaptureScreen();
      // display image in a Picture control named imageDisplay
      this.imageDisplay.Image = img;
      // capture this window, and save it
      sc.CaptureWindowToFile(this.Handle, "C:\\temp2.gif", ImageFormat.Gif);
    }
  }
}
