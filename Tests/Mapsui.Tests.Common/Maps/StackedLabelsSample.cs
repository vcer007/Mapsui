﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;

namespace Mapsui.Tests.Common.Maps
{
    public static class StackedLabelsSample
    {
        private const string LabelColumn = "Label";

        public static Map CreateMap()
        {
            var map = new Map
            {
                Viewport = {Center = new Point(0, 0), Width = 250, Height = 250, Resolution = 0.5}
            };
            var provider = CreateRandomPointsProvider(GenerateRandomPoints(new BoundingBox(-100, -100, 100, 100), 20));
            map.Layers.Add(CreateLabelLayer(provider));
            map.Layers.Add(CreateLayer(provider));
            return map;
        }

        private static ILayer CreateLabelLayer(IProvider provider)
        {
            return new LabelLayer("stacks")
            {
                DataSource = provider,
                UseLabelStacking = true,
                LabelColumn = LabelColumn,
                Style = new LabelStyle {LabelColumn = LabelColumn}
            };
        }

        private static ILayer CreateLayer(IProvider dataSource)
        {
            return new MemoryLayer
            {
                DataSource = dataSource,
                Style = new SymbolStyle {SymbolScale = 1, Fill = new Brush(new Color {A = 128, R = 8, G = 20, B = 192})}
            };
        }

        private static MemoryProvider CreateRandomPointsProvider(IEnumerable<IGeometry> randomPoints)
        {
            var features = new Features();
            var count = 0;
            foreach (var point in randomPoints)
            {
                var feature = new Feature
                {
                    Geometry = point,
                    [LabelColumn] = count.ToString(CultureInfo.InvariantCulture)
                };
                features.Add(feature);
                count++;
            }
            return new MemoryProvider(features);
        }

        private static IEnumerable<IGeometry> GenerateRandomPoints(BoundingBox box, int count = 25)
        {
            var result = new List<IGeometry>();
            var random = new Random(0);

            for (var i = 0; i < count; i++)
            {
                var x = random.NextDouble()*box.Width + box.Left;
                var y = random.NextDouble()*box.Height - (box.Height - box.Top);
                result.Add(new Point(x, y));
            }

            return result;
        }
    }
}