using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS
{
    class VeDb
    {
        private static string coluna0;
        public static string c0
        {
            get { return coluna0; }
            set { coluna0 = value; }
        }

        private static string coluna1;
        public static string c1
        {
            get { return coluna1; }
            set { coluna1 = value; }
        }
        private static string coluna2;

        public static string c2
        {
            get { return coluna2; }
            set { coluna2 = value; }
        }
        private static string coluna3;
        public static string c3
        {
            get { return coluna3; }
            set { coluna3 = value; }
        }
        private static string coluna4;
        public static string c4
        {
            get { return coluna4; }
            set { coluna4 = value; }
        }
        private static string coluna5;
        public static string c5
        {
            get { return coluna5; }
            set { coluna5 = value; }
        }
        private static string coluna6;
        public static string c6
        {
            get { return coluna6; }
            set { coluna6 = value; }
        }
        private static string coluna7;
        public static string c7
        {
            get { return coluna7; }
            set { coluna7 = value; }
        }
        private static string coluna8;
        public static string c8
        {
            get { return coluna8; }
            set { coluna8 = value; }
        }
        private static string coluna9;
        public static string c9
        {
            get { return coluna9; }
            set { coluna9 = value; }
        }
        private static string coluna10;
        public static string c10
        {
            get { return coluna10; }
            set { coluna10 = value; }
        }

        public static void dbnull(int db)
        {
            switch (db)
            {
                case 0:
                    c0 = string.Empty;
                    break;
                case 1:
                    c1 = string.Empty;
                    break;
                case 2:
                    c2 = string.Empty;
                    break;
                case 3:
                    c3 = string.Empty;
                    break;
                case 4:
                    c4 = string.Empty;
                    break;
                case 5:
                    c5 = string.Empty;
                    break;
                case 6:
                    c6 = string.Empty;
                    break;
                case 7:
                    c7 = string.Empty;
                    break;
                case 8:
                    c8 = string.Empty;
                    break;
                case 9:
                    c9 = string.Empty;
                    break;
                case 10:
                    c10 = string.Empty;
                    break;
            }
        }

    }
}
