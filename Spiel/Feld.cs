using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Spiel
{
    /// <summary>
    /// Ein Feld innerhalb des Minenfelds.
    /// </summary>
    public class Feld
    {
        /// <summary>
        /// Gibt an, ob sich im Feld eine Mine befindet.
        /// </summary>
        private bool hatEigeneMine;

        /// <summary>
        /// Gibt die Anzahl der benachbarten Minen des Feldes an.
        /// </summary>
        private int angrenzendeMinen;

        /// <summary>
        /// Gibt an, ob dieses Feld vom Benutzer markiert wurde.
        /// </summary>
        private bool istMarkiert;

        /// <summary>
        /// Gibt an, ob dieses Feld vom Benutzer geöffnet wurde.
        /// </summary>
        private bool istOffen;

        /// <summary>
        /// Konstruktor - erzeugt ein neues Feld mit Anfangswerten.
        /// </summary>
        public Feld()
        {
            this.hatEigeneMine = false;
            this.angrenzendeMinen = 0;
            this.istMarkiert = false;
            this.istOffen = false;
        }

        /// <summary>
        /// Gibt an, ob sich im Feld eine Mine befindet.
        /// Setzt den Wert auf true oder false.
        /// </summary>
        public bool HatEigeneMine 
        { 
            get { return hatEigeneMine; }
            set { hatEigeneMine = value; }
        }

        /// <summary>
        /// Gibt die Anzahl der benachbarten Minen des Feldes an.
        /// Setzt den Wert auf eine ganze Zahl.
        /// </summary>
        public int AngrenzendeMinen 
        { 
            get { return angrenzendeMinen; }
            set { angrenzendeMinen = value; }
        }

        /// <summary>
        /// Gibt an, ob dieses Feld vom Benutzer markiert wurde.
        /// </summary>
        public bool IstMarkiert 
        { 
            get { return istMarkiert; } 
        }

        /// <summary>
        /// Ändert den Markierungszustand des Feldes auf "markiert" oder "unmarkiert".
        /// </summary>
        public void WechseleMarkierung()
        {
            istMarkiert = !istMarkiert;
        }

        /// <summary>
        /// Gibt an, ob dieses Feld vom Benutzer geöffnet wurde.
        /// Setzt den Wert auf true oder False.
        /// </summary>
        public bool IstOffen 
        {
            get { return istOffen; }
            set { istOffen = value; }
        }
    }
}
