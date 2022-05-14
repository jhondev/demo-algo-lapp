// There is a set of points with integer coordinates, e.g. (10; -3)
// input example (symmetrical): (-5; -4), (1; 9), (7; -4), (-6; 4), (8; 4), (-5; -4).
// input example (non symmetrical): (-5; -4), (1; 9), (7; -4), (8; 4), (-5; -4).
// output: true or false

// You need to determine if there is a line, parallel to y-axes, that split this set of points in such a way 
// that those separated sets are symmetrical to each other relative to this line.

var testCases = new (int x, int y)[][]
{
    new[] { (-5, -4), (1, 9), (7, -4), (-6, 4), (8, 4), (-5, -4) }, // symmetrical
    new[] { (-5, -4), (1, 9), (7, -4), (8, 4), (-5, -4) }, // not symmetrical
    new[] { (-4, -4), (1, 9), (7, -4), (-6, 4), (8, 4) }, // not symmetrical
    new[] { (-5, -4), (1, -4), (7, -4), (-6, 4), (1, 4), (8, 4), (-5, -4) }, // symmetrical
    new[] { (2, 4), (2, 3), (2, 2), (2, 1), (2, 0) }, // symmetrical
    new[] { (-5, -4), (1, 9) }, // not symmetrical
};

foreach (var testCase in testCases)
{
    Console.WriteLine($"POINTS: {string.Join(',', testCase.Select(p => $"({p.x};{p.y})"))} \n");
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

    // lets extract and grouped points at the same y-axis level
    var yPointsGp = set.Distinct().GroupBy(p => p.y).ToArray();

    // uncomment to print y-axis grouped points
    // foreach (var yg in yPointsGp)
    // {
    //     var points = string.Join(',', yg.Distinct().Select(p => $"({p.x},{p.y})"));
    //     Console.WriteLine($"Points for Y level {yg.Key}: {points}");
    // }

    // if all the points are in different y-axis level, all of them must be in the same x-axis level
    // i.e. all the points in the same y-axis parallel line
    if (yPointsGp.All(pts => pts.Count() == 1))
    {
        var singlePts = yPointsGp.SelectMany(pts => pts).ToArray();
        return singlePts.All(pt => pt.x == singlePts.First().x);
    }

    // cannot be more than 3 points in the same y-axis level.
    // i.e. 2 symmetric points and 1 point in the parallel line at most
    if (yPointsGp.Any(pts => pts.Count() > 3)) return false;

    // at this point we've guaranteed in same y-axis level
    //   - more than 1 point in some level
    //   - maximum 3 points
    //   - optionally 1 point in some level
    // let's take the 2 x-axis extreme points from any y-axis group with more than 1 point and get the x mid point between them
    var xPoints = yPointsGp
        .Where(pts => pts.Count() > 1)
        .First()
        .Select(pt => pt.x)
        .OrderBy(x => x)
        .ToArray();
    var getXMidPoint = (int[] xPts) =>
    {
        int x1 = xPts.Min(), x2 = xPts.Max();
        var distance = x2 - x1;
        // since we are using ints the distance must be even
        if (distance % 2 != 0) return (0, false);
        var midDistance = distance / 2;
        return (x1 + midDistance, true);
    };
    var (xMidPoint, valid) = getXMidPoint(xPoints);
    if (!valid) return false;

    // now every y-axis grouped needs to comply these conditions:
    //   - x-axis extreme points need to have the same xMidPoint already calculated
    //   - if there are more than 2 points the remaining point needs to have the x value equals to xMidPoint
    //          (i.e in the same y-axis parallel line)
    //   - if there is only 1 point needs to have the x value equals to xMidPoint

    foreach (var points in yPointsGp)
    {
        var currentXPoints = points.Select(pt => pt.x).OrderBy(x => x).ToArray();
        
        if (currentXPoints.Length == 1 && currentXPoints[0] != xMidPoint) return false;
        
        var (currentXMidPoint, currentValid) = getXMidPoint(currentXPoints);
        if (currentXMidPoint != xMidPoint || !currentValid ||
            (currentXPoints.Length == 3 && currentXPoints[1] != xMidPoint))
            return false;
    }

    return true;
}