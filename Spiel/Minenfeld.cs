using System;

namespace Minesweeper.Spiel
{
    /// <summary>
    /// Das Minenfeld enthält alle Felder und versteckt Minen unter manche Felder.
    /// Alle Berechnungen und sind in dieser Klasse enthalten.
    /// </summary>
    public class Minenfeld
    {
        // Zeigen die Eigenschaften des Minenfelds an.
        private int hoehe;          // Anzahl Zeilen
        private int breite;         // Anzahl Spalten
        private int minen;          // Anzahl Minen
        private Feld[,] matrix;     // 2-D Array mit den Feldern

        // Zustand des aktuellen Spiels.
        private int spielZustand;
        public const int LAUFEND = 1;
        public const int GEWONNEN = 2;
        public const int VERLOREN = 3;

        /// <summary>
        /// Konstruktor - erzeugt ein neues Minenfeld mit Breite, Höhe und Anzahl der Minen.
        /// Verteilt die Minen zufällig auf die Felder in der Matrix.
        /// </summary>
        public Minenfeld(int hoehe, int breite, int minen)
        {
            this.breite = breite;
            this.hoehe = hoehe;
            this.minen = minen;
            
            ErzeugeMatrix();
            VersteckeMinen();
            BerechneNachbarn();
            
            this.spielZustand = LAUFEND;
        }

        /// <summary>
        /// Erzeugt die Matrix und füllt sie mit Felder.
        /// </summary>
        private void ErzeugeMatrix()
        {
            matrix = new Feld[hoehe, breite];
            for (int zeile = 0; zeile < hoehe; zeile++)
            {
                for (int spalte = 0; spalte < breite; spalte++)
                {
                    matrix[zeile, spalte] = new Feld();
                }
            }
        }

        /// <summary>
        /// Berechnet zufällige Positionen der Minen und setzt diese auf die Felder.
        /// </summary>
        private void VersteckeMinen()
        {
            Random random = new Random();
            int versteckteMinen = 0;
            while (versteckteMinen < minen)
            {
                int zufaelligeZeile = random.Next(hoehe);
                int zufaelligeSpalte = random.Next(breite);
                Feld feld = matrix[zufaelligeZeile, zufaelligeSpalte];
                if (feld.HatEigeneMine == false)
                {
                    feld.HatEigeneMine = true;
                    versteckteMinen++;
                }
            }
        }

        /// <summary>
        /// Berechnet für jedes Feld die Anzahl der benachbarten Minen und 
        /// setzt diese auf das Feld.
        /// </summary>
        private void BerechneNachbarn()
        {
            for (int zeile = 0; zeile < hoehe; zeile++)
            {
                for (int spalte = 0; spalte < breite; spalte++)
                {
                    Feld feld = matrix[zeile, spalte];
                    int angrenzendeMinen = LeseNachbarn(zeile, spalte);
                    feld.AngrenzendeMinen = angrenzendeMinen;
                }
            }
        }

        /// <summary>
        /// Gibt das Feld an der Position (Zeile, Spalte) zurück.
        /// </summary>
        public Feld GibFeld(int zeile, int spalte)
        {
            return matrix[zeile, spalte];
        }

        /// <summary>
        /// Liest die 8 Nachbarn des Feldes aus und addiert alle Minen.
        /// </summary>
        private int LeseNachbarn(int zeile, int spalte)
        {
            int anzahl = 0;
            anzahl += LeseFeld(zeile, spalte - 1); // links
            anzahl += LeseFeld(zeile, spalte + 1); // rechts
            anzahl += LeseFeld(zeile - 1, spalte); // oben
            anzahl += LeseFeld(zeile + 1, spalte); // unten
            anzahl += LeseFeld(zeile - 1, spalte - 1); // oben links
            anzahl += LeseFeld(zeile - 1, spalte + 1); // oben rechts
            anzahl += LeseFeld(zeile + 1, spalte - 1); // unten links
            anzahl += LeseFeld(zeile + 1, spalte + 1); // unten rechts
            return anzahl;
        }

        /// <summary>
        /// Gibt eine 1 zurück, falls das Feld eine Mine enhtält, sonst 0.
        /// </summary>
        private int LeseFeld(int zeile, int spalte)
        {
            if (zeile < 0 || zeile >= breite) return 0;
            if (spalte < 0 || spalte >= hoehe) return 0;

            Feld feld = matrix[zeile, spalte];
            if (feld.HatEigeneMine)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Markiert das Feld als Minenträger.
        /// </summary>
        public void MarkiereFeld(int zeile, int spalte)
        {
            Feld feld = matrix[zeile, spalte];
            feld.WechseleMarkierung();
        }

        /// <summary>
        /// Öffnet das Feld an der gegebenen Position und bestimmt den Spielzustand der
        /// sich daraus ergibt. Falls das Feld markiert ist, hat diese Methode keine Wirkung.
        /// </summary>
        public void OeffneFeld(int zeile, int spalte)
        {
            Feld feld = matrix[zeile, spalte];
            
            if (feld.IstMarkiert)
            {
                return;
            }

            feld.IstOffen = true;
            BestimmeSpielZustand(feld);
        }

        /// <summary>
        /// Bestimmt den aktuellen Spielzustand nach dem Öffnen eines Felds und merkt 
        /// sich diesen in der Variable spielZustand. Wird von der GUI ausgewertet.
        /// </summary>
        private void BestimmeSpielZustand(Feld feld)
        {
            if (feld.HatEigeneMine)
            {
                spielZustand = VERLOREN;
            }
            else
            {
                int offeneFelder = ZaehleOffeneFelder();
                if (offeneFelder < (breite * hoehe - minen))
                {
                    spielZustand = LAUFEND;
                }
                else
                {
                    spielZustand = GEWONNEN;
                }
            }
        }

        /// <summary>
        /// Zählt die bereits geöffneten Felder in dem Minenfeld.
        /// Wird benutzt, um zu bestimmen ob der Spieler gewonnen hat.
        /// </summary>
        private int ZaehleOffeneFelder()
        {
            int offeneFelder = 0;
            for (int zeile = 0; zeile < hoehe; zeile++)
            {
                for (int spalte = 0; spalte < breite; spalte++)
                {
                    Feld feld = matrix[zeile, spalte];
                    if (feld.IstOffen)
                    {
                        offeneFelder++;
                    }
                }
            }
            return offeneFelder;
        }

        public int Hoehe 
        { 
            get { return hoehe; } 
        }

        public int Breite 
        { 
            get { return breite; } 
        }

        public int SpielZustand 
        {
            get { return spielZustand; }
        }
    }
}

