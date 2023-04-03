using Chess;
using NUnit.Framework;
using UnityEngine;

public class VectorExtensionTests
{
    public class ManhattanDistanceTests
    {
        [Test]
        public void distance_between_identical_points_is_zero()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Assert.AreEqual(0, pos1.ManhattanDistance(pos1));
        }

        [Test]
        public void distance_between_horizontally_adjacent_points_is_1()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(1, 0);
            Assert.That(pos1.ManhattanDistance(pos2), Is.EqualTo(1));
        }

        [Test]
        public void distance_between_diagonally_adjacent_points_is_2()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(1, 1);
            Assert.That(pos1.ManhattanDistance(pos2), Is.EqualTo(2));
        }

        [Test]
        public void distance_between_points_with_negative_coordinates()
        {
            Vector2Int pos1 = new Vector2Int(-2, -1);
            Vector2Int pos2 = new Vector2Int(1, 2);
            Assert.That(pos1.ManhattanDistance(pos2), Is.EqualTo(6));
        }

        [Test]
        public void distance_between_points_not_in_a_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(2, 3);
            Assert.That(pos1.ManhattanDistance(pos2), Is.EqualTo(5));
        }
    }

    public class ChebyshevDistanceTests
    {
        [Test]
        public void distance_between_identical_points_is_zero()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Assert.That(pos1.ChebyshevDistance(pos1), Is.EqualTo(0));
        }

        [Test]
        public void distance_between_horizontally_adjacent_points_is_1()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(1, 0);
            Assert.That(pos1.ChebyshevDistance(pos2), Is.EqualTo(1));
        }

        [Test]
        public void distance_between_diagonally_adjacent_points_is_1()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(1, 1);
            Assert.That(pos1.ChebyshevDistance(pos2), Is.EqualTo(1));
        }

        [Test]
        public void distance_between_points_with_negative_coordinates()
        {
            Vector2Int pos1 = new Vector2Int(-2, -1);
            Vector2Int pos2 = new Vector2Int(1, 2);
            Assert.That(pos1.ChebyshevDistance(pos2), Is.EqualTo(3));
        }

        [Test]
        public void distance_between_points_not_in_a_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(2, 3);
            Assert.That(pos1.ChebyshevDistance(pos2), Is.EqualTo(3));
        }
    }

    public class IsOnSameLineTests
    {
        [Test]
        public void identical_points_is_on_same_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Assert.That(pos1.IsOnSameLine(pos1), Is.True);
        }

        [Test]
        public void diagonal_points_are_on_same_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(3, 3);
            Assert.That(pos1.IsOnSameLine(pos2), Is.True);
        }

        [Test]
        public void horizontal_points_are_on_same_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(3, 0);
            Assert.That(pos1.IsOnSameLine(pos1), Is.True);
        }

        [Test]
        public void skewed_points_are_not_on_same_line()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(2, 3);
            Assert.That(pos1.IsOnSameLine(pos2), Is.False);
        }
    }

    public class IsBetweenTests
    {
        [Test]
        public void start_point_cannot_be_end_point()
        {
            Vector2Int pos1 = new Vector2Int();
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() => pos1.IsBetween(pos1, pos1));
        }

        [Test]
        public void start_point_is_between()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(3, 3);
            Assert.That(pos1.IsBetween(pos1, pos2), Is.True);
        }

        [Test]
        public void end_point_is_between()
        {
            Vector2Int pos1 = new Vector2Int(0, 0);
            Vector2Int pos2 = new Vector2Int(3, 3);
            Assert.That(pos2.IsBetween(pos1, pos2), Is.True);
        }

        [Test]
        public void diagonal_point_is_between()
        {
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int end = new Vector2Int(3, 3);
            Vector2Int between = new Vector2Int(2, 2);
            Assert.That(between.IsBetween(start, end), Is.True);
        }

        [Test]
        public void skewed_point_is_outside()
        {
            Vector2Int start = new Vector2Int(0, 0);
            Vector2Int end = new Vector2Int(3, 3);
            Vector2Int outside = new Vector2Int(1, 2);
            Assert.That(outside.IsBetween(start, end), Is.False);
        }
    }
}
