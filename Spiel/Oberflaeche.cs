using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper.Spiel
{
    /// <summary>
    /// Die grafische Oberfläche für das Spiel.
    /// </summary>
    public partial class Oberflaeche : Form
    {
        /// <summary>
        /// Dies ist das aktuelle Minenfeld im aktuellen Spiel.
        /// </summary>
        private Minenfeld minenfeld;

        /// <summary>
        /// Konstruktor - initialisiert die grafischen Elemente und das Minenfeld.
        /// </summary>
        public Oberflaeche()
        {
            // Methode für die Initialisierung der GUI.
            InitializeComponent();

            ErzeugeNeuesMinenfeld();
            PositioniereButtons();
            AktualisiereDarstellung();
        }

        /// <summary>
        /// Erzeugt das Minenfeld mit der Größe, die im tableLayoutPanel definiert ist und 20 Minen.
        /// </summary>
        private void ErzeugeNeuesMinenfeld()
        {
            minenfeld = new Minenfeld(tableLayoutPanel.RowCount,
                                      tableLayoutPanel.ColumnCount,
                                      20);
        }

        /// <summary>
        /// Positioniert einen Button für jedes Feld aus dem Minenfeld.
        /// </summary>
        private void PositioniereButtons()
        {
            for (int zeile = 0; zeile < minenfeld.Hoehe; zeile++)
            {
                for (int spalte = 0; spalte < minenfeld.Breite; spalte++)
                {
                    Button button = new Button();
                    button.MouseDown +=new MouseEventHandler(button_MouseDown);
                    button.Dock = DockStyle.Fill;
                    button.Tag = new Point(zeile, spalte);
                    tableLayoutPanel.Controls.Add(button);
                }
            }
        }

        /// <summary>
        /// Reagiert auf den Klick auf einen Button.
        /// </summary>
        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            Point point = (Point)button.Tag;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    minenfeld.OeffneFeld(point.X, point.Y);
                    break;
                case MouseButtons.Right:
                    minenfeld.MarkiereFeld(point.X, point.Y);
                    break;
                default:
                    break;
            }

            AktualisiereDarstellung();

            switch (minenfeld.SpielZustand)
            {
                case Minenfeld.GEWONNEN:
                    MessageBox.Show("Sie haben gewonnen!\n\nUm ein neues Spiel zu starten, klicken Sie auf OK.");
                    ErzeugeNeuesMinenfeld();
                    AktualisiereDarstellung();
                    break;
                case Minenfeld.VERLOREN:
                    ZeigeAlleMinen();
                    MessageBox.Show("Sie haben verloren!\n\nUm ein neues Spiel zu starten, klicken Sie auf OK.");
                    ErzeugeNeuesMinenfeld();
                    AktualisiereDarstellung();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Zeigt alle Minen an nach dem der Spieler auf eine Mine klickt.
        /// </summary>
        private void ZeigeAlleMinen()
        {
            for (int zeile = 0; zeile < minenfeld.Hoehe; zeile++)
            {
                for (int spalte = 0; spalte < minenfeld.Breite; spalte++)
                {
                    Button button = (Button)tableLayoutPanel.GetControlFromPosition(spalte, zeile);
                    Point point = (Point)button.Tag;
                    Feld feld = minenfeld.GibFeld(point.X, point.Y);
                    if (feld.HatEigeneMine)
                    {
                        button.BackColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// Aktualisiert die grafische Anzeige abhängig von den Feldern im gesamten Minenfeld.
        /// Setzt Hintergrundfarbe, Text und Zustand aller Buttons, abhängig von dem 
        /// Zustand des jeweiligen Felds.
        /// </summary>
        private void AktualisiereDarstellung()
        {
            for (int zeile = 0; zeile < minenfeld.Hoehe; zeile++)
            {
                for (int spalte = 0; spalte < minenfeld.Breite; spalte++)
                {
                    Button button = (Button)tableLayoutPanel.GetControlFromPosition(spalte, zeile);
                    Point point = (Point)button.Tag;
                    Feld feld = minenfeld.GibFeld(point.X, point.Y);
                    if (feld.IstOffen)
                    {
                        button.Enabled = false;
                        if (feld.HatEigeneMine)
                        {
                            button.BackColor = Color.Red;
                        }
                        else
                        {
                            button.Text = feld.AngrenzendeMinen.ToString();
                        }
                    }
                    else
                    {
                        button.Enabled = true;
                        button.Text = "";
                        if (feld.IstMarkiert)
                        {
                            button.BackColor = Color.LightPink;
                        }
                        else
                        {
                            button.BackColor = Color.Beige;
                        }
                    }
                }
            }
            tableLayoutPanel.Select();
        }
    }
}
