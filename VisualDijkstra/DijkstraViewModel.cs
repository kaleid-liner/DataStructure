using System.Collections.Generic;
using System.ComponentModel;
using DataStructure.VisualDijkstra;
using System.Windows;

namespace VisualDijkstra
{
    class DijkstraViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int[] _path;
        public int[] Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        private int _startPt = -1;
        public int StartPT
        {
            get => _startPt;
            set
            {
                _startPt = value;
                OnPropertyChanged(nameof(StartPT));
            }
        }

        private int _destination = -1;
        public int Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }

        private int _distance;
        public int Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged(nameof(Distance));
            }
        }

        private List<Edge>[] _graph;
        public List<Edge>[] Graph
        {
            get => _graph;
            set
            {
                _graph = value;
                OnPropertyChanged(nameof(NodeNum));
            }
        }

        public int NodeNum => Graph?.Length ?? 0;

        public bool Solve()
        {
            if (Graph != null)
            {
                if (StartPT >= 0 && StartPT < Graph.Length && Destination >= 0 && Destination < Graph.Length)
                {
                    var result = Dijkstra.Solve(Graph, StartPT, Destination);
                    Path = result.Item1;
                    Distance = result.Item2;
                    return true;
                }
                return false;
            }
            return false;
        }


        private void ExportResultExecute()
        {
            string result = $"Min={Distance}\nPath " + string.Join(" ", Path);
            Clipboard.SetText(result);
        }

    }
}
