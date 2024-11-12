using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace PROJET___BOUTET___BOULLAY
{
    /// <summary>
    /// Classe contenant les plateaux du jeu ainsi que ses paramètres de contrôles
    /// </summary>
    internal class Plateau
    {
        //Déclaration des variables (plateau_jeu, lettre, ponderation)
        private char[,] plateau_jeu; 
        private Dictionary<char, int[]> lettre;
        private List<char> ponderation; 

        /// <summary>
        /// Constructeur de la classe Plateau.
        /// Celui ci concevoit un input un chemin relatif si le fichier existe. 
        /// Sinon (si le fichier n'existe pas)e il génère un plateu aléatoire de dimension 8x8.
        /// </summary>
        /// <param name="fichierExistant"></param>
        /// <param name="path"></param>
        public Plateau(bool fichierExistant, string path = null)
        {
            plateau_jeu = new char[8,8];  // Selon l'énoncé, on considère que la dimension du plateau ne change pas et reste [8x8]
            lettre = new Dictionary<char, int[]>(); // On configure les autres variables (lettre, ponderation) 
            ponderation = new List<char>();

            //Lecture du fichier Lettre.txt (situé dans le même fichier que Plateau.cs)
            List<string> Lignes_lettres = new List<string>();
            using (StreamReader sr = new StreamReader("Lettre.txt"))
            {
                string ligne;
                int i = 0;
                while ((ligne = sr.ReadLine()) != null)
                {
                    Lignes_lettres.Add(ligne);
                    i++;
                }
            }

            for (int i = 0; i < Lignes_lettres.Count && i < 26; i++)
            {
                string[] lettres = Lignes_lettres[i].Split(',');
                lettre.Add(lettres[0][0], new int[2] { Convert.ToInt32(lettres[1]), Convert.ToInt32(lettres[2]) }); // On ajoute les points dus à chaque lettres.
                for (int j = 0; j < Convert.ToInt32(lettres[1]); j++) // On ajoute désormais la pondération 
                {
                    ponderation.Add(lettres[0][0]);
                }
       
            }

            // On teste si le fichier existe (on lit alors le plateau dans le fichier)
            if (fichierExistant)
            {
                List<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                        i++;
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

            // Si le fichier n'existe pas, alors génération aléatoire du plateau
            else
            {
                // On balaye toutes les valeurs de la matrice par une double boucle for()
                for (int i = 0; i < plateau_jeu.GetLength(0); i++)
                {
                    for (int j = 0; j < plateau_jeu.GetLength(1); j++)
                    {
                        Random random = new Random(); // Generation random du plateau 
                        bool autorisation = false;
                        while (!autorisation)
                        {
                            int rand = random.Next(ponderation.Count); // Recuperation de la lettre ainsi que sa ponderation
                            char lettre_p = ponderation[rand];

                            if (lettre[lettre_p][0] > 1)  // Verification du nombre autorisé de lettres
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
                if (i == 7) { Console.ForegroundColor = ConsoleColor.Blue; } // Pour l'esthétisme, on décide de changer la couleur de la ligne du bas !
                else { Console.ForegroundColor = ConsoleColor.Gray; }

                Console.SetCursorPosition(59, Console.CursorTop);
                Console.WriteLine("+---+---+---+---+---+---+---+---+"); // On définit la forme du plateau
                Console.SetCursorPosition(59, Console.CursorTop);
                string ret = "";
                // For de balayage ligne
                for (int j = 0; j < plateau_jeu.GetLength(1); j++)
                {
                    bool inCoord = false;

                    if (coordonnees != null && coordonnees.Count > 0)
                    {
                        // Check des coordonnées d'override
                        for (int k = 0; k < coordonnees.Count; k++)
                        {
                            int[] coord = coordonnees[k];
                            // Si dans les coordonnées d'override : affichage modifié
                            if (i == coord[0] && j == coord[1])
                            {
                                Console.ResetColor();
                                Console.ForegroundColor = couleur_coordonnees;
                                inCoord = true;
                                // Si multiplicateur affichage modifié
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

                    // Si pas dans les coordonnées d'override, affichage classique
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
            Console.WriteLine("+---+---+---+---+---+---+---+---+"); // On réitère cette forme de tableau entre chaque ligne.
            Console.ResetColor();

        }

        /// <summary>
        /// La fonction d'override ToString renverra toutes les informations nécessaires sur le plateau (Sa taille en particulier)
        /// <returns>string de description</returns>
        public override string ToString() { return "Le plateau est de dimension : 8x8"; }

        /// <summary>
        /// Fonction d'exportation du plateau
        /// </summary>
        /// <param name="filename"></param>
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

        /// <summary>
        /// Fonction de lecture du fichier
        /// </summary>
        /// <param name="path"></param>
        public void ToRead(string path)
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                    i++;
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

        /// <summary>
        /// Fonction d'initialisation : Recherche dans le plateau
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>Vide si pas trouvé, sinon renvoie les coordonnées</returns>
        public List<int[]> RechercheMot(string mot)
        {
            if (string.IsNullOrEmpty(mot)) return (null);
            List<int[]> path = new List<int[]>();
            for (int i = 0; i < 8; i++)
            {
                if (mot.ToUpper()[0] == plateau_jeu[7, i])
                {
                    //Console.WriteLine(i);
                    path = RehercheRecursive(mot.ToUpper().Substring(1), 7, i).Prepend(new int[2] { 7, i }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
            }
            return (null);
        }
        /// <summary>
        /// Recherche Récursive dans le tableau
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>Rien si l'on ne trouve pas, sinon renvoie les coordonnées</returns>
        private List<int[]> RehercheRecursive(string mot, int i, int j)
        {
            //Console.WriteLine(mot);

            // Condition d'arret : si mot trouvé en entier revoyer la liste a remplir par la pile d'appel
            if (mot == "")
            {
                return new List<int[]> { };
            }
            else
            {
                List<int[]> path = new List<int[]>();
                // Si direction suivante contient la bonne lettre :
                if (i > 0 && plateau_jeu[i - 1, j] == mot[0])
                {              
                    path = RehercheRecursive(mot.Substring(1), i - 1, j).Prepend(new int[2] { i - 1, j }).ToList(); // Appel de la recursive sur la case de cette direction et ajout de la coordonnées a la liste recue
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; } // Verification de repassage sur la coordonnée
                    if (path != null && path.Count == mot.Length && !passed) return path; // Si tout valide : revoyer la nouvelle liste
                }

                
                if (j > 0 && plateau_jeu[i, j - 1] == mot[0]) // Pour la direction Gauche
                {
                    path = RehercheRecursive(mot.Substring(1), i, j - 1).Prepend(new int[2] { i, j - 1 }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
                if (j < 7 && plateau_jeu[i, j + 1] == mot[0]) // Pour la direction Droite
                {
                    path = RehercheRecursive(mot.Substring(1), i, j + 1).Prepend(new int[2] { i, j + 1 }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
                if (i > 0 && j > 0 && plateau_jeu[i - 1, j - 1] == mot[0]) // Pour la diagonale à Gaucbe
                {
                    path = RehercheRecursive(mot.Substring(1), i - 1, j - 1).Prepend(new int[2] { i - 1, j - 1 }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
                if (i > 0 && j < 7 && plateau_jeu[i - 1, j + 1] == mot[0]) // Pour la diagonale à Droite
                {
                    path = RehercheRecursive(mot.Substring(1), i - 1, j + 1).Prepend(new int[2] { i - 1, j + 1 }).ToList();
                    bool passed = false;
                    if (path.Count > 2 && path[2][0] == path[0][0] && path[2][1] == path[0][1]) { passed = true; }
                    if (path != null && path.Count == mot.Length && !passed) return path;
                }
            }
            return new List<int[]> { };
        }

        /// <summary>
        /// Fontion de mise a jour du tableau animée
        /// Recupère les coordonnées couplées au score propre a celles-ci et les constante de multiplication des points
        /// </summary>
        /// <param name="motFound"></param>
        /// <param name="multiplicateur"></param>
        /// <param name="distanceMultiplicateur"></param>
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

            Console.Clear(); // Nettoyage de la console
            
            for (int i = motFound.Count - 1; i >= 0; i--) { Animation(motFound[i][0], motFound[i][1]); } // Descente des lettres recursive pour chaque coordonnée
        }

        /// <summary>
        /// EFFET CASCADE : Fonction recursive permettant de faire tomber les lettres au dessus de la coordonnée donnée 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Animation(int i, int j)
        {
            // Test de la ligne d'en haut
            if (i != 0)
            {
                // Echange la lettre du dessus
                plateau_jeu[i, j] = plateau_jeu[i - 1, j];
                plateau_jeu[i - 1, j] = ' ';

                
                Thread.Sleep(70); // Effet d'attente pour la cascade des lettres vers le bas.
                Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                
                Afficher_Plateau();// On affiche le tableau
                Animation(i - 1, j); // Appel de la case superieure et ainsi de suite (Récursivité)
            }
        }

    }
}
