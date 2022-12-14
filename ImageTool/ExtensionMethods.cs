// A few extension methods, primarily for easier geometric operations on Points.
//
// Copyright(C) 2022 Peter Thoman / PH3 GmbH
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

using System.Numerics;

namespace ExtensionMethods
{
    public static class GeometryExtensions
    {
        public static PointF Center(this RectangleF rectangle)
        {
            return new PointF(rectangle.X + rectangle.Width / 2,
                              rectangle.Y + rectangle.Height / 2);
        }
        public static void CenterOn(this ref RectangleF rectangle, PointF target)
        {
            rectangle.X = target.X - rectangle.Width / 2;
            rectangle.Y = target.Y - rectangle.Height / 2;
        }
        public static void Scale(this ref RectangleF rectangle, float v)
        {
            rectangle.X *= v;
            rectangle.Y *= v;
            rectangle.Width *= v;
            rectangle.Height *= v;
        }

        public static Vector2 ToVec(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }
        public static PointF ToPointF(this Vector2 v)
        {
            return new PointF(v.X, v.Y);
        }
        public static Point Clamp(this Point p, Point min, Point max)
        {
            return new Point(Math.Clamp(p.X, min.X, max.X), Math.Clamp(p.Y, min.Y, max.Y));
        }

        public static int Area(this Rectangle r)
        {
            return r.Width * r.Height;
        }

        public static Rectangle RoundToInt(this RectangleF r)
        {
            int x = (int)Math.Floor(r.X);
            int y = (int)Math.Floor(r.Y);
            int w = (int)Math.Floor(r.Width);
            int h = (int)Math.Floor(r.Height);
            return new Rectangle(x, y, w, h);
        }
    }
}