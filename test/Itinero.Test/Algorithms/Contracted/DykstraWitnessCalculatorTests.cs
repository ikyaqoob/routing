﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2015 Abelshausen Ben
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
using Itinero.Algorithms.Contracted;
using Itinero.Algorithms.Contracted.Witness;
using Itinero.Data.Contracted;
using Itinero.Graphs.Directed;
using System.Collections.Generic;
using Itinero.Data.Contracted.Edges;

namespace Itinero.Test.Algorithms.Contracted
{
    /// <summary>
    /// Contains tests for the dykstra witness calculator.
    /// </summary>
    [TestFixture]
    public class DykstraWitnessCalculatorTests
    {
        /// <summary>
        /// Test on one edge with one hop.
        /// </summary>
        [Test]
        public void TestOneEdgeOneHop()
        {
            // build graph.
            var graph = new DirectedGraph(ContractedEdgeDataSerializer.Size);
            graph.AddEdge(0, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = null,
                Weight = 100
            }));

            var witnessCalculator = new DykstraWitnessCalculator(1);

            // calculate witness for weight of 200.
            var forwardWitnesses = new bool[1];
            var backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 1 }), new List<float>(new float[] { 200 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(true, backwardWitnesses[0]);

            // calculate witness for weight of 50.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 1 }), new List<float>(new float[] { 50 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);

            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);
        }

        /// <summary>
        /// Test on two edges with two hops.
        /// </summary>
        [Test]
        public void TestTwoEdgeInfiniteHops()
        {
            // build graph.
            var graph = new DirectedGraph(ContractedEdgeDataSerializer.Size);
            graph.AddEdge(0, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = null,
                Weight = 100
            }));
            graph.AddEdge(1, 2, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = null,
                Weight = 100
            }));

            var witnessCalculator = new DykstraWitnessCalculator(int.MaxValue);

            // calculate witness for weight of 200.
            var forwardWitnesses = new bool[1];
            var backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 1000 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(true, backwardWitnesses[0]);

            // calculate witness for weight of 50.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 50 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);

            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);
        }

        /// <summary>
        /// Test on two oneway edges with two hops.
        /// </summary>
        [Test]
        public void TestTwoOnewayEdgeInfiniteHops()
        {
            // build graph.
            var graph = new DirectedGraph(ContractedEdgeDataSerializer.Size);
            graph.AddEdge(0, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 100
            }));
            graph.AddEdge(1, 2, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 100
            }));
            graph.AddEdge(0, 2, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 300
            }));

            var witnessCalculator = new DykstraWitnessCalculator(int.MaxValue);

            // calculate witness for weight of 200.
            var forwardWitnesses = new bool[1];
            var backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 1000 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witness for weight of 50.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 50 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);

            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // build graph.
            graph = new DirectedGraph(ContractedEdgeDataSerializer.Size);
            graph.AddEdge(1, 0, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 100
            }));
            graph.AddEdge(2, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 100
            }));
            graph.AddEdge(2, 0, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 300
            }));


            // calculate witness for weight of 200.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 2, new List<uint>(new uint[] { 0 }), new List<float>(new float[] { 1000 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witness for weight of 50.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 2, new List<uint>(new uint[] { 0 }), new List<float>(new float[] { 50 }),
                ref forwardWitnesses, ref backwardWitnesses, uint.MaxValue);

            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);
        }

        /// <summary>
        /// Tests calculating witnesses in a quadrilateral.
        /// </summary>
        [Test]
        public void TestQuadrilateralOneWay()
        {
            // build graph.
            var graph = new DirectedGraph(ContractedEdgeDataSerializer.Size);
            graph.AddEdge(0, 2, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 100
            }));
            graph.AddEdge(2, 0, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = false,
                Weight = 100
            }));
            graph.AddEdge(0, 3, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = false,
                Weight = 10
            }));
            graph.AddEdge(3, 0, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 10
            }));
            graph.AddEdge(1, 2, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = false,
                Weight = 1000
            }));
            graph.AddEdge(2, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 1000
            }));
            graph.AddEdge(1, 3, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = true,
                Weight = 10000
            }));
            graph.AddEdge(3, 1, ContractedEdgeDataSerializer.SerializeMeta(new ContractedEdgeData()
            {
                ContractedId = Constants.NO_VERTEX,
                Direction = false,
                Weight = 10000
            }));
            graph.Compress(false);

            var witnessCalculator = new DykstraWitnessCalculator(int.MaxValue);

            // calculate witnesses for 0.
            var forwardWitnesses = new bool[1];
            var backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 2, new List<uint>(new uint[] { 3 }), new List<float>(new float[] { 110 }),
                ref forwardWitnesses, ref backwardWitnesses, 0);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 0.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 3, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 110 }),
                ref forwardWitnesses, ref backwardWitnesses, 0);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 2.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 1 }), new List<float>(new float[] { 1100 }),
                ref forwardWitnesses, ref backwardWitnesses, 2);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 2.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 1, new List<uint>(new uint[] { 0 }), new List<float>(new float[] { 1100 }),
                ref forwardWitnesses, ref backwardWitnesses, 2);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 1.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 3, new List<uint>(new uint[] { 2 }), new List<float>(new float[] { 11000 }),
                ref forwardWitnesses, ref backwardWitnesses, 1);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 1.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 2, new List<uint>(new uint[] { 3 }), new List<float>(new float[] { 11000 }),
                ref forwardWitnesses, ref backwardWitnesses, 1);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(true, backwardWitnesses[0]);

            // calculate witnesses for 3.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 0, new List<uint>(new uint[] { 1 }), new List<float>(new float[] { 10010 }),
                ref forwardWitnesses, ref backwardWitnesses, 3);
            Assert.AreEqual(true, forwardWitnesses[0]);
            Assert.AreEqual(false, backwardWitnesses[0]);

            // calculate witnesses for 3.
            forwardWitnesses = new bool[1];
            backwardWitnesses = new bool[1];
            witnessCalculator.Calculate(graph, 1, new List<uint>(new uint[] { 0 }), new List<float>(new float[] { 10010 }),
                ref forwardWitnesses, ref backwardWitnesses, 3);
            Assert.AreEqual(false, forwardWitnesses[0]);
            Assert.AreEqual(true, backwardWitnesses[0]);
        }
    }
}