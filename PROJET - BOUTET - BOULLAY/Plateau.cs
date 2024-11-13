using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PROJET___BOUTET___BOULLAY
{
    /// <summary>
    /// Classe contenant les plateaux du jeu ainsi que ses paramètres de contrôles
    /// </summary>
    internal class Plateau
    {
        // Déclaration des variables (plateau_jeu, lettre, ponderation)
        private char[,] plateau_jeu;
        private Dictionary<char, int[]> lettre;
        private List<char> ponderation;

        public char[,] Vals { get { return this.plateau_jeu; } set { this.plateau_jeu = value; } }
        public Dictionary<char, int[]> Vars { get { return this.lettre; } set { this.lettre = value; } }

        /// <summary>
        /// Constructeur de la classe Plateau.
        /// Celui ci concevoit un input un chemin relatif si le fichier existe. 
        /// Sinon (si le fichier n'existe pas) il génère un plateau aléatoire de dimension 8x8.
        /// </summary>
        /// <param name="fichierExistant"></param>
        /// <param name="path"></param>
        public Plateau(bool fichierExistant, string path = null)
        {
            plateau_jeu = new char[8, 8];  // Dimension du plateau fixe à 8x8
            lettre = new Dictionary<char, int[]>(); // On configure les autres variables (lettre, ponderation) 
            ponderation = new List<char>();

            // Lecture du fichier Lettres.txt (situé dans le même dossier que Plateau.cs)
            List<string> Lignes_lettres = new List<string>();
            using (StreamReader sr = new StreamReader("Lettres.txt"))
            {
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    Lignes_lettres.Add(ligne);
                }
            }

            // On remplit le dictionnaire 'lettre' et la liste 'ponderation'
            for (int i = 0; i < Lignes_lettres.Count && i < 26; i++)
            {
                string[] lettres = Lignes_lettres[i].Split(';');
                lettre.Add(lettres[0][0], new int[2] { Convert.ToInt32(lettres[1]), Convert.ToInt32(lettres[2]) });
                for (int j = 0; j < Convert.ToInt32(lettres[1]); j++)
                {
                    ponderation.Add(lettres[0][0]);
                }
            }

            // Si le fichier existe, on charge le plateau depuis ce fichier
            if (fichierExistant)
            {
                List<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                for (int i = 0; i < lines.Count && i < 8; i++)
                {
                    string[] lettres = lines[i].Split(';', ',');
                    for (int j = 0; j < lettres.Length && j < 8; j++)
                    {
                        plateau_jeu[i, j] = lettres[j].ToUpper()[0];
                    }
                }
            }
            else
            {
                // Génération aléatoire du plateau
                Random random = new Random();
                for (int i = 0; i < plateau_jeu.GetLength(0); i++)
                {
                    for (int j = 0; j < plateau_jeu.GetLength(1); j++)
                    {
                        bool autorisation = false;
                        while (!autorisation)
                        {
                            int rand = random.Next(ponderation.Count);
                            char lettre_p = ponderation[rand];
                            if (lettre[lettre_p][0] > 1)
                            {
                                plateau_jeu[i, j] = lettre_p;
                                lettre[lettre_p][0] -= 1;
                                autorisation = true;
                            }
                        }
                    }
                }
            }
        }

        public void Afficher_Plateau(List<int[]> coordonnees = null, ConsoleColor couleur_coordonnees = ConsoleColor.White, List<bool> mult = null, ConsoleColor couleur_mult = ConsoleColor.White)
        {
            for (int i = 0; i < plateau_jeu.GetLength(0); i++)
            {
                Console.ResetColor();
                if (i == 7) { Console.ForegroundColor = ConsoleColor.Blue; }
                else { Console.ForegroundColor = ConsoleColor.Gray; }

                Console.SetCursorPosition(59, Console.CursorTop);
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
                Console.SetCursorPosition(59, Console.CursorTop);
                string ret = "";
                for (int j = 0; j < plateau_jeu.GetLength(1); j++)
                {
                    bool inCoord = false;

                    if (coordonnees != null && coordonnees.Count > 0)
                    {
                        for (int k = 0; k < coordonnees.Count; k++)
                        {
                            int[] coord = coordonnees[k];
                            if (i == coord[0] && j == coord[1])
                            {
                                Console.ResetColor();
                                Console.ForegroundColor = couleur_coordonnees;
                                inCoord = true;
                                if (mult != null && k < mult.Count && mult[k] == true)
                                {
                                    Console.ForegroundColor = couleur_mult;
                                    Console.Write(((j == 0) ? "|" : "") + coord[2] + "x");
                                }
                                else
                                {
                                    Console.Write(((j == 0) ? "|" : "") + coord[2] + "p");
                                }
                                Console.ResetColor();
                                if (i == 7)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                }
                                Console.Write(((coord[2] % 10 < coord[2]) ? "|" : " |"));
                            }
                        }
                    }

                    if (!inCoord)
                    {
                        Console.ResetColor();
                        if (i == 7)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(((j == 0) ? "| " : " ") + plateau_jeu[i, j] + " |");
                    }

                }
                Console.Write('\n');
            }
            Console.SetCursorPosition(59, Console.CursorTop);
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            Console.ResetColor();
        }

        public override string ToString() { return "Le plateau est de dimension : 8x8"; }

        public void ToFile(string filename)
        {
            string[] linesPlateau = new string[8];
            for (int i = 0; i < plateau_jeu.GetLength(0); i++)
            {
                for (int j = 0; j < plateau_jeu.GetLength(1); j++)
                {
                    linesPlateau[i] += plateau_jeu[i, j] + ((j != plateau_jeu.GetLength(1) - 1) ? ";" : "");
                }
            }
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (string line in linesPlateau)
                {
                    sw.WriteLine(line);
                }
            }
        }

        public void ToRead(string path)
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            for (int i = 0; i < lines.Count && i < 8; i++)
            {
                string[] lettres = lines[i].Split(';');
                for (int j = 0; j < lettres.Length && j < 8; j++)
                {
                    plateau_jeu[i, j] = lettres[j].ToUpper()[0];
                }
            }
        }

        public List<int[]> RechercheMot(string mot)
        {
            if (string.IsNullOrEmpty(mot)) return null;
            List<int[]> path = new List<int[]>();
            for (int i = 0; i < 8; i++)
            {
                if (mot.ToUpper()[0] == plateau_jeu[7, i])
                {
                    path = RehercheRecursive(mot.ToUpper().Substring(1), 7, i).Prepend(new int[2] { 7, i }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
            }
            return null;
        }

        private List<int[]> RehercheRecursive(string mot, int i, int j)
        {
            if (mot == "")
            {
                return new List<int[]> { };
            }
            else
            {
                List<int[]> path = new List<int[]>();
                if (i > 0 && plateau_jeu[i - 1, j] == mot[0])
                {
                    path = RehercheRecursive(mot.Substring(1), i - 1, j).Prepend(new int[2] { i - 1, j }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (!passed) return path;
                }
                if (j > 0 && plateau_jeu[i, j - 1] == mot[0])
                {
                    path = RehercheRecursive(mot.Substring(1), i, j - 1).Prepend(new int[2] { i, j - 1 }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (!passed) return path;
                }
                return path;
            }
        }

        public void MajAnim(List<int[]> motFound, double multiplicateur, double distanceMultiplicateur)
        {

            // Temps entre les changements d'animation
            int tempsIteration = 400;

            // Declaration de variables buffer
            List<int[]> coords = new List<int[]>();
            List<int[]> motFoundBase = new List<int[]>();
            foreach (int[] co in motFound) { motFoundBase.Add(new int[3] { co[0], co[1], co[2] }); }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Afficher_Plateau(coords, ConsoleColor.Red);

            // For de balayage des coordonnées données pour points sans multiplicateurs
            for (int i = 0; i < motFound.Count; i++)
            {
                // Attente ralentie vers la fin du mot : à 0 : tempsIteration, à fin : temps*2.5
                Thread.Sleep(tempsIteration * (int)Math.Pow(2.5, (double)i / (double)motFound.Count));
                Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
                // Calcul des points pour affichage sans multiplicateur
                motFoundBase[i][2] = lettre[plateau_jeu[motFound[i][0], motFound[i][1]]][1];
                // Recupère une partie de la liste jusqu'à i pour afficher l'avance 'en serpentin'
                coords = motFoundBase.GetRange(0, i + 1);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Affiche le tableau avec les coordonnées overrides
                Afficher_Plateau(coords, ConsoleColor.Red);
            }
            List<bool> mults = new List<bool>();
            // For de balayage des coordonnées données pour multiplicateurs
            for (int i = 0; i < motFound.Count; i++)
            {
                Thread.Sleep(tempsIteration * (int)Math.Pow(2.5, (double)i / (double)motFound.Count));
                Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);

                // Remplace graduellement les valeurs par les multiplicateurs
                motFoundBase[i][2] = (int)(Math.Pow(multiplicateur, i / distanceMultiplicateur));
                // Ajoute la coordonnée au flag multiplicateur
                mults.Add(true);
                coords = motFoundBase;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Affiche le tableau avec les coordonnées overrides et les flag de multiplicateur
                Afficher_Plateau(coords, ConsoleColor.Blue, mults, ConsoleColor.Yellow);
            }
            // For de balayage des coordonnées données pour points avec multiplicateurs
            for (int i = motFound.Count - 1; i >= 0; i--)
            {
                plateau_jeu[motFound[i][0], motFound[i][1]] = ' ';
                Thread.Sleep(tempsIteration * (int)Math.Pow(2.5, (double)(motFound.Count - i) / (double)motFound.Count));
                Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);

                // Additionne graduellement les points pour faire 'l'effet de retour'
                motFound[i][2] += ((i == motFound.Count - 1) ? 0 : motFound[i + 1][2]);
                coords = motFound.GetRange(0, i + 1);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Affiche le tableau avec les coordonnées overrides et les flag de multiplicateur
                Afficher_Plateau(coords, ConsoleColor.Cyan);
            }
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                coords = motFound.GetRange(0, 1);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Affiche le tableau avec les coordonnées overrides et les flag de multiplicateur
                if (i % 2 == 0)
                {
                    Afficher_Plateau(coords, ConsoleColor.Cyan);
                }
                else
                {
                    Afficher_Plateau(coords, ConsoleColor.White);
                }
            }
            Thread.Sleep(100);

            Console.Clear();
            // Initie la descente des lettres recursive pour chaque coordonnée
            for (int i = motFound.Count - 1; i >= 0; i--)
            {
                DropAnim(motFound[i][0], motFound[i][1]);
            }
        }

        /// <summary>
        /// Fonction recursive permettant de faire tomber les lettres au dessus de la coordonnée donnée
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void DropAnim(int i, int j)
        {
            // Si pas a la ligne d'en haut
            if (i != 0)
            {
                // Echange la lettre du dessus
                plateau_jeu[i, j] = plateau_jeu[i - 1, j];
                plateau_jeu[i - 1, j] = ' ';
                //Console.WriteLine(this.ToString());

                // Attente pour l'effet cascade
                Thread.Sleep(70);
                Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Affiche le tableau
                Afficher_Plateau();
                // Appel de la case superieure
                DropAnim(i - 1, j);
            }
        }
    }
}
