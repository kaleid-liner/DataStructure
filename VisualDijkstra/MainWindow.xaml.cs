using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using Microsoft.Msagl.Drawing;
using Edge = DataStructure.VisualDijkstra.Edge;
using Microsoft.Msagl.WpfGraphControl;

namespace VisualDijkstra
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DijkstraViewModel();
            gviewer = new GraphViewer();
            gviewer.MouseDown += Gviewer_MouseDown;
            gviewer.BindToPanel(viewerPanel);
            gviewer.PopupMenus();
        }

        private void Gviewer_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            selectedObject = gviewer.ObjectUnderMouseCursor;
        }

        private Microsoft.Msagl.Drawing.Edge[][] edges;
        private Node[] nodes;
        private object selectedObject;
        GraphViewer gviewer;

        private void LoadGraph_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Choose input file",
                InitialDirectory = Environment.CurrentDirectory
            };
            int num;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    List<Edge>[] graph;
                    using (var file = File.OpenText(dialog.FileName))
                    {
                        string line = file.ReadLine();
                        num = int.Parse(line);
                        string[] strz;
                        graph = new List<Edge>[num];
                        edges = new Microsoft.Msagl.Drawing.Edge[num][];
                        nodes = new Node[num];
                        for (int i = 0; i < num; i++)
                        {
                            graph[i] = new List<Edge>();
                            edges[i] = new Microsoft.Msagl.Drawing.Edge[num];
                        }
                        for (int i = 0; i < num; i++)
                        {
                            nodes[i] = new Node(i.ToString());
                            line = file.ReadLine();
                            strz = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                            for (int j = i + 1; j < num; j++)
                            {
                                int cost = int.Parse(strz[j]);
                                if (cost > 0)
                                {
                                    graph[i].Add(new Edge { To = j, Cost = cost });
                                    graph[j].Add(new Edge { To = i, Cost = cost });
                                }
                            }
                        }
                    }
                    var viewModel = DataContext as DijkstraViewModel;
                    viewModel.Graph = graph;
                    viewModel.StartPT = -1;
                    viewModel.Destination = -1;
                    viewModel.Path = null;
                    Graph view = new Graph();
                    for (int i = 0; i < num; i++)
                    {
                        nodes[i].Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                        nodes[i].LabelText = i.ToString();
                        view.AddNode(nodes[i]);
                    }
                    for (int i = 0; i < num; i++)
                    {
                        foreach (var eg in graph[i])
                        {
                            if (edges[i][eg.To] == null)
                            {
                                edges[i][eg.To] = view.AddEdge(i.ToString(), eg.Cost.ToString(), eg.To.ToString());
                                edges[eg.To][i] = edges[i][eg.To];
                                edges[i][eg.To].Attr.ArrowheadAtSource = ArrowStyle.None;
                                edges[i][eg.To].Attr.ArrowheadAtTarget = ArrowStyle.None;
                            }
                        }
                    }
                    gviewer.Graph = view;
                }
                catch (Exception)
                {
                    tipsSnackBar.MessageQueue.Enqueue("Input File illegal format", "GOT IT", () => { });
                }
            }
        }

        private void SetStart_Click(object sender, RoutedEventArgs e)
        {
            var selected = selectedObject;
            var viewModel = DataContext as DijkstraViewModel;
            if (selected is IViewerNode)
            {
                if (viewModel.StartPT != -1)
                    nodes[viewModel.StartPT].Attr.FillColor = Microsoft.Msagl.Drawing.Color.Transparent;
                var node = (selected as IViewerNode).Node;
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                viewModel.StartPT = int.Parse(node.Id);
            }
        }

        private void SetDestination_Click(object sender, RoutedEventArgs e)
        {
            var selected = selectedObject;
            var viewModel = DataContext as DijkstraViewModel;
            if (selected is IViewerNode)
            {
                if (viewModel.Destination != -1)
                    nodes[viewModel.Destination].Attr.FillColor = Microsoft.Msagl.Drawing.Color.Transparent;
                var node = (selected as IViewerNode).Node;
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGreen;
                viewModel.Destination = int.Parse(node.Id);
            }
        }

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DijkstraViewModel;
            var path = viewModel.Path;
            if (path != null)
            {
                for (int i = 0; i < path.Length - 1; i++)
                {
                    edges[path[i]][path[i + 1]].Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
                }
            }
            if (viewModel.Solve())
            {
                path = viewModel.Path;
                for (int i = 0; i < path.Length - 1; i++)
                {
                    edges[path[i]][path[i + 1]].Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }
            }
        }

        private void ExportResult_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DijkstraViewModel;
            if (viewModel.Path != null)
            {
                string result = $"Min={viewModel.Distance}\nPath " + string.Join(" ", viewModel.Path);
                Clipboard.SetText(result);
                tipsSnackBar.MessageQueue.Enqueue("Result has been copied to Clipboard.", "GOT IT", ()=> { });
            }
        }
    }
}
