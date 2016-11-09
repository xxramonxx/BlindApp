using MathNet.Numerics.LinearAlgebra;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
namespace BlindApp
{
    public class BeaconPage : ContentPage
    {
        ListView _list;
        BeaconViewModel ViewModel;

        private SKMatrix _m = SKMatrix.MakeIdentity();

        public BeaconPage()
        {
            BackgroundColor = Color.White;
            Title = "Beacon locator";

            ViewModel = new BeaconViewModel();
            ViewModel.ListChanged += (sender, e) =>
            {
                _list.ItemsSource = ViewModel.VisibleData;
            };

            BindingContext = ViewModel;

            /*
                        var view = new SKCanvasView
                        {
                            BackgroundColor = Color.Black,
                            HeightRequest = App.ScreenHeight / 6,
                            WidthRequest = App.ScreenWidth,
                        };

                        view.PaintSurface += PaintCanvas;

                        var layout = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Spacing = 0,
                            Padding = new Thickness(0, 0, 0, 0),
                            Children = { view, BuildContent() }
                        };*/

            Content = BuildContent();
        }

        private void PaintCanvas(object sender, SKPaintSurfaceEventArgs psea)
        {
            psea.Surface.Canvas.SetMatrix(_m);
            psea.Surface.Canvas.Clear();

            SKCanvas canvas = psea.Surface.Canvas;
       //     canvas.DrawColor(SKColors.White);

            // set up drawing tools
            using (var paint = new SKPaint())
            {
                paint.TextSize = 64.0f;
                paint.IsAntialias = true;
                paint.Color = new SKColor(0x42, 0x81, 0xA4);
                paint.IsStroke = false;

                // draw the text
                canvas.DrawText("Skia", 50.0f, 164.0f, paint);
            }
            /*     using (var paint = new SKPaint())
                    {
                        paint.Color = SKColor.FromHsl(0,0.61f,0.5f);
                        SKSize imgSize = new SKSize(200, 200);
                        SKRect aspectRect = SKRect.Create(psea.Info.Width, psea.Info.Height);
                        SKBitmap _bitmap = new SKBitmap(100,100);
                        psea.Surface.Canvas.DrawBitmap(_bitmap, aspectRect, paint);
                    }*/
            }

        private View BuildContent()
        {
            _list = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(ListItemView)),
                RowHeight = 170,
            };

            _list.SetBinding(ListView.ItemsSourceProperty, "Data");

            return _list;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}