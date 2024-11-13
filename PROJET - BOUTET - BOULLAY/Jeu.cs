using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET___BOUTET___BOULLAY
{
    /// <summary>
    /// Classe contenant les données et processus d'execution d'une partie
    /// </summary>
    internal class Jeu
    {
        //  Declaration des variables (dictionnaire, plateau, joueurs, mode, timer)
        private Dictionnaire dictionnaire; // Dictionnaire de la partie (default de l'annexe)
        private Plateau plateau;// Initialisation du plateau de la partie (Configuration dans la classe Plateau)
        private Joueur[] joueurs; // Tableau des joueurs (Configuration dans la classe Joueur)
        private int mode = 0; // Mode de jeu (2 Modes : -lecture plateau fichier / -Plateau aleatoire)
        private TimeSpan timer; // Durée d'une partie


        //  Définition des propriétées
        public Dictionnaire Dictionnaire { get { return this.dictionnaire; } set { this.dictionnaire = value; } }
        public Plateau Plateau { get { return this.plateau; } set { this.plateau = value; } }
        public Joueur[] Joueurs { get { return this.joueurs; } set { this.joueurs = value; } }
        public int Mode { get { return this.mode; } set { this.mode = value; } }

        /// <summary>
        /// Constructeur de la classe Jeu (Vide car l'initialisation des variables se fait dans Launcher())
        /// </summary>
        public Jeu() {}

        /// <summary>
        /// Fonction de Lancement d'une partie (Appeler depuis la classe Program, Fonction Main")
        /// </summary>
        /// <returns>Le programme s'arrête si et seulemement si l'utilisateur décide de sortir (booléen "condition_arret" dans la classe Program)</returns>
        public bool Launcher()
        {
            // Initialisation des variables auxilières
            double multiplicateur = 4;
            double distanceMultiplicateur = 6;


            // Affichage Titre
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Clear();
            Console.Write(@"

 
                                     JJJJJJJJ  EEEEE   U    U      DDDD    U    U      BBBBB    OOO     GGGG    GGGG   L       EEEEE
                                         J     E       U    U      D   D   U    U      B    B  O   O   G       G       L       E    
                                         J     EEEE    U    U      D    D  U    U      BBBBB   O   O   G  GG   G  GG   L       EEEE 
                                     J   J     E       U    U      D   D   U    U      B    B  O   O   G   G   G   G   L       E    
                                      JJJ      EEEEE     UU        DDDD      UU        BBBBB    OOO    GGGG    GGGG    LLLLL   EEEEE
                                                    
");

            // Défintiion du menu de selection (Sortir / Jouer)
            bool selected = false;
            int selection = 0;
            int tabul = Console.CursorLeft;
            while (!selected)
            {
                tabul = Console.CursorLeft;
                switch (selection)
                {
                    case 0: WriteCentered("Jouer <", true); WriteCentered(" Sortir"); break;
                    case 1: WriteCentered("Jouer"); WriteCentered(" Sortir <", true); break;
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow)
                {
                    selection = (selection + 1) % 2; // Si 0 -> 1 , si 1 -> 0
                    Nettoyage_Lignes(2, tabul);
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    selected = true;
                }
                else
                {
                    Nettoyage_Lignes(2, tabul);
                }
            }
            if (selection == 1) { return true; } // Si le joueur veux sortir on renvoit true pour l'arret complet du programme.

            // Initialisation d'un plateau Random (default)
            plateau = new Plateau(false);

            // Menu d'initialisation des joueurs
            Console.WriteLine();
            WriteCentered("Nombre de joueurs : ");
            tabul = Console.CursorLeft;
            int nbJ = 0;
            bool valid = false;
            while (!valid)
            {
                try
                {
                    nbJ = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Nettoyage_Lignes(1, tabul);
                    continue;
                }
                if (nbJ > 0)
                {
                    valid = true;
                }
                else
                {
                    Nettoyage_Lignes(1, tabul);
                }
            }
            this.joueurs = new Joueur[nbJ];
            for (int i = 0; i < nbJ; i++)
            {
                Console.WriteLine();
                WriteCentered("Nom du joueur " + (i + 1) + " : ");
                tabul = Console.CursorLeft;
                string name = " ";
                valid = false;
                while (!valid)
                {
                    try
                    {
                        name = Console.ReadLine();
                    }
                    catch
                    {
                        Nettoyage_Lignes(1, tabul);
                        continue;
                    }
                    if (name != null && name.Length > 0)
                    {
                        valid = true;
                    }
                    else
                    {
                        Nettoyage_Lignes(1, tabul);
                    }
                }
                this.joueurs[i] = new Joueur(name);
            }
            Nettoyage_Lignes(nbJ * 3 + 6);

            // Menu de selection du temps de partie
            WriteCentered("Temps de la partie (minutes) : ");
            tabul = Console.CursorLeft;
            int temps = 0;
            valid = false;
            while (!valid)
            {
                try
                {
                    temps = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Nettoyage_Lignes(1, tabul);
                    continue;
                }
                if (temps > 0)
                {
                    valid = true;
                }
                else
                {
                    Nettoyage_Lignes(1, tabul);
                }
            }
            this.timer = new TimeSpan(0, temps, 0);


            // Menu de selection du mode
            mode = 0;
            selected = false;
            while (!selected)
            {
                tabul = Console.CursorLeft;
                Nettoyage_Lignes(2, tabul);
                switch (mode)
                {
                    case 0: WriteCentered("Jouer à partir d'un fichier <", true); WriteCentered("Jouer à partir d'un tableau généré aléatoirement"); break;
                    case 1: WriteCentered("Jouer à partir d'un fichier"); WriteCentered("Jouer à partir d'un tableau généré aléatoirement <", true); break;
                }
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow)
                {
                    mode = (mode + 1) % 2; // Si 1 -> 0 , si 0 -> 1
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    selected = true;
                }
            }
            Console.WriteLine();

            // Menu de demande du fichier (Si mode = fichier)
            if (mode == 0)
            {
                bool validIn = false;
                while (!validIn)
                {
                    WriteCentered("Nom du fichier : ");
                    tabul = Console.CursorLeft;
                    string path = Console.ReadLine();
                    if (File.Exists(path)) // Verification de la validité du fichier
                    {
                        validIn = true;
                        this.plateau = new Plateau(true, path);
                    }
                    else
                    {
                        Nettoyage_Lignes(2, tabul);
                    }
                }
            }


            // Initialisation du dictionnaire (dictionnaire par default de l'annexe)
            this.dictionnaire = new Dictionnaire("MotsPossiblesFR.txt");
            Dictionnaire.Tri_QuickSort();

            // -----------------------------------------------------------

            // Debut de la partie
            Console.Clear();
            //Mesure de la date de départ
            DateTime gameStart = DateTime.Now;
            // Tant que le temps de la partie n'est pas depassé faire 1 tour de plus pour chaque joueur (meme si ca depasse le temps)
            while (DateTime.Now - gameStart < timer)
            {
                // Fais jouer chaque joueur un à un
                for (int i = 0; i < joueurs.Length; i++)
                {
                    // Menu d'entrée du mot
                    bool validMot = false;
                    while (!validMot)
                    {
                        Console.Clear();
                        // Met a jour l'interface (timers) tant que le joueur n'entre pas d'input
                        while (!Console.KeyAvailable)
                        {
                            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
                            // Affichage Points et Timer
                            string recap = "";
                            for (int j = 0; j < joueurs.Length; j++)
                            {
                                recap += joueurs[j].Nom + " : " + joueurs[j].Score + ((j != joueurs.Length - 1) ? "pts ; " : "pts");
                            }
                            WriteCentered(recap);
                            TimeSpan timeLeft = timer - (DateTime.Now - gameStart);
                            //Si timer negatif -> rouge (signifie que la partie fini a la fin du tour)
                            if (timeLeft < TimeSpan.Zero) Console.ForegroundColor = ConsoleColor.Red;
                            WriteCentered("Temps restant : " + (int)timeLeft.TotalMinutes + "min" + timeLeft.Seconds % 60 + "s");

                            // Affichage du joueur en cours de tour
                            Console.ForegroundColor = ConsoleColor.Gray;
                            WriteCentered("Tour de " + joueurs[i]);
                            Console.ResetColor();

                            // Affiche le plateau
                            plateau.Afficher_Plateau();
                            WriteCentered("Mot a placer dans le plateau :");
                            Thread.Sleep(100);
                        }

                        // Lit le mot
                        string mot = Console.ReadLine();
                        if (mot == null || mot.Length == 0)
                        {
                            break;
                        }
                        // Cherche le mot dans le dictionnaire
                        if (dictionnaire.RechDichoRecursif(mot.ToUpper()) && !joueurs[i].Contient(mot))
                        {
                            // Recherche des coordonnées du mot dans le plateau (vide == pas trouvé)
                            List<int[]> motFound = plateau.RechercheMot(mot);
                            //Console.WriteLine(mot.Length + " : " + motFound.Count);

                            // Si mot trouvé (non vide) : ajout des scores
                            if (motFound != null && motFound.Count == mot.Length)
                            {
                                // Ajoute le mot aux mots utilisés du joueur
                                joueurs[i].Ajout_Mot(mot);
                                List<int[]> coordsScore = new List<int[]>();

                                // Formatage d'une Liste avec coordonnées plus score associé (pour affichage)
                                for (int j = 0; j < motFound.Count; j++)
                                {
                                    // Application du multiplicateur de point celon la distance (exponentielle  où à 0 : *1 , à distanceMultiplicateur : *multiplicateur)
                                    int score_Mot = plateau.Vars[Plateau.Vals[motFound[j][0], motFound[j][1]]][1] * (int)(Math.Pow(multiplicateur, j / distanceMultiplicateur));
                                    coordsScore.Add(new int[3] { motFound[j][0], motFound[j][1], score_Mot });
                                    //Console.WriteLine(score_Mot);
                                    joueurs[i].Ajout_Score(score_Mot);
                                }
                                
                                plateau.MajAnim(coordsScore, multiplicateur, distanceMultiplicateur);// Envoie les coordonnées et scores associés a l'animation
                                validMot = true;// Condition d'arret
                            }
                        }
                    }
                }
            }

            // Affichage de fin de partie
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"

  EEEEE   N   N   DDDD  
  E       NN  N   D   D 
  EEEE    N N N   D   D 
  E       N  NN   D   D 
  EEEEE   N   N   DDDD  

                                                           
                            
");


            // Menu du recapitulatif de fin
            WriteCentered("Recapitulatif de la partie :");

            // Check gagnant(s) (si ex aequo aussi d'où la liste)
            // La Liste contient l'indice du(des) joueur(s) gagnant(s) dans la liste des joueurs
            // Liste de taille 1 si un seul gagnant
            List<int> gagnants = new List<int> { 0 };
            for (int i = 1; i < joueurs.Count(); i++)
            {
                if (joueurs[i].Score > joueurs[gagnants[0]].Score)
                {
                    gagnants = new List<int> { i }; // Si superieur trouvé nouvelle liste avec le gagnant unique
                }
                else if (joueurs[i].Score == joueurs[gagnants[0]].Score)
                {
                    gagnants.Add(i); // Si egal ajout du n-ieme gagnant
                }
            }

            for (int i = 0; i < joueurs.Count(); i++)// Affichage des scores et des mots trouvés !
            {
                Console.WriteLine();
                if (gagnants.Contains(i)) // Test si c'est gagnant
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteCentered("Gagnant" + ((gagnants.Count > 1) ? " ex aequo !" : " !")); // (Test e l'ex aequo)
                }
                WriteCentered(joueurs[i] + " : " + joueurs[i].Score + "pts, mots trouvés : ");
                tabul = Console.CursorLeft;

                foreach (string mot in joueurs[i].Mots) //On affiche les mots trouvés dans la partie par le joueur (depuis la classe Joueur).
                {
                    Console.SetCursorPosition(tabul, Console.CursorTop);
                    Console.WriteLine("- " + mot);
                }

            }
            Console.ReadLine();
            return false;
        }

        /// <summary>
        /// Fonction d'affichage console centré
        /// Le booléein sel permet d'indiquer si la ligne est selctionnée (dans un menu par exemple) ce qui change le decalage et formatage
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sel"></param>
        private void WriteCentered(string input, bool sel = false)
        {
            Console.SetCursorPosition(75 - input.Length / 2 + (sel ? 1 : 0), Console.CursorTop);
            if (sel)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(input);
            Console.ResetColor();
            Console.SetCursorPosition(75 - input.Length / 2 + (sel ? 1 : 0), Console.CursorTop);
        }


        /// <summary>
        /// Fonction supprimant les "n" dernières lignes de la console
        /// Le int dec permet de decaler le debut de la ligne apres supression (pour conserver un centrage par exemple)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="dec"></param>
        private void Nettoyage_Lignes(int n, int dec = 0)
        {
            int currentLineCursor = Console.CursorTop - n;
            Console.SetCursorPosition(0, currentLineCursor);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                    Console.Write(" ");
            }
            Console.SetCursorPosition(dec, currentLineCursor);
        }
    }
}
