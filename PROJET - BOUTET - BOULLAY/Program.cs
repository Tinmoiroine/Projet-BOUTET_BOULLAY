namespace PROJET___BOUTET___BOULLAY
{
    internal class Program
    {
        /// <summary>
        /// Fonction Main de la classe programme : Lancement du Jeu
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {       
            bool condition_arret = false; // Booléen de condition d'arrêt du programme 
            while (!condition_arret)
            {
                Jeu jeu = new Jeu(); // Declaration d'un nouveau jeu (classe jeu)
                condition_arret = jeu.Launcher(); // Appel Lancement d'une partie (classe jeu)
            }
        }
    }    
}
