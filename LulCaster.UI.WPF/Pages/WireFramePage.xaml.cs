﻿using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Workers;
using LulCaster.Utility.ScreenCapture.Windows;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LulCaster.UI.WPF.Pages
{
  /// <summary>
  /// Interaction logic for WireFramePage.xaml
  /// </summary>
  public partial class WireFramePage : Page
  {
    private readonly ScreenCaptureTimer _screenCaptureTimer;
    private const double CAPTURE_TIMER_INTERVAL = 500;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();
    private readonly Dictionary<string, Rectangle> _boundingBoxCollection = new Dictionary<string, Rectangle>(); //TODO: This will live in the segement configuration tool
    private Rectangle _currentBoundingBox; //TODO: This will live in the segement configuration tool
    private IConfigService _configService;

    public WireFramePage(IConfigService configService)
    {
      InitializeComponent();

      //TODO: Add dependacy injection
      _screenCaptureTimer = new ScreenCaptureTimer(new ScreenCaptureService(), CAPTURE_TIMER_INTERVAL);
      _screenCaptureTimer.ScreenCaptureCompleted += _screenCaptureTimer_ScreenCaptureCompleted;
      _screenCaptureTimer.Start();
      _configService = configService;

      cntrlSegmentList.DataContext = _configService.GetAllRegionsAsViewModels();
    }

    private void _screenCaptureTimer_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      this.Dispatcher?.Invoke(() =>
      {
        var imageStream = new MemoryStream(captureArgs.ScreenImageStream);
        var screenCaptureImage = new BitmapImage();
        screenCaptureImage.BeginInit();
        screenCaptureImage.StreamSource = imageStream;
        screenCaptureImage.CacheOption = BitmapCacheOption.OnLoad;
        screenCaptureImage.EndInit();
        screenCaptureImage.Freeze();

        var imageBrush = new ImageBrush(screenCaptureImage);

        canvasScreenFeed.Background = imageBrush;
      }, System.Windows.Threading.DispatcherPriority.Background);
    }

    private void CanvasScreenFeed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released) return;

        canvasScreenFeed.Children.Clear();
        var newBox = _boundingBoxBrush.OnMouseDown(e);
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(newBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, newBox.X);
        Canvas.SetTop(windowsBox, newBox.Y);
        _boundingBoxCollection[""] = windowsBox; //TODO: The name needs to be taken from the segment configuration control
        _currentBoundingBox = windowsBox;
      });
    }

    private void CanvasScreenFeed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Pressed) return;
        //TODO: Update the configuration of the selected bounding box in the segment configuration control
      });
    }

    private void CanvasScreenFeed_MouseMove(object sender, MouseEventArgs e)
    {
      this.Dispatcher?.Invoke(() =>
      {
        if (e.LeftButton == MouseButtonState.Released) return;

        canvasScreenFeed.Children.Clear();

        var newBox = _boundingBoxBrush.OnMouseMove(e);
        var windowsBox = _boundingBoxBrush.ConvertRectToWindowsRect(newBox);
        canvasScreenFeed.Children.Add(windowsBox);
        Canvas.SetLeft(windowsBox, newBox.X);
        Canvas.SetTop(windowsBox, newBox.Y);
        _boundingBoxCollection[""] = windowsBox;
        _currentBoundingBox = windowsBox;
      });
    }
  }
}