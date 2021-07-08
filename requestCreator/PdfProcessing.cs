﻿/*

Copyright 2019 Yamborisov Alexey

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using iText.Kernel.Pdf;

namespace requestCreator
{
    class PdfProcessing
    {
        public static PdfFormat ProcessFile(string path) // Counting formats in PDF
        {
            try
            {
                PdfDocument pdfDocument = new PdfDocument(new PdfReader(path));
                int n = pdfDocument.GetNumberOfPages();
                int Formats = 0, A4 = 0, A3 = 0, A2 = 0, A1 = 0, A0 = 0;
                for (int i = 1; i <= n; i++) // Cycle through pages
                {
                    PdfPage pg = pdfDocument.GetPage(i);
                    float pgHeight;
                    float pgWidth;
                    pgHeight = pg.GetCropBox().GetHeight();
                    pgWidth = pg.GetCropBox().GetWidth();

                    if (pgHeight < pgWidth) // Rotate page dimensions
                    {
                        float t = pgHeight;
                        pgHeight = pgWidth;
                        pgWidth = t;
                    }

                    int size = (int)Math.Round(pgHeight * pgWidth / 842 / 595 + 0.2F, MidpointRounding.AwayFromZero); // A4 = 842*595 pdf points, 0.2 is constant for scan size variety
                    if (size > 1)
                    {
                        Formats += size;
                    }
                    else
                    {
                        Formats += 1;
                    }

                    if ((pgHeight > 2450) || (pgWidth > 1760))
                    {
                        A0 += 1;
                    }
                    else if ((pgHeight > 1760) || (pgWidth > 1250))
                    {
                        A1 += 1;
                    }
                    else if ((pgHeight > 1250) || (pgWidth > 900))
                    {
                        A2 += 1;
                    }
                    else if ((pgHeight > 900) || (pgWidth > 650))
                    {
                        A3 += 1;
                    }
                    else
                    {
                        A4 += 1;
                    }
                }
                PdfFormat result = new PdfFormat(Formats, A4, A3, A2, A1, A0);
                return result;
            }
            catch (Exception e)
            {
                return new PdfFormat(0, 0, 0, 0, 0, 0);
            }
        }

        public static PdfFormat ProcessFolder(string path, bool flagAllDir)
        {
            String[] files;
            PdfFormat result = new PdfFormat(0, 0, 0, 0, 0, 0);
            if (flagAllDir) // Get files in child folders
            {
                files = Directory.GetFiles(path, "*.pdf", SearchOption.AllDirectories);
            }
            else
            {
                files = Directory.GetFiles(path, "*.pdf", SearchOption.TopDirectoryOnly);
            }
            foreach (String file in files)
            {
                result += ProcessFile(file);
            }
            return result;
        }
    }

    class PdfFormat
    {
        public int Formats { get; set; }
        public int A4 { get; set; }
        public int A3 { get; set; }
        public int A2 { get; set; }
        public int A1 { get; set; }
        public int A0 { get; set; }

        public PdfFormat()
        {
            Formats = 0;
            A4 = 0;
            A3 = 0;
            A2 = 0;
            A1 = 0;
            A0 = 0;
        }
        public PdfFormat(int Formats_, int A4_, int A3_, int A2_, int A1_, int A0_)
        {
            Formats = Formats_;
            A4 = A4_;
            A3 = A3_;
            A2 = A2_;
            A1 = A1_;
            A0 = A0_;
        }
        public PdfFormat(PdfFormat src)
        {
            Formats = src.Formats;
            A4 = src.A4;
            A3 = src.A3;
            A2 = src.A2;
            A1 = src.A1;
            A0 = src.A0;
        }

        public static PdfFormat operator+ (PdfFormat p1, PdfFormat p2)
        {

            PdfFormat res = new PdfFormat(p1.Formats + p2.Formats,
                p1.A4 + p2.A4, p1.A3 + p2.A3, p1.A2 + p2.A2, p1.A1 + p2.A1, p1.A0 + p2.A0);
            return res;
        }

        public override string ToString()
        {
            return this.Formats + "(" + this.A4 + ", " + this.A3 + ", " + this.A2 + ", " + this.A1 + ", " + this.A0 + ")";
        }

        public static PdfFormat FromString(string str)
        {
            PdfFormat pdfFormat = new PdfFormat();
            int result = 0;
            if (str.Contains("("))
            {
                int d1 = str.IndexOf("(");
                int d2 = str.IndexOf(",");
                int d3 = str.IndexOf(",", d2 + 1);
                int d4 = str.IndexOf(",", d3 + 1);
                int d5 = str.IndexOf(",", d4 + 1);
                int d6 = str.IndexOf(")", d5 + 1);
                Int32.TryParse(str.Substring(0, d1), out result);
                pdfFormat.Formats = result;
                Int32.TryParse(str.Substring(d1+1, d2 - d1-1), out result);
                pdfFormat.A4 = result;
                Int32.TryParse(str.Substring(d2+1, d3 - d2-1), out result);
                pdfFormat.A3 = result;
                Int32.TryParse(str.Substring(d3+1, d4 - d3-1), out result);
                pdfFormat.A2 = result;
                Int32.TryParse(str.Substring(d4+1, d5 - d4-1), out result);
                pdfFormat.A1 = result;
                Int32.TryParse(str.Substring(d5+1, d6 - d5-1), out result);
                pdfFormat.A0 = result;
            }
            else if (Int32.TryParse(str, out result))
            {
                pdfFormat.Formats = result;
            }
            return pdfFormat;
        }
    }

    public class PdfFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((PdfFormat)value).ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return PdfFormat.FromString((string)value);
        }
    }
}