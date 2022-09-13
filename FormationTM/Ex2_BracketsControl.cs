using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_IV
{
    public static class BracketsControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public static bool BracketsControls(string sentence)
        {
            // 1. La structure de données la plus adaptée est la pile. On va ajouter un élément dans la pile à chaque parenthèse ouvrante,
            // en enlever un à chaque parenthèse fermante, et vérifier qu'on a bien 0 éléments dans la pile à la fin.

            Stack<char> pile = new Stack<char>();

            foreach (char c in sentence)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                        pile.Push(c);
                        break;
                    case ')':
                        if (pile.Peek() == '(')
                        {
                            pile.Pop();
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case ']':
                        if (pile.Peek() == '[')
                        {
                            pile.Pop();
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case '}':
                        if (pile.Peek() == '{')
                        {
                            pile.Pop();
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }
            if (pile.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
