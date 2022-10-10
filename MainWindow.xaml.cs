using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using MyCollection.Test;

namespace MyCollection
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var point1 = Nominatim.AddressToPoint("4 Pl. du Louvre, 75001 Paris");
            var point2 = Nominatim.AddressToPoint("1 Pl. Jules Joffrin, 75018 Paris");
            var point3 = Nominatim.AddressToPoint("2-4 Rue Sainte Catherine, 33000 Bordeaux");

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
                GeomType = Maps.GeomType.Point,
            });

            var layer = Maps.AddLayer(new Maps.MyStruct
            {
                Name = "Apple Store",
                Points = new List<PointLatLng> { point3 },
                GeomType = Maps.GeomType.Point
            });

            // todo bug de merde
            Maps.AddPoint(layer, new PointLatLng(0, 0));
            Maps.AddPoint(layer, new PointLatLng(10, 10));
        }
    }
}