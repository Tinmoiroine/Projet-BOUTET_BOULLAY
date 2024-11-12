using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET___BOUTET___BOULLAY
{
    internal class Joueur
    {
        private string nom; 
        private List<string> mots;
        private int score;

        public Joueur(string nom)
        {
            this.nom = nom;
            this.score = 0;
            this.mots = new List<string>();
        }

    }
}
