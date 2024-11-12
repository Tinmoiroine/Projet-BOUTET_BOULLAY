using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace PROJET___BOUTET___BOULLAY
{
    /// <summary>
    /// Classe contenant les plateaux du jeu ainsi que ses paramètres de contrôles
    /// </summary>
    internal class Plateau
    {

        //Déclarations des variables (plateau_jeu, lettre, ponderation)
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

    }
}
