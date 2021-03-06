﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2016 Abelshausen Ben
// 
// This file is part of Itinero.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Itinero. If not, see <http://www.gnu.org/licenses/>.

using NUnit.Framework;
using Itinero.Algorithms.Collections;
using Itinero.Algorithms.Contracted.EdgeBased;
using Itinero.Graphs.Directed;
using System;
using Itinero.Algorithms;
using Itinero.Data.Contracted.Edges;

namespace Itinero.Test.Algorithms.Contracted.EdgeBased
{
    /// <summary>
    /// Containts tests for the edge difference priority calculator.
    /// </summary>
    [TestFixture]
    public class EdgeDifferencePriorityCalculatorTests
    {
        /// <summary>
        /// Tests calculator with no neighbours.
        /// </summary>
        [Test]
        public void TestNoNeighbours()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(1, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph, 
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with one neighbour and no witnesses.
        /// </summary>
        [Test]
        public void TestOneNeighbour()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, null);
            graph.AddEdge(1, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(-1, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighbours()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, null);
            graph.AddEdge(1, 0, 100, null);
            graph.AddEdge(0, 2, 100, null);
            graph.AddEdge(2, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock((source, target) =>
                {
                    return new Tuple<EdgePath<float>, EdgePath<float>>(
                        new EdgePath<float>(0),
                        new EdgePath<float>(0));
                }));
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with three neighbours and no witnesses.
        /// </summary>
        [Test]
        public void TestThreeNeighbours()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, null);
            graph.AddEdge(1, 0, 100, null);
            graph.AddEdge(0, 2, 100, null);
            graph.AddEdge(2, 0, 100, null);
            graph.AddEdge(0, 3, 100, null);
            graph.AddEdge(3, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock((source, target) =>
                {
                    return new Tuple<EdgePath<float>, EdgePath<float>>(
                        new EdgePath<float>(0),
                        new EdgePath<float>(0));
                }));
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(3, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours, oneway edges, and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighboursOneWay()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, true);
            graph.AddEdge(1, 0, 100, false);
            graph.AddEdge(0, 2, 100, false);
            graph.AddEdge(2, 0, 100, true);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock((source, target) =>
                {
                    return new Tuple<EdgePath<float>, EdgePath<float>>(
                        new EdgePath<float>(0),
                        new EdgePath<float>(0));
                }));
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(0, priority);

            // build another graph.
            graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, false);
            graph.AddEdge(1, 0, 100, true);
            graph.AddEdge(0, 2, 100, true);
            graph.AddEdge(2, 0, 100, false);

            // create a witness calculator and the priority calculator.
            priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock((source, target) =>
                {
                    return new Tuple<EdgePath<float>, EdgePath<float>>(
                        new EdgePath<float>(0),
                        new EdgePath<float>(0));
                }));
            priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbours, oneway opposite edges, and no witnesses.
        /// </summary>
        [Test]
        public void TestTwoNeighboursOneWayOpposite()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, true);
            graph.AddEdge(1, 0, 100, false);
            graph.AddEdge(0, 2, 100, true);
            graph.AddEdge(2, 0, 100, false);

            // create a witness calculator and the priority calculator.
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock((source, target) =>
                {
                    if (source == 0 && target == 1)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(0, 100, new EdgePath<float>(1)),
                            new EdgePath<float>());
                    }
                    if (source == 0 && target == 2)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(0, 100, new EdgePath<float>(2)),
                            new EdgePath<float>());
                    }
                    if (source == 1 && target == 0)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(),
                            new EdgePath<float>(0, 100, new EdgePath<float>(1)));
                    }
                    if (source == 2 && target == 0)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(),
                            new EdgePath<float>(0, 100, new EdgePath<float>(2)));
                    }
                    if (source == 1 && target == 2)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(),
                            new EdgePath<float>());
                    }
                    if (source == 2 && target == 1)
                    {
                        return new Tuple<EdgePath<float>, EdgePath<float>>(
                            new EdgePath<float>(),
                            new EdgePath<float>());
                    }
                    return null;
                }));
            var priority = priorityCalculator.Calculate(new BitArray32(graph.VertexCount), (i) => null, 0);

            Assert.AreEqual(-2, priority);
        }

        /// <summary>
        /// Tests calculator with one neighbour but with contracted neighbour.
        /// </summary>
        [Test]
        public void TestOneNeighboursContracted()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, true);
            graph.AddEdge(1, 0, 100, false);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new BitArray32(graph.VertexCount);
            contractedFlags[1] = true;
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(contractedFlags, (i) => null, 0);

            Assert.AreEqual(-2, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with one contracted neighbour.
        /// </summary>
        [Test]
        public void TestTwoNeighboursContracted()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, null);
            graph.AddEdge(1, 0, 100, null);
            graph.AddEdge(0, 2, 100, null);
            graph.AddEdge(2, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new BitArray32(graph.VertexCount);
            contractedFlags[1] = true;
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            var priority = priorityCalculator.Calculate(contractedFlags, (i) => null, 0);

            Assert.AreEqual(-3, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with contracted neighbour.
        /// </summary>
        [Test]
        public void TestOneNeighboursNotifyContracted()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, true);
            graph.AddEdge(1, 0, 100, false);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new BitArray32(graph.VertexCount);
            contractedFlags[1] = true;
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            priorityCalculator.NotifyContracted(1);
            var priority = priorityCalculator.Calculate(contractedFlags, (i) => null, 0);

            Assert.AreEqual(1, priority);
        }

        /// <summary>
        /// Tests calculator with two neighbour but with one contracted neighbour.
        /// </summary>
        [Test]
        public void TestTwoNeighboursNotifyContracted()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 1, 100, null);
            graph.AddEdge(1, 0, 100, null);
            graph.AddEdge(0, 2, 100, null);
            graph.AddEdge(2, 0, 100, null);

            // create a witness calculator and the priority calculator.
            var contractedFlags = new BitArray32(graph.VertexCount);
            contractedFlags[1] = true;
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new WitnessCalculatorMock());
            priorityCalculator.NotifyContracted(1);
            var priority = priorityCalculator.Calculate(contractedFlags, (i) => null, 0);

            Assert.AreEqual(0, priority);
        }

        /// <summary>
        /// Tests calculator on a small network with 4 vertices in a quadrilateral.
        /// </summary>
        [Test]
        public void TestQuadrilateralOneWay()
        {
            // build graph.
            var graph = new DirectedDynamicGraph(ContractedEdgeDataSerializer.DynamicFixedSize);
            graph.AddEdge(0, 2, 100, true);
            graph.AddEdge(2, 0, 100, false);
            graph.AddEdge(0, 3, 10, false);
            graph.AddEdge(3, 0, 10, true);
            graph.AddEdge(1, 2, 1000, false);
            graph.AddEdge(2, 1, 1000, true);
            graph.AddEdge(1, 3, 10000, true);
            graph.AddEdge(3, 1, 10000, false);
            graph.Compress();

            // create a witness calculator and the priority calculator.
            var contractedFlags = new BitArray32(graph.VertexCount);
            var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
                new Itinero.Algorithms.Contracted.EdgeBased.Witness.DykstraWitnessCalculator(10));
            //var priorityCalculator = new EdgeDifferencePriorityCalculator(graph,
            //    new WitnessCalculatorMock(new uint[][]
            //        {
            //            new uint[] { 1, 3, 2, 1 },
            //            new uint[] { 3, 0, 1, 1 }
            //        }));

            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, (i) => null, 0));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, (i) => null, 1));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, (i) => null, 2));
            Assert.AreEqual(0, priorityCalculator.Calculate(contractedFlags, (i) => null, 3));
        }
    }
}