using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public class Percolation
    {
        private readonly bool[,] _open;
        private readonly bool[,] _full;
        private readonly int _size;
        private bool _percolate;

        public Percolation(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Taille de la grille négative ou nulle.");
            }

            _open = new bool[size, size];
            _full = new bool[size, size];
            _size = size;
        }

        public bool IsOpen(int i, int j)
        {
            if (_open[i, j])
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        private bool IsFull(int i, int j)
        {
            if (_full[i, j])
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public bool Percolate()
        {
            if (_percolate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();

            // on ajoute le voisin uniquement à moins qu'il soit en dehors du tableau.
            if (i > 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            }
            if (i < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            if (j > 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
            }
            if (j < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
            }

            return (voisins);
        }

        private void Fill(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = CloseNeighbors(i, j);
            
            foreach(KeyValuePair<int, int> voisin in voisins)
            {
                if (IsOpen(voisin.Key, voisin.Value) && !IsFull(voisin.Key, voisin.Value))
                {
                    // on remplit le voisin qui est ouvert et vide.
                    _full[voisin.Key, voisin.Value] = true;

                    // il y a percolation si on remplit une case de la dernière ligne.
                    if (voisin.Key == _size - 1)
                    {
                        _percolate = true;
                    }

                    // on rappelle la méthode de manière récursive pour propager le remplissage.
                    Fill(voisin.Key, voisin.Value);
                }
            }
        }

        public void Open(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = CloseNeighbors(i, j);

            // on ouvre la case.
            _open[i, j] = true;

            // on remplit la case si elle est sur la première ligne.
            if (i == 0)
            {
                _full[i, j] = true;
            }

            // on remplit la case si elle a un voisin plein.
            foreach (KeyValuePair<int,int> voisin in voisins)
            {
                if (IsFull(voisin.Key, voisin.Value))
                {
                   _full[i, j] = true;

                    // il y a percolation si on remplit une case de la dernière ligne.
                    if (voisin.Key == _size - 1)
                    {
                       _percolate = true;
                    }

                    // on remplit toutes les cases vides et ouvertes connectées à la case qu'on vient de remplir.
                    Fill(i, j);

                    // puisqu'on a trouvé un voisin plein, plus besoin de tester les autres voisins.
                    break;
                }
            }
        }
    }
}
