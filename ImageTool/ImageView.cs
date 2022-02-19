using System.Data;
using System.Reflection;
using System.Text.Json;

namespace ImageTool
{
    using ExtensionMethods;
    using System.Diagnostics;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Numerics;

    public partial class ImageView : UserControl
    {
        internal readonly string imageFn = "";
        internal readonly Image image;
        readonly ImageController controller;

        internal int ImageWidth { get { return image.Width; } }
        internal int ImageHeight { get { return image.Height; } }
        internal Image ShownImage { get { return image; } }

        Point? dragStartLoc = null;

        Point? selectStartLoc = null;
        Point? selectCurLoc = null;

        bool isRedrawSelection = false;

        public Rectangle SelectionRect
        {
            get
            {
                if (selectCurLoc == null) return Rectangle.Empty;
                if (selectStartLoc == null) return Rectangle.Empty;
                var startX = Math.Min(selectStartLoc.Value.X, selectCurLoc.Value.X);
                var startY = Math.Min(selectStartLoc.Value.Y, selectCurLoc.Value.Y);
                var w = Math.Abs(selectStartLoc.Value.X - selectCurLoc.Value.X);
                var h = Math.Abs(selectStartLoc.Value.Y - selectCurLoc.Value.Y);
                return new Rectangle(startX, startY, w, h);
            }
        }

        private void SharedInit()
        {
            InitializeComponent();

            panelImg.MouseWheel += PanelImg_MouseWheel;

            // enable double buffering on Panel via reflection because fuck you that's why
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panelImg, new object[] { true });
        }

        public ImageView(ImageController controller)
        {
            this.controller = controller;
            SharedInit();
            image = new Bitmap(controller.TargetW, controller.TargetH);
            controller.SetOutputImage(this);

            labelName.Text = "Output";
            labelName.ForeColor = Color.Red;
        }

        public ImageView(string imageFn, ImageController controller)
        {
            this.imageFn = imageFn;
            this.controller = controller;
            SharedInit();
            image = Image.FromFile(imageFn);
            controller.AddImage(image);
            UpdateLabelText();
        }

        private void UpdateLabelText()
        {
            if (imageFn == "") return;
            labelName.Text = Path.GetFileNameWithoutExtension(imageFn);
            if (controller.BaseImage == this)
            {
                labelName.Text += " (Base)";
            }
        }

        private void PanelImg_MouseWheel(object? sender, MouseEventArgs e)
        {
            controller.Zoom(Math.Sign(e.Delta), e.Location);
        }

        private void panelImg_Paint(object sender, PaintEventArgs e)
        {
            UpdateLabelText();
            if (controller.BaseImage == this)
            {
                ControlPaint.DrawBorder(e.Graphics, panelImg.ClientRectangle, Color.Blue, ButtonBorderStyle.Solid);
            }
            controller.DrawImage(this, e.Graphics, panelImg.Bounds);
            if (selectCurLoc != null)
            {
                controller.DrawActiveSelectionRect(SelectionRect, e.Graphics, isRedrawSelection);
            }
        }

        private void panelImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStartLoc != null)
            {
                controller.Drag(e.Location.ToVec() - ((Point)dragStartLoc).ToVec());
                dragStartLoc = e.Location;
            }
            if (selectStartLoc != null)
            {
                selectCurLoc = controller.MouseToImageCoords(e.Location);
            }
            controller.UpdateMousePos(e.Location);
            panelImg.Refresh();
        }

        private void panelImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragStartLoc = e.Location;
            }
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                selectStartLoc = controller.MouseToImageCoords(e.Location);
                selectCurLoc = selectStartLoc;

                isRedrawSelection = e.Button == MouseButtons.Middle;
            }
        }

        private void panelImg_MouseLeave(object sender, EventArgs e)
        {
            dragStartLoc = null;
            selectStartLoc = null;
            selectCurLoc = null;
        }

        private void panelImg_DoubleClick(object sender, EventArgs e)
        {
            controller.BaseImage = this;
        }

        private void panelImg_MouseUp(object sender, MouseEventArgs e)
        {
            dragStartLoc = null;

            if (!isRedrawSelection && e.Button == MouseButtons.Right && selectCurLoc != null && SelectionRect.Area() > 4)
            {
                controller.AddSelectionRect(SelectionRect, this);
                selectStartLoc = null;
                selectCurLoc = null;
            }
            else if (isRedrawSelection && e.Button == MouseButtons.Middle && selectCurLoc != null && SelectionRect.Area() > 4)
            {
                controller.AddRedrawRect(SelectionRect);
                selectStartLoc = null;
                selectCurLoc = null;
            }
        }

        private void panelImg_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SelectionRect.Area() <= 2)
            {
                controller.DeleteSelectionRectAt(controller.MouseToImageCoords(e.Location), this);
                selectStartLoc = null;
                selectCurLoc = null;
            }

            if (e.Button == MouseButtons.Middle && SelectionRect.Area() <= 2)
            {
                controller.DeleteRedrawRectAt(controller.MouseToImageCoords(e.Location));
                selectStartLoc = null;
                selectCurLoc = null;
            }
        }
    }

    struct SelectionRect
    {
        readonly ImageView source;
        public ImageView Source { get { return source; } }

        readonly Rectangle rect;
        public Rectangle Rect { get { return rect; } }

        public SelectionRect(ImageView source, Rectangle rect)
        {
            this.source = source;
            this.rect = rect;
        }
    }

    struct SelectionRectSpec
    {
        public Rectangle Rect { get; set; }
        public string SourceFn { get; set; }

        public SelectionRectSpec(string sourceFn, Rectangle rect) { SourceFn = sourceFn; Rect = rect; }
    }

    struct OutputSpec
    {
        public string BaseImageFn { get; set; }
        public List<SelectionRectSpec> SelectionRects { get; set; }
        public List<Rectangle> RedrawRects { get; set; }
        public OutputSpec(ImageView? baseImageView, List<SelectionRect> selectionRects, List<Rectangle> redrawRects)
        {
            BaseImageFn = baseImageView != null ? baseImageView.imageFn : "";
            SelectionRects = selectionRects.ConvertAll(x => new SelectionRectSpec(x.Source.imageFn, x.Rect));
            RedrawRects = redrawRects;
        }
    }

    public class ImageController
    {
        readonly Font font = new Font("Calibri", 10, FontStyle.Bold, GraphicsUnit.Point);

        private readonly MainForm mainForm;

        List<SelectionRect> selectionRects = new List<SelectionRect>();

        ImageView? baseImage;
        public ImageView? BaseImage
        {
            get { return baseImage; }
            set
            {
                baseImage = value;
                mainForm.Refresh();
                RedrawOutputImage();
            }
        }
        Image? outputImage;
        public Image OutputImage
        {
            get
            {
                Debug.Assert(outputImage != null);
                return outputImage;
            }
        }

        ImageView? outputImageView = null;
        public ImageView OutputImageView
        {
            get
            {
                Debug.Assert(outputImageView != null);
                return outputImageView;
            }
        }

        private Point lastMouseImageCoords = new Point(0, 0);
        public Point LastMouseImageCoords { get { return lastMouseImageCoords; } }

        PointF drawCenter = PointF.Empty;
        Vector2 offset = Vector2.Zero;
        int largestNativeWidth = 0, largestNativeHeight = 0;

        private int zoom = 0;
        private readonly float ZOOM_STEP = 0.25f;
        private readonly float MIN_ZOOM = 0.25f;
        private float ZoomFactor { get { return 1.0f + zoom * ZOOM_STEP; } }

        public ImageController(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public int TargetW { get { return largestNativeWidth; } }
        public int TargetH { get { return largestNativeHeight; } }

        public void AddImage(Image img)
        {
            largestNativeWidth = Math.Max(largestNativeWidth, img.Width);
            largestNativeHeight = Math.Max(largestNativeHeight, img.Height);
        }

        internal void Drag(Vector2 offset)
        {
            this.offset += offset / ZoomFactor;
            mainForm.Refresh();
        }

        private bool showSelectionsOnAll = true;
        public bool ShowSelectionsOnAll
        {
            get { return showSelectionsOnAll; }
            set
            {
                showSelectionsOnAll = value;
                mainForm.Refresh();
            }
        }

        private bool repeatTexture = false;
        public bool RepeatTexture { 
            get { return RepeatTexture; }
            set
            {
                repeatTexture = value;
                mainForm?.Refresh();
            }
        }

        List<ImageView> imageViews = new List<ImageView>();
        internal void AddView(ImageView view)
        {
            imageViews.Add(view);
        }

        internal void DrawImage(ImageView imView, Graphics graphics, RectangleF bounds)
        {
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            DrawBG(graphics);

            RectangleF sourceRect = new RectangleF(0, 0, imView.ImageWidth, imView.ImageHeight);
            float wz = TargetW * ZoomFactor;
            float hz = TargetH * ZoomFactor;
            RectangleF targetRect = new RectangleF(0, 0, wz, hz);
            drawCenter = bounds.Center();

            targetRect.CenterOn((drawCenter.ToVector2() + offset * ZoomFactor).ToPointF());

            graphics.DrawImage(imView.ShownImage, targetRect, sourceRect, GraphicsUnit.Pixel);

            if(repeatTexture)
            {
                var offsets = new PointF[] { new PointF(0, hz), new PointF(0, -hz), new PointF(wz, 0), new PointF(-wz, 0)};
                foreach(var offset in offsets)
                {
                    RectangleF currentTargetRect = targetRect;
                    currentTargetRect.Offset(offset);
                    graphics.DrawImage(imView.ShownImage, currentTargetRect, sourceRect, GraphicsUnit.Pixel);
                }
            }

            var srsToDraw = selectionRects;
            if (!ShowSelectionsOnAll) srsToDraw = selectionRects.Where(s => s.Source == imView).ToList();
            srsToDraw.ForEach(s => DrawSelectionRect(s, graphics, imView));

            if (imView != outputImageView) redrawRects.ForEach(r => DrawRedrawRect(r, graphics));
        }


        internal Point MouseToImageCoords(Point mousePoint)
        {
            Vector2 ret = mousePoint.ToVec();
            ret -= drawCenter.ToVector2();
            ret /= ZoomFactor;
            ret += new Vector2(TargetW / 2, TargetH / 2);
            ret -= offset;
            ret.X = Math.Clamp(ret.X, 0, TargetW - 1);
            ret.Y = Math.Clamp(ret.Y, 0, TargetH - 1);
            return ret.ToPoint();
        }

        internal Point ImageToMouseCoords(Point imagePoint)
        {
            Vector2 ret = imagePoint.ToVec();
            ret += offset;
            ret -= new Vector2(TargetW / 2, TargetH / 2);
            ret *= ZoomFactor;
            ret += drawCenter.ToVector2();
            return ret.ToPoint();
        }

        internal Rectangle ImageToMouse(Rectangle selectionRect)
        {
            selectionRect.Location = ImageToMouseCoords(selectionRect.Location);
            selectionRect.Width = (int)Math.Round(selectionRect.Width * ZoomFactor);
            selectionRect.Height = (int)Math.Round(selectionRect.Height * ZoomFactor);
            return selectionRect;
        }

        internal void Zoom(int delta, Point location)
        {
            // adjust offset to keep image centered on mouse pointer
            var pointerOffset = location.ToVec() - drawCenter.ToVector2();
            var beforePointerOffsetInImagePixels = pointerOffset / ZoomFactor;

            // zoom roughly equally fast independent of zoom level (except for very small)
            zoom += delta * Math.Clamp(zoom / 4, 1, 8);
            while (ZoomFactor < MIN_ZOOM) zoom++;

            var afterPointerOffsetInImagePixels = pointerOffset / ZoomFactor;
            offset += afterPointerOffsetInImagePixels - beforePointerOffsetInImagePixels;

            mainForm.Refresh();
        }

        internal void UpdateMousePos(Point location)
        {
            lastMouseImageCoords = MouseToImageCoords(location).Clamp(new Point(0, 0), new Point(TargetW - 1, TargetH - 1));
            mainForm.RefreshStatus();
        }

        public string GetStatusString()
        {
            return String.Format(" Zoom: {0,4:f0}% - X:{1,4} Y:{2,4}  ", ZoomFactor*100, lastMouseImageCoords.X, lastMouseImageCoords.Y);
        }

        internal void SetOutputImage(ImageView imView)
        {
            outputImage = imView.ShownImage;
            outputImageView = imView;
            mainForm.Refresh();
        }

        internal void RedrawOutputImage()
        {
            if (outputImage == null) return;
            using (Graphics g = Graphics.FromImage(outputImage))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);
                if (baseImage != null) g.DrawImage(baseImage.image, 0, 0, TargetW, TargetH);
                foreach (var sel in selectionRects)
                {
                    var img = sel.Source.ShownImage;
                    RectangleF dstRec = sel.Rect;
                    RectangleF srcRec = dstRec;
                    float scale = img.Width / (float)TargetW;
                    srcRec.Scale(scale);
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.DrawImage(img, dstRec, srcRec, GraphicsUnit.Pixel);
                    g.CompositingMode = CompositingMode.SourceOver;
                }
                foreach (var r in redrawRects)
                {
                    DrawRedrawRect(r, g, true);
                }
            }
            mainForm.RefreshOutput();
        }

        internal void AddSelectionRect(Rectangle rect, ImageView imView)
        {
            selectionRects.Add(new SelectionRect(imView, rect));
            RedrawOutputImage();
            mainForm.Refresh();
        }

        internal void DeleteSelectionRectAt(Point location, ImageView imView)
        {
            selectionRects.RemoveAll(sr => sr.Source == imView && sr.Rect.Contains(location));
            RedrawOutputImage();
            mainForm.Refresh();
        }

        internal void DrawSelectionRect(SelectionRect selectionRect, Graphics g, ImageView imView)
        {
            if (imView == outputImageView) return;

            if (selectionRect.Source == imView)
            {
                DrawActiveSelectionRect(selectionRect.Rect, g);
            }
            else
            {
                var rect = ImageToMouse(selectionRect.Rect);
                Brush brush = new HatchBrush(HatchStyle.WideDownwardDiagonal, Color.FromArgb(125, Color.White), Color.FromArgb(125, Color.Black));
                g.FillRectangle(brush, rect);
            }
        }
        internal void DrawActiveSelectionRect(Rectangle rect, Graphics g, bool isRedrawRect = false)
        {
            rect = ImageToMouse(rect);
            rect.Inflate(1, 1);
            g.DrawRectangle(isRedrawRect ? Pens.Red : Pens.White, rect);
            rect.Inflate(1, 1);
            g.DrawRectangle(Pens.Black, rect);
        }

        private void DrawRedrawRect(Rectangle rect, Graphics g, bool forOutput = false)
        {
            if (!forOutput) rect = ImageToMouse(rect);
            Brush brush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.FromArgb(125, Color.Red), Color.Transparent);
            g.FillRectangle(brush, rect);

            if (forOutput)
            {
                const string text = "Redraw";
                var size = TextRenderer.MeasureText(text, font);
                var pos = ((RectangleF)rect).Center();
                pos.X -= size.Width / 2;
                pos.Y -= size.Height / 2;
                g.FillRectangle(Brushes.White, pos.X, pos.Y, size.Width, size.Height);
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(text, font, Brushes.Black, pos);
            }
        }

        internal void DrawBG(Graphics g)
        {
            Brush brush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.LightGray, Color.DarkGray);
            switch (currentBG)
            {
                case BG.Black: brush = Brushes.Black; break;
                case BG.White: brush = Brushes.White; break;
                case BG.Checker: break;
                case BG.Pink: brush = Brushes.DeepPink; break;
            }
            g.FillRectangle(brush, g.ClipBounds);
        }

        enum BG
        {
            Black,
            White,
            Checker,
            Pink
        };
        BG currentBG = BG.Checker;
        internal void ChangeBG()
        {
            switch (currentBG)
            {
                case BG.Black: currentBG = BG.White; break;
                case BG.White: currentBG = BG.Checker; break;
                case BG.Checker: currentBG = BG.Pink; break;
                case BG.Pink: currentBG = BG.Black; break;
            }
        }

        List<Rectangle> redrawRects = new List<Rectangle>();
        internal void AddRedrawRect(Rectangle selectionRect)
        {
            redrawRects.Add(selectionRect);
            RedrawOutputImage();
            mainForm.Refresh();
        }
        internal void DeleteRedrawRectAt(Point location)
        {
            redrawRects.RemoveAll(r => r.Contains(location));
            RedrawOutputImage();
            mainForm.Refresh();
        }

        string getOutputSpecFn(string folder)
        {
            return folder + "/output_spec.json";
        }

        internal void SaveOutput(string folder)
        {
            OutputImage.Save(folder + "/output.png");

            OutputSpec outputSpec = new OutputSpec(BaseImage, selectionRects, redrawRects);
            string jsonString = JsonSerializer.Serialize(outputSpec);
            File.WriteAllText(getOutputSpecFn(folder), jsonString);
        }

        internal ImageView? FindImageViewByFn(string fn)
        {
            return imageViews.Find(v => v.imageFn == fn);
        }

        internal void LoadOutput(string folder)
        {
            var specFn = getOutputSpecFn(folder);
            if (File.Exists(specFn))
            {
                selectionRects.Clear();
                redrawRects.Clear();

                var jsonString = File.ReadAllText(getOutputSpecFn(folder));
                var outputSpec = JsonSerializer.Deserialize<OutputSpec>(jsonString);
                if (outputSpec.BaseImageFn.Length > 0)
                {
                    var view = FindImageViewByFn(outputSpec.BaseImageFn);
                    if (view != null)
                    {
                        BaseImage = view;
                    }
                }
                outputSpec.SelectionRects.ForEach(spec =>
                {
                    var view = FindImageViewByFn(spec.SourceFn);
                    if (view != null)
                    {
                        AddSelectionRect(spec.Rect, view);
                    }
                });
                redrawRects = outputSpec.RedrawRects;
                RedrawOutputImage();
                mainForm.Refresh();
            }
        }
    }
}
