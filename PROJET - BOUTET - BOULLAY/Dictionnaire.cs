using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET___BOUTET___BOULLAY
{
    internal class Dictionnaire
    {
        // Declaration du dictionnaire (format dictionary pour optimiser le temps de recherche)
        private Dictionary<char, List<string>> dict;

        // Définition des propriétés (get only par securité)
        public Dictionary<char, List<string>> Dict { get { return dict; } }

        /// <summary>
        /// Constructeur du dictionnaire, prenant en entrée le chemin du fichier
        /// </summary>
        /// <param name="path"></param>
        public Dictionnaire(string path)
        {
            // Lecture
            dict = new Dictionary<char, List<string>>();
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
            // Interpretation (découpe et insère dans la bonne clef du dictionnaire)
            foreach (string line in lines)
            {
                string[] mots = line.Split(' ');
                foreach (string mot in mots)
                {
                    if (!dict.ContainsKey(mot[0]))
                    {
                        dict.Add(mot[0], new List<string>());
                    }
                    dict[mot[0]].Add(mot);
                }
            }
        }

        /// <summary>
        /// Override la fonction to string
        /// </summary>
        /// <returns>String contenant le nombre de mot par première lettre dans l'ordre alphabétique</returns>
        public override string ToString()
        {
            string ret = "Dictionnaire Francais :\n";
            foreach (char key in dict.Keys)
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
                dict[key] = QuickSortlist(dict[key], 0, dict[key].Count - 1);
            }
        }

        /// <summary>
        /// Algorithme QuickSort Classique
        /// </summary>
        /// <param name="list"></param>
        /// <param name="leftIndex"></param>
        /// <param name="rightIndex"></param>
        /// <returns></returns>
        private List<string> QuickSortlist(List<string> list, int leftIndex, int rightIndex)
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
                QuickSortlist(list, leftIndex, j);
            if (i < rightIndex)
                QuickSortlist(list, i, rightIndex);
            return list;
        }

        /// <summary>
        /// Intialisation de la recherche réccursive
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>True : mot trouvé; False : mot inexistant</returns>
        public bool RechDichoRecursif(string mot)
        {
            if (mot != null && mot.Length > 0 && dict.Keys.Contains(mot[0]))
            {
                return RechercheDicho(mot.ToUpper(), 0, dict[mot.ToUpper()[0]].Count - 1);
            }
            else
            { return false; }
        }

        /// <summary>
        /// Recherche dichotomique réccursive d'un mot dans le dictionnaire
        /// Algorithme de recherche dichotomique classique
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="mini"></param>
        /// <param name="maxi"></param>
        /// <returns>True : mot trouvé; False : mot inexistant</returns>
        private bool RechercheDicho(string mot, int mini, int maxi)
        {
            //DateTime dateTime = DateTime.Now;
            //Console.WriteLine(mot + " " + mini + " " + dict[mot[0]][mini] + " " + maxi + " " + dict[mot[0]][maxi]);
            if (mini == (maxi + mini) / 2)
            {
                if (dict[mot[0]][mini] == mot || dict[mot[0]][maxi] == mot)
                {
                    return true;
                }
                else
                {
                    //Console.WriteLine(DateTime.Now - dateTime);
                    return false;
                }
            }
            else
            {
                if (dict[mot[0]][(maxi + mini) / 2].CompareTo(mot) < 0)
                {
                    return RechercheDicho(mot, (maxi + mini) / 2, maxi);
                }
                else if (dict[mot[0]][(maxi + mini) / 2].CompareTo(mot) > 0)
                {
                    return RechercheDicho(mot, mini, (maxi + mini) / 2);
                }
                else
                {
                    //Console.WriteLine(DateTime.Now - dateTime);
                    //Console.WriteLine("research success");
                    return true;
                }
            }
        }
    }
}
