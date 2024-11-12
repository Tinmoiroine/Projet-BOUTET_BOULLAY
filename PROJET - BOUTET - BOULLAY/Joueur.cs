using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET___BOUTET___BOULLAY
{
    /// <summary>
    /// Classe contenant tous les paramètres des joueurs 
    /// </summary>
    internal class Joueur
    {
        // Déclaration des variables (nom, mots, score)
        private string nom; 
        private List<string> mots;
        private int score;

        /// <summary>
        /// Constructeur de la classe Joueur prenant en paramètre le nom du joueur
        /// </summary>
        /// <param name="nom"></param>
        public Joueur(string nom)
        {
            // Initialisation des variables dans le constructeur
            this.nom = nom;
            this.score = 0;
            this.mots = new List<string>();
        }

        public string Nom { get { return nom; } set { this.nom = value; } } 
        public List<string> Mots { get {  return mots; } set {  this.mots = value; } }   
        public int Score { get { return score; } set { this.score = value; } }

        /// <summary>
        /// Fontion ajoutant le mot en entrée a la liste des mots deja trouvés par le joueur
        /// </summary>
        /// <param name="mot"></param>
        public void Ajout_Mot(string mot)
        {
            mots.Add(mot); // Ajout du mot à la liste "mots"
        }

        /// <summary>
        /// Fonction ToString() qui retourne les information sur le joueur (en l'occurrence, son nom)
        /// </summary>
        /// <returns>string de description</returns>
        public override string ToString()
        {
            return this.nom;
        }

        /// <summary>
        /// Fonction ajoutant le score gagné au joueur
        /// </summary>
        /// <param name="val"></param>
        public void Ajout_Score(int val)
        {
            score += val;
        }

        /// <summary>
        /// Focntion vérifiant si le mot entré a déja été trouvé.
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>Retiurne le booléen du test </returns>
        public bool Contient(string mot)
        {
            return mots.Contains(mot); // booléen du test
        }
    }
}
