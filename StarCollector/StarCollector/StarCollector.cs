using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

/// @author  Niklas Nieminen
/// @version 09.12.2020
///
/// todo: https://tim.jyu.fi/answers/kurssit/tie/ohj1/2020s/demot/demo7?answerNumber=11&task=pisteidenaprominen&user=nialniem
/// funktioaliohjelma
/// <summary>
/// Tehdään tasohyppely peli nimeltä StarCollector
/// </summary>
public class StarCollector : PhysicsGame
{
    private const double NOPEUS = 200;
    private const double HYPPYNOPEUS = 850;

   private PlatformCharacter Pallo;
   private PhysicsObject Tahti;
   private PhysicsObject Nelio;
   private PhysicsObject Vihu;
   private PhysicsObject Ovi;
   private PhysicsObject Tolppa;
   private PhysicsObject Neliopitka;
   private PhysicsObject Tolppax;
   private PhysicsObject Tolppa2;

    private PhysicsObject VasenReuna;
    private PhysicsObject OikeaReuna;

    private IntMeter pisteLaskuri;
    private Timer aikaLaskuri;
    private ScoreList topLista = new ScoreList(10, true, 1000);


    /// <summary>
    /// Pääohjelmassa laitetaan "peli" käyntiin.
    /// </summary>
    public override void Begin()
    {
        Valikko();
        topLista = DataStorage.TryLoad<ScoreList>(topLista, "pisteet.xml");
    }


    /// <summary>
    ///Aliohjelmassa luodaan tason kenttä nr.1
    /// </summary>
    private void LuoKentta()
    {
        LuoPallo(Level.Left, -370);

        Tolppax = new PhysicsObject(450, 30);
        Tolppax.Shape = Shape.Rectangle;
        Tolppax.X = -70;
        Tolppax.Y = -350;
        Tolppax.IgnoresGravity = true;
        Tolppax.Angle = Angle.FromDegrees(90);
        Add(Tolppax);


        LuoVihu(Level.Right - 200, -300);
        LuoVihu(200, -300);


        /// <summary>
        ///Luodaan neliöille lista
        /// </summary>
        double[,] sijainnitNelioille = {
            { -100.0, 200.0, 250.0, 50.0     },
            {   50.0, 150.0, 250.0, 50.0     },
            { Level.Left, 100.0, 100.0, 30.0 },
            { -350.0, -220.0, 150.0, 50.0    },
            { -150.0,-100.0, 150.0, 50.0     },
            { -350.0, 0.0, 150.0, 50.0       },
            { -350.0, 220.0, 150.0, 50.0     },
            {  350.0,-100.0, 150.0, 50.0     }
        };


        /// <summary>
        ///Luodaan Neliöt peliin
        /// </summary>
        for (int indeksiA = 0; indeksiA < sijainnitNelioille.GetLength(0); indeksiA++)
        {
            double x = sijainnitNelioille[indeksiA, 0];
            double y = sijainnitNelioille[indeksiA, 1];

            double leveys = sijainnitNelioille[indeksiA, 2];
            double paksuus = sijainnitNelioille[indeksiA, 3];
            LuoNelio(x, y, leveys, paksuus);

           
        }


        /// <summary>
        ///Luodaan tähdille lista
        /// </summary>
        double[,] sijainnitTahdelle = {
            { -170, -50      },
            {Level.Left, 130 },
            {-100, 250       },
            {300, 20         },
            { 100, 20        },
        };


        /// <summary>
        ///Luodaan tähdet peliin
        /// </summary>
        for (int indeksiA = 0; indeksiA < sijainnitTahdelle.GetLength(0); indeksiA++)
        {
            double x = sijainnitTahdelle[indeksiA, 0];
            double y = sijainnitTahdelle[indeksiA, 1];
            LuoTahti(x, y);


        }

        

        VasenReuna = Level.CreateLeftBorder();
        VasenReuna.Restitution = 1.0;
        VasenReuna.IsVisible = false;

        OikeaReuna = Level.CreateRightBorder();
        OikeaReuna.Restitution = 1.0;
        OikeaReuna.IsVisible = false;

        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Restitution = 1.0;
        alaReuna.IsVisible = false;

        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Restitution = 1.0;
        ylaReuna.IsVisible = false;

        Gravity = new Vector(0.0, -500.0);
        
        LuoAikaNaytto();

        AddCollisionHandler(Pallo, "tahti", delegate (PhysicsObject p1, PhysicsObject ovi)
        {
            PelaajaTormasi(p1, ovi, 0);
        });

        AddCollisionHandler(Pallo, "ovi", delegate (PhysicsObject p1, PhysicsObject ovi)
        {
            PelaajaMaalissa(p1, ovi, 0);
        });

        AddCollisionHandler(Pallo, "vihu", PelaajaKuoli);

        Camera.ZoomToLevel();
        Level.Background.CreateGradient(Color.Blue, Color.Black);
    }


    /// <summary>
    ///Aliohjelmassa luodaan tason kenttä nr.2
    /// </summary>
    private void LuoKentta2()
    {


        LuoPallo(Level.Left, -370);


        /// <summary>
        ///Luodaan neliöille lista
        /// </summary>
        double[,] sijainnitNelioille = {
            { -150, 0, 200, 50   },
            {  -275, 150, 200, 50},
            {-450, 275, 200, 50  },
            {-375, -250, 325, 50 }
        };


        /// <summary>
        ///Luodaan Neliöt peliin
        /// </summary>
        for (int indeksiA = 0; indeksiA < sijainnitNelioille.GetLength(0); indeksiA++)
        {
            double x = sijainnitNelioille[indeksiA, 0];
            double y = sijainnitNelioille[indeksiA, 1];

            double leveys = sijainnitNelioille[indeksiA, 2];
            double paksuus = sijainnitNelioille[indeksiA, 3];
      
            LuoNelio(x, y, leveys, paksuus);
        }


        /// <summary>
        ///Luodaan tähdille lista
        /// </summary>
        double[,] sijainnitTahdelle = {
            { -170, -50      },
            {Level.Left, -300},
            {-450, 350       },
            {100, 250        },
            { 450, 100      },
        };


        /// <summary>
        ///Luodaan tähdet peliin
        /// </summary>
        for (int indeksiA = 0; indeksiA < sijainnitTahdelle.GetLength(0); indeksiA++)
            {
                double x = sijainnitTahdelle[indeksiA, 0];
                double y = sijainnitTahdelle[indeksiA, 1];
    
                LuoTahti(x, y);
            }
       

        Neliopitka = PhysicsObject.CreateStaticObject(800, 50);
        Neliopitka.Shape = Shape.Rectangle;
        Neliopitka.X = 150;
        Neliopitka.Y = -100;
        Neliopitka.IgnoresGravity = true;
        Add(Neliopitka);

        Tolppa2 = PhysicsObject.CreateStaticObject(100, 150);
        Tolppa2.Shape = Shape.Rectangle;
        Tolppa2.X = 450;
        Tolppa2.Y = 0;
        Tolppa2.IgnoresGravity = true;
        Add(Tolppa2);

        LuoTolppa(100, 75);
        LuoTolppa(0, -250);


        LuoVihu(200, -300);
        LuoVihu(200, 150);

        LuoPisteLaskuri();
        LisaaNappaimet();

        VasenReuna = Level.CreateLeftBorder();
        VasenReuna.Restitution = 1.0;
        VasenReuna.IsVisible = false;

        OikeaReuna = Level.CreateRightBorder();
        OikeaReuna.Restitution = 1.0;
        OikeaReuna.IsVisible = false;

        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Restitution = 1.0;
        alaReuna.IsVisible = false;

        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Restitution = 1.0;
        ylaReuna.IsVisible = false;


        Gravity = new Vector(0.0, -500.0);

        LuoAikaNaytto();
        aikaLaskuri.Start();

        AddCollisionHandler(Pallo, "tahti", delegate (PhysicsObject p1, PhysicsObject ovi)
        {
            PelaajaTormasi(p1, ovi, 1);
        });

        AddCollisionHandler(Pallo, "ovi", delegate (PhysicsObject p1, PhysicsObject ovi)
        {
            PelaajaMaalissa(p1, ovi, 1);
        });

        AddCollisionHandler(Pallo, "vihu", PelaajaKuoli);


        Camera.ZoomToLevel();
        Level.Background.CreateGradient(Color.Blue, Color.Black);
    }


    /// <summary>
    /// Aliohjelma joka asettaa pelin asetukset
    /// </summary>
    private void AloitaPeli()
    {
        LuoAikaLaskuri();
        LuoKentta();
        LuoPisteLaskuri();
        LisaaNappaimet();
        

        Vector impulssi = new Vector(10, 0.0);
        Pallo.Hit(impulssi);
    }


    /// <summary>
    /// Aliohjelma piirtää pallon
    /// annettuun paikkaan.
    /// </summary>
    /// <param name="x">pallon x-koordinaatti.</param>
    /// <param name="y">pallon y-koordinaatti.</param>
    private void LuoPallo(double x, double y)
    {
        Pallo = new PlatformCharacter(40.0, 40.0);
        Pallo.Shape = Shape.Circle;
        Pallo.X = Level.Left;
        Pallo.Y = -370;
        Pallo.Restitution = 0;
        Pallo.CollisionIgnoreGroup = 1;
        Pallo.Jump(60.0);
        Pallo.Mass = 50.0;
        Add(Pallo);
    }


    /// <summary>
    /// Aliohjelma piirtää nelion
    /// annettuun paikkaan.
    /// </summary>
    /// <param name="x">nelion x-koordinaatti.</param>
    /// <param name="y">nelion y-koordinaatti.</param>
    private void LuoNelio(double x, double y, double leveys, double pituus)
    {
        Nelio = PhysicsObject.CreateStaticObject(leveys, pituus);
        Nelio.Shape = Shape.Rectangle;
        Nelio.X = x;
        Nelio.Y = y;
        Nelio.IgnoresGravity = true;
        Add(Nelio);
    }


    /// <summary>
    /// Aliohjelma piirtää tähden
    /// annettuun paikkaan.
    /// </summary>
    /// <param name="x">tähden x-koordinaatti.</param>
    /// <param name="y">tähden y-koordinaatti.</param>
    private void LuoTahti(double x, double y)
    {
        Tahti = PhysicsObject.CreateStaticObject(50.0, 30.0);
        Tahti.Shape = Shape.Star;
        Tahti.X = x;
        Tahti.Y = y;
        Tahti.IgnoresGravity = true;
        Tahti.Color = Color.Yellow;
        Tahti.Tag = "tahti";
        Add(Tahti);
    }


    /// <summary>
    /// Aliohjelma piirtää tolppan
    /// annettuun paikkaan.
    /// </summary>
    /// <param name="x">tolppan x-koordinaatti.</param>
    /// <param name="y">tolppan y-koordinaatti.</param>
    private void LuoTolppa(double x, double y)
    {

        Tolppa = PhysicsObject.CreateStaticObject(300, 100);
        Tolppa.Shape = Shape.Rectangle;
        Tolppa.X = x;
        Tolppa.Y = y;
        Tolppa.IgnoresGravity = true;
        Tolppa.Angle = Angle.FromDegrees(90);
        Add(Tolppa);
    }


    /// <summary>
    /// Aliohjelma piirtää vihollisen
    /// annettuun paikkaan.
    /// </summary>
    /// <param name="x">vihollisen x-koordinaatti.</param>
    /// <param name="y">vihollisen y-koordinaatti.</param>
    private void LuoVihu(double x, double y)
    {
        Vihu = new PhysicsObject(50.0, 50.0);
        Vihu.Shape = Shape.Circle;
        Vihu.X = x;
        Vihu.Y = y;
        Vihu.Restitution = 1.0;
        Vihu.Tag = "vihu";
        Add(Vihu);

        Level.CreateBorders(1.0, false);

        Vector impulssi = new Vector(-200.0, 0.0);
        Vihu.Hit(impulssi);
    }


    /// <summary>
    /// taso 1
    /// Aliohjelma toteutuu pelaajan kerättyä tähden.
    /// Aliohjelma tarkistaa onko tähtiä kerätty tarpeeksi, jotta pääsee seuraavalle tasolle.
    /// </summary>
    /// <param name="pallo">pallo (pelaaja)</param>
    /// <param name="tahti">tähti</param>
    private void PelaajaTormasi(PhysicsObject pallo, PhysicsObject tahti, int mones)
    {
        int luku = mones;
        int maximi = 5;

        switch (luku)
        { 
            case 0:
            tahti.Destroy();
            MessageDisplay.Add("Keräsit tähden!");
            pisteLaskuri.Value += 1;
            

            if (pisteLaskuri.Value == maximi)

            {

                MessageDisplay.Add("keräsit kaikki");
                Ovi = PhysicsObject.CreateStaticObject(100.0, 200.0);
                Ovi.Shape = Shape.Hexagon;
                Ovi.X = Level.Right - 50;
                Ovi.Y = -310;
                Ovi.IgnoresGravity = true;
                Ovi.Color = Color.Yellow;
                Ovi.Tag = "ovi";
                Add(Ovi);
            }
                break;

        case 1:
            tahti.Destroy();
            MessageDisplay.Add("Keräsit tähden!");
            pisteLaskuri.Value += 1;

            if (pisteLaskuri.Value == maximi)
            {
                MessageDisplay.Add("keräsit kaikki");
                Ovi = PhysicsObject.CreateStaticObject(100.0, 200.0);
                Ovi.Shape = Shape.Hexagon;
                Ovi.X = Level.Right - 50;
                Ovi.Y = -310;
                Ovi.IgnoresGravity = true;
                Ovi.Color = Color.Yellow;
                Ovi.Tag = "ovi";
                Add(Ovi);
                Remove(Tolppa);
            }
         break;
        }
    }

   
    /// <summary>
    /// taso 1
    /// Aliohjelma toteutuu pelaajan osuttua oveen.
    /// </summary>
    /// <param name="pallo">pallo (pelaaja)</param>
    /// <param name="ovi">ovi</param>
    private void PelaajaMaalissa(PhysicsObject pallo, PhysicsObject ovi, int oviluku)
    {
        int luku = oviluku;

        switch (luku)
        {
        case 0:
                ClearAll();
                LuoKentta2();
                luku++;
                break;

         case 1:
                Valikko();
                aikaLaskuri.Stop();
                HighScoreWindow topIkkuna = new HighScoreWindow(
                                 "Parhaat ajat",
                                 "Onneksi olkoon, pääsit listalle pisteillä %p! Syötä nimesi:",
                                 topLista, aikaLaskuri.SecondCounter.Value);
                topIkkuna.Closed += TallennaPisteet;
                Add(topIkkuna);
                break;
                
         }
            
      
    }


    /// <summary>
    /// Aliohjelma toteutuu kun pelaaja osuu viholliseen.
    /// </summary>
    /// <param name="pallo">pallo (pelaaja)</param>
    /// <param name="vihu">vihollinen</param>
    private void PelaajaKuoli(PhysicsObject pallo, PhysicsObject vihu)
    {
        ClearAll();
        AloitaPeli();
    }


    /// <summary>
    /// Aliohjelma toteuttaa pistelaskurin näyttöön.
    /// </summary>
    private void LuoPisteLaskuri()
    {
        pisteLaskuri = new IntMeter(0);

        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Left + 100;
        pisteNaytto.Y = Screen.Top - 50;
        pisteNaytto.TextColor = Color.White;
        pisteNaytto.Color = Color.Transparent;

        pisteNaytto.BindTo(pisteLaskuri);
        Add(pisteNaytto);
    }


    /// <summary>
    /// Aliohjelma toteuttaa pistelaskurin näyttöön.
    /// </summary>
    private void TallennaPisteet(Window sender)
    {
        DataStorage.Save<ScoreList>(topLista, "pisteet.xml");
    }


    /// <summary>
    /// Aliohjelma joka asettaa aikalaskurin ja käynnistää sen.
    /// </summary>
    private void LuoAikaLaskuri()
    {
        aikaLaskuri = new Timer();
        aikaLaskuri.Start();
    }


    /// <summary>
    /// Aliohjelma lisää aikanäytön kenttään.
    /// </summary>
    private void LuoAikaNaytto()
    {
        Label aikaNaytto = new Label();
        aikaNaytto.X = Screen.Left + 200;
        aikaNaytto.Y = Screen.Top - 50;
        aikaNaytto.Width = 200;
        aikaNaytto.TextColor = Color.White;
        aikaNaytto.Color = Color.Red;
        aikaNaytto.DecimalPlaces = 1;
        aikaNaytto.BindTo(aikaLaskuri.SecondCounter);
        Add(aikaNaytto);
    }

    /// <summary>
    /// Aliohjelma asettaa näppäinasetukset
    /// </summary>
    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", Pallo, -NOPEUS);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", Pallo, NOPEUS);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", Pallo, HYPPYNOPEUS);

        Keyboard.Listen(Key.Escape, ButtonState.Pressed, Exit, "Poistu");
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
    }


    /// <summary>
    /// Aliohjelma luo peliin aloitusvalikon.
    /// </summary>
    private void Valikko()
    {
        ClearAll(); // Tyhjennetään kenttä kaikista peliolioista
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Pelin alkuvalikko",
        "Aloita uusi peli", "Lopeta");
        Add(alkuValikko);

        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, Exit);
    }


    /// <summary>
    /// Aliohjelma joka asettaa pelaajan liikkumisen
    /// </summary>
    /// <param name="pallo">pallo (pelaaja)</param>
    /// <param name="nopeus">olion liikkumisen nopeus</param>
    private void Liikuta(PlatformCharacter pallo, double nopeus)
    {
        pallo.Walk(nopeus);
    }


    /// <summary>
    /// Aliohjelma joka asettaa pelaajan liikkumisen
    /// </summary>
    /// <param name="pallo">pallo (pelaaja)</param>
    /// <param name="nopeus">olion liikkumisen nopeus</param>
    private void Hyppaa(PlatformCharacter pallo, double nopeus)
    {
        pallo.Jump(nopeus);  
    }

}


