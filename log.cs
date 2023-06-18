using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class log
    {
        public static log startState = null;
        public static log currentState = null;
        public static int activelog = 0;
        public static List<string> undo()
        {
            if (currentState.getPre() == null) return null;
            currentState = currentState.getPre();
            currentState.setnextState(null);
            activelog--;
            return currentState.getFiles();
        }
        public static void initLog(List<string> InFile)
        {
            activelog = 0;
            currentState = null;
            startState = new log(InFile);
            currentState = startState;
        }
        public static void update(List<string> Infile)
        {
            log newlog = new log(Infile);
            currentState.setnextState(newlog);
            currentState = newlog;
            if (activelog == 5)
            {
                startState = startState.getNext();
                startState.setpreState(null);
            }
            else
            {
                activelog++;
            }
        }
        private log preState;
        private log nextState;
        private List<string> files;
        public log(List<string> InFile)
        {
            nextState = null;
            preState = currentState;
            files = InFile;
        }
        public void setnextState(log state)
        {
            this.nextState = state;
        }
        public void setpreState(log state)
        {
            this.preState = state;
        }
        public log getNext()
        {
            return nextState;
        }
        public log getPre()
        {
            return preState;
        }
        public List<string> getFiles()
        {
            return files;
        }
    }
}
