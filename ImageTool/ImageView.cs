using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTool
{
    using ExtensionMethods;
    using System.Drawing.Drawing2D;
    using System.Numerics;

    public partial class ImageView : UserControl
    {
        string imageFn = "";
        Image image;
        ImageController controller;

        Point dragStartLoc = Point.Empty;

        Point selectStartLoc = Point.Empty;
        Point selectCurLoc = Point.Empty;

        public Rectangle SelectionRect
        {
            get
            {
                var startX = Math.Min(selectStartLoc.X, selectCurLoc.X);
                var startY = Math.Min(selectStartLoc.Y, selectCurLoc.Y);
                var w = Math.Abs(selectStartLoc.X - selectCurLoc.X);
                var h = Math.Abs(selectStartLoc.Y - selectCurLoc.Y);
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
            controller.SetOutputImage((Bitmap)image);

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
            if (controller.BaseImage == image)
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
            if (controller.BaseImage == image)
            {
                ControlPaint.DrawBorder(e.Graphics, panelImg.ClientRectangle, Color.Blue, ButtonBorderStyle.Solid);
            }
            controller.DrawImage(image, e.Graphics);
            if (selectCurLoc != Point.Empty)
            {
                controller.DrawSelectionRect(SelectionRect, e.Graphics);
            }
        }

        private void panelImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStartLoc != Point.Empty)
            {
                controller.Drag(e.Location.ToVec() - dragStartLoc.ToVec());
                dragStartLoc = e.Location;
            }
            if (selectStartLoc != Point.Empty)
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
            if (e.Button == MouseButtons.Right)
            {
                selectStartLoc = controller.MouseToImageCoords(e.Location);
            }
        }

        private void panelImg_MouseLeave(object sender, EventArgs e)
        {
            dragStartLoc = Point.Empty;
            selectStartLoc = Point.Empty;
            selectCurLoc = Point.Empty;
        }

        private void panelImg_DoubleClick(object sender, EventArgs e)
        {
            controller.BaseImage = image;
        }

        private void panelImg_MouseUp(object sender, MouseEventArgs e)
        {
            dragStartLoc = Point.Empty;


            if(e.Button == MouseButtons.Right && selectCurLoc != Point.Empty)
            {
                controller.AddSelectionRect(SelectionRect, image);
                selectStartLoc = Point.Empty;
                selectCurLoc = Point.Empty;
            }
        }
    }

    struct SelectionRect
    {
        Image source;
        public Image Source { get { return source; } }

        Rectangle rect;
        public Rectangle Rect { get { return rect; } }

        public SelectionRect(Image source, Rectangle rect)
        {
            this.source = source;
            this.rect = rect;
        }
    }

    public class ImageController
    {
        private MainForm mainForm;

        List<SelectionRect> selectionRects = new List<SelectionRect>();

        Image? baseImage;
        public Image? BaseImage
        {
            get { return baseImage; }
            set
            {
                baseImage = value;
                mainForm.Refresh();
                RedrawOutputImage();
            }
        }
        Bitmap? outputImage;

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

        internal void DrawImage(Image image, Graphics graphics)
        {
            DrawBG(graphics);

            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            RectangleF sourceRect = new RectangleF(0, 0, image.Width, image.Height);
            RectangleF targetRect = new RectangleF(0, 0, TargetW * ZoomFactor, TargetH * ZoomFactor);
            drawCenter = graphics.ClipBounds.Center();

            targetRect.CenterOn((drawCenter.ToVector2() + offset * ZoomFactor).ToPointF());

            graphics.DrawImage(image, targetRect, sourceRect, GraphicsUnit.Pixel);

            selectionRects.Where(s => s.Source == image).ToList().ForEach(r => DrawSelectionRect(r.Rect, graphics));
        }

        internal Point MouseToImageCoords(Point mousePoint)
        {
            Vector2 ret = mousePoint.ToVec();
            ret -= drawCenter.ToVector2();
            ret /= ZoomFactor;
            ret += new Vector2(TargetW / 2, TargetH / 2);
            ret -= offset;
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

        public string GetMousePosString()
        {
            return String.Format("X:{0,4} Y:{1,4}", lastMouseImageCoords.X, lastMouseImageCoords.Y);
        }

        internal void SetOutputImage(Bitmap image)
        {
            outputImage = image;
            mainForm.Refresh();
        }

        internal void RedrawOutputImage()
        {
            if (outputImage == null) return;
            using (Graphics g = Graphics.FromImage(outputImage))
            {
                DrawBG(g);
                if (baseImage != null) g.DrawImage(baseImage, 0, 0, TargetW, TargetH);
                foreach(var sel in selectionRects)
                {
                    var img = sel.Source;
                    RectangleF dstRec = sel.Rect;
                    RectangleF srcRec = dstRec;
                    float scale = img.Width / (float)TargetW;
                    srcRec.Scale(scale);
                    g.DrawImage(img, dstRec, srcRec, GraphicsUnit.Pixel);
                }
            }
            mainForm.RefreshOutput();
        }

        internal void AddSelectionRect(Rectangle rect, Image image)
        {
            selectionRects.Add(new SelectionRect(image, rect));
            RedrawOutputImage();
        }

        internal void DrawSelectionRect(Rectangle selectionRect, Graphics g)
        {
            var rect = ImageToMouse(selectionRect);
            rect.Inflate(1, 1);
            g.DrawRectangle(Pens.White, rect);
            rect.Inflate(1, 1);
            g.DrawRectangle(Pens.Black, rect);
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
            switch(currentBG)
            {
                case BG.Black: currentBG = BG.White; break;
                case BG.White: currentBG = BG.Checker; break;
                case BG.Checker: currentBG = BG.Pink; break;
                case BG.Pink: currentBG = BG.Black; break;
            }
        }
    }
}
