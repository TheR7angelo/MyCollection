using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace MyCollection;

public partial class Maps
{
    private readonly Maps _instance;
    private static readonly Random Rng = new ();
    private static List<MyStruct> _listLayer = new();
    // private ObservableCollection<MyStruct> _listLayout = new();

    public Maps()
    {
        InitializeComponent();
        _instance = this;
    }

    private void myList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Console.Write("hey");
    }

    public MyStruct AddLayer(MyStruct layer)
    {
        if (layer.Id is null) layer.SetId();

        var color = GetRandomColor();
        
        var check = new CheckBox
        {
            Name = layer.Id,
            Content = $"{layer.Name}_{color.ToString()}",
            IsChecked = true
        };
        
        check.Click += CheckOnClick;

        var item = new ListBoxItem{Content = check};

        layer.ZIndex = -ListLayer.Items.Add(item);

        foreach (var marker in layer.Points.Select(pt => new GMapMarker(pt)))
        {
            marker.Shape = layer.Shape;
            marker.ZIndex = layer.ZIndex;

            layer.Markers.Add(marker);
            Gmap.Markers.Add(marker);
        }
        _listLayer.Add(layer);

        return layer;
    }

    private void CheckOnClick(object sender, RoutedEventArgs e)
    {
        var check = sender as CheckBox;
        var visibility = (bool)check!.IsChecked! ? Visibility.Visible : Visibility.Hidden;

        _listLayer.Find(s => s.Id!.Equals(check.Name)).SetVisibility(check.Name, visibility);
    }

    private void Gmap_OnLoaded(object sender, RoutedEventArgs e)
    {
        GMaps.Instance.Mode = AccessMode.ServerAndCache;
        Gmap.SetPositionByKeywords("Paris, France");
        Gmap.ShowCenter = false;
        // choose your provider here
        Gmap.MapProvider = OpenStreetMapProvider.Instance;
        Gmap.MinZoom = 3;
        Gmap.MaxZoom = 30;
        // whole world zoom
        Gmap.Zoom = 3;
        // lets the map use the mousewheel to zoom
        Gmap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
        // lets the user drag the map
        Gmap.CanDragMap = true;
        // lets the user drag the map with the left mouse button
        Gmap.DragButton = MouseButton.Left;
    }

    public enum GeomType
    {
        Point,
    }
    
    public struct MyStruct
    {
        public string? Id = null;
        public string? Name = null;
        public int ZIndex = 0;
        public GeomType? GeomType = null;
        public List<PointLatLng> Points = new ();
        public Shape? Shape = new Ellipse { Height = 7, Width = 7, Fill = GetRandomColor() };
        public Visibility Visibility = Visibility.Visible;
        public List<GMapMarker> Markers = new ();
        public List<GMapPolygon> Polygons = new ();

        public MyStruct()
        {
        }

        #region Setter

        public void SetId()
        {
            var name = Name!.Replace(" ", "_");
            Id = $"{name}_{Guid.NewGuid():N}";
        }
        
        public void SetIndex(string id, int index)
        {
            foreach (var layout in _listLayer.Where(layout => layout.Id!.Equals(id)))
            {
                foreach (var point in layout.Markers) point.ZIndex = index;
            }
            ZIndex = index;
        }

        public void SetVisibility(string id, Visibility visibility)
        {
            foreach (var layout in _listLayer.Where(layout => layout.Id!.Equals(id)))
            {
                foreach (var point in layout.Markers) point.Shape.Visibility = visibility;
            }
            Visibility = visibility;
        }

        #endregion
    }

    private void ListLayer_OnDrop(object sender, DragEventArgs e)
    {
        // todo optimise si index inférieur ne pas toucher

        foreach (var (item, index) in ListLayer.Items.OfType<ListBoxItem>().Select((value, index) => ( value, index )))
        {
            var check = item.Content as CheckBox;

            _listLayer.Find(s => s.Id!.Equals(check!.Name)).SetIndex(check!.Name, index);
        }
    }

    public void AddPoint(MyStruct layer, PointLatLng point)
    {
        foreach (var _layer in _listLayer.Where(s => s.Id.Equals(layer.Id)))
        {
            _layer.Points.Add(point);

            var marker = new GMapMarker(point);
            marker.Shape = layer.Shape;
            marker.ZIndex = layer.ZIndex;
            Gmap.Markers.Add(marker);
        }
    }

    
    public void AddGeom(GMapMarker gMapMarker)
    {
        Gmap.Markers.Add(gMapMarker);
    }

    private static SolidColorBrush GetRandomColor()
    {
        return new SolidColorBrush(Color.FromArgb(255, (byte)Rng.Next(255), (byte)Rng.Next(255), (byte)Rng.Next(255)));
    }
}