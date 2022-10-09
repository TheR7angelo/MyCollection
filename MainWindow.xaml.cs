using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using MyCollection.Test;

namespace MyCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var point1 = Nominatim.AddressToPoint("4 Pl. du Louvre, 75001 Paris");
            var point2 = Nominatim.AddressToPoint("1 Pl. Jules Joffrin, 75018 Paris");

            var address = Nominatim.PointToAddress(point1);
            Console.WriteLine(address);
            
            Maps.AddLayer(new Maps.MyStruct
            {
                Name = "Mairie 1",
                Points = new List<PointLatLng>{point1},
                GeomType = Maps.GeomType.Point,
                Shape = new Rectangle{Fill = Brushes.Coral, MinHeight = 7, MinWidth = 7}
            });
            
            Maps.AddLayer(new Maps.MyStruct
            {
                Name = "Mairie 2",
                Points = new List<PointLatLng>{point2},
                GeomType = Maps.GeomType.Point
            });
            Maps.AddLayer(new Maps.MyStruct
            {
                Name = "Mairie 3",
                Points = new List<PointLatLng>{point2},
                GeomType = Maps.GeomType.Point
            });
        }
    }
}