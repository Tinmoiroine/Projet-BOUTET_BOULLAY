using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PROJET___BOUTET___BOULLAY
{
    internal class Dictionnaire
    {
        // Déclaration du dictionnaire (format dictionary pour optimiser le temps de recherche)
        private Dictionary<char, List<string>> dict;

        // Définition des propriétés (get only par sécurité)
        public Dictionary<char, List<string>> Dict { get { return dict; } }

        /// <summary>
        /// Constructeur du dictionnaire, prenant en entrée le chemin du fichier
        /// </summary>
        /// <param name="path"></param>
        public Dictionnaire(string path)
        {
            // Lecture du fichier
            dict = new Dictionary<char, List<string>>();
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            // Interprétation (découpe et insère dans la bonne clé du dictionnaire)
            foreach (string line in lines)
            {
                string[] mots = line.Split(' ');
                foreach (string mot in mots)
                {
                    // Vérification que mot n'est pas vide avant de l'ajouter
                    if (!string.IsNullOrEmpty(mot))
                    {
                        // Normalisation du mot en majuscules
                        char key = char.ToUpper(mot[0]);

                        // Si la clé n'existe pas, on l'ajoute
                        if (!dict.ContainsKey(key))
                        {
                            dict.Add(key, new List<string>());
                        }

                        // Ajout du mot dans la liste correspondant à la clé
                        dict[key].Add(mot);
                    }
                }
            }
        }

        /// <summary>
        /// Override de la fonction ToString
        /// </summary>
        /// <returns>String contenant le nombre de mots par première lettre dans l'ordre alphabétique</returns>
        public override string ToString()
        {
            string ret = "Dictionnaire Français :\n";
            foreach (char key in dict.Keys.OrderBy(k => k))  // Tri par ordre alphabétique des clés
            {
                ret += key + ": " + dict[key].Count() + '\n';
            }
            return ret;
        }

        /// <summary>
        /// Initialisation du quicksort sur chaque première lettre du dictionnaire
        /// </summary>
        public void Tri_QuickSort()
        {
            foreach (char key in dict.Keys)
            {
                dict[key] = QuickSortList(dict[key], 0, dict[key].Count - 1);
            }
        }

        /// <summary>
        /// Algorithme QuickSort classique
        /// </summary>
        /// <param name="list"></param>
        /// <param name="leftIndex"></param>
        /// <param name="rightIndex"></param>
        /// <returns></returns>
        private List<string> QuickSortList(List<string> list, int leftIndex, int rightIndex)
        {
            int i = leftIndex;
            int j = rightIndex;
            string pivot = list[leftIndex];
            while (i <= j)
            {
                while (list[i].CompareTo(pivot) < 0)
                {
                    i++;
                }
                while (list[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                if (i <= j)
                {
                    (list[i], list[j]) = (list[j], list[i]);
                    i++;
                    j--;
                }
            }
            if (leftIndex < j)
                QuickSortList(list, leftIndex, j);
            if (i < rightIndex)
                QuickSortList(list, i, rightIndex);
            return list;
        }

        /// <summary>
        /// Initialisation de la recherche récursive
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>True : mot trouvé; False : mot inexistant</returns>
        public bool RechDichoRecursif(string mot)
        {
            if (!string.IsNullOrEmpty(mot) && dict.Keys.Contains(char.ToUpper(mot[0])))
            {
                return RechercheDicho(mot.ToUpper(), 0, dict[char.ToUpper(mot[0])].Count - 1);
            }
            return false;
        }

        /// <summary>
        /// Recherche dichotomique récursive d'un mot dans le dictionnaire
        /// Algorithme de recherche dichotomique classique
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="mini"></param>
        /// <param name="maxi"></param>
        /// <returns>True : mot trouvé; False : mot inexistant</returns>
        private bool RechercheDicho(string mot, int mini, int maxi)
        {
            if (mini == (maxi + mini) / 2)
            {
                if (dict[mot[0]][mini] == mot || dict[mot[0]][maxi] == mot)
                {
                    return true;
                }
                return false;
            }
            else
            {
                int mid = (maxi + mini) / 2;
                if (dict[mot[0]][mid].CompareTo(mot) < 0)
                {
                    return RechercheDicho(mot, mid + 1, maxi);
                }
                else if (dict[mot[0]][mid].CompareTo(mot) > 0)
                {
                    return RechercheDicho(mot, mini, mid - 1);
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
