using CS2TrainingPlugin.Domain;

namespace CS2TrainingPlugin.Maps;

public class Dust2Walls
{
    private static readonly WallPlacement[] BAttack =
    [
        new(-317.07f, 1940.20f, -125.73f, 90, 3), // Double doors CT side
        new(529.21f, 2295.97f, -125.69f, 180, 2), // CT Spawn Road to A
        
        new(-506.94f, -352.07f, 1.41f, -90, 2), // T Spawn AWP nest 
        new(378.93f, -433.54f, 0.45f, 90, 3), // T Spawn Road to Long
        new(-1404.92f, 1027.44f, 42.17f, 0, 2), // Upper tunnels to lower tunnels
    ];

    private static readonly WallPlacement[] AAttack = [
        new(-1308.55f, -250.24f, 129.42f, 180, 7), // T Spawn Road to B
        new(290.88f, 2040.12f, 96.85f, -90, 2), // A Site Road to Short
        new(-239.23f, 1995.21f, -125.52f, 0, 3), // CT Spawn Road to Mid
        new(119.64f, 478.83f, 0.03f, 180, 2), // T Spawn Road to Mid
        new(-506.94f, -352.07f, 1.41f, -90, 2), // T Spawn AWP nest 
    ];

    private static readonly WallPlacement[] MidAttack = [
        new(529.21f, 2295.97f, -125.69f, 180, 2), // CT Spawn Road to A
        new(-522.45f, 1485.92f, -110.97f, 180, 1), // Mid Road to Lower Tunnels
        new(-1308.55f, -250.24f, 129.42f, 180, 7), // T Spawn Road to B
        new(85.97f, 1535.95f, 0.92f, 180, 2), // Mid Road to Short A
        new(-630.70f, 2082.03f, -122.14f, 0, 4), // Mid Road to B
        new(583.95f, 301.36f, 1.15f, -90, 1), // T Spawn Double Doors
    ];
    
    public static readonly WallPlacement[][] RoundAttacks = [
        AAttack,
        BAttack,
        MidAttack
    ];
}