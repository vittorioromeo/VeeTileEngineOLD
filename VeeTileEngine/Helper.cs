#region
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SFML.Graphics;
#endregion

namespace VeeTileEngine
{
    public static class Helper
    {
        #region Direction enum
        public enum Direction
        {
            North = 0,
            East = 2,
            South = 4,
            West = 6,
            Northeast = 1,
            Southeast = 3,
            Northwest = 7,
            Southwest = 5
        } ;
        #endregion

        public static Random Random { get; set; } // Random class instance
        public static string DataPath { get; set; } // Data folder path

        static Helper()
        {
            Random = new Random();
            DataPath = Environment.CurrentDirectory + @"\Data";
        }

        public static Direction GetRandomDirection()
        {
            int x = Random.Next(-1, 2);
            int y = Random.Next(-1, 2);

            List<int> l = new List<int> { x, y };

            return IntToDirection(l);
        }
        public static List<int> DirectionToInt(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new List<int> { 0, -1 };
                case Direction.East:
                    return new List<int> { 1, 0 };
                case Direction.South:
                    return new List<int> { 0, 1 };
                case Direction.West:
                    return new List<int> { -1, 0 };
                case Direction.Northeast:
                    return new List<int> { 1, -1 };
                case Direction.Southeast:
                    return new List<int> { 1, 1 };
                case Direction.Northwest:
                    return new List<int> { -1, -1 };
                case Direction.Southwest:
                    return new List<int> { -1, 1 };
            }

            return null;
        }
        public static Direction IntToDirection(List<int> num)
        {
            if (num[0] == 0 && num[1] == -1) return Direction.North;
            if (num[0] == 1 && num[1] == 0) return Direction.East;
            if (num[0] == 0 && num[1] == 1) return Direction.South;
            if (num[0] == -1 && num[1] == 0) return Direction.West;
            if (num[0] == 1 && num[1] == -1) return Direction.Northeast;
            if (num[0] == 1 && num[1] == 1) return Direction.Southeast;
            if (num[0] == -1 && num[1] == -1) return Direction.Northwest;
            if (num[0] == -1 && num[1] == 1) return Direction.Southwest;

            return Direction.North;
        }
        public static int Distance(int x1, int y1, int x2, int y2)
        {
            return Convert.ToInt32(Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1))));
        } // Calculates distance between two points
        public static Vector2 Vector2Lerp(Vector2 mVector1, Vector2 mVector2, float value)
        {
            return new Vector2(mVector1.X + (mVector2.X - mVector1.X) * value, mVector1.Y + (mVector2.Y - mVector1.Y) * value);
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

    }
}