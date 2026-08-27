using System.Numerics;

namespace CS2TrainingPlugin.Domain;

public record WallPlacement
(
    float X,
    float Y,
    float Z,
    float Yaw,
    int Amount
);