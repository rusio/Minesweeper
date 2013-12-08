using System;
using NUnit.Framework;
using Minesweeper.Spiel;

namespace Test
{
    [TestFixture]
    public class MinenfeldTest
    {
        [Test]
        public void InitialStateOfMinimalFieldWithoutMines()
        {
            Minenfeld minenfeld = new Minenfeld(1, 1, 0);
            Assert.AreEqual(Minenfeld.LAUFEND, minenfeld.SpielZustand);
            Feld feld = minenfeld.GibFeld(0, 0);
            Assert.IsFalse(feld.HatEigeneMine);
            Assert.IsFalse(feld.IstMarkiert);
        }

        [Test]
        public void InitialStateOfMinimalFieldWithOneMine()
        {
            Minenfeld minenfeld = new Minenfeld(1, 1, 1);
            Assert.AreEqual(Minenfeld.LAUFEND, minenfeld.SpielZustand);
            Feld feld = minenfeld.GibFeld(0, 0);
            Assert.IsTrue(feld.HatEigeneMine);
            Assert.IsFalse(feld.IstMarkiert);
        }

        [Test]
        public void InitialStateOfSmallFieldWithoutMines()
        {
            Minenfeld minenfeld = new Minenfeld(2, 2, 0);
            for (int row = 0; row < minenfeld.Hoehe; row++)
            {
                for (int col = 0; col < minenfeld.Breite; col++)
                {
                    Feld feld = minenfeld.GibFeld(row, col);
                    Assert.IsFalse(feld.HatEigeneMine);
                    Assert.AreEqual(0, feld.AngrenzendeMinen);
                }
            }
        }

        [Test]
        public void InitialStateOfSmallFieldWithOneMine()
        {
            Minenfeld minenfeld = new Minenfeld(2, 2, 1);
            for (int row = 0; row < minenfeld.Hoehe; row++)
            {
                for (int col = 0; col < minenfeld.Breite; col++)
                {
                    Feld feld = minenfeld.GibFeld(row, col);
                    if (feld.HatEigeneMine)
                    {
                        Assert.AreEqual(0, feld.AngrenzendeMinen);
                    }
                    else
                    {
                        Assert.AreEqual(1, feld.AngrenzendeMinen);
                    }
                }
            }
        }

        [Test]
        public void MarkingOfSmallField()
        {
            Minenfeld minenfeld = new Minenfeld(2, 2, 0);
            minenfeld.MarkiereFeld(0, 0);
            minenfeld.MarkiereFeld(1, 1);
            Assert.IsTrue(minenfeld.GibFeld(0, 0).IstMarkiert);
            Assert.IsTrue(minenfeld.GibFeld(1, 1).IstMarkiert);
            Assert.IsFalse(minenfeld.GibFeld(1, 0).IstMarkiert);
            Assert.IsFalse(minenfeld.GibFeld(0, 1).IstMarkiert);
        }

        [Test]
        public void OpenOfMarkedFieldHasNoEffect()
        {
            Minenfeld minenfeld = new Minenfeld(2, 2, 1);
            minenfeld.MarkiereFeld(0, 0);
            minenfeld.OeffneFeld(0, 0);
            Assert.IsFalse(minenfeld.GibFeld(0, 0).IstOffen);
        }

        [Test]
        public void GameLoseWithMinimalField()
        {
            Minenfeld minenfeld = new Minenfeld(1, 1, 1);
            minenfeld.OeffneFeld(0, 0);
            Assert.AreEqual(Minenfeld.VERLOREN, minenfeld.SpielZustand);
        }

        [Test]
        public void GameWinWithSmallField()
        {
            Minenfeld minenfeld = new Minenfeld(2, 2, 1);
            Assert.AreEqual(Minenfeld.LAUFEND, minenfeld.SpielZustand);
            minenfeld.OeffneFeld(0, 1);
            Assert.AreEqual(Minenfeld.LAUFEND, minenfeld.SpielZustand);
            minenfeld.OeffneFeld(1, 0);
            Assert.AreEqual(Minenfeld.LAUFEND, minenfeld.SpielZustand);
            minenfeld.OeffneFeld(1, 1);
            Assert.AreEqual(Minenfeld.GEWONNEN, minenfeld.SpielZustand);
        }
    }
}

