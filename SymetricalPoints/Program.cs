// There is a set of points with integer coordinates, e.g. (10; -3)
// input example (symmetrical): (-5; -4), (1; 9), (7; -4), (-6; 4), (8; 4), (-5; -4).
// input example (non symmetrical): (-5; -4), (1; 9), (7; -4), (8; 4), (-5; -4).
// output: true or false

// You need to determine if there is a line, parallel to y-axes, that split this set of points in such a way 
// that those separated sets are symmetrical to each other relative to this line.

var testCases = new (int x, int y)[][]
{
    new[] { (-1, 0), (1, 0) }, // symmetrical
    new[] { (-1, 0), (1, 0), (-2, 0), (2, 0) }, // symmetrical
    new[] { (-5, -4), (1, 9), (7, -4), (-6, 4), (8, 4), (-5, -4) }, // symmetrical
    new[] { (-5, -4), (1, 9), (7, -4), (8, 4), (-5, -4) }, // not symmetrical
    new[] { (-4, -4), (1, 9), (7, -4), (-6, 4), (8, 4) }, // not symmetrical
    new[] { (-5, -4), (1, -4), (7, -4), (-6, 4), (1, 4), (8, 4), (-5, -4) }, // symmetrical
    new[] { (2, 4), (2, 3), (2, 2), (2, 1), (2, 0) }, // symmetrical
    new[] { (-5, -4), (1, 9) }, // not symmetrical
    new[] { (-1, 0), (1, 0), (-2, 0), (2, 0) }, // symmetrical
    new[] { (-1, 0), (1, 0), (-2, 0), (2, 0), (-3, 0), (3, 0) }, // symmetrical
    new[] { (-1, 0), (1, 0), (-2, 0), (2, 0), (-5, 3), (5, 3) }, // symmetrical
    new[] { (2, 1), (5, 1) } // symmetrical with decimal mean
};

foreach (var testCase in testCases)
{
    Console.WriteLine($"POINTS: {string.Join(',', testCase.Select(p => $"({p.x};{p.y})"))} \n");
    // var isSymmetrical = IsSymmetrical(testCase);
    var isSymmetrical = IsSymmetrical(testCase);
    var isSymmetricalText = isSymmetrical ? "" : "NOT ";
    Console.WriteLine($">> {isSymmetricalText}SYMMETRICAL");
    Console.WriteLine("\n--------------------------------------------------------------------------");
}

static bool IsSymmetrical((int x, int y)[] set)
{
    if (set.Length == 0) throw new Exception("Set should not be empty");

    // one point is symmetrical to itself
    if (set.Length == 1) return true;

    // let's calculate the arithmetic mean of x-axis (we need to ignore repeated x values)
    // also let's create a hash set to be queried later 
    var total = 0;
    var xHashSet = new HashSet<int>();
    var pointsHashSet = new HashSet<(int x, int y)>();
    foreach (var point in set)
    {
        if (!xHashSet.Contains(point.x))
        {
            xHashSet.Add(point.x);
            total += point.x;
        }

        if (!pointsHashSet.Contains(point))
            pointsHashSet.Add(point);
    }

    var meanX = (decimal)total / xHashSet.Count;

    // having the mean point, let's calculate the symmetric point of each set point and validate it in the hashSet
    foreach (var (x, y) in pointsHashSet)
    {
        if (x == meanX) continue; // symmetric to itself
        var distanceToMean = Math.Abs(meanX - x);
        var xS = x < meanX ? meanX + distanceToMean : meanX - distanceToMean;
        if (!pointsHashSet.Contains(((int)xS, y))) return false;
    }

    return true;
}